using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DesignerTool.Common.AttachedProperties
{
    public static class PasswordBoxAttached
    {
        #region PasswordText

        public static readonly DependencyProperty PasswordText =
            DependencyProperty.RegisterAttached("PasswordText", typeof(string), typeof(PasswordBoxAttached), new PropertyMetadata(string.Empty, OnPasswordTextChanged));

        public static string GetPasswordText(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordText);
        }

        public static void SetPasswordText(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordText, value);
        }

        private static void OnPasswordTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox box = d as PasswordBox;

            // only handle this event when the property is attached to a PasswordBox
            // and when the IsPasswordTextBindable attached property has been set to true
            if (d == null || !GetIsPasswordTextBindable(d))
            {
                return;
            }

            // avoid recursive updating by ignoring the box's changed event
            box.PasswordChanged -= HandlePasswordChanged;

            string newPassword = (string)e.NewValue;

            if (!GetUpdatingPassword(box))
            {
                box.Password = newPassword;
            }

            box.PasswordChanged += HandlePasswordChanged;
        }

        #endregion

        #region IsPasswordTextBindable

        public static readonly DependencyProperty IsPasswordTextBindable = DependencyProperty.RegisterAttached(
            "IsPasswordTextBindable", typeof(bool), typeof(PasswordBoxAttached), new PropertyMetadata(false, OnIsPasswordTextBindableChanged));

        public static void SetIsPasswordTextBindable(DependencyObject dp, bool value)
        {
            dp.SetValue(IsPasswordTextBindable, value);
        }

        public static bool GetIsPasswordTextBindable(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsPasswordTextBindable);
        }

        private static void OnIsPasswordTextBindableChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            // when the BindPassword attached property is set on a PasswordBox,
            // start listening to its PasswordChanged event

            PasswordBox box = dp as PasswordBox;

            if (box == null)
            {
                return;
            }

            bool wasBound = (bool)(e.OldValue);
            bool needToBind = (bool)(e.NewValue);

            if (wasBound)
            {
                box.PasswordChanged -= HandlePasswordChanged;
            }

            if (needToBind)
            {
                box.PasswordChanged += HandlePasswordChanged;
            }
        }

        #endregion

        #region UpdatingPassword

        private static readonly DependencyProperty UpdatingPassword =
            DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordBoxAttached), new PropertyMetadata(false));

        private static bool GetUpdatingPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(UpdatingPassword);
        }

        private static void SetUpdatingPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdatingPassword, value);
        }

        #endregion

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;

            // set a flag to indicate that we're updating the password
            SetUpdatingPassword(box, true);
            // push the new password into the BoundPassword property
            SetPasswordText(box, box.Password);
            SetUpdatingPassword(box, false);
        }
    }
}