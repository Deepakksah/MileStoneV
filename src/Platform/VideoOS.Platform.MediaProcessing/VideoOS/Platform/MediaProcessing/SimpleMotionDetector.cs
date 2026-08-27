using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmCommon;
using NmMediaProcessing;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class SimpleMotionDetector : IDisposable
{
	public enum maskType_t
	{
		monochrome,
		green,
		semitransparentWhite,
		semitransparentGreen
	}

	private unsafe CmSimpleRegionDetector* m_pDetector;

	public unsafe SimpleMotionDetector(SimpleMotionRegion region, double noiseSensitivity)
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_00a6: Expected I, but got I8
		//IL_007b: Expected I, but got I8
		//IL_003f: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pDetector = null;
		base._002Ector();
		if (region == null)
		{
			throw new ArgumentNullException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmSimpleRegionDetector* ptr = (CmSimpleRegionDetector*)global::_003CModule_003E.@new(80uL);
			CmSimpleRegionDetector* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmSimpleRegionDetector_002E_007Bctor_007D(ptr, region.GetNative(), noiseSensitivity));
				CmSimpleRegionDetector* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 80uL);
				throw;
			}
			m_pDetector = ptr2;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
		{
			uint num2 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new Error((exception*)error);
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
		GC.KeepAlive(region);
		GC.KeepAlive(this);
	}

	public unsafe SimpleMotionDetector(double noiseSensitivity)
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_0096: Expected I, but got I8
		//IL_006b: Expected I, but got I8
		//IL_002f: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pDetector = null;
		base._002Ector();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmSimpleRegionDetector* ptr = (CmSimpleRegionDetector*)global::_003CModule_003E.@new(80uL);
			CmSimpleRegionDetector* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmSimpleRegionDetector_002E_007Bctor_007D(ptr, noiseSensitivity));
				CmSimpleRegionDetector* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 80uL);
				throw;
			}
			m_pDetector = ptr2;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
		{
			uint num2 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new Error((exception*)error);
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
	}

	private void _007ESimpleMotionDetector()
	{
		_0021SimpleMotionDetector();
	}

	private unsafe void _0021SimpleMotionDetector()
	{
		//IL_001f: Expected I, but got I8
		//IL_0016: Expected I, but got I8
		CmSimpleRegionDetector* pDetector = m_pDetector;
		if (pDetector != null)
		{
			long num = *(long*)(*(ulong*)pDetector);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)pDetector, 1u);
		}
		m_pDetector = null;
	}

	public unsafe double Detect(Image oldImage, Image newImage)
	{
		//IL_0008: Expected I8, but got I
		//IL_009c: Expected I, but got I8
		//IL_0071: Expected I, but got I8
		//IL_004a: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (oldImage != null && newImage != null)
		{
			double result = 0.0;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmSimpleRegionDetector* native = GetNative();
				long num2 = *(long*)(*(long*)native + 16);
				CmImage* native2 = newImage.GetNative();
				CmImage* native3 = oldImage.GetNative();
				result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, CmImage*, CmImage*, double>)num2)((nint)native, native3, native2);
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
			{
				uint num3 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new Error((exception*)error);
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
			GC.KeepAlive(oldImage);
			GC.KeepAlive(newImage);
			GC.KeepAlive(this);
			return result;
		}
		throw new ArgumentNullException();
	}

	public unsafe double UpdateMotionMask(Image oldImage, Image newImage, Image* mask, maskType_t maskType)
	{
		//IL_0009: Expected I8, but got I
		//IL_00ec: Expected I, but got I8
		//IL_00c0: Expected I, but got I8
		//IL_0098: Expected I, but got I8
		//IL_003c: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (oldImage != null && newImage != null)
		{
			double result = 0.0;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				if (*mask == null)
				{
					CmImage* ptr = (CmImage*)global::_003CModule_003E.@new(120uL);
					CmImage* ptr2;
					try
					{
						ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmImage_002E_007Bctor_007D(ptr));
						CmImage* ptr3 = ptr2;
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.delete(ptr, 120uL);
						throw;
					}
					System.Runtime.CompilerServices.Unsafe.Write(mask, new Image(ptr2));
				}
				CmSimpleRegionDetector* native = GetNative();
				long num2 = *(long*)(*(long*)native + 32);
				CmImage* native2 = mask->GetNative();
				CmImage* native3 = newImage.GetNative();
				CmImage* native4 = oldImage.GetNative();
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmFlags_003Cenum_0020NmMediaProcessing_003A_003ACmSimpleRegionDetector_003A_003AmaskFlag_t_003E obj);
				result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, CmImage*, CmImage*, CmImage*, CmFlags_003Cenum_0020NmMediaProcessing_003A_003ACmSimpleRegionDetector_003A_003AmaskFlag_t_003E, double>)num2)((nint)native, native4, native3, native2, *ConvertMaskType(&obj, maskType));
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 8, &error) != 0)
			{
				uint num3 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new Error((exception*)error);
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
			GC.KeepAlive(oldImage);
			GC.KeepAlive(newImage);
			GC.KeepAlive(*mask);
			GC.KeepAlive(this);
			return result;
		}
		throw new ArgumentNullException();
	}

	public unsafe CmSimpleRegionDetector* GetNative()
	{
		CmSimpleRegionDetector* pDetector = m_pDetector;
		if (pDetector == null)
		{
			throw new InvalidOperationException();
		}
		return pDetector;
	}

	public unsafe static CmFlags_003Cenum_0020NmMediaProcessing_003A_003ACmSimpleRegionDetector_003A_003AmaskFlag_t_003E* ConvertMaskType(CmFlags_003Cenum_0020NmMediaProcessing_003A_003ACmSimpleRegionDetector_003A_003AmaskFlag_t_003E* P_0, maskType_t maskType)
	{
		*(int*)P_0 = 0;
		switch (maskType)
		{
		default:
			throw new ArgumentOutOfRangeException();
		case maskType_t.semitransparentGreen:
			*(int*)P_0 = 3;
			break;
		case maskType_t.semitransparentWhite:
			*(int*)P_0 = 2;
			break;
		case maskType_t.green:
			*(int*)P_0 = 1;
			break;
		case maskType_t.monochrome:
			break;
		}
		return P_0;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007ESimpleMotionDetector();
			return;
		}
		try
		{
			_0021SimpleMotionDetector();
		}
		finally
		{
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(A_0: true);
		GC.SuppressFinalize(this);
		GC.KeepAlive(this);
	}

	~SimpleMotionDetector()
	{
		Dispose(A_0: false);
	}
}
