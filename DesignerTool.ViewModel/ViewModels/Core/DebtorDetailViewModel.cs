﻿using DesignerTool.AppLogic.ViewModels.Base;
using DesignerTool.Common.Exceptions;
using DesignerTool.DataAccess.Data;
using DesignerTool.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;

namespace DesignerTool.AppLogic.ViewModels.Core
{
    public class DebtorDetailViewModel : PageViewModel
    {
        private DebtorRepository rep;

        #region Constructors

        public DebtorDetailViewModel(IDesignerToolContext ctx)
            : base()
        {
            if (!base.PagePermissions.CanRead)
            {
                throw new AuthenticationException();
            }
            this.rep = new DebtorRepository(ctx);
        }

        public DebtorDetailViewModel(IDesignerToolContext ctx, long id)
            : this(ctx)
        {
            this.ID = id;
        }

        #endregion

        #region Properties

        public override string Heading
        {
            get { return "Debtor Detail"; }
        }

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

        private Debtor _model;
        public Debtor Model
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

        public bool CanSave
        {
            get { return base.PagePermissions.CanModify; }
        }

        public override bool CanGoBack
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Load, Refresh, Back

        /// <summary>
        /// All initialization must happen here.
        /// </summary>
        public override void Load()
        {
            base.ShowLoading(() =>
                {
                    if (this.ID.HasValue)
                    {
                        // Get item
                        this.Model = this.rep.GetById(this.ID.Value);
                        this.Model.IsValidate = true;
                    }
                    else
                    {
                        // New record. Set defaults
                        this.Model = Debtor.New();
                    }
                }, "Loading debtor details...");
        }

        public override void GoBack()
        {
            // Go back to list.
            AppSession.Current.Navigate(new DebtorListViewModel(this.rep.Context));
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
                        this.ID = this.Model.DebtorID;
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
            }, "Saving debtor...");
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
