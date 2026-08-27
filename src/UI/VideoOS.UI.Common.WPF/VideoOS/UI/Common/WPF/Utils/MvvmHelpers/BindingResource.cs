using System.Windows;
using System.Windows.Data;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class BindingResource : FrameworkElement
{
	private FrameworkElement _bindingTarget;

	private DependencyProperty _bindingProperty;

	public static readonly DependencyProperty ResourceKeyProperty = DependencyProperty.Register("ResourceKey", typeof(string), typeof(BindingResource), new FrameworkPropertyMetadata(ResourceKeyChangedCallback));

	public string ResourceKey
	{
		get
		{
			return (string)GetValue(ResourceKeyProperty);
		}
		set
		{
			SetValue(ResourceKeyProperty, value);
		}
	}

	private static void ResourceKeyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		(d as BindingResource).OnResourceKeyChanged(e);
	}

	protected virtual void OnResourceKeyChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue != null)
		{
			_bindingTarget.SetResourceReference(_bindingProperty, e.NewValue);
		}
		else
		{
			_bindingTarget.ClearValue(_bindingProperty);
		}
	}

	protected BindingResource(BindingBase resourceKeyBinding, FrameworkElement bindingTarget, DependencyProperty bindingProperty)
	{
		_bindingTarget = bindingTarget;
		_bindingProperty = bindingProperty;
		NameScope.SetNameScope(this, NameScope.GetNameScope(_bindingTarget));
		Binding binding = new Binding
		{
			Source = _bindingTarget,
			Path = new PropertyPath(FrameworkElement.DataContextProperty)
		};
		SetBinding(FrameworkElement.DataContextProperty, binding);
		if (resourceKeyBinding is Binding { RelativeSource: not null } binding2 && binding2.RelativeSource.Mode == RelativeSourceMode.Self)
		{
			resourceKeyBinding = new Binding
			{
				Source = _bindingTarget,
				Path = binding2.Path,
				Mode = binding2.Mode,
				StringFormat = binding2.StringFormat,
				TargetNullValue = binding2.TargetNullValue,
				BindingGroupName = binding2.BindingGroupName,
				BindsDirectlyToSource = binding2.BindsDirectlyToSource,
				Converter = binding2.Converter,
				ConverterCulture = binding2.ConverterCulture,
				ConverterParameter = binding2.ConverterParameter,
				FallbackValue = binding2.FallbackValue,
				IsAsync = binding2.IsAsync,
				NotifyOnSourceUpdated = binding2.NotifyOnSourceUpdated,
				NotifyOnTargetUpdated = binding2.NotifyOnTargetUpdated,
				NotifyOnValidationError = binding2.NotifyOnValidationError,
				UpdateSourceExceptionFilter = binding2.UpdateSourceExceptionFilter,
				UpdateSourceTrigger = binding2.UpdateSourceTrigger,
				ValidatesOnDataErrors = binding2.ValidatesOnDataErrors,
				ValidatesOnExceptions = binding2.ValidatesOnExceptions
			};
		}
		BindingOperations.SetBinding(this, ResourceKeyProperty, resourceKeyBinding);
	}

	public static BindingResource SetResourceBinding(BindingBase resourceKeyBinding, FrameworkElement bindingTarget, DependencyProperty bindingProperty)
	{
		return new BindingResource(resourceKeyBinding, bindingTarget, bindingProperty);
	}
}
