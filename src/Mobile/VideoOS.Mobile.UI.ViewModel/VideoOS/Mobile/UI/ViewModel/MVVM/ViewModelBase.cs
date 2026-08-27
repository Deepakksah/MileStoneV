using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using VideoOS.Mobile.UI.ViewModel.Extensions;
using VideoOS.Mobile.UI.ViewModel.Interfaces;

namespace VideoOS.Mobile.UI.ViewModel.MVVM;

public abstract class ViewModelBase : BindableBase, IViewModelBase, INotifyPropertyChanged
{
	private uint m_IsBusy;

	public bool IsBusy
	{
		get
		{
			return m_IsBusy != 0;
		}
		set
		{
			if (value)
			{
				m_IsBusy++;
			}
			else if (m_IsBusy != 0)
			{
				m_IsBusy--;
			}
			((BindableBase)this).OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
			((BindableBase)this).OnPropertyChanged(new PropertyChangedEventArgs("IsIdle"));
			RaiseCanExecuteAll();
		}
	}

	public bool IsIdle => !IsBusy;

	public virtual void OnPropertyChangedAll()
	{
		PropertyInfo[] allProperties = ((object)this).GetType().GetAllProperties();
		foreach (PropertyInfo propertyInfo in allProperties)
		{
			((BindableBase)this).OnPropertyChanged(new PropertyChangedEventArgs(propertyInfo.Name));
		}
	}

	public virtual void RaiseCanExecute(ICommand command)
	{
		Action action = delegate
		{
			ICommand command2 = command;
			DelegateCommandBase val = (DelegateCommandBase)((command2 is DelegateCommandBase) ? command2 : null);
			if (val != null)
			{
				val.RaiseCanExecuteChanged();
			}
		};
		if (Application.Current != null && Application.Current.Dispatcher != null)
		{
			Application.Current.Dispatcher.Invoke(action);
		}
		else
		{
			action();
		}
	}

	public virtual void RaiseCanExecuteAll()
	{
		PropertyInfo[] allProperties = ((object)this).GetType().GetAllProperties();
		if (allProperties == null)
		{
			return;
		}
		PropertyInfo[] array = allProperties;
		foreach (PropertyInfo propertyInfo in array)
		{
			if (propertyInfo.PropertyType.IsFromType(typeof(ICommand)))
			{
				ICommand command = (ICommand)propertyInfo.GetValue(this);
				RaiseCanExecute(command);
			}
		}
	}

	protected virtual Task Load()
	{
		return null;
	}
}
