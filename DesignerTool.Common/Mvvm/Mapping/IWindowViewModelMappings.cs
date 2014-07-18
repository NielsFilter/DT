using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Windows;

namespace DesignerTool.Common.Mvvm.Mapping
{
	/// <summary>
	/// Interface describing the Window-ViewModel mappings used by the dialog service.
	/// </summary>
	public interface IWindowViewModelMappings
	{
        /// <summary>
        /// Gets or sets the collection of mapped views and viewModels
        /// </summary>
        IDictionary<Type, Type> Mappings { get; set; }

		/// <summary>
		/// Gets the window type based on registered ViewModel type.
		/// </summary>
		/// <param name="viewModelType">The type of the ViewModel.</param>
		/// <returns>The window type based on registered ViewModel type.</returns>
		Type GetViewType(Type viewModelType);
	}
}
