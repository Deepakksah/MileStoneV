using System;
using System.IO;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Http.FileSystem;

public class DriveFolder : BasicFolder
{
	private readonly string _drivePath;

	public DriveFolder(string name, string path)
		: base(name)
	{
		_drivePath = path;
		EnumerateFolders();
		EnumerateFiles();
	}

	private void EnumerateFolders()
	{
		try
		{
			foreach (string item2 in Directory.EnumerateDirectories(_drivePath))
			{
				DriveFolder item = new DriveFolder(item2.Substring(item2.LastIndexOf("\\") + 1), item2);
				AddItem(item);
			}
		}
		catch (Exception ex)
		{
			Log.Error("DriveFolder.EnumerateFolders", ex.Message);
		}
	}

	private void EnumerateFiles()
	{
		try
		{
			foreach (string item in Directory.EnumerateFiles(_drivePath))
			{
				string text = item.Substring(item.LastIndexOf("\\") + 1);
				DriveFile driveFile = new DriveFile(text, item);
				AddItem(driveFile);
				if (base.Index == null && (string.Compare(text, "index.htm", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "index.html", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "default.htm", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "default.html", StringComparison.OrdinalIgnoreCase) == 0))
				{
					base.Index = driveFile;
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("DriveFolder.EnumerateFiles", ex.Message);
		}
	}
}
