using System;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class EnsureValidSelectionBehaviorExtension : MarkupExtension
{
	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return new EnsureValidSelectionBehavior();
	}
}
