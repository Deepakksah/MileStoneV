using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

[MarkupExtensionReturnType(typeof(object))]
public class BindingResourceExtension : MarkupExtension
{
	[ConstructorArgument("resourcekeybinding")]
	public BindingBase ResourceKeyBinding { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		if (ResourceKeyBinding == null)
		{
			throw new InvalidOperationException("ResourceKeyBinding must be specified.");
		}
		if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget)
		{
			if (provideValueTarget.TargetObject.GetType().FullName == "System.Windows.SharedDp")
			{
				return this;
			}
			FrameworkElement frameworkElement = provideValueTarget.TargetObject as FrameworkElement;
			if (!(provideValueTarget.TargetProperty is DependencyProperty bindingProperty) || frameworkElement == null)
			{
				throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "The property '{0}' on target '{1}' is not valid for a BindingResource. The BindingResource target must be a FrameworkElement, and the target property must be a DependencyProperty.", provideValueTarget.TargetProperty, provideValueTarget.TargetObject));
			}
			BindingResource.SetResourceBinding(ResourceKeyBinding, frameworkElement, bindingProperty);
		}
		return null;
	}
}
