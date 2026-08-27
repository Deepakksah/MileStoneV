using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VideoOS.Mobile.CommonUtilities;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Utilities.Database.KeyManagement.PasswordProvider;

namespace VideoOS.Mobile.KeyManagement;

public class PasswordManager : IPasswordManager, IControl
{
	private const string PasswordFolder = "KeyManagement";

	private const string OldPasswordFolder = "KeyManagement_old";

	private const string PasswordFile = "C85D2CE0-2C7D-4F1B-B479-1CD0E12D20DF.txt";

	private const string OldPasswordFile = "F8E1C6BA-64C8-4861-995E-7E9E9C67A7D1.txt";

	private readonly IPasswordProvider _passwordProvider;

	private readonly IPasswordProvider _oldPasswordProvider;

	private readonly List<IPasswordChangeSubscriber> _subscribers = new List<IPasswordChangeSubscriber>();

	private readonly object _lockObject = new object();

	public static IPasswordManager Instance = new PasswordManager();

	private bool _changeInProgress;

	private PasswordManager()
	{
		string folderName = Path.Combine(OemResources.Instance.CompanyNameShort, OemResources.Instance.MobileServerApplicationNameShort, "KeyManagement");
		string folderName2 = Path.Combine(OemResources.Instance.CompanyNameShort, OemResources.Instance.MobileServerApplicationNameShort, "KeyManagement_old");
		_passwordProvider = new FilePasswordProvider(folderName, "C85D2CE0-2C7D-4F1B-B479-1CD0E12D20DF.txt");
		_oldPasswordProvider = new FilePasswordProvider(folderName2, "F8E1C6BA-64C8-4861-995E-7E9E9C67A7D1.txt", userAccountReadOnly: false);
	}

	public bool HasPassword()
	{
		return _passwordProvider.HasPassword();
	}

	public string ReadPassword()
	{
		return _passwordProvider.ReadPassword();
	}

	public void SetPassword(string password, string accountName)
	{
		_passwordProvider.WritePassword(password, accountName);
		if (_oldPasswordProvider.HasPassword())
		{
			_oldPasswordProvider.DeletePassword();
		}
	}

	public bool HasPendingPasswordChange()
	{
		return _oldPasswordProvider.HasPassword();
	}

	public string ReadOldPassword()
	{
		return _oldPasswordProvider.ReadPassword();
	}

	public PasswordChangeStatus ChangePassword(string password, string accountName)
	{
		if (HasPendingPasswordChange())
		{
			return PasswordChangeStatus.Ignored;
		}
		if (HasPassword())
		{
			_oldPasswordProvider.WritePassword(ReadPassword(), accountName);
		}
		_passwordProvider.WritePassword(password, accountName);
		return PasswordChangeStatus.Changed;
	}

	public void DeletePassword()
	{
		if (_passwordProvider.HasPassword())
		{
			_passwordProvider.DeletePassword();
		}
		if (_oldPasswordProvider.HasPassword())
		{
			_oldPasswordProvider.DeletePassword();
		}
	}

	public void RegisterForPasswordChange(IPasswordChangeSubscriber subscriber)
	{
		lock (_lockObject)
		{
			_subscribers.Add(subscriber);
		}
	}

	public void UnRegisterForPasswordChange(IPasswordChangeSubscriber subscriber)
	{
		lock (_lockObject)
		{
			_subscribers.Remove(subscriber);
		}
	}

	private void OnPasswordChanging()
	{
		try
		{
			IEnumerable<IPasswordChangeSubscriber> items;
			lock (_lockObject)
			{
				items = new List<IPasswordChangeSubscriber>(_subscribers);
			}
			List<Task> tasks = new List<Task>();
			items.Foreach(delegate(IPasswordChangeSubscriber s)
			{
				tasks.Add(Task.Factory.StartNew(delegate
				{
					try
					{
						s.PasswordChanged(_passwordProvider.ReadPassword(), _oldPasswordProvider.ReadPassword());
					}
					catch (Exception exception2)
					{
						Log.Exception(exception2);
					}
				}));
			});
			Task.WaitAll(tasks.ToArray());
			_oldPasswordProvider.DeletePassword();
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		_changeInProgress = false;
	}

	public void Start()
	{
		if (HasPendingPasswordChange() && !_changeInProgress)
		{
			_changeInProgress = true;
			Task.Factory.StartNew(OnPasswordChanging);
		}
	}

	public void Stop()
	{
	}
}
