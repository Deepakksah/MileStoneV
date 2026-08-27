using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public static class FreezableCollectionExtension
{
	public static void AddRange<T>(this FreezableCollection<T> self, IEnumerable<T> range) where T : DependencyObject
	{
		range.ForEach(delegate(T value)
		{
			self.Add(value);
		});
	}
}
