using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Sequencer;

public static class NativeMethods
{
	public delegate uint CopyProgressCallbackDelegate(long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred, uint dwStreamNumber, CopyCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);

	[Flags]
	public enum CopyFileOptions : uint
	{
		None = 0u,
		COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 8u,
		COPY_FILE_COPY_SYMLINK = 0x800u,
		COPY_FILE_FAIL_IF_EXISTS = 1u,
		COPY_FILE_NO_BUFFERING = 0x1000u,
		COPY_FILE_OPEN_SOURCE_FOR_WRITE = 4u,
		COPY_FILE_RESTARTABLE = 2u
	}

	public enum CopyCallbackReason : uint
	{
		CALLBACK_CHUNK_FINISHED,
		CALLBACK_STREAM_SWITCH
	}

	public enum ProgressCallbackResult : uint
	{
		PROGRESS_CONTINUE,
		PROGRESS_CANCEL,
		PROGRESS_STOP,
		PROGRESS_QUIET
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool CopyFileExW(string lpExistingFileName, string lpNewFileName, CopyProgressCallbackDelegate lpProgressRoutine, IntPtr lpData, ref int pbCancel, CopyFileOptions dwCopyFlags);
}
