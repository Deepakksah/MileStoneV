using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Core.Cab;

public class CabHandler
{
	private string _installationDir;

	private UpdateProgress _progressCallback;

	private UpdateStatus _statusCallback;

	private int _fileCount;

	private int _totalFilesInCabinet;

	private string _folderName;

	private int _byteDistance;

	private CabError _erf = new CabError();

	private readonly byte[] _mscfTag = new byte[6] { 77, 83, 67, 70, 0, 0 };

	private FdiFileReadDelegate FileReadDelegate;

	private FdiFileOpenDelegate FileOpenDelegate;

	private FdiMemAllocDelegate MemAllocDelegate;

	private FdiFileSeekDelegate FileSeekDelegate;

	private FdiMemFreeDelegate MemFreeDelegate;

	private FdiFileWriteDelegate FileWriteDelegate;

	private FdiFileCloseDelegate FileCloseDelegate;

	private FdiNotifyDelegate NotifyDelegate;

	public CabHandler(string installationDir)
	{
		FileReadDelegate = FileRead;
		FileOpenDelegate = FileOpen;
		MemAllocDelegate = MemAlloc;
		FileSeekDelegate = FileSeek;
		MemFreeDelegate = MemFree;
		FileWriteDelegate = FileWrite;
		FileCloseDelegate = FileClose;
		NotifyDelegate = Notify;
		_installationDir = installationDir;
		_fileCount = 0;
		_totalFilesInCabinet = 0;
		_folderName = "";
	}

	public CabHandler()
	{
		FileReadDelegate = FileRead;
		FileOpenDelegate = FileOpen;
		MemAllocDelegate = MemAlloc;
		FileSeekDelegate = FileSeek;
		MemFreeDelegate = MemFree;
		FileWriteDelegate = FileWrite;
		FileCloseDelegate = FileClose;
		NotifyDelegate = Notify;
		_installationDir = string.Empty;
		_fileCount = 0;
		_totalFilesInCabinet = 0;
		_folderName = "";
	}

	protected virtual uint OnFileFound(uint context, uint notification, IntPtr param1, IntPtr param2)
	{
		uint num = 0u;
		CabInvoke.FILE_IN_CABINET_INFO fILE_IN_CABINET_INFO = (CabInvoke.FILE_IN_CABINET_INFO)Marshal.PtrToStructure(param1, typeof(CabInvoke.FILE_IN_CABINET_INFO));
		switch (context)
		{
		case 0u:
			num = 2u;
			_totalFilesInCabinet++;
			break;
		case 1u:
			num = OnFileExtractBefore(fILE_IN_CABINET_INFO.NameInCabinet);
			if (num == 1)
			{
				fILE_IN_CABINET_INFO.FullTargetName = MakeExtractFileName(fILE_IN_CABINET_INFO.NameInCabinet);
				Marshal.StructureToPtr(fILE_IN_CABINET_INFO, param1, fDeleteOld: true);
			}
			break;
		}
		return num;
	}

	protected virtual uint OnFileExtractBefore(string file)
	{
		if (string.IsNullOrEmpty(_folderName) && !file.StartsWith(_folderName))
		{
			return 2u;
		}
		return 1u;
	}

	protected virtual uint OnFileExtractComplete(IntPtr param1)
	{
		return ((CabInvoke.FILEPATHS)Marshal.PtrToStructure(param1, typeof(CabInvoke.FILEPATHS))).Win32Error;
	}

	private string MakeExtractFileName(string file)
	{
		string text = Path.Combine(_installationDir, file);
		if (!Directory.Exists(Path.GetDirectoryName(text)))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(text));
		}
		return text;
	}

	private uint CallBack(uint context, uint notification, IntPtr param1, IntPtr param2)
	{
		uint result = 0u;
		switch (notification)
		{
		case 17u:
			result = OnFileFound(context, notification, param1, param2);
			break;
		case 19u:
			result = OnFileExtractComplete(param1);
			_fileCount++;
			if (_progressCallback != null)
			{
				_progressCallback(_fileCount, _totalFilesInCabinet);
			}
			break;
		case 18u:
			result = 0u;
			break;
		}
		return result;
	}

	public void SetProgressCallbacks(UpdateProgress updateProgress, UpdateStatus statusCallback)
	{
		_progressCallback = updateProgress;
		_statusCallback = statusCallback;
	}

	public void CountFiles(string cabFile)
	{
		CabInvoke.PSP_FILE_CALLBACK callBack = CallBack;
		uint context = 0u;
		if (!CabInvoke.SetupIterateCabinet(cabFile, 0u, callBack, context))
		{
			throw new CabException(new Win32Exception((int)CabInvoke.GetLastError()).Message);
		}
	}

	public void ExtractFolder(string cabFile, string installationDir, string folderName)
	{
		_folderName = folderName;
		CabInvoke.PSP_FILE_CALLBACK callBack = CallBack;
		uint context = 1u;
		if (!CabInvoke.SetupIterateCabinet(cabFile, 0u, callBack, context))
		{
			throw new CabException(new Win32Exception((int)CabInvoke.GetLastError()).Message);
		}
	}

	public void ExtractAll(string cabFile, string installationDir)
	{
		_folderName = "";
		if (!string.IsNullOrEmpty(installationDir))
		{
			_installationDir = installationDir;
		}
		InitializeUnpack(cabFile);
		CabInvoke.PSP_FILE_CALLBACK callBack = CallBack;
		uint context = 1u;
		if (!CabInvoke.SetupIterateCabinet(cabFile, 0u, callBack, context))
		{
			throw new CabException(new Win32Exception((int)CabInvoke.GetLastError()).Message);
		}
	}

	private void InitializeUnpack(string cabFile)
	{
		_fileCount = 0;
		CountFiles(cabFile);
	}

	public void ExtractInstallerFiles(string cabPath, string targetDir)
	{
		if (!string.IsNullOrEmpty(targetDir))
		{
			_installationDir = targetDir;
		}
		_byteDistance = GetByteDistance(cabPath);
		if (_byteDistance == -1)
		{
			throw new CabException("Invalid CAB file - MSCF tag not found.");
		}
		if (!ExtractCabinetFiles("\\" + Path.GetFileName(cabPath), Path.GetDirectoryName(cabPath)))
		{
			throw new CabException(new Win32Exception((int)CabInvoke.GetLastError()).Message);
		}
	}

	private int GetByteDistance(string cabPath)
	{
		try
		{
			using Stream stream = new FileStream(cabPath, FileMode.Open, FileAccess.Read);
			return FindByteSequence(stream);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private int FindByteSequence(Stream stream)
	{
		int num = 0;
		int num2;
		while ((num2 = stream.ReadByte()) != -1)
		{
			if (num2 == _mscfTag[num++])
			{
				if (num == _mscfTag.Length)
				{
					return (int)(stream.Position - _mscfTag.Length);
				}
			}
			else
			{
				num = 0;
			}
		}
		return -1;
	}

	public bool ExtractCabinetFiles(string fileName, string filePath)
	{
		IntPtr zero = IntPtr.Zero;
		zero = CabInvoke.FDICreate(MemAllocDelegate, MemFreeDelegate, FileOpenDelegate, FileReadDelegate, FileWriteDelegate, FileCloseDelegate, FileSeekDelegate, -1, _erf);
		if (zero == IntPtr.Zero)
		{
			throw new CabException(GetLastErrorMessage());
		}
		if (!CabInvoke.FDICopy(zero, fileName, filePath, 0, NotifyDelegate, null, _installationDir))
		{
			CabInvoke.FDIDestroy(zero);
			zero = IntPtr.Zero;
			throw new CabException(GetLastErrorMessage());
		}
		CabInvoke.FDIDestroy(zero);
		zero = IntPtr.Zero;
		return true;
	}

	private string GetLastErrorMessage()
	{
		return new Win32Exception((int)CabInvoke.GetLastError()).Message;
	}

	private IntPtr Notify(CabInvoke.FDINotificationType fdint, FDINotification pfdin)
	{
		IntPtr zero = IntPtr.Zero;
		return fdint switch
		{
			CabInvoke.FDINotificationType.CopyFile => RunCopyFile(pfdin), 
			CabInvoke.FDINotificationType.CloseFileInfo => RunCloseFileInfo(pfdin), 
			CabInvoke.FDINotificationType.NextCabinet => IntPtr.Zero, 
			CabInvoke.FDINotificationType.PartialFile => IntPtr.Zero, 
			CabInvoke.FDINotificationType.CabinetInfo => IntPtr.Zero, 
			CabInvoke.FDINotificationType.Enumerate => IntPtr.Zero, 
			_ => new IntPtr(-1), 
		};
	}

	private IntPtr RunCloseFileInfo(FDINotification pfdin)
	{
		string path = _installationDir + "\\" + pfdin.str1;
		DateTime dateTime = DateTimeFromDosDateTime(pfdin.FileDate, pfdin.FileTime);
		File.SetCreationTime(path, dateTime);
		File.SetLastWriteTime(path, dateTime);
		FileAttributes fileAttributes = FileAttributesFromFAttrs(pfdin.FileAttributes);
		File.SetAttributes(path, fileAttributes);
		FileClose(pfdin.FileHandle);
		return new IntPtr(1);
	}

	private IntPtr RunCopyFile(FDINotification pfdin)
	{
		string str = pfdin.str1;
		if (str.Contains("\\"))
		{
			CreateDirectory(str);
		}
		string fileName = Path.Combine(_installationDir, str);
		return FileOpenStream(fileName, FileAccess.Write, FileShare.ReadWrite, FileMode.Create, 0);
	}

	private void CreateDirectory(string fileName)
	{
		string[] array = fileName.Split('\\');
		string text = null;
		if (array.Length > 1)
		{
			for (int i = 0; i < array.Length - 1; i++)
			{
				string text2 = array[i];
				text = ((text == null) ? text2 : Path.Combine(text, text2));
				Directory.CreateDirectory(Path.Combine(_installationDir, text));
			}
		}
	}

	protected virtual IntPtr MemAlloc(int cb)
	{
		try
		{
			return Marshal.AllocHGlobal(cb);
		}
		catch (Exception)
		{
			return IntPtr.Zero;
		}
	}

	protected virtual void MemFree(IntPtr memory)
	{
		try
		{
			Marshal.FreeHGlobal(memory);
		}
		catch (Exception)
		{
		}
	}

	protected virtual IntPtr FileOpen(string fileName, int oflag, int pmode)
	{
		FileAccess fAccess = FileAccessFromOFlag(oflag);
		FileMode fMode = FileModeFromOFlag(oflag);
		FileShare fShare = FileShareFromPMode(pmode);
		return FileOpenStream(fileName, fAccess, fShare, fMode, _byteDistance);
	}

	private IntPtr FileOpenStream(string fileName, FileAccess fAccess, FileShare fShare, FileMode fMode, int offset)
	{
		FileStream fileStream = new FileStream(fileName, fMode, fAccess, fShare);
		if (offset != 0)
		{
			fileStream.Seek(offset, SeekOrigin.Begin);
		}
		return (IntPtr)GCHandle.Alloc(fileStream);
	}

	protected virtual int FileRead(IntPtr hf, byte[] buffer, int cb)
	{
		try
		{
			return FileStreamFromHandle(hf).Read(buffer, 0, cb);
		}
		catch (Exception)
		{
			return 0;
		}
	}

	private FileStream FileStreamFromHandle(IntPtr hf)
	{
		return (FileStream)((GCHandle)hf).Target;
	}

	protected virtual int FileWrite(IntPtr hf, byte[] buffer, int cb)
	{
		try
		{
			FileStreamFromHandle(hf).Write(buffer, 0, cb);
			return cb;
		}
		catch (Exception)
		{
			return 0;
		}
	}

	protected virtual int FileClose(IntPtr hf)
	{
		try
		{
			FileStreamFromHandle(hf).Close();
			return 0;
		}
		catch (Exception)
		{
			return -1;
		}
		finally
		{
			((GCHandle)hf).Free();
		}
	}

	protected virtual int FileSeek(IntPtr hf, int dist, int seektype)
	{
		int num = ((seektype == 0 && dist < _byteDistance) ? (dist + _byteDistance) : dist);
		FileStream fileStream = FileStreamFromHandle(hf);
		SeekOrigin origin = SeekOriginFromSeekType(seektype);
		return (int)fileStream.Seek(num, origin);
	}

	private FileAccess FileAccessFromOFlag(int oflag)
	{
		FileAccess result = FileAccess.Read;
		switch (oflag & 3)
		{
		case 0:
			result = FileAccess.Read;
			break;
		case 1:
			result = FileAccess.Write;
			break;
		case 2:
			result = FileAccess.ReadWrite;
			break;
		}
		return result;
	}

	private FileShare FileShareFromPMode(int pmode)
	{
		FileShare fileShare = FileShare.None;
		if ((pmode & 0x100) != 0)
		{
			fileShare |= FileShare.Read;
		}
		if ((pmode & 0x80) != 0)
		{
			fileShare |= FileShare.Write;
		}
		return fileShare;
	}

	private FileMode FileModeFromOFlag(int oflag)
	{
		if ((oflag & 0x100) != 0)
		{
			if ((oflag & 0x400) != 0)
			{
				return FileMode.CreateNew;
			}
			if ((oflag & 0x200) != 0)
			{
				return FileMode.Create;
			}
			return FileMode.OpenOrCreate;
		}
		if ((oflag & 0x200) != 0)
		{
			return FileMode.Truncate;
		}
		if ((oflag & 0x400) != 0)
		{
			return FileMode.Open;
		}
		return FileMode.Open;
	}

	private SeekOrigin SeekOriginFromSeekType(int seektype)
	{
		SeekOrigin result = SeekOrigin.Begin;
		switch (seektype)
		{
		case 0:
			result = SeekOrigin.Begin;
			break;
		case 1:
			result = SeekOrigin.Current;
			break;
		case 2:
			result = SeekOrigin.End;
			break;
		}
		return result;
	}

	private DateTime DateTimeFromDosDateTime(short date, short time)
	{
		int day = date & 0x1F;
		int month = (date >> 5) & 0xF;
		int year = 1980 + ((date >> 9) & 0x7F);
		int second = 2 * (time & 0x1F);
		int minute = (time >> 5) & 0x3F;
		int hour = (time >> 11) & 0x1F;
		return new DateTime(year, month, day, hour, minute, second);
	}

	private void DosDateTimeFromDateTime(DateTime fileDate, ref short fatDate, ref short fatTime)
	{
		fatDate = (short)((fileDate.Year - 1980 << 9) | (fileDate.Month << 5) | fileDate.Day);
		fatTime = (short)((fileDate.Hour << 11) | (fileDate.Minute << 5) | (fileDate.Second / 2));
	}

	private FileAttributes FileAttributesFromFAttrs(short attrs)
	{
		FileAttributes fileAttributes = FileAttributes.Normal;
		if (((ulong)attrs & 0x20uL) != 0L)
		{
			fileAttributes |= FileAttributes.Archive;
		}
		if (((ulong)attrs & 2uL) != 0L)
		{
			fileAttributes |= FileAttributes.Hidden;
		}
		if (((ulong)attrs & 1uL) != 0L)
		{
			fileAttributes |= FileAttributes.ReadOnly;
		}
		if (((ulong)attrs & 4uL) != 0L)
		{
			fileAttributes |= FileAttributes.System;
		}
		return fileAttributes;
	}
}
