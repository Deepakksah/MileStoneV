using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace VideoOS.Mobile.Helpers;

public static class DirectoryHelper
{
	public const string InvalidPathCharsRegex = "[\\\\/:*?\"<>|]";

	public static string BuildCommonApplicationDataFilePath(string pathAddition, string applicationName, string vendorShortName, bool createDir = true)
	{
		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), vendorShortName);
		path = Path.Combine(path, applicationName);
		if (!string.IsNullOrEmpty(pathAddition))
		{
			path = Path.Combine(path, pathAddition);
		}
		if (createDir)
		{
			try
			{
				Directory.CreateDirectory(path);
			}
			catch (Exception)
			{
				path = "";
			}
		}
		return path;
	}

	public static long GetAllFilesSize(string path)
	{
		long num = 0L;
		try
		{
			foreach (string item in Directory.EnumerateDirectories(path))
			{
				num += GetAllFilesSize(item);
			}
			foreach (string item2 in Directory.EnumerateFiles(path))
			{
				num += new FileInfo(item2).Length;
			}
		}
		catch (Exception)
		{
		}
		return num;
	}

	public static long MegaBytesToBytes(long megaBytes)
	{
		return megaBytes * 1024 * 1024;
	}

	public static void CopyDirectory(string sourcePath, string destPath)
	{
		if (!Directory.Exists(destPath))
		{
			Directory.CreateDirectory(destPath);
		}
		string[] files = Directory.GetFiles(sourcePath);
		foreach (string text in files)
		{
			string text2 = Path.Combine(destPath, Path.GetFileName(text));
			if (File.Exists(text2))
			{
				File.SetAttributes(text2, FileAttributes.Normal);
			}
			File.Copy(text, text2, overwrite: true);
		}
		files = Directory.GetDirectories(sourcePath);
		foreach (string text3 in files)
		{
			string destPath2 = Path.Combine(destPath, Path.GetFileName(text3));
			CopyDirectory(text3, destPath2);
		}
	}

	public static void CopyDirectoryNoRecursion(string sourcePath, string destPath)
	{
		string[] directories = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);
		for (int i = 0; i < directories.Length; i++)
		{
			Directory.CreateDirectory(directories[i].Replace(sourcePath, destPath));
		}
		directories = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);
		foreach (string obj in directories)
		{
			File.Copy(obj, obj.Replace(sourcePath, destPath), overwrite: true);
		}
	}

	public static void DeleteContents(string path)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(path);
		FileInfo[] files = directoryInfo.GetFiles();
		for (int i = 0; i < files.Length; i++)
		{
			files[i].Delete();
		}
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		for (int i = 0; i < directories.Length; i++)
		{
			directories[i].Delete(recursive: true);
		}
	}

	public static void SafeMoveDirectory(string sourcePath, string destPath)
	{
		try
		{
			if (!Directory.Exists(destPath))
			{
				Directory.CreateDirectory(destPath);
			}
			string[] files = Directory.GetFiles(sourcePath);
			foreach (string text in files)
			{
				string text2 = Path.Combine(destPath, Path.GetFileName(text));
				if (File.Exists(text2))
				{
					File.SetAttributes(text2, FileAttributes.Normal);
				}
				File.Copy(text, text2, overwrite: true);
			}
			files = Directory.GetDirectories(sourcePath);
			foreach (string text3 in files)
			{
				string destPath2 = Path.Combine(destPath, Path.GetFileName(text3));
				SafeMoveDirectory(text3, destPath2);
			}
			Directory.Delete(sourcePath, recursive: true);
		}
		catch (Exception)
		{
		}
	}

	public static string ReplaceInvalidFileNameCharacters(string input, char replaceWith)
	{
		return input.SanitizedCustom(new string(Path.GetInvalidFileNameChars()), replaceWith);
	}

	public static string ReplaceInvalidPathCharacters(string input, char replaceWith)
	{
		return input.SanitizedCustom(new string(Path.GetInvalidPathChars()), replaceWith);
	}

	public static bool IsDirectoryWritable(string dirPath)
	{
		if (!Directory.Exists(dirPath))
		{
			return false;
		}
		try
		{
			using (File.Create(Path.Combine(dirPath, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
			{
			}
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static string GetFirstExistingParentDirectory(string dirPath)
	{
		if (string.IsNullOrWhiteSpace(dirPath))
		{
			return null;
		}
		string text = dirPath;
		while (!string.IsNullOrWhiteSpace(text) && !Directory.Exists(text))
		{
			text = Path.GetDirectoryName(text);
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		return text;
	}

	public static bool IsPathValid(string path, bool checkRootExistence = true)
	{
		bool flag = true;
		try
		{
			if (!Path.IsPathRooted(path))
			{
				flag = false;
			}
			if (flag)
			{
				string fullPath = Path.GetFullPath(path);
				if (path != fullPath)
				{
					flag = false;
				}
			}
			if (flag && checkRootExistence && !Directory.Exists(Path.GetPathRoot(path)))
			{
				flag = false;
			}
		}
		catch (Exception)
		{
			flag = false;
		}
		return flag;
	}

	public static bool AddDirectorySecurity(string directoryName, WellKnownSidType sid, FileSystemRights rights = FileSystemRights.FullControl, InheritanceFlags inheritanceFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags propagationFlags = PropagationFlags.None, AccessControlType controlType = AccessControlType.Allow)
	{
		NTAccount nTAccount = new SecurityIdentifier(sid, null).Translate(typeof(NTAccount)) as NTAccount;
		if (nTAccount != null)
		{
			return AddDirectorySecurity(directoryName, nTAccount.ToString(), rights, inheritanceFlags, propagationFlags, controlType);
		}
		return false;
	}

	public static bool AddDirectorySecurity(string directoryName, string account, FileSystemRights rights = FileSystemRights.FullControl, InheritanceFlags inheritanceFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags propagationFlags = PropagationFlags.None, AccessControlType controlType = AccessControlType.Allow)
	{
		bool result = true;
		try
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
			DirectorySecurity accessControl = directoryInfo.GetAccessControl();
			accessControl.AddAccessRule(new FileSystemAccessRule(account, rights, inheritanceFlags, propagationFlags, controlType));
			directoryInfo.SetAccessControl(accessControl);
		}
		catch (Exception)
		{
			result = false;
		}
		return result;
	}
}
