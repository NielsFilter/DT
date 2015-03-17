using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace System.Linq // Keep this namespace System.Linq, as it is an addition to the existing Linq Extensions.
{
    public static class LINQExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return null;
            }
            else if(enumerable.Count() == 0)
            {
                return new ObservableCollection<T>();
            }
            return new ObservableCollection<T>(enumerable);
        }
    }
}
