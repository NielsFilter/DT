using DesignerTool.Common.Base;
using DesignerTool.DataAccess.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Data
{
    public abstract class BaseModel : NotifyPropertyChangedBase, IValidatable
    {
        #region Validation

        private bool _isValidate;
        public bool IsValidate
        {
            get
            {
                return this._isValidate;
            }
            set
            {
                if (value != this._isValidate)
                {
                    this._isValidate = value;
                    base.NotifyPropertyChanged("IsValidate");
                }
            }
        }

        public virtual string Validation(string columnName)
        {
            return String.Empty;
        }

        public List<string> ValidateAll()
        {
            List<string> validationErrors = new List<string>();
            if (this.IsValidate)
            {
                foreach (var prop in this.GetType().GetProperties())
                {
                    string valResult = this[prop.Name];
                    if (!string.IsNullOrEmpty(valResult))
                    {
                        validationErrors.Add(valResult);
                    }
                }
            }
            return validationErrors;
        }

        #region IDataErrorInfo

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                if (!this.IsValidate)
                {
                    return String.Empty;
                }

                return Validation(columnName);
            }
        }

        #endregion

        #endregion
    }
}
