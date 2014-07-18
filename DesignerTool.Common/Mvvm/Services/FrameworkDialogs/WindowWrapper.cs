using System;
using System.Windows;
using System.Windows.Forms;

namespace DesignerTool.Common.Mvvm.Services
{
	/// <summary>
	/// WindowWrapper is an IWin32Window wrapper around a WPF window.
	/// </summary>
	class WindowWrapper : IWin32Window
	{
		/// <summary>
		/// Construct a new wrapper taking a WPF window.
		/// </summary>
		/// <param name="window">The WPF window to wrap.</param>
		public WindowWrapper(Window window)
		{
			Handle = new System.Windows.Interop.WindowInteropHelper(window).Handle;
		}


		/// <summary>
		/// Gets the handle to the window represented by the implementer.
		/// </summary>
		/// <returns>A handle to the window represented by the implementer.</returns>
		public IntPtr Handle { get; private set; }
	}
}
