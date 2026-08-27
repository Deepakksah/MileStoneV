using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using _003FA0x4cdda686;
using NmMediaProcessing;
using NmTimeStamp;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class RandomAccessVideoDecoder : IDisposable
{
	private unsafe CmRandomAccessVideoDecoder* m_pDecoder;

	private VideoDecoderParameters m_pParameters;

	private IVideoDataAccess m_pAccess;

	private unsafe CmUnmanagedVideoDataAccessProxy* m_pUnmanagedProxy;

	private CmManagedVideoDataAccessProxy m_pManagedProxy;

	public unsafe IVideoDataAccess VideoDataAccess
	{
		get
		{
			return m_pAccess;
		}
		set
		{
			//IL_0028: Expected I, but got I8
			//IL_001f: Expected I, but got I8
			if (m_pAccess != value)
			{
				CmUnmanagedVideoDataAccessProxy* pUnmanagedProxy = m_pUnmanagedProxy;
				if (pUnmanagedProxy != null)
				{
					long num = *(long*)(*(ulong*)pUnmanagedProxy);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)pUnmanagedProxy, 1u);
				}
				m_pUnmanagedProxy = null;
				if (m_pManagedProxy is IDisposable disposable)
				{
					disposable.Dispose();
				}
				m_pManagedProxy = null;
				m_pAccess = value;
			}
		}
	}

	public VideoDecoderParameters Parameters => m_pParameters;

	public unsafe void OnParametersChanged()
	{
		//IL_0008: Expected I8, but got I
		//IL_008e: Expected I, but got I8
		//IL_0064: Expected I, but got I8
		//IL_003e: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_pParameters == null)
		{
			throw new InvalidOperationException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmRandomAccessVideoDecoder* native = GetNative();
			long num2 = *(long*)(*(long*)native + 56);
			CmVideoDecoderParameters* native2 = m_pParameters.GetNative();
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, CmVideoDecoderParameters*, void>)num2)((nint)native, native2);
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
		GC.KeepAlive(m_pParameters);
		GC.KeepAlive(this);
	}

	public unsafe RandomAccessVideoDecoder()
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_0026: Expected I, but got I8
		//IL_00cf: Expected I, but got I8
		//IL_00a4: Expected I, but got I8
		//IL_0065: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pDecoder = null;
		m_pParameters = null;
		m_pAccess = null;
		m_pUnmanagedProxy = null;
		m_pManagedProxy = null;
		base._002Ector();
		m_pParameters = new VideoDecoderParameters(OnParametersChanged);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmRandomAccessVideoDecoder* ptr = (CmRandomAccessVideoDecoder*)global::_003CModule_003E.@new(192uL);
			CmRandomAccessVideoDecoder* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmRandomAccessVideoDecoder_002E_007Bctor_007D(ptr));
				CmRandomAccessVideoDecoder* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 192uL);
				throw;
			}
			m_pDecoder = ptr2;
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

	private void _007ERandomAccessVideoDecoder()
	{
		((IDisposable)m_pParameters)?.Dispose();
		m_pParameters = null;
		if (m_pManagedProxy is IDisposable disposable)
		{
			disposable.Dispose();
		}
		m_pManagedProxy = null;
		_0021RandomAccessVideoDecoder();
	}

	private unsafe void _0021RandomAccessVideoDecoder()
	{
		//IL_0023: Expected I, but got I8
		//IL_001a: Expected I, but got I8
		//IL_0042: Expected I, but got I8
		//IL_0039: Expected I, but got I8
		CmRandomAccessVideoDecoder* pDecoder = m_pDecoder;
		if (pDecoder != null)
		{
			long num = *(long*)(*(long*)pDecoder + 80);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)pDecoder, 1u);
		}
		m_pDecoder = null;
		CmUnmanagedVideoDataAccessProxy* pUnmanagedProxy = m_pUnmanagedProxy;
		if (pUnmanagedProxy != null)
		{
			long num2 = *(long*)(*(ulong*)pUnmanagedProxy);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num2)((nint)pUnmanagedProxy, 1u);
		}
		m_pUnmanagedProxy = null;
		m_pAccess = null;
		m_pParameters = null;
		m_pManagedProxy = null;
	}

	public unsafe Image GetImage(DateTime timeStamp, IVideoDataAccess.matchCriterion matchCriterion)
	{
		//IL_0009: Expected I8, but got I
		//IL_0125: Expected I, but got I8
		//IL_00f9: Expected I, but got I8
		//IL_0039: Expected I, but got I8
		//IL_0066: Expected I, but got I8
		//IL_00a4: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		Image result = null;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			if (!VerifyProxy())
			{
				CmRandomAccessVideoDecoder* native = GetNative();
				long num2 = *(long*)(*(long*)native + 48);
				CmUnmanagedVideoDataAccessProxy* pUnmanagedProxy = m_pUnmanagedProxy;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImVideoDataAccess*, void>)num2)((nint)native, (ImVideoDataAccess*)pUnmanagedProxy);
			}
			CmRandomAccessVideoDecoder* native2 = GetNative();
			long num3 = *(long*)(*(long*)native2 + 24);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
			NmTimeStamp.CmTimeStamp* ptr = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_002Cclass_0020System_003A_003ADateTime_003E(&cmTimeStamp, &timeStamp);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmImage cmImage);
			try
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, CmImage*, NmTimeStamp.CmTimeStamp, ImVideoDataAccess.timeMatch_t, CmImage*>)num3)((nint)native2, &cmImage, *ptr, (ImVideoDataAccess.timeMatch_t)matchCriterion);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
				throw;
			}
			try
			{
				global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
				m_pManagedProxy.ThrowPendingExceptionIfAny();
				CmImage* ptr2 = (CmImage*)global::_003CModule_003E.@new(120uL);
				CmImage* ptr3;
				try
				{
					ptr3 = ((ptr2 == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmImage_002E_007Bctor_007D(ptr2, &cmImage));
					CmImage* ptr4 = ptr3;
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.delete(ptr2, 120uL);
					throw;
				}
				result = new Image(ptr3);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmImage*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmImage_002E_007Bdtor_007D), &cmImage);
				throw;
			}
			global::_003CModule_003E.NmMediaProcessing_002ECmImage_002E_007Bdtor_007D(&cmImage);
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
		{
			uint num4 = 0u;
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
		GC.KeepAlive(this);
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool GetImage(DateTime timeStamp, IVideoDataAccess.matchCriterion matchCriterion, ref Image rpImage)
	{
		//IL_0009: Expected I8, but got I
		//IL_0154: Expected I, but got I8
		//IL_0128: Expected I, but got I8
		//IL_0036: Expected I, but got I8
		//IL_006b: Expected I, but got I8
		//IL_00d3: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			if (!VerifyProxy())
			{
				CmRandomAccessVideoDecoder* native = GetNative();
				long num2 = *(long*)(*(long*)native + 48);
				CmUnmanagedVideoDataAccessProxy* pUnmanagedProxy = m_pUnmanagedProxy;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImVideoDataAccess*, void>)num2)((nint)native, (ImVideoDataAccess*)pUnmanagedProxy);
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmImage cmImage);
			global::_003CModule_003E.NmMediaProcessing_002ECmImage_002E_007Bctor_007D(&cmImage);
			try
			{
				CmRandomAccessVideoDecoder* native2 = GetNative();
				long num3 = *(long*)(*(long*)native2 + 16);
				System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
				NmTimeStamp.CmTimeStamp* ptr = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_002Cclass_0020System_003A_003ADateTime_003E(&cmTimeStamp, &timeStamp);
				ImRandomAccessVideoDecoder.get_image_result_t get_image_result_t;
				try
				{
					get_image_result_t = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, NmTimeStamp.CmTimeStamp, ImVideoDataAccess.timeMatch_t, CmImage*, ImRandomAccessVideoDecoder.get_image_result_t>)num3)((nint)native2, *ptr, (ImVideoDataAccess.timeMatch_t)matchCriterion, &cmImage);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
					throw;
				}
				global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
				if (get_image_result_t != (ImRandomAccessVideoDecoder.get_image_result_t)1)
				{
					bool flag = false;
					goto IL_009e;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmImage*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmImage_002E_007Bdtor_007D), &cmImage);
				throw;
			}
			try
			{
				m_pManagedProxy.ThrowPendingExceptionIfAny();
				CmImage* ptr2 = (CmImage*)global::_003CModule_003E.@new(120uL);
				CmImage* ptr3;
				try
				{
					ptr3 = ((ptr2 == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmImage_002E_007Bctor_007D(ptr2, &cmImage));
					CmImage* ptr4 = ptr3;
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.delete(ptr2, 120uL);
					throw;
				}
				rpImage = new Image(ptr3);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmImage*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmImage_002E_007Bdtor_007D), &cmImage);
				throw;
			}
			global::_003CModule_003E.NmMediaProcessing_002ECmImage_002E_007Bdtor_007D(&cmImage);
			goto end_IL_0009;
			IL_009e:
			global::_003CModule_003E.NmMediaProcessing_002ECmImage_002E_007Bdtor_007D(&cmImage);
			return false;
			end_IL_0009:;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
		{
			uint num4 = 0u;
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
		GC.KeepAlive(this);
		return true;
	}

	public unsafe void Reset()
	{
		//IL_0012: Expected I, but got I8
		CmRandomAccessVideoDecoder* native = GetNative();
		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, void>)(*(ulong*)(*(long*)native + 72)))((nint)native);
		GC.KeepAlive(this);
	}

	public unsafe CmRandomAccessVideoDecoder* GetNative()
	{
		CmRandomAccessVideoDecoder* pDecoder = m_pDecoder;
		if (pDecoder == null)
		{
			throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.RandomAccessVideoDecoder");
		}
		return pDecoder;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool VerifyProxy()
	{
		//IL_0051: Expected I, but got I8
		if (m_pUnmanagedProxy != null)
		{
			return true;
		}
		CmUnmanagedVideoDataAccessProxy* ptr = (CmUnmanagedVideoDataAccessProxy*)global::_003CModule_003E.@new(32uL);
		CmUnmanagedVideoDataAccessProxy* pUnmanagedProxy;
		try
		{
			if (ptr != null)
			{
				*(long*)ptr = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_7ImVideoDataAccess_0040NmMediaProcessing_0040_00406B_0040);
				try
				{
					*(long*)ptr = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_7CmUnmanagedVideoDataAccessProxy_0040_003FA0x4cdda686_0040_00406B_0040);
					*(long*)((ulong)(nint)ptr + 8uL) = 0L;
					*(long*)((ulong)(nint)ptr + 16uL) = 0L;
					*(long*)((ulong)(nint)ptr + 24uL) = 0L;
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<ImVideoDataAccess*, void>)(&global::_003CModule_003E.NmMediaProcessing_002EImVideoDataAccess_002E_007Bdtor_007D), ptr);
					throw;
				}
				pUnmanagedProxy = ptr;
			}
			else
			{
				pUnmanagedProxy = null;
			}
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr, 32uL);
			throw;
		}
		m_pUnmanagedProxy = pUnmanagedProxy;
		m_pManagedProxy = new CmManagedVideoDataAccessProxy(m_pAccess, pUnmanagedProxy);
		return false;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007ERandomAccessVideoDecoder();
			return;
		}
		try
		{
			_0021RandomAccessVideoDecoder();
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

	~RandomAccessVideoDecoder()
	{
		Dispose(A_0: false);
	}
}
