using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace System.Linq
{
    public static class LINQExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return null;
            }
            return new ObservableCollection<T>(enumerable);
        }
    }
}
