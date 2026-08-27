using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using NSpring.Logging.Common.Pattern.Patterns;
using NSpring.Logging.DataFormatters;
using NSpring.Logging.EventFormatters;
using NSpring.Logging.Exceptions;

namespace NSpring.Logging.Loggers;

public sealed class FileLogger : Logger
{
	private class OpenFileStreamInfo : IDisposable
	{
		public StreamWriter StreamWriter { get; private set; }

		public string Path { get; private set; }

		public string ActualPath { get; private set; }

		public OpenFileStreamInfo(StreamWriter streamWriter, string path, string actualPath)
		{
			StreamWriter = streamWriter;
			Path = path;
			ActualPath = actualPath;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}

		private void Dispose(bool disposing)
		{
			if (disposing && StreamWriter != null)
			{
				StreamWriter.Dispose();
				StreamWriter = null;
			}
		}
	}

	private string filePath;

	private FilePathPattern pattern;

	private bool isFilePathStatic;

	private bool isDirectoryPathNonStatic;

	private bool isFilePathVolatile;

	private bool isArchivingEnabled;

	private int maximumFileSize = 104857600;

	private string archiveDirectoryPath;

	private int maximumOpenFileCount = 10;

	public bool IsArchivingEnabled
	{
		get
		{
			return isArchivingEnabled;
		}
		set
		{
			lock (monitor)
			{
				if (base.IsOpen)
				{
					throw new InvalidOperationException("Cannot set property while the logger is open.");
				}
				isArchivingEnabled = value;
			}
		}
	}

	public int MaximumFileSize
	{
		get
		{
			return maximumFileSize;
		}
		set
		{
			lock (monitor)
			{
				if (base.IsOpen)
				{
					throw new InvalidOperationException("Cannot set property while the logger is open.");
				}
				maximumFileSize = Math.Max(5120, value);
			}
		}
	}

	public string ArchiveDirectoryPath
	{
		get
		{
			return archiveDirectoryPath;
		}
		set
		{
			lock (monitor)
			{
				if (base.IsOpen)
				{
					throw new InvalidOperationException("Cannot set property while the logger is open.");
				}
				value = value.Trim();
				if (value.Length == 0)
				{
					archiveDirectoryPath = null;
				}
				else
				{
					archiveDirectoryPath = value;
				}
			}
		}
	}

	public int MaximumOpenFileCount
	{
		get
		{
			return maximumOpenFileCount;
		}
		set
		{
			lock (monitor)
			{
				if (base.IsOpen)
				{
					throw new InvalidOperationException("Cannot set property while the logger is open.");
				}
				maximumOpenFileCount = Math.Max(1, value);
			}
		}
	}

	public string FilePath
	{
		get
		{
			return filePath;
		}
		set
		{
			if (value == null || value.Trim().Equals(string.Empty))
			{
				throw new ArgumentException("Invalid file path");
			}
			lock (monitor)
			{
				if (base.IsOpen)
				{
					throw new InvalidOperationException("Cannot set property while the logger is open.");
				}
				filePath = value;
				pattern = new FilePathPattern(filePath);
				isFilePathStatic = pattern.IsFilePathStatic;
				isDirectoryPathNonStatic = !pattern.IsDirectoryPathStatic;
				isFilePathVolatile = pattern.IsFilePathVolatile;
			}
		}
	}

	public FileLogger()
	{
		eventFormatter = new PatternEventFormatter("{ts}{z}  [{ln:1w}]  {msg}{data}");
		eventFormatter.Suffix = "\n";
		FlatDataFormatter dataFormatter = new FlatDataFormatter
		{
			Prefix = "  "
		};
		eventFormatter.DataFormatter = dataFormatter;
		base.IsBufferingEnabled = true;
		base.BufferSize = 1000;
		base.AutoFlushInterval = 10000;
	}

	public FileLogger(string filePath)
	{
		FilePath = filePath;
		eventFormatter = new PatternEventFormatter("{ts}{z}  [{ln:1w}]  {msg}{data}");
		eventFormatter.Suffix = "\n";
		FlatDataFormatter dataFormatter = new FlatDataFormatter
		{
			Prefix = "  "
		};
		eventFormatter.DataFormatter = dataFormatter;
		base.IsBufferingEnabled = true;
		base.BufferSize = 1000;
		base.AutoFlushInterval = 10000;
	}

	public FileLogger(string filePath, string outputPattern)
	{
		if (outputPattern == null)
		{
			throw new ArgumentNullException();
		}
		FilePath = filePath;
		eventFormatter = new PatternEventFormatter(outputPattern);
		eventFormatter.Suffix = "\n";
		eventFormatter.DataFormatter = new FlatDataFormatter();
		base.IsBufferingEnabled = true;
		base.BufferSize = 1000;
		base.AutoFlushInterval = 10000;
	}

	protected override void AcquireResources()
	{
		if (isArchivingEnabled)
		{
			if (archiveDirectoryPath == null)
			{
				throw new InvalidOperationException("Archiving is enabled, but no archive directory path is set-- cannot start");
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(archiveDirectoryPath);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			archiveDirectoryPath = directoryInfo.FullName + Path.DirectorySeparatorChar;
		}
		if (pattern.IsDirectoryPathStatic)
		{
			new FileInfo(pattern.Format(new Event(Level.Info, "dummy", null, null, null))).Directory.Create();
		}
	}

	protected override void ReleaseResources()
	{
	}

	private void ArchiveFile(string filePath)
	{
		FileInfo fileInfo = new FileInfo(filePath);
		if (fileInfo.Length > maximumFileSize)
		{
			MoveFileToArchive(fileInfo);
		}
	}

	private void MoveFileToArchive(FileInfo fi)
	{
		string path = $"{Path.GetFileNameWithoutExtension(fi.Name)} ({DateTime.UtcNow:yyyyMMddHHmmss}){Path.GetExtension(fi.Name)}";
		string destFileName = Path.Combine(archiveDirectoryPath, path);
		try
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(archiveDirectoryPath);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			else
			{
				string[] array = new string[3]
				{
					Path.GetFileNameWithoutExtension(fi.Name) + " (*)" + Path.GetExtension(fi.Name),
					Path.GetFileNameWithoutExtension(fi.Name) + "-? (*)" + Path.GetExtension(fi.Name),
					Path.GetFileName(fi.Name) + " (*).txt"
				};
				foreach (string searchPattern in array)
				{
					FileInfo[] files = directoryInfo.GetFiles(searchPattern);
					foreach (FileInfo fileInfo in files)
					{
						if (!string.Equals(fileInfo.Name, fi.Name, StringComparison.OrdinalIgnoreCase))
						{
							try
							{
								fileInfo.Attributes &= ~FileAttributes.ReadOnly;
								fileInfo.Delete();
							}
							catch (OutOfMemoryException)
							{
								throw;
							}
							catch (FileNotFoundException)
							{
							}
							catch (DirectoryNotFoundException)
							{
							}
							catch (ArgumentException)
							{
							}
							catch (SecurityException)
							{
							}
							catch (IOException)
							{
							}
							catch (UnauthorizedAccessException)
							{
							}
						}
					}
				}
			}
			fi.MoveTo(destFileName);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	protected override void Write(Event logEvent)
	{
		string path = ((!isFilePathStatic) ? pattern.Format(logEvent) : filePath);
		using (StreamWriter streamWriter = CreateStreamWriter(ref path, append: true, encoding, 1024))
		{
			streamWriter.Write(eventFormatter.Format(logEvent));
			streamWriter.Close();
		}
		if (isArchivingEnabled)
		{
			ArchiveFile(path);
		}
	}

	protected override void Write(Event[] logEvents, int length)
	{
		if (length == 0)
		{
			return;
		}
		if (isFilePathStatic)
		{
			string path = filePath;
			using (StreamWriter streamWriter = CreateStreamWriter(ref path, append: true, encoding, 8192))
			{
				for (int i = 0; i < length; i++)
				{
					streamWriter.Write(eventFormatter.Format(logEvents[i]));
				}
				streamWriter.Close();
			}
			if (isArchivingEnabled)
			{
				ArchiveFile(path);
			}
			return;
		}
		List<OpenFileStreamInfo> list = new List<OpenFileStreamInfo>();
		for (int j = 0; j < length; j++)
		{
			Event obj = logEvents[j];
			bool flag = false;
			string text = pattern.Format(obj);
			for (int k = 0; k < list.Count; k++)
			{
				OpenFileStreamInfo openFileStreamInfo = list[k];
				if (!string.Equals(text, openFileStreamInfo.Path, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				try
				{
					openFileStreamInfo.StreamWriter.Write(eventFormatter.Format(obj));
					if (k > 0)
					{
						list.RemoveAt(k);
						list.Insert(k - 1, openFileStreamInfo);
					}
					flag = true;
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
					try
					{
						openFileStreamInfo.StreamWriter.Close();
						openFileStreamInfo.Dispose();
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch
					{
					}
					list.RemoveAt(k);
				}
				break;
			}
			if (flag)
			{
				continue;
			}
			StreamWriter streamWriter2 = null;
			string path2 = text;
			try
			{
				streamWriter2 = CreateStreamWriter(ref path2, append: true, encoding, 8192);
				streamWriter2.Write(eventFormatter.Format(obj));
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception exception)
			{
				try
				{
					streamWriter2.Close();
					streamWriter2.Dispose();
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
				base.ExceptionHandler.Handle(new LoggerException(this, obj, exception));
				continue;
			}
			if (list.Count >= maximumOpenFileCount)
			{
				OpenFileStreamInfo openFileStreamInfo2 = list.Last();
				try
				{
					openFileStreamInfo2.StreamWriter.Close();
					openFileStreamInfo2.StreamWriter.Dispose();
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
				list.Remove(openFileStreamInfo2);
				if (isArchivingEnabled)
				{
					ArchiveFile(openFileStreamInfo2.Path);
				}
			}
			list.Insert(0, new OpenFileStreamInfo(streamWriter2, text, path2));
		}
		foreach (OpenFileStreamInfo item in list)
		{
			try
			{
				item.StreamWriter.Close();
				item.StreamWriter.Dispose();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			if (isArchivingEnabled)
			{
				ArchiveFile(item.ActualPath);
			}
		}
		list.Clear();
	}

	private static StreamWriter CreateStreamWriter(ref string path, bool append, Encoding encoding, int bufferSize)
	{
		StreamWriter result = null;
		FileSystemAccessRule rule = new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), FileSystemRights.FullControl, AccessControlType.Allow);
		bool flag = File.Exists(path);
		try
		{
			result = new StreamWriter(path, append, encoding, bufferSize);
			if (!flag)
			{
				FileSecurity accessControl = File.GetAccessControl(path, AccessControlSections.Access);
				accessControl.AddAccessRule(rule);
				File.SetAccessControl(path, accessControl);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (DirectoryNotFoundException)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			result = new StreamWriter(path, append, encoding, bufferSize);
			if (!flag)
			{
				FileSecurity accessControl2 = File.GetAccessControl(path, AccessControlSections.Access);
				accessControl2.AddAccessRule(rule);
				File.SetAccessControl(path, accessControl2);
			}
		}
		catch (SecurityException)
		{
			throw;
		}
		catch (UnauthorizedAccessException)
		{
			for (int i = 1; i <= 3; i++)
			{
				string directoryName = Path.GetDirectoryName(path);
				string path2 = string.Format("{0}-{2}{1}", Path.GetFileNameWithoutExtension(path), Path.GetExtension(path), i);
				string text = Path.Combine(directoryName, path2);
				flag = File.Exists(text);
				try
				{
					result = new StreamWriter(text, append, encoding, bufferSize);
					if (!flag)
					{
						FileSecurity accessControl3 = File.GetAccessControl(text, AccessControlSections.Access);
						accessControl3.AddAccessRule(rule);
						File.SetAccessControl(text, accessControl3);
					}
					path = text;
					break;
				}
				catch (UnauthorizedAccessException)
				{
					if (i == 3)
					{
						throw;
					}
				}
			}
		}
		return result;
	}
}
