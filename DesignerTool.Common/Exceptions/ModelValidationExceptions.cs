using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Exceptions
{
    [Serializable]
    public class ModelValidationExceptions : Exception
    {
        #region Ctors

        public ModelValidationExceptions()
        {
        }

        public ModelValidationExceptions(string message) : base(message)
        {
        }

        public ModelValidationExceptions(string message, List<string> validationExceptions) : this(message)
        {
            this.ValidationExceptions = validationExceptions;
        }

        public ModelValidationExceptions(string message, Exception inner) : base(message, inner)
        {
        }

        public ModelValidationExceptions(string message, Exception inner, List<string> validationExceptions)
            : this(message, inner)
        {
            this.ValidationExceptions = validationExceptions;
        }

        protected ModelValidationExceptions(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        public List<string> ValidationExceptions { get; set; }
    }
}
