using System;
using System.IO;
using System.Runtime.InteropServices;
using IWshRuntimeLibrary;

namespace VideoOS.Installer.Common.Utilities;

public class ShortcutManager
{
	private string _path;

	public ShortcutManager(string path)
	{
		_path = path;
	}

	public void Create(string executable, string arguments, string icon)
	{
		try
		{
			WshShell wshShell = (WshShell)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
			IWshShortcut obj = (IWshShortcut)(dynamic)wshShell.CreateShortcut(_path);
			obj.TargetPath = executable;
			obj.Arguments = arguments;
			obj.IconLocation = icon;
			obj.WorkingDirectory = Path.GetDirectoryName(executable);
			obj.Save();
		}
		catch (Exception innerException)
		{
			throw new ShortcutException("Error occurs during creating shortcut. See details:", innerException);
		}
	}

	public void Remove()
	{
		try
		{
			if (File.Exists(_path))
			{
				File.Delete(_path);
			}
		}
		catch (IOException innerException)
		{
			throw new ShortcutException("Cannot delete shortcut: " + _path + "See details:", innerException);
		}
		catch (UnauthorizedAccessException innerException2)
		{
			throw new ShortcutException("Cannot delete shortcut: " + _path + "See details:", innerException2);
		}
	}
}
