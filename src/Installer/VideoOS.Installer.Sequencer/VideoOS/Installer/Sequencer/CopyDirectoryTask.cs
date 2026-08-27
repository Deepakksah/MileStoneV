using System;
using System.IO;
using System.Runtime.InteropServices;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Sequencer.Properties;

namespace VideoOS.Installer.Sequencer;

public class CopyDirectoryTask : InstallerTask
{
	private string _sourceDIR;

	private string _destinationDIR;

	private bool _overwrite;

	private long _totalBytes;

	private long _transferredBytes;

	private long _totalTransferredBytes;

	private readonly object _lock = new object();

	public override bool RunInHardwareManufacturerMode => true;

	public override bool RunInFinalizeMode => false;

	public CopyDirectoryTask(string source, string destination, bool overwrite)
	{
		base.Id = "CopyTask";
		_sourceDIR = source;
		_destinationDIR = destination;
		_overwrite = overwrite;
		Status = TaskStatus.NotStarted;
	}

	private static long GetDirectorySize(DirectoryInfo searchDirectory)
	{
		FileInfo[] files = searchDirectory.GetFiles();
		long num = 0L;
		FileInfo[] array = files;
		foreach (FileInfo fileInfo in array)
		{
			num += fileInfo.Length;
		}
		DirectoryInfo[] directories = searchDirectory.GetDirectories();
		long num2 = 0L;
		DirectoryInfo[] array2 = directories;
		foreach (DirectoryInfo searchDirectory2 in array2)
		{
			num2 += GetDirectorySize(searchDirectory2);
		}
		return num + num2;
	}

	private void CopyDirectory(string source, string destination)
	{
		if (destination[destination.Length - 1] != Path.DirectorySeparatorChar)
		{
			destination += Path.DirectorySeparatorChar;
		}
		if (!Directory.Exists(destination))
		{
			Directory.CreateDirectory(destination);
		}
		string[] fileSystemEntries = Directory.GetFileSystemEntries(source);
		foreach (string text in fileSystemEntries)
		{
			if (Directory.Exists(text))
			{
				CopyDirectory(text, destination + Path.GetFileName(text));
				continue;
			}
			try
			{
				string text2 = Path.Combine(destination, Path.GetFileName(text));
				NativeMethods.CopyFileOptions copyFileOptions = NativeMethods.CopyFileOptions.None;
				if (!_overwrite)
				{
					copyFileOptions |= NativeMethods.CopyFileOptions.COPY_FILE_FAIL_IF_EXISTS;
				}
				else
				{
					FileInfo fileInfo = new FileInfo(text2);
					if (fileInfo.Exists)
					{
						if ((fileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
						{
							fileInfo.Attributes--;
						}
						if ((fileInfo.Attributes & FileAttributes.System) == FileAttributes.System)
						{
							fileInfo.Attributes -= 4;
						}
						if ((fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
						{
							fileInfo.Attributes -= 2;
						}
					}
				}
				string lpExistingFileName = "\\\\?\\" + text;
				if (Path.GetPathRoot(text).StartsWith("\\\\"))
				{
					lpExistingFileName = text;
				}
				string lpNewFileName = "\\\\?\\" + text2;
				if (Path.GetPathRoot(text2).StartsWith("\\\\"))
				{
					lpNewFileName = text2;
				}
				_transferredBytes = 0L;
				bool flag = false;
				int pbCancel = 0;
				try
				{
					flag = NativeMethods.CopyFileExW(lpExistingFileName, lpNewFileName, CopyProgressCallback, IntPtr.Zero, ref pbCancel, copyFileOptions);
				}
				catch (Exception)
				{
					Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
				}
				if (!flag)
				{
					LogManager.Write($"Could not copy file {text}.");
				}
			}
			catch (Exception exception)
			{
				LogManager.Write(exception);
			}
		}
	}

	public uint CopyProgressCallback(long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred, uint dwStreamNumber, NativeMethods.CopyCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
	{
		lock (_lock)
		{
			if (dwCallbackReason == NativeMethods.CopyCallbackReason.CALLBACK_CHUNK_FINISHED)
			{
				long num = TotalBytesTransferred - _transferredBytes;
				_totalTransferredBytes += num;
				base.CurrentProgress = (int)((double)_totalTransferredBytes / (double)_totalBytes * (double)base.MaximumProgress);
				_transferredBytes = TotalBytesTransferred;
				if (base.CurrentProgress != 0)
				{
					NotifyObservers();
				}
			}
		}
		return 0u;
	}

	protected override void DoRun()
	{
		try
		{
			base.Message = Resources.Install_CopingFiles;
			base.CurrentProgress = 0;
			base.MaximumProgress = 100;
			NotifyObservers();
			DirectoryInfo directoryInfo = new DirectoryInfo(_sourceDIR);
			if (directoryInfo.Exists)
			{
				_totalBytes = GetDirectorySize(directoryInfo);
				CopyDirectory(_sourceDIR, _destinationDIR);
			}
			LogManager.Write($"Installation files are copied to {_destinationDIR}.");
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
		}
		finally
		{
			_result = TaskResult.Success;
		}
	}

	protected override void PostRun()
	{
	}

	protected override void PreRun()
	{
	}
}
