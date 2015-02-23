using System.Windows.Interactivity;
using DesignerTool.Styles.Wpf.Controls;

namespace DesignerTool.Styles.Wpf.Behaviours
{
    public class WindowsSettingBehaviour : Behavior<MetroWindow>
    {
        protected override void OnAttached()
        {
            if (AssociatedObject != null && AssociatedObject.SaveWindowPosition) {
                // save with custom settings class or use the default way
                var windowPlacementSettings = this.AssociatedObject.WindowPlacementSettings ?? new WindowApplicationSettings(this.AssociatedObject);
                WindowSettings.SetSave(AssociatedObject, windowPlacementSettings);
            }
        }
    }
}