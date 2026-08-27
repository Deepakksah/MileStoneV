using System;
using System.Windows;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class LabeledValueCollection : FreezableCollection<LabeledValue>
{
	public static LabeledValueCollection FromEnum(Type enumType)
	{
		LabeledValueCollection labeledValueCollection = new LabeledValueCollection();
		foreach (object value in Enum.GetValues(enumType))
		{
			labeledValueCollection.Add(new LabeledValue(Enum.GetName(enumType, value), value));
		}
		return labeledValueCollection;
	}
}
