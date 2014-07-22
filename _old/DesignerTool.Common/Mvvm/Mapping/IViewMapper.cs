using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Mvvm.Mapping
{
    public interface IViewMapper
    {
        IDictionary<Type, Type> MappedViews { get; }
    }
}
