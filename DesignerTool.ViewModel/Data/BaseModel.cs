using DesignerTool.Common.Data;
using DesignerTool.Common.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic.Data
{
    public class BaseModel : NotifyPropertyChangedBase, IValidatable
    {
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

        public void Validate()
        {
            //foreach(var prop in this.GetType().GetProperties())
            //{
            //    this[]
            //}
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
                //TODO:
                //if (!this.IsValidate)
                //{
                //    return String.Empty;
                //}

                return Validation(columnName);
            }
        }

        #endregion
    }
}
