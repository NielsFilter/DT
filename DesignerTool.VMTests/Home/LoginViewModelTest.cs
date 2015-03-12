using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesignerTool.Pages.Shell;
using DesignerTool.DataAccess.Data;

namespace DesignerTool.VMTests
{
    [TestClass]
    public class LoginViewModelTest : VMTestBase
    {
        private LoginViewModel vm;

        public LoginViewModelTest()
            : base()
        {
            vm = new LoginViewModel(base.Context);
            TestSession.Current.CurrentViewModel = vm;
        }

        #region Login Tests

        /// <summary>
        /// Valid login credentials test.
        /// </summary>
        [TestMethod]
        public void LoginValid()
        {
            vm.Username = "TestUser";
            vm.Password = "password";
            vm.Login();

            Assert.IsNotNull(TestSession.Current.CurrentViewModel);
            Assert.IsNotNull(TestSession.Current.CurrentViewModel.GetType() == typeof(HomeViewModel));
        }

        /// <summary>
        /// Invalid username
        /// </summary>
        [TestMethod]
        public void LoginInvalid1()
        {
            vm.Username = "UserThatDoesntExist";
            vm.Password = "password";
            vm.Login();

            // Stay on Login Page
            Assert.AreEqual(vm, TestSession.Current.CurrentViewModel);

            // Error notifications shown
            Assert.AreEqual(Common.Enums.ResultType.Error, vm.NotificationType);
            Assert.AreEqual(true, vm.IsShowNotification);
        }

        /// <summary>
        /// Invalid password test.
        /// </summary>
        [TestMethod]
        public void LoginInvalid2()
        {
            vm.Username = "TestUser";
            vm.Password = "INVALIDPassword";
            vm.Login();

            // Stay on Login Page
            Assert.AreEqual(vm, TestSession.Current.CurrentViewModel);

            // Error notifications shown
            Assert.AreEqual(Common.Enums.ResultType.Error, vm.NotificationType);
            Assert.AreEqual(true, vm.IsShowNotification);
        }

        /// <summary>
        /// Invalid password, case-sensitive test.
        /// </summary>
        [TestMethod]
        public void LoginInvalid3()
        {
            vm.Username = "TestUser";
            vm.Password = "passWoRd";
            vm.Login();

            // Stay on Login Page
            Assert.AreEqual(vm, TestSession.Current.CurrentViewModel);

            // Error notifications shown
            Assert.AreEqual(Common.Enums.ResultType.Error, vm.NotificationType);
            Assert.AreEqual(true, vm.IsShowNotification);
        }

        #endregion
    }
}
