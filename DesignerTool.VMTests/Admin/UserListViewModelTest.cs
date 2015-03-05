using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesignerTool.Pages.Admin;
using DesignerTool.Common.Enums;
using DesignerTool.DataAccess.Data;
using System.Security.Authentication;

namespace DesignerTool.VMTests.Admin
{
    [TestClass]
    public class UserListViewModelTest : VMTestBase
    {
        private UserListViewModel vm;

        public UserListViewModelTest()
            : base()
        {
            TestSession.Current.LoggedInUser = this.createAdminUser();
            vm = new UserListViewModel(base.Context);
            TestSession.Current.CurrentViewModel = vm;
        }

        #region Add

        /// <summary>
        /// Test permissions as normal user.
        /// </summary>
        [TestMethod]
        public void AddNew_PermissionUser()
        {
            addNew_Permission(this.createNormalUser());
        }

        [TestMethod]
        public void AddNew_PermissionAdmin()
        {
            addNew_Permission(this.createAdminUser());
        }

        private void addNew_Permission(User user)
        {
            TestSession.Current.LoggedInUser = user;
            vm.SelectedItem = this.createNormalUser();

            try
            {
                vm.AddNew();
            }
            catch (AuthenticationException)
            {
                Assert.IsFalse(vm.PagePermissions.CanModify);
                return;
            }

            Assert.IsTrue(vm.PagePermissions.CanModify);
            Assert.IsNotNull(TestSession.Current.CurrentViewModel.GetType() == typeof(UserDetailViewModel));
        }

        #endregion

        #region Edit

        #endregion

        #region Delete

        /// <summary>
        /// Check that there's a delete confirmation before deleting
        /// </summary>
        [TestMethod]
        public void DeleteConfirmation()
        {
            vm.SelectedItem = this.createNormalUser();
            vm.Delete();
            Assert.AreEqual(UserMessageButtons.YesNo, TestSession.Current.ShowMessage_Button);
        }

        /// <summary>
        /// Test delete if user said "Yes" to delete prompt.
        /// </summary>
        [TestMethod]
        public void DeleteYes()
        {
            vm.SelectedItem = this.createNormalUser();
            vm.Delete();
            TestSession.Current.ShowMessage_UserResponse = () => UserMessageResults.Yes; // Simulate "Yes" click.
            vm.Delete();

            // Make sure that successfully deleted is shown.
            Assert.AreEqual(UserMessageType.Success, vm.NotificationType);
            Assert.AreEqual(true, vm.IsShowNotification);
        }

        /// <summary>
        /// Test delete if user said "No" to delete prompt.
        /// </summary>
        [TestMethod]
        public void DeleteNo()
        {
            vm.SelectedItem = this.createNormalUser();
            vm.Delete();
            TestSession.Current.ShowMessage_UserResponse = () => UserMessageResults.No; // Simulate "No" click.

            vm.Delete();
            //TODO: Check here if user is still there.
            Assert.AreEqual(false, vm.IsShowNotification);
        }

        #endregion

        private User createNormalUser()
        {
            var random = new Random();
            User user = User.New();
            user.Username = "NormalUser" + random.Next(1, 1000000).ToString();
            user.SetPassword("Password" + random.Next(1, 1000000).ToString());

            base.Context.Users.Add(user);
            base.Context.SaveChanges();
            return user;
        }

        private User createAdminUser()
        {
            var random = new Random();
            User user = User.New();
            user.Username = "AdminUser" + random.Next(1, 1000000).ToString();
            user.SetPassword("Password" + random.Next(1, 1000000).ToString());
            user.Role = RoleType.Admin.ToString();


            base.Context.Users.Add(user);
            base.Context.SaveChanges();
            return user;
        }

    }
}
