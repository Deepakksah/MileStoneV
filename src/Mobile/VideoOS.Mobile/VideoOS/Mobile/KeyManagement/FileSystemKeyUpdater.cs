using System;
using System.IO;
using System.Reflection;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.KeyManagement;

public class FileSystemKeyUpdater : IKeyUpdater
{
	private bool _inProgress;

	private readonly string _keyName;

	private string _folder;

	public string FolderPath
	{
		get
		{
			return _folder;
		}
		set
		{
			_folder = value;
		}
	}

	public FileSystemKeyUpdater(string keyName, string folder)
	{
		_keyName = keyName;
		_folder = folder;
	}

	public void PasswordChanged(string newPassword, string oldPassword)
	{
		if (_inProgress)
		{
			return;
		}
		_inProgress = true;
		foreach (string item in Directory.EnumerateFiles(_folder, _keyName, SearchOption.AllDirectories))
		{
			try
			{
				KeyProvider.UpdateKey(newPassword, oldPassword, item);
			}
			catch (Exception exception)
			{
				Log.Debug(MethodBase.GetCurrentMethod().Name, "Failed to change key: " + item);
				Log.Exception(exception);
			}
		}
		_inProgress = false;
	}
}
