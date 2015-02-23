using DesignerTool.Common.Licensing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DesignerTool.Common.Utils
{
    public static class Security
    {
        /// <summary>
        /// Encrypts the given plain text using fast encryption
        /// </summary>
        public static string Encrypt(this string val, string pwd)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                throw new Exception("Encryption value may not be empty.");
            }
            //if (pwd.Trim().Length < 8)
            //{
            //    throw new Exception("Encryption password must consist of 8 or more characters.");
            //}

            byte[] plaintextBytes = System.Text.Encoding.Unicode.GetBytes(val);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider();

            tripleDes.Key = TruncateHash(pwd, tripleDes.KeySize / 8);
            tripleDes.IV = TruncateHash(pwd, tripleDes.BlockSize / 8);

            CryptoStream encStream = new CryptoStream(ms, tripleDes.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

            encStream.Write(plaintextBytes, 0, plaintextBytes.Length);
            encStream.FlushFinalBlock();

            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// Decrypts the given cipher using fast decryption
        /// </summary>
        public static string Decrypt(this string value, string password)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception("Decryption value may not be empty.");
            }
            //if (password.Trim().Length < 8)
            //{
            //    throw new Exception("Decryption password must consist of 8 or more characters.");
            //}

            byte[] encryptedBytes = Convert.FromBase64String(value);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider();

            tripleDes.Key = TruncateHash(password, tripleDes.KeySize / 8);
            tripleDes.IV = TruncateHash(password, tripleDes.BlockSize / 8);

            CryptoStream decStream = new CryptoStream(ms, tripleDes.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

            decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
            decStream.FlushFinalBlock();

            return System.Text.Encoding.Unicode.GetString(ms.ToArray());
        }

        /// <summary>
        /// Hashes the given key using secure hashing and then returns the relevant number of characters only (as a byte array)
        /// </summary>
        public static byte[] TruncateHash(this string key, int length)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            Byte[] keyBytes = System.Text.Encoding.Unicode.GetBytes(key);
            Byte[] hash = sha1.ComputeHash(keyBytes);
            Array.Resize(ref hash, length);
            return hash;
        }

        public static string HashString(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new System.Security.SecurityException("Cannot hash an empty string.");
            }

            //'Convert the password string into an Array of bytes.
            byte[] passBytes = Encoding.Unicode.GetBytes(str);

            //'Get the encrypted bytes
            byte[] encBytes = new SHA1CryptoServiceProvider().ComputeHash(passBytes);

            //'Return the encrypted string
            return Convert.ToBase64String(encBytes);
        }

        public static string CreateCode(this ActivationCode activation)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ActivationCodeFormatter formatter = new ActivationCodeFormatter();
                formatter.Serialize(ms, activation);
                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public static ActivationCode ReadCode(this string code)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    sw.Write(code);
                    sw.Flush();
                    ActivationCodeFormatter formatter = new ActivationCodeFormatter();
                    return formatter.Deserialize(ms) as ActivationCode;
                }
            }
        }
    }
}
