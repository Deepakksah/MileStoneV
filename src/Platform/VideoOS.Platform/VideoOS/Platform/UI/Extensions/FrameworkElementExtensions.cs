using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Xaml;

namespace VideoOS.Platform.UI.Extensions;

public static class FrameworkElementExtensions
{
	private static readonly Lazy<MethodInfo> GetTemplateChildMethodInfo = new Lazy<MethodInfo>(() => typeof(FrameworkElement).GetMethod("GetTemplateChild", BindingFlags.Instance | BindingFlags.NonPublic));

	public static TChild GetTemplateChild<TChild>(this FrameworkElement frameworkElement, string childName) where TChild : DependencyObject
	{
		if (!(GetTemplateChildMethodInfo.Value.Invoke(frameworkElement, new object[1] { childName }) is TChild result))
		{
			throw new XamlException("Child with name='" + childName + "' and Type='" + typeof(TChild).Name + "' not found in template for '" + frameworkElement.GetType().Name + "'.");
		}
		return result;
	}

	public static TDescendant GetDescendant<TDescendant>(this FrameworkElement frameworkElement, string descendantName) where TDescendant : FrameworkElement
	{
		TDescendant val = frameworkElement.GetDescendants<TDescendant>().FirstOrDefault((TDescendant d) => d.Name == descendantName);
		if (val == null)
		{
			throw new XamlException("Descendant with name='" + descendantName + "' and Type='" + typeof(TDescendant).Name + "' not found as descendant of '" + frameworkElement.GetType().Name + "'.");
		}
		return val;
	}
}
