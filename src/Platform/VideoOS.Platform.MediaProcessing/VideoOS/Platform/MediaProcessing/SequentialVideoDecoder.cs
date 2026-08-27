using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using _003FA0x4cdda686;
using NmGenericByteData;
using NmMediaProcessing;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class SequentialVideoDecoder : IDisposable
{
	protected unsafe CmSequentialVideoDecoder* m_pDecoder;

	protected VideoDecoderParameters m_pParameters;

	protected IImageConsumer m_pConsumer;

	public IImageConsumer Consumer
	{
		get
		{
			return m_pConsumer;
		}
		set
		{
			m_pConsumer = value;
		}
	}

	public VideoDecoderParameters Parameters => m_pParameters;

	public unsafe void OnParametersChanged()
	{
		//IL_0008: Expected I8, but got I
		//IL_007f: Expected I, but got I8
		//IL_0055: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_pParameters == null)
		{
			throw new InvalidOperationException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmSequentialVideoDecoder* native = GetNative();
			CmVideoDecoderParameters* native2 = m_pParameters.GetNative();
			global::_003CModule_003E.NmMediaProcessing_002ECmSequentialVideoDecoder_002ESetParameters(native, native2);
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
		GC.KeepAlive(m_pParameters);
		GC.KeepAlive(this);
	}

	public unsafe SequentialVideoDecoder()
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_00ba: Expected I, but got I8
		//IL_008f: Expected I, but got I8
		//IL_0053: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pDecoder = null;
		m_pParameters = null;
		m_pConsumer = null;
		base._002Ector();
		m_pParameters = new VideoDecoderParameters(OnParametersChanged);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmSequentialVideoDecoder* ptr = (CmSequentialVideoDecoder*)global::_003CModule_003E.@new(32uL);
			CmSequentialVideoDecoder* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmSequentialVideoDecoder_002E_007Bctor_007D(ptr));
				CmSequentialVideoDecoder* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 32uL);
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

	private void _007ESequentialVideoDecoder()
	{
		((IDisposable)m_pParameters)?.Dispose();
		m_pParameters = null;
		_0021SequentialVideoDecoder();
	}

	private unsafe void _0021SequentialVideoDecoder()
	{
		//IL_001a: Expected I, but got I8
		CmSequentialVideoDecoder* pDecoder = m_pDecoder;
		if (pDecoder != null)
		{
			global::_003CModule_003E.NmMediaProcessing_002ECmSequentialVideoDecoder_002E__delDtor(pDecoder, 1u);
		}
		m_pDecoder = null;
		m_pConsumer = null;
		m_pParameters = null;
	}

	public unsafe void Decode(GenericByteData byteData)
	{
		//IL_0008: Expected I8, but got I
		//IL_00be: Expected I, but got I8
		//IL_0094: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (byteData == null)
		{
			throw new ArgumentNullException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmUnmanagedImageConsumerProxy cmUnmanagedImageConsumerProxy);
			global::_003CModule_003E._003FA0x4cdda686_002ECmUnmanagedImageConsumerProxy_002E_007Bctor_007D(&cmUnmanagedImageConsumerProxy);
			try
			{
				CmManagedImageConsumerProxy cmManagedImageConsumerProxy = new CmManagedImageConsumerProxy(m_pConsumer, &cmUnmanagedImageConsumerProxy);
				global::_003CModule_003E.NmMediaProcessing_002ECmSequentialVideoDecoder_002ESetConsumer(GetNative(), (ImDecoderConsumer*)(&cmUnmanagedImageConsumerProxy));
				CmSequentialVideoDecoder* native = GetNative();
				NmGenericByteData.CmGenericByteData* native2 = byteData.GetNative();
				global::_003CModule_003E.NmMediaProcessing_002ECmSequentialVideoDecoder_002EDecode(native, native2);
				cmManagedImageConsumerProxy.ThrowPendingExceptionIfAny();
				GC.KeepAlive(cmManagedImageConsumerProxy);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmUnmanagedImageConsumerProxy*, void>)(&global::_003CModule_003E._003FA0x4cdda686_002ECmUnmanagedImageConsumerProxy_002E_007Bdtor_007D), &cmUnmanagedImageConsumerProxy);
				throw;
			}
			*(long*)(&cmUnmanagedImageConsumerProxy) = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_7ImDecoderConsumer_0040NmMediaProcessing_0040_00406B_0040);
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
		finally
		{
			global::_003CModule_003E.NmMediaProcessing_002ECmSequentialVideoDecoder_002EClearConsumer(GetNative());
		}
		GC.KeepAlive(byteData);
		GC.KeepAlive(this);
	}

	public unsafe void EndStream()
	{
		//IL_0008: Expected I8, but got I
		//IL_00aa: Expected I, but got I8
		//IL_0080: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmUnmanagedImageConsumerProxy cmUnmanagedImageConsumerProxy);
			global::_003CModule_003E._003FA0x4cdda686_002ECmUnmanagedImageConsumerProxy_002E_007Bctor_007D(&cmUnmanagedImageConsumerProxy);
			try
			{
				CmManagedImageConsumerProxy cmManagedImageConsumerProxy = new CmManagedImageConsumerProxy(m_pConsumer, &cmUnmanagedImageConsumerProxy);
				global::_003CModule_003E.NmMediaProcessing_002ECmSequentialVideoDecoder_002ESetConsumer(GetNative(), (ImDecoderConsumer*)(&cmUnmanagedImageConsumerProxy));
				global::_003CModule_003E.NmMediaProcessing_002ECmSequentialVideoDecoder_002EEndStream(GetNative());
				cmManagedImageConsumerProxy.ThrowPendingExceptionIfAny();
				GC.KeepAlive(cmManagedImageConsumerProxy);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmUnmanagedImageConsumerProxy*, void>)(&global::_003CModule_003E._003FA0x4cdda686_002ECmUnmanagedImageConsumerProxy_002E_007Bdtor_007D), &cmUnmanagedImageConsumerProxy);
				throw;
			}
			*(long*)(&cmUnmanagedImageConsumerProxy) = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_7ImDecoderConsumer_0040NmMediaProcessing_0040_00406B_0040);
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
		finally
		{
			global::_003CModule_003E.NmMediaProcessing_002ECmSequentialVideoDecoder_002EClearConsumer(GetNative());
		}
		GC.KeepAlive(this);
	}

	public unsafe void Reset()
	{
		global::_003CModule_003E.NmMediaProcessing_002ECmSequentialVideoDecoder_002EReset(GetNative());
		GC.KeepAlive(this);
	}

	public unsafe CmSequentialVideoDecoder* GetNative()
	{
		CmSequentialVideoDecoder* pDecoder = m_pDecoder;
		if (pDecoder == null)
		{
			throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.SequentialVideoDecoder");
		}
		return pDecoder;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007ESequentialVideoDecoder();
			return;
		}
		try
		{
			_0021SequentialVideoDecoder();
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

	~SequentialVideoDecoder()
	{
		Dispose(A_0: false);
	}
}
