using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace DesignerTool.Common.Mvvm.Mapping
{
	/// <summary>
	/// Class describing the Window-ViewModel mappings used by the dialog service.
	/// </summary>
	public class WindowViewModelMappings : IWindowViewModelMappings
	{
        public IDictionary<Type, Type> Mappings { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="WindowViewModelMappings"/> class.
		/// </summary>
		public WindowViewModelMappings()
		{
            this.Mappings = new Dictionary<Type, Type>();
		}
        

		/// <summary>
		/// Gets the window type based on registered ViewModel type.
		/// </summary>
		/// <param name="viewModelType">The type of the ViewModel.</param>
		/// <returns>The window type based on registered ViewModel type.</returns>
		public Type GetViewType(Type viewModelType)
		{
			return this.Mappings[viewModelType];
		}
	}
}