using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NmToolkit;
using std;

namespace VideoOS.Toolkit;

public unsafe class MediaData(ImMediaData* data) : Data((ImData*)data), IMediaData
{
	private unsafe ImMediaData* m_MediaData = data;

	public unsafe virtual byte[] GetMediaBody()
	{
		//IL_0008: Expected I8, but got I
		//IL_00cc: Expected I, but got I8
		//IL_00a1: Expected I, but got I8
		//IL_011e: Expected I, but got I8
		//IL_00f3: Expected I, but got I8
		//IL_002e: Expected I, but got I8
		//IL_0062: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_MediaData == null)
		{
			throw new ArgumentNullException("Internal native media data is uninitialized.");
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
		try
		{
			ImMediaData* mediaData = m_MediaData;
			byte* ptr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte*>)(*(ulong*)(*(long*)mediaData + 64)))((nint)mediaData);
			if (ptr == null)
			{
				return null;
			}
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = new IntPtr(ptr);
			zero = intPtr;
			ImMediaData* mediaData2 = m_MediaData;
			int num2 = (int)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong>)(*(ulong*)(*(long*)mediaData2 + 56)))((nint)mediaData2);
			byte[] array = new byte[num2];
			Marshal.Copy(intPtr, array, 0, num2);
			return array;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new ToolkitException(error);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num3 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num3 != 0;
				}).Invoke())
				{
				}
				if (num3 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num3);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new StdCppException(error2);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num3 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num3 != 0;
				}).Invoke())
				{
				}
				if (num3 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num3);
			}
		}
		return null;
	}
}
