using DesignerTool.AppLogic;
using DesignerTool.AppLogic.Data;
using DesignerTool.Common.Enums;
using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Pages.Admin
{
    public class UserDetailViewModel : PageViewModel
    {
        DesignerToolDbEntities ctx;

        #region Constructors

        public UserDetailViewModel()
            : base()
        {
            ctx = new DesignerToolDbEntities();
        }

        public UserDetailViewModel(long id)
            : this()
        {
            this.ID = id;
        }

        #endregion

        #region Properties

        private long? _iD;
        public long? ID
        {
            get
            {
                return this._iD;
            }
            set
            {
                if (value != this._iD)
                {
                    this._iD = value;
                    base.NotifyPropertyChanged("ID");
                }
            }
        }

        private User _model;
        public User Model
        {
            get { return this._model; }
            set
            {
                if (value != this._model)
                {
                    this._model = value;
                    base.NotifyPropertyChanged("Model");
                }
            }
        }

        private IEnumerable<string> _roles;
        public IEnumerable<string> Roles
        {
            get { return this._roles; }
            set
            {
                if (value != this._roles)
                {
                    this._roles = value;
                    base.NotifyPropertyChanged("Roles");
                }
            }
        }

        #endregion

        #region Load & Refresh

        /// <summary>
        /// All initialization must happen here.
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();

            base.ShowLoading(() =>
                {
                    //TODO:
                    this.Roles = Enum.GetNames(typeof(RoleType));

                    if (this.ID.HasValue)
                    {
                        // Get item
                        this.Model = ctx.Users.FirstOrDefault(u => u.UserID == this.ID.Value);
                    }
                    else
                    {
                        // New record. Set defaults
                        this.Model = new User();
                        this.Model.Role = RoleType.User.ToString();
                    }
                }, "Retrieving user details...");
        }

        #endregion

        #region Save

        /// <summary>
        /// Save the model to the database
        /// </summary>
        public void Save()
        {
            base.ShowLoading(() =>
            {
                if (this.Model != null)
                {
                    try
                    {
                        if (!this.ID.HasValue)
                        {
                            // New Insert
                            ctx.Users.Add(this.Model);
                        }
                        ctx.SaveChanges();

                        // Save successful
                        this.ID = this.Model.UserID;

                        //TODO: Implement better "saved succesful"
                        SessionContext.Current.ShowMessage("Saved successful!", "Saved successful");
                    }
                    catch (Exception ex)
                    {
                        SessionContext.Current.ShowError("Save Failed", ex.Message);
                        //TODO: base.ShowError("Save failed", ex.Message);
                    }
                }
            }, "Saving user details");
        }

        #endregion
    }
}
