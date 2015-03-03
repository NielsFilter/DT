using DesignerTool.AppLogic;
using DesignerTool.Common.Enums;
using DesignerTool.Common.Exceptions;
using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.ViewModels;
using DesignerTool.DataAccess.Data;
using DesignerTool.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;

namespace DesignerTool.Pages.Admin
{
    public class UserDetailViewModel : PageViewModel
    {
        private UserRepository rep;

        #region Constructors

        public UserDetailViewModel(IDesignerToolContext ctx)
            : base()
        {
            if (!base.PagePermissions.CanRead)
            {
                throw new AuthenticationException();
            }
            this.rep = new UserRepository(ctx);
        }

        public UserDetailViewModel(IDesignerToolContext ctx, long id)
            : this(ctx)
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

        public bool CanSave
        {
            get { return base.PagePermissions.CanModify; }
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
                    this.Roles = Enum.GetNames(typeof(RoleType));

                    if (this.ID.HasValue)
                    {
                        // Get item
                        this.Model = this.rep.GetById(this.ID.Value);
                        this.Model.IsValidate = true;
                    }
                    else
                    {
                        // New record. Set defaults
                        this.Model = User.New();
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
            this.toggleValidation(true); // Turn on validation.

            base.ShowLoading(() =>
            {
                if (this.Model != null)
                {
                    try
                    {
                        if (!this.ID.HasValue)
                        {
                            // New Insert
                            this.rep.AddNew(this.Model);
                        }
                        this.rep.ValidateAndCommit();

                        // Save successful
                        this.ID = this.Model.UserID;
                        base.ShowSaved();
                    }
                    catch (ModelValidationExceptions mvEx)
                    {
                        base.ShowErrors("Please correct the indicated fields and try again.", mvEx.ValidationExceptions);
                    }
                    catch (Exception ex)
                    {
                        base.ShowError("Save Failed. " + ex.Message);
                    }
                }
            }, "Saving user details");
        }

        private void toggleValidation(bool isValidateEnabled)
        {
            if (this.Model.IsValidate != isValidateEnabled)
            {
                this.Model.IsValidate = isValidateEnabled;
                base.NotifyPropertyChanged("Model"); // Tells the UI to re-evaluate the Validation Conditions.
            }
        }

        #endregion
    }
}
