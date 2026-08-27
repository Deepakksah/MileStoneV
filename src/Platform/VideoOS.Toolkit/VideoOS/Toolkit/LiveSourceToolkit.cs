using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NmTimeStamp;
using NmToolkit;
using std;

namespace VideoOS.Toolkit;

public class LiveSourceToolkit : SourceToolkit, ILiveSourceToolkit
{
	private unsafe ImLiveSourceToolkit* m_LiveSourceToolkit;

	public unsafe LiveSourceToolkit(ImLiveSourceToolkit* liveSourceToolkit)
	{
		//IL_001f: Expected I, but got I8
		//IL_000d: Expected I, but got I8
		m_LiveSourceToolkit = liveSourceToolkit;
		ImSourceToolkit* sourceToolkit;
		if (liveSourceToolkit == null)
		{
			sourceToolkit = null;
		}
		else
		{
			sourceToolkit = (ImSourceToolkit*)((long)(nint)liveSourceToolkit + (long)(*(int*)(*(long*)((ulong)(nint)liveSourceToolkit + 8uL) + 4)) + 8);
		}
		base._002Ector(sourceToolkit);
	}

	public unsafe virtual void StartLiveStream()
	{
		//IL_0008: Expected I8, but got I
		//IL_008a: Expected I, but got I8
		//IL_005f: Expected I, but got I8
		//IL_00df: Expected I, but got I8
		//IL_00b4: Expected I, but got I8
		//IL_0134: Expected I, but got I8
		//IL_0109: Expected I, but got I8
		//IL_0185: Expected I, but got I8
		//IL_015b: Expected I, but got I8
		//IL_0036: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImNotConnectedError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImStreamError* error2);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error3);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error4);
		try
		{
			ImLiveSourceToolkit* liveSourceToolkit = m_LiveSourceToolkit;
			ImLiveSourceToolkit* ptr = liveSourceToolkit;
			long num2 = *(long*)(*(ulong*)liveSourceToolkit);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out shared_ptr_003CNmToolkit_003A_003AImStreamSelection_0020const_0020_003E obj);
			shared_ptr_003CNmToolkit_003A_003AImStreamSelection_0020const_0020_003E* ptr2 = &obj;
			*(long*)(&obj) = 0L;
			System.Runtime.CompilerServices.Unsafe.As<shared_ptr_003CNmToolkit_003A_003AImStreamSelection_0020const_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8)) = 0L;
			shared_ptr_003CNmToolkit_003A_003AImStreamSelection_0020const_0020_003E* ptr3 = &obj;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, shared_ptr_003CNmToolkit_003A_003AImStreamSelection_0020const_0020_003E*, void>)num2)((nint)ptr, &obj);
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImNotConnectedError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new NotConnectedException((ImToolkitError*)error);
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
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImStreamError_0040NmToolkit_0040_0040_00408), 9, &error2) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new StreamException((ImToolkitError*)error2);
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
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error3) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new ToolkitException(error3);
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
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error4) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new StdCppException(error4);
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
		GC.KeepAlive(this);
	}

	public unsafe virtual void StopLiveStream()
	{
		//IL_0008: Expected I8, but got I
		//IL_006d: Expected I, but got I8
		//IL_0043: Expected I, but got I8
		//IL_00be: Expected I, but got I8
		//IL_0094: Expected I, but got I8
		//IL_001a: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
		try
		{
			ImLiveSourceToolkit* liveSourceToolkit = m_LiveSourceToolkit;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, void>)(*(ulong*)(*(long*)liveSourceToolkit + 16)))((nint)liveSourceToolkit);
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
		{
			uint num2 = 0u;
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
					num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num2 != 0;
				}).Invoke())
				{
				}
				if (num2 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
		{
			uint num2 = 0u;
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
					num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num2 != 0;
				}).Invoke())
				{
				}
				if (num2 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
			}
		}
		GC.KeepAlive(this);
	}

	public unsafe virtual ILiveSourceToolkit.GetLiveStatus GetLiveData(out IData data, TimeSpan timeout)
	{
		//IL_0008: Expected I8, but got I
		//IL_00bd: Expected I, but got I8
		//IL_0092: Expected I, but got I8
		//IL_0112: Expected I, but got I8
		//IL_00e7: Expected I, but got I8
		//IL_0167: Expected I, but got I8
		//IL_013c: Expected I, but got I8
		//IL_01bc: Expected I, but got I8
		//IL_0191: Expected I, but got I8
		//IL_0211: Expected I, but got I8
		//IL_01e6: Expected I, but got I8
		//IL_0266: Expected I, but got I8
		//IL_023b: Expected I, but got I8
		//IL_02bb: Expected I, but got I8
		//IL_0290: Expected I, but got I8
		//IL_0310: Expected I, but got I8
		//IL_02e5: Expected I, but got I8
		//IL_0365: Expected I, but got I8
		//IL_033a: Expected I, but got I8
		//IL_03ba: Expected I, but got I8
		//IL_038f: Expected I, but got I8
		//IL_040f: Expected I, but got I8
		//IL_03e4: Expected I, but got I8
		//IL_0464: Expected I, but got I8
		//IL_0439: Expected I, but got I8
		//IL_04b9: Expected I, but got I8
		//IL_048e: Expected I, but got I8
		//IL_050b: Expected I, but got I8
		//IL_04e0: Expected I, but got I8
		//IL_0037: Expected I, but got I8
		//IL_0053: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImAuthorizationError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImBreakError* error2);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImConfigurationError* error3);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImDecoderError* error4);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImFileError* error5);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImEncoderError* error6);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImInvalidArgumentError* error7);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImInvalidOperationError* error8);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImNotConnectedError* error9);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImNotSupportedError* error10);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImSigningError* error11);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImStreamError* error12);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error13);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error14);
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmTimeSpan cmTimeSpan);
			CmTimeSpan* ptr = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeSpan_002Cclass_0020System_003A_003ATimeSpan_003E(&cmTimeSpan, &timeout);
			long num2;
			try
			{
				num2 = global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002EGetMilliseconds(ptr);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTimeSpan*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D), &cmTimeSpan);
				throw;
			}
			global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D(&cmTimeSpan);
			ImData* nativeData = null;
			ImLiveSourceToolkit* liveSourceToolkit = m_LiveSourceToolkit;
			long num3 = *(long*)(*(long*)liveSourceToolkit + 24);
			ImLiveSourceToolkit.get_live_status_t get_live_status_t = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImData**, long, ImLiveSourceToolkit.get_live_status_t>)num3)((nint)liveSourceToolkit, &nativeData, num2);
			data = null;
			if (get_live_status_t == (ImLiveSourceToolkit.get_live_status_t)0)
			{
				data = DataFactory.CreateFromImData(nativeData);
			}
			ILiveSourceToolkit.GetLiveStatus getLiveStatus = (ILiveSourceToolkit.GetLiveStatus)get_live_status_t;
			return (ILiveSourceToolkit.GetLiveStatus)get_live_status_t;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImAuthorizationError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new AuthorizationException((ImToolkitError*)error);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num4 != 0;
				}).Invoke())
				{
				}
				if (num4 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImBreakError_0040NmToolkit_0040_0040_00408), 9, &error2) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new BreakException((ImToolkitError*)error2);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num4 != 0;
				}).Invoke())
				{
				}
				if (num4 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImConfigurationError_0040NmToolkit_0040_0040_00408), 9, &error3) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new ConfigurationException((ImToolkitError*)error3);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num4 != 0;
				}).Invoke())
				{
				}
				if (num4 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImDecoderError_0040NmToolkit_0040_0040_00408), 9, &error4) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new DecoderException((ImToolkitError*)error4);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num4 != 0;
				}).Invoke())
				{
				}
				if (num4 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImFileError_0040NmToolkit_0040_0040_00408), 9, &error5) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new FileException((ImToolkitError*)error5);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num4 != 0;
				}).Invoke())
				{
				}
				if (num4 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImEncoderError_0040NmToolkit_0040_0040_00408), 9, &error6) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new EncoderException((ImToolkitError*)error6);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num4 != 0;
				}).Invoke())
				{
				}
				if (num4 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImInvalidArgumentError_0040NmToolkit_0040_0040_00408), 9, &error7) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new InvalidArgumentException((ImToolkitError*)error7);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num4 != 0;
				}).Invoke())
				{
				}
				if (num4 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImInvalidOperationError_0040NmToolkit_0040_0040_00408), 9, &error8) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new InvalidOperationException((ImToolkitError*)error8);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num4 != 0;
				}).Invoke())
				{
				}
				if (num4 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImNotConnectedError_0040NmToolkit_0040_0040_00408), 9, &error9) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new NotConnectedException((ImToolkitError*)error9);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num4 != 0;
				}).Invoke())
				{
				}
				if (num4 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImNotSupportedError_0040NmToolkit_0040_0040_00408), 9, &error10) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new NotSupportedException((ImToolkitError*)error10);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num4 != 0;
				}).Invoke())
				{
				}
				if (num4 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImSigningError_0040NmToolkit_0040_0040_00408), 9, &error11) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new SigningException((ImToolkitError*)error11);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num4 != 0;
				}).Invoke())
				{
				}
				if (num4 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImStreamError_0040NmToolkit_0040_0040_00408), 9, &error12) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new StreamException((ImToolkitError*)error12);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num4 != 0;
				}).Invoke())
				{
				}
				if (num4 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error13) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new ToolkitException(error13);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num4 != 0;
				}).Invoke())
				{
				}
				if (num4 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error14) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new StdCppException(error14);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num4 != 0;
				}).Invoke())
				{
				}
				if (num4 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
			}
		}
		return ILiveSourceToolkit.GetLiveStatus.LIVE_DATA_RETRIEVED;
	}

	public virtual ILiveSourceToolkit.GetLiveStatus GetLiveData(out IData data)
	{
		TimeSpan timeout = new TimeSpan(0, 0, 0, 0, -1);
		GC.KeepAlive(this);
		return GetLiveData(out data, timeout);
	}
}
