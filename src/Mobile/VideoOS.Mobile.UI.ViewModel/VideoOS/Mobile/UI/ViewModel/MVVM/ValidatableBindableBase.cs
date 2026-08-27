using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Prism.Mvvm;
using VideoOS.Mobile.UI.Interfaces;

namespace VideoOS.Mobile.UI.ViewModel.MVVM;

public class ValidatableBindableBase : BindableBase, IDataErrorInfo, IValidatable
{
	private readonly Dictionary<string, string> _validationErrors = new Dictionary<string, string>();

	private bool _validate;

	public string this[string columnName]
	{
		get
		{
			string text = null;
			if (_validate)
			{
				text = ValidateProperty(columnName);
				_validationErrors[columnName] = text;
			}
			return text;
		}
	}

	public string Error
	{
		get
		{
			if (AreAllPropertiesValid)
			{
				return null;
			}
			return _validationErrors.Values.FirstOrDefault((string error) => error != null);
		}
	}

	public virtual bool AreAllPropertiesValid => _validationErrors.All((KeyValuePair<string, string> error) => error.Value == null);

	public virtual bool Validate()
	{
		_validate = true;
		((BindableBase)this).OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
		_validate = false;
		return AreAllPropertiesValid;
	}

	protected virtual string ValidateProperty(string columnName)
	{
		return null;
	}
}
