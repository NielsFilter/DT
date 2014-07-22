using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using DesignerTool.Common.Mvvm.Commands;

namespace DesignerTool.Common.Mvvm.Triggers
{
    // NF: To use Events in MVVM we need to be able to convert them to Commands. This TriggerAction class does that
    // How to use this class:
    /* The example above will file the 'MyCommand' command with 'MyParameter' as a parameter when the TextChanged event fires
 
             <TextBox Text="{Binding MyText, UpdateSourceTrigger=PropertyChanged}" Margin="20">
                    <i:Interaction.Triggers>
                         <i:EventTrigger EventName="TextChanged">
                             <local:EventToCommand Command="{Binding MyCommand}" CommandParameter="{Binding MyParameter}"/>
                         </i:EventTrigger>
                </i:Interaction.Triggers>
            </TextBox>
     
     */

    public class EventToCommand : TriggerAction<FrameworkElement>
    {
        #region Dependency Properties
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommand), new UIPropertyMetadata(null));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventToCommand), new UIPropertyMetadata(null));

        #endregion

        #region Protected Methods

        protected override void Invoke(object parameter)
        {
            if (Command == null)
            {
                // No command bound
                return;
            }

            if (Command is RoutedCommand)
            {
                // RoutedCommand
                var rc = Command as RoutedCommand;
                if (rc.CanExecute(CommandParameter, base.AssociatedObject))
                {
                    rc.Execute(CommandParameter, base.AssociatedObject);
                }
            }
            else
            {
                // Any other ICommand implementations
                if (Command.CanExecute(CommandParameter))
                {
                    Command.Execute(CommandParameter);
                }
            }
        }

        #endregion
    }
}
