using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmMediaProcessing;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class VideoDecoderParameters : IDisposable
{
	public enum decodingScope_t
	{
		allFrames,
		referenceFramesOnly,
		keyFramesOnly
	}

	public enum conformanceCheck_t
	{
		relaxed,
		strictHeaders
	}

	public enum resultResolution_t
	{
		res_1_1 = 1,
		res_1_2 = 2,
		res_1_4 = 4,
		res_1_8 = 8
	}

	public enum decoderImplementation_t
	{
		none,
		autoNvidia,
		onlyIntel,
		onlyNvidia,
		hardwareIntel,
		softwareIntel,
		hardwareNvidia,
		ipp
	}

	private unsafe CmVideoDecoderParameters* m_pParameters;

	private DecodingParametersChanged m_listener;

	public unsafe decoderImplementation_t Implementation
	{
		get
		{
			int result = *(int*)((ulong)(nint)GetNative() + 24uL);
			GC.KeepAlive(this);
			return (decoderImplementation_t)result;
		}
		set
		{
			*(decoderImplementation_t*)((ulong)(nint)GetNative() + 24uL) = value;
			GC.KeepAlive(this);
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe int NumberOfThreads
	{
		get
		{
			int result = *(int*)((ulong)(nint)GetNative() + 20uL);
			GC.KeepAlive(this);
			return result;
		}
		set
		{
			*(int*)((ulong)(nint)GetNative() + 20uL) = value;
			GC.KeepAlive(this);
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe bool LuminanceOnly
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			bool result = *(bool*)((ulong)(nint)GetNative() + 15uL);
			GC.KeepAlive(this);
			return result;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			*(bool*)((ulong)(nint)GetNative() + 15uL) = value;
			GC.KeepAlive(this);
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe bool LowDelay
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			bool result = *(bool*)((ulong)(nint)GetNative() + 14uL);
			GC.KeepAlive(this);
			return result;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			*(bool*)((ulong)(nint)GetNative() + 14uL) = value;
			GC.KeepAlive(this);
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe bool SplitFields
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			bool result = *(bool*)((ulong)(nint)GetNative() + 13uL);
			GC.KeepAlive(this);
			return result;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			*(bool*)((ulong)(nint)GetNative() + 13uL) = value;
			GC.KeepAlive(this);
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe bool ProduceImage
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			bool result = *(bool*)((ulong)(nint)GetNative() + 12uL);
			GC.KeepAlive(this);
			return result;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			*(bool*)((ulong)(nint)GetNative() + 12uL) = value;
			GC.KeepAlive(this);
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe resultResolution_t ResultResolution
	{
		get
		{
			int result = *(int*)((ulong)(nint)GetNative() + 16uL);
			GC.KeepAlive(this);
			return (resultResolution_t)result;
		}
		set
		{
			*(resultResolution_t*)((ulong)(nint)GetNative() + 16uL) = value;
			GC.KeepAlive(this);
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe conformanceCheck_t ConformanceCheck
	{
		get
		{
			int result = *(int*)((ulong)(nint)GetNative() + 8uL);
			GC.KeepAlive(this);
			return (conformanceCheck_t)result;
		}
		set
		{
			*(conformanceCheck_t*)((ulong)(nint)GetNative() + 8uL) = value;
			GC.KeepAlive(this);
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe decodingScope_t DecodingScope
	{
		get
		{
			int native = *(int*)GetNative();
			GC.KeepAlive(this);
			return (decodingScope_t)native;
		}
		set
		{
			*(decodingScope_t*)GetNative() = value;
			GC.KeepAlive(this);
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe bool UseOnlyIntegratedGPU
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			bool result = *(bool*)((ulong)(nint)GetNative() + 48uL);
			GC.KeepAlive(this);
			return result;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			*(bool*)((ulong)(nint)GetNative() + 48uL) = value;
			GC.KeepAlive(this);
		}
	}

	public unsafe VideoDecoderParameters(DecodingParametersChanged listener)
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_00ab: Expected I, but got I8
		//IL_0080: Expected I, but got I8
		//IL_0044: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pParameters = null;
		m_listener = listener;
		base._002Ector();
		if (m_listener == null)
		{
			throw new ArgumentNullException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmVideoDecoderParameters* ptr = (CmVideoDecoderParameters*)global::_003CModule_003E.@new(52uL);
			CmVideoDecoderParameters* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmVideoDecoderParameters_002E_007Bctor_007D(ptr));
				CmVideoDecoderParameters* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 52uL);
				throw;
			}
			m_pParameters = ptr2;
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

	public unsafe VideoDecoderParameters()
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_009c: Expected I, but got I8
		//IL_0071: Expected I, but got I8
		//IL_0035: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pParameters = null;
		m_listener = null;
		base._002Ector();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmVideoDecoderParameters* ptr = (CmVideoDecoderParameters*)global::_003CModule_003E.@new(52uL);
			CmVideoDecoderParameters* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmVideoDecoderParameters_002E_007Bctor_007D(ptr));
				CmVideoDecoderParameters* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 52uL);
				throw;
			}
			m_pParameters = ptr2;
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

	private void _007EVideoDecoderParameters()
	{
		_0021VideoDecoderParameters();
	}

	private unsafe void _0021VideoDecoderParameters()
	{
		//IL_0016: Expected I, but got I8
		global::_003CModule_003E.delete(m_pParameters, 52uL);
		m_pParameters = null;
	}

	public unsafe CmVideoDecoderParameters* GetNative()
	{
		CmVideoDecoderParameters* pParameters = m_pParameters;
		if (pParameters == null)
		{
			throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.VideoDecoderParameters");
		}
		return pParameters;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_0021VideoDecoderParameters();
			return;
		}
		try
		{
			_0021VideoDecoderParameters();
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

	~VideoDecoderParameters()
	{
		Dispose(A_0: false);
	}
}
