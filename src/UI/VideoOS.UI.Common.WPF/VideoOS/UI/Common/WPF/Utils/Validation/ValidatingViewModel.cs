using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.UI.Common.WPF.Utils.Validation;

public abstract class ValidatingViewModel : ViewModelBase
{
	public event EventHandler<ValidationEventArgs> ValidationRequested = delegate
	{
	};

	protected new bool SetProperty<TProperty>(ref TProperty storageField, TProperty value, [CallerMemberName] string propertyName = "")
	{
		if (object.Equals(storageField, value))
		{
			return false;
		}
		ValidationEventArgs<TProperty> e = new ValidationEventArgs<TProperty>(storageField, value, propertyName);
		this.ValidationRequested(this, e);
		bool num = base.SetProperty(ref storageField, e.EffectiveValue, propertyName);
		if (!num)
		{
			Task.Factory.RunOnCurrentSynchronizationContext(delegate
			{
				OnPropertyChanged(propertyName);
			});
		}
		return num;
	}
}
