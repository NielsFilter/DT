﻿namespace DesignerTool.Common.Mvvm.Services
{
	/// <summary>
	/// ViewModel of the OpenFileDialog.
	/// </summary>
	public class OpenFileDialogViewModel : FileDialogViewModel, IOpenFileDialog
	{
		/// <summary>
		/// Gets or sets a value indicating whether the dialog box allows multiple files to be selected.
		/// </summary>
		public bool Multiselect { get; set; }
	}
}
