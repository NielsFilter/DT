using DesignerTool.Common.Enums;
using DesignerTool.Common.Mvvm.Commands;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Pages.Admin
{
    public class UserDetailViewModel : PageViewModel
    {
        DesignerDbEntities ctx;

        #region Constructors

        public UserDetailViewModel()
            : base(false)
        {
            ctx = new DesignerDbEntities();
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

        #region Commands

        public Command SaveCommand { get; set; }

        public override void OnWireCommands()
        {
            base.OnWireCommands();

            this.SaveCommand = new Command(save, () => true);
        }

        #endregion

        #region Load

        /// <summary>
        /// All initialization must happen here.
        /// </summary>
        public override void OnLoaded()
        {
            base.ShowLoading(() =>
                {
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
        private void save()
        {
            base.ShowLoading(() =>
            {
                if (this.Model != null)
                {
                    try
                    {
                        if (!this.ID.HasValue)
                        {
                            //// Update
                            //ctx.Users.Attach(ctx.Users.First(u => u.UserID == this.Model.UserID));
                            //ctx.Users.ApplyCurrentValues(this.Model);
                            //}
                            //else
                            //{
                            // New Insert
                            ctx.Users.AddObject(this.Model);
                        }
                        ctx.SaveChanges();

                        // Save successful
                        this.ID = this.Model.UserID;

                        base.ShowSave();
                    }
                    catch (Exception ex)
                    {
                        base.ShowError("Save failed", ex.Message);
                    }
                }
            }, "Saving user details");
        }

        #endregion
    }
}
