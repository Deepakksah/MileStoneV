using System;
using System.Collections.Generic;
using System.IO;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Http.FileSystem;

public class DynamicDriveFolder : IFolder, IResource, IDisposable
{
	private string _drivePath;

	public IResource this[string name]
	{
		get
		{
			try
			{
				foreach (string item in Directory.EnumerateDirectories(_drivePath))
				{
					string text = item.Substring(item.LastIndexOf("\\") + 1);
					if (text == name)
					{
						return new DynamicDriveFolder(text, item);
					}
				}
				foreach (string item2 in Directory.EnumerateFiles(_drivePath))
				{
					string text2 = item2.Substring(item2.LastIndexOf("\\") + 1);
					if (text2 == name)
					{
						return new DriveFile(text2, item2);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("DynamicDriveFolder.this[]", ex.Message);
			}
			return null;
		}
	}

	public IEnumerable<IResource> Items
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public IFile Index
	{
		get
		{
			foreach (string item in Directory.EnumerateFiles(_drivePath))
			{
				string text = item.Substring(item.LastIndexOf("\\") + 1);
				if (string.Compare(text, "index.htm", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "index.html", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "default.htm", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "default.html", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return new DriveFile(text, item);
				}
			}
			return null;
		}
	}

	public Types Type => Types.Folder;

	public string Name { get; private set; }

	public DynamicDriveFolder(string name, string path)
	{
		SetNamePath(name, path);
	}

	public void SetNamePath(string name, string path)
	{
		Name = name;
		_drivePath = path;
	}

	public void AddItem(IResource item)
	{
		throw new NotImplementedException();
	}

	public void RemoveItem(IResource item)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<IResource> GetItems(Types types)
	{
		throw new NotImplementedException();
	}

	public void Dispose()
	{
	}
}
