using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmMediaProcessing;
using NmTimeStamp;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class StreamMotionDetectionParameters : IDisposable
{
	public enum detectionScope_t
	{
		allFrames,
		referenceFramesOnly,
		keyFramesOnly,
		lowDelay,
		autoSelectFromInterval
	}

	public enum detectionResolution_t
	{
		res_1_1 = 1,
		res_1_2 = 2,
		res_1_4 = 4,
		res_1_8 = 8,
		autoSelectFromInput = 9
	}

	public enum decoderMode_t
	{
		none = 0,
		ipp = 7,
		automatic = 1,
		autoNvidia = 1,
		onlyNvidia = 3,
		onlyIntel = 2
	}

	private unsafe CmStreamMotionDetectionParameters* m_pParameters;

	private DetectionParametersChanged m_listener;

	public unsafe double motionLevelGridSensitivity
	{
		get
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			double result = (int)(*(double*)((ulong)(nint)pParameters + 64uL));
			GC.KeepAlive(this);
			return result;
		}
		set
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			*(double*)((ulong)(nint)pParameters + 64uL) = value;
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe bool motionLevelGridAutomaticSensitivity
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			bool result = *(bool*)((ulong)(nint)pParameters + 56uL);
			GC.KeepAlive(this);
			return result;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			*(bool*)((ulong)(nint)pParameters + 56uL) = value;
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe int motionLevelGridHeight
	{
		get
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			int result = (int)(*(long*)((ulong)(nint)pParameters + 48uL));
			GC.KeepAlive(this);
			return result;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentException("Grid height must not be negative");
			}
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			*(long*)((ulong)(nint)pParameters + 48uL) = value;
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe int motionLevelGridWidth
	{
		get
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			int result = (int)(*(long*)((ulong)(nint)pParameters + 40uL));
			GC.KeepAlive(this);
			return result;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentException("Grid width must not be be negative");
			}
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			*(long*)((ulong)(nint)pParameters + 40uL) = value;
			GC.KeepAlive(this);
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe int motionHistogramGridHeight
	{
		get
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			int result = (int)(*(long*)((ulong)(nint)pParameters + 32uL));
			GC.KeepAlive(this);
			return result;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentException("Grid height must not be negative");
			}
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			*(long*)((ulong)(nint)pParameters + 32uL) = value;
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe int motionHistogramGridWidth
	{
		get
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			int result = (int)(*(long*)((ulong)(nint)pParameters + 24uL));
			GC.KeepAlive(this);
			return result;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentException("Grid width must not be negative");
			}
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			*(long*)((ulong)(nint)pParameters + 24uL) = value;
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe bool automaticSensitivity
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			bool result = *(bool*)((ulong)(nint)pParameters + 18uL);
			GC.KeepAlive(this);
			return result;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			*(bool*)((ulong)(nint)pParameters + 18uL) = value;
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe bool forceLowDelayDecoding
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			bool result = *(bool*)((ulong)(nint)pParameters + 17uL);
			GC.KeepAlive(this);
			return result;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			*(bool*)((ulong)(nint)pParameters + 17uL) = value;
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe bool provideFullColorImage
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			bool result = *(bool*)((ulong)(nint)pParameters + 16uL);
			GC.KeepAlive(this);
			return result;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			*(bool*)((ulong)(nint)pParameters + 16uL) = value;
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe decoderMode_t decoderMode
	{
		get
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			int result = *(int*)((ulong)(nint)pParameters + 72uL);
			GC.KeepAlive(this);
			return (decoderMode_t)result;
		}
		set
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			*(decoderMode_t*)((ulong)(nint)pParameters + 72uL) = value;
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe detectionResolution_t detectionResolution
	{
		get
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			int result = *(int*)((ulong)(nint)pParameters + 12uL);
			GC.KeepAlive(this);
			return (detectionResolution_t)result;
		}
		set
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			*(detectionResolution_t*)((ulong)(nint)pParameters + 12uL) = value;
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe detectionScope_t detectionScope
	{
		get
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			int result = *(int*)((ulong)(nint)pParameters + 8uL);
			GC.KeepAlive(this);
			return (detectionScope_t)result;
		}
		set
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			*(detectionScope_t*)((ulong)(nint)pParameters + 8uL) = value;
			m_listener?.Invoke();
			GC.KeepAlive(this);
		}
	}

	public unsafe TimeSpan minimumInterval
	{
		get
		{
			CmStreamMotionDetectionParameters* pParameters = m_pParameters;
			if (pParameters == null)
			{
				throw new InvalidOperationException();
			}
			CmStreamMotionDetectionParameters* ptr = pParameters;
			TimeSpan result = new TimeSpan(0, 0, 0, 0, (int)global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002EGetMilliseconds((CmTimeSpan*)ptr));
			GC.KeepAlive(this);
			return result;
		}
		set
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmTimeSpan cmTimeSpan);
			CmTimeSpan* ptr = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeSpan_002Cclass_0020System_003A_003ATimeSpan_003E(&cmTimeSpan, &value);
			try
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out _0024VCls_00240000000080 _0024VCls_002481);
				// IL cpblk instruction
				System.Runtime.CompilerServices.Unsafe.CopyBlock(ref _0024VCls_002481, ptr, 8);
				CmStreamMotionDetectionParameters* pParameters = m_pParameters;
				if (pParameters == null)
				{
					throw new InvalidOperationException();
				}
				// IL cpblk instruction
				System.Runtime.CompilerServices.Unsafe.CopyBlock(pParameters, ref _0024VCls_002481, 8);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTimeSpan*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D), &cmTimeSpan);
				throw;
			}
			global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D(&cmTimeSpan);
			m_listener?.Invoke();
			GC.KeepAlive(this);
			try
			{
				return;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTimeSpan*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D), &cmTimeSpan);
				throw;
			}
		}
	}

	public unsafe StreamMotionDetectionParameters(DetectionParametersChanged listener)
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
			CmStreamMotionDetectionParameters* ptr = (CmStreamMotionDetectionParameters*)global::_003CModule_003E.@new(80uL);
			CmStreamMotionDetectionParameters* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetectionParameters_002E_007Bctor_007D(ptr));
				CmStreamMotionDetectionParameters* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 80uL);
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
		GC.KeepAlive(this);
	}

	public unsafe StreamMotionDetectionParameters()
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
			CmStreamMotionDetectionParameters* ptr = (CmStreamMotionDetectionParameters*)global::_003CModule_003E.@new(80uL);
			CmStreamMotionDetectionParameters* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetectionParameters_002E_007Bctor_007D(ptr));
				CmStreamMotionDetectionParameters* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 80uL);
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

	private void _007EStreamMotionDetectionParameters()
	{
		_0021StreamMotionDetectionParameters();
	}

	private unsafe void _0021StreamMotionDetectionParameters()
	{
		//IL_0021: Expected I, but got I8
		CmStreamMotionDetectionParameters* pParameters = m_pParameters;
		if (pParameters != null)
		{
			global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D((CmTimeSpan*)pParameters);
			global::_003CModule_003E.delete(pParameters, 80uL);
		}
		m_pParameters = null;
		m_listener = null;
	}

	public unsafe CmStreamMotionDetectionParameters* GetNative()
	{
		CmStreamMotionDetectionParameters* pParameters = m_pParameters;
		if (pParameters == null)
		{
			throw new InvalidOperationException();
		}
		return pParameters;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007EStreamMotionDetectionParameters();
			return;
		}
		try
		{
			_0021StreamMotionDetectionParameters();
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

	~StreamMotionDetectionParameters()
	{
		Dispose(A_0: false);
	}
}
