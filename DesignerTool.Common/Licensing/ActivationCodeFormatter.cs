using DesignerTool.Common.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DesignerTool.Common.Licensing
{
    public class ActivationCodeFormatter : IFormatter
    {
        private const string START_TRIM = "CL";
        private const char CHAR_SPLIT = 'A';

        private const int DIGIT_ADD = 22;
        private const int CHAR_ADD = 1;

        private const int MODE_LOW = 1;
        private const int MODE_MID = 50;
        private const int MODE_HIGH = 99;


        public ActivationCodeFormatter()
        {
            this.Context = new StreamingContext(StreamingContextStates.All);
        }

        public SerializationBinder Binder { get; set; }
        public StreamingContext Context { get; set; }
        public ISurrogateSelector SurrogateSelector { get; set; }

        public object Deserialize(Stream serializationStream)
        {
            serializationStream.Seek(0, SeekOrigin.Begin);

            var activationCode = new ActivationCode();

            StreamReader sr = new StreamReader(serializationStream);
            string code = sr.ReadLine().Substring(1); // First character
            var items =
                String.Concat(code.Reverse()) // Key is reversed for security
                    .Substring(1) // Last character means nothing. 
                    .Split(CHAR_SPLIT).ToList(); // Split into items

            activationCode.IsExpiryMode = Int32.Parse(items[0]) < MODE_MID; // Less than mid = Expiry Mode

            StringBuilder sbClientCode = new StringBuilder(START_TRIM);
            foreach (int ccItem in items[2])
            {
                sbClientCode.Append((char)(ccItem - DIGIT_ADD));
            }
            activationCode.ClientCode = sbClientCode.ToString();

            if (activationCode.IsExpiryMode)
            {
                int year = Int32.Parse(String.Concat(items[1].Substring(0, 4).Select(c => (char)(((int)c) - DIGIT_ADD))));
                int month = Int32.Parse(String.Concat(items[1].Substring(4, 2).Select(c => (char)(((int)c) - DIGIT_ADD))));
                int day = Int32.Parse(String.Concat(items[1].Substring(6, 2).Select(c => (char)(((int)c) - DIGIT_ADD))));
                activationCode.ExpiryDate = new DateTime(year, month, day);
            }
            else
            {
                activationCode.ExtensionPeriod = (PeriodType)Enum.Parse(typeof(PeriodType), items[1][3].ToString()); // Fourth char is enum representation (first 3 are dummy)
                activationCode.Extension = Int32.Parse(items[1].Substring(4)); // Rest is the extension value
            }

            // Return the deserialized activation code.
            return activationCode;
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            if (graph is ActivationCode)
            {
                var activationCode = (ActivationCode)graph;
                StringBuilder sbClientCode = new StringBuilder();
                foreach (int asciiChar in activationCode.ClientCode.Replace(START_TRIM, string.Empty))
                {
                    sbClientCode.Append((char)(asciiChar + DIGIT_ADD));
                }

                Random r = new Random();
                StringBuilder sbExpiryValue = new StringBuilder();
                int mode;

                if (activationCode.IsExpiryMode)
                {
                    mode = r.Next(MODE_LOW, MODE_MID - 1);
                    sbExpiryValue.Append(String.Concat(activationCode.ExpiryDate.ToString("yyyy").Select(c => (char)((int)c + DIGIT_ADD))));
                    sbExpiryValue.Append(String.Concat(activationCode.ExpiryDate.ToString("MM").Select(c => (char)((int)c + DIGIT_ADD))));
                    sbExpiryValue.Append(String.Concat(activationCode.ExpiryDate.ToString("dd").Select(c => (char)((int)c + DIGIT_ADD))));
                }
                else
                {
                    mode = r.Next(MODE_MID, MODE_HIGH);
                    // Add 3 random alpha values. The next digit is the enum, the remaining number is the extension amount.
                    sbExpiryValue.Append(string.Format("{0}{1}{2}{3}{4}", (char)r.Next(66, 72), (char)r.Next(73, 80), (char)r.Next(81, 89), (int)activationCode.ExtensionPeriod, activationCode.Extension));
                }

                StreamWriter sw = new StreamWriter(serializationStream);
                char randomChar = (char)new Random().Next(66, 76); // To add complexity
                char randomChar2 = (char)new Random().Next(76, 89); // To add complexity
                sw.Write(
                    String.Concat(
                        String.Format("{1}{2}{0}{3}{0}{4}{5}", CHAR_SPLIT, randomChar, mode, sbExpiryValue, sbClientCode, randomChar2).Reverse()));
                sw.Flush();
            }
            else
            {
                throw new InvalidCastException("This Formatter may only be used for the ActivationCode class");
            }
        }

    }
}
