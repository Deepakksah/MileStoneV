using System;
using System.IO;
using Microsoft.Deployment.WindowsInstaller;

namespace VideoOS.Installer.Common.Utilities;

public class InstallerSession : ISession
{
	private Session _session;

	public string this[string property]
	{
		get
		{
			return _session[property];
		}
		set
		{
			_session[property] = value;
		}
	}

	public InstallerSession(Session session)
	{
		_session = session;
	}

	public void SetMode(InstallRunMode mode, bool value)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_session.SetMode(mode, value);
	}

	public bool GetMode(InstallRunMode mode)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return _session.GetMode(mode);
	}

	public void SetPersistedProperty(string name, string value)
	{
		string filePathFromName = GetFilePathFromName(name);
		try
		{
			File.WriteAllText(filePathFromName, value);
		}
		catch (IOException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
	}

	public string GetPersistedProperty(string name)
	{
		string filePathFromName = GetFilePathFromName(name);
		try
		{
			if (File.Exists(filePathFromName))
			{
				return File.ReadAllText(filePathFromName);
			}
		}
		catch (IOException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		return null;
	}

	public void RemovePersistedProperty(string name)
	{
		string filePathFromName = GetFilePathFromName(name);
		if (File.Exists(filePathFromName))
		{
			try
			{
				File.Delete(filePathFromName);
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}
	}

	private static string GetFilePathFromName(string name)
	{
		char[] invalidPathChars = Path.GetInvalidPathChars();
		foreach (char oldChar in invalidPathChars)
		{
			name = name.Replace(oldChar, '_');
		}
		return Path.Combine(Path.GetTempPath(), name);
	}

	public void Log(string msg)
	{
		_session.Log(msg);
	}

	public void Log(string format, params object[] args)
	{
		_session.Log(format, args);
	}
}
