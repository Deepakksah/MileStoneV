using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmGenericByteData;
using NmMediaProcessing;
using NmTimeStamp;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class ByteDataHeaderAnalyzer : IDisposable
{
	private unsafe CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E* m_StreamAnalyzer;

	private streamAnalyserErrorCode_t m_ErrorCode;

	public unsafe ByteDataHeaderAnalyzer()
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_00a2: Expected I, but got I8
		//IL_0077: Expected I, but got I8
		//IL_003b: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_StreamAnalyzer = null;
		m_ErrorCode = (streamAnalyserErrorCode_t)0;
		base._002Ector();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E* ptr = (CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E*)global::_003CModule_003E.@new(16uL);
			CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E* ptr3;
			try
			{
				if (ptr != null)
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out _Zero_then_variadic_args_t zero_then_variadic_args_t2);
					_Zero_then_variadic_args_t zero_then_variadic_args_t = zero_then_variadic_args_t2;
					*(long*)ptr = 0L;
					CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E* ptr2 = ptr;
					ptr3 = ptr;
				}
				else
				{
					ptr3 = null;
				}
				CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E* ptr4 = ptr3;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 16uL);
				throw;
			}
			m_StreamAnalyzer = ptr3;
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

	private unsafe void _007EByteDataHeaderAnalyzer()
	{
		//IL_001a: Expected I, but got I8
		CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E* streamAnalyzer = m_StreamAnalyzer;
		if (streamAnalyzer != null)
		{
			global::_003CModule_003E.NmMediaProcessing_002ECmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E_002E__delDtor(streamAnalyzer, 1u);
		}
		m_StreamAnalyzer = null;
	}

	private unsafe void _0021ByteDataHeaderAnalyzer()
	{
		//IL_001a: Expected I, but got I8
		CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E* streamAnalyzer = m_StreamAnalyzer;
		if (streamAnalyzer != null)
		{
			global::_003CModule_003E.NmMediaProcessing_002ECmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E_002E__delDtor(streamAnalyzer, 1u);
		}
		m_StreamAnalyzer = null;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool Analyze(ReadOnlySharedBuffer unmanagedData, out ByteDataHeaderAnalysisResult analysisResult)
	{
		//IL_0008: Expected I8, but got I
		//IL_00ac: Expected I, but got I8
		//IL_0072: Expected I, but got I8
		//IL_0082: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		bool flag = false;
		if (m_StreamAnalyzer != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmByteDataHeaderAnalysisResult cmByteDataHeaderAnalysisResult);
			global::_003CModule_003E.NmMediaProcessing_002ECmByteDataHeaderAnalysisResult_002E_007Bctor_007D(&cmByteDataHeaderAnalysisResult);
			try
			{
				try
				{
					CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E* streamAnalyzer = m_StreamAnalyzer;
					ulong length = unmanagedData.Length;
					byte* data = unmanagedData.Data;
					flag = global::_003CModule_003E.NmMediaProcessing_002ECmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E_002EAnalyze(streamAnalyzer, data, length, &cmByteDataHeaderAnalysisResult);
					if (flag)
					{
						(analysisResult = new ByteDataHeaderAnalysisResult()).Set(&cmByteDataHeaderAnalysisResult);
					}
					else
					{
						m_ErrorCode = (streamAnalyserErrorCode_t)1;
					}
				}
				catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAV_003F_0024CmGenericError_0040VCmIntegrityErrorCode_0040NmError_0040_0040VCmError_00402_0040_0040NmError_0040_0040_00408), 9, null) != 0)
				{
					uint num2 = 0u;
					global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
					try
					{
						try
						{
							m_ErrorCode = (streamAnalyserErrorCode_t)1;
							goto end_IL_0083;
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
						end_IL_0083:;
					}
					finally
					{
						global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
					}
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmByteDataHeaderAnalysisResult*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmByteDataHeaderAnalysisResult_002E_007Bdtor_007D), &cmByteDataHeaderAnalysisResult);
				throw;
			}
			global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D((NmTimeStamp.CmTimeStamp*)(&cmByteDataHeaderAnalysisResult));
		}
		else
		{
			m_ErrorCode = (streamAnalyserErrorCode_t)1;
		}
		GC.KeepAlive(unmanagedData);
		GC.KeepAlive(analysisResult);
		GC.KeepAlive(this);
		return flag;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool Analyze(byte[] byteArray, int byteArrayLength, out ByteDataHeaderAnalysisResult analysisResult)
	{
		//IL_0008: Expected I8, but got I
		//IL_00a5: Expected I, but got I8
		//IL_006b: Expected I, but got I8
		//IL_007b: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		bool flag = false;
		if (m_StreamAnalyzer != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmByteDataHeaderAnalysisResult cmByteDataHeaderAnalysisResult);
			global::_003CModule_003E.NmMediaProcessing_002ECmByteDataHeaderAnalysisResult_002E_007Bctor_007D(&cmByteDataHeaderAnalysisResult);
			try
			{
				try
				{
					fixed (byte* pByteData = &byteArray[0])
					{
						flag = global::_003CModule_003E.NmMediaProcessing_002ECmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E_002EAnalyze(m_StreamAnalyzer, pByteData, (ulong)byteArrayLength, &cmByteDataHeaderAnalysisResult);
						if (flag)
						{
							(analysisResult = new ByteDataHeaderAnalysisResult()).Set(&cmByteDataHeaderAnalysisResult);
						}
						else
						{
							m_ErrorCode = (streamAnalyserErrorCode_t)1;
						}
					}
				}
				catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAV_003F_0024CmGenericError_0040VCmIntegrityErrorCode_0040NmError_0040_0040VCmError_00402_0040_0040NmError_0040_0040_00408), 9, null) != 0)
				{
					uint num2 = 0u;
					global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
					try
					{
						try
						{
							m_ErrorCode = (streamAnalyserErrorCode_t)1;
							goto end_IL_007c;
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
						end_IL_007c:;
					}
					finally
					{
						global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
					}
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmByteDataHeaderAnalysisResult*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmByteDataHeaderAnalysisResult_002E_007Bdtor_007D), &cmByteDataHeaderAnalysisResult);
				throw;
			}
			global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D((NmTimeStamp.CmTimeStamp*)(&cmByteDataHeaderAnalysisResult));
		}
		else
		{
			m_ErrorCode = (streamAnalyserErrorCode_t)1;
		}
		GC.KeepAlive(byteArray);
		GC.KeepAlive(analysisResult);
		GC.KeepAlive(this);
		return flag;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool Analyze(GenericByteData byteData, out ByteDataHeaderAnalysisResult analysisResult)
	{
		//IL_0008: Expected I8, but got I
		//IL_00a2: Expected I, but got I8
		//IL_0068: Expected I, but got I8
		//IL_0078: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		bool flag = false;
		if (m_StreamAnalyzer != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmByteDataHeaderAnalysisResult cmByteDataHeaderAnalysisResult);
			global::_003CModule_003E.NmMediaProcessing_002ECmByteDataHeaderAnalysisResult_002E_007Bctor_007D(&cmByteDataHeaderAnalysisResult);
			try
			{
				try
				{
					CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E* streamAnalyzer = m_StreamAnalyzer;
					NmGenericByteData.CmGenericByteData* native = byteData.GetNative();
					flag = global::_003CModule_003E.NmMediaProcessing_002ECmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmByteDataHeaderAnalysisTraits_003E_002EAnalyze(streamAnalyzer, native, &cmByteDataHeaderAnalysisResult);
					if (flag)
					{
						(analysisResult = new ByteDataHeaderAnalysisResult()).Set(&cmByteDataHeaderAnalysisResult);
					}
					else
					{
						m_ErrorCode = (streamAnalyserErrorCode_t)1;
					}
				}
				catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAV_003F_0024CmGenericError_0040VCmIntegrityErrorCode_0040NmError_0040_0040VCmError_00402_0040_0040NmError_0040_0040_00408), 9, null) != 0)
				{
					uint num2 = 0u;
					global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
					try
					{
						try
						{
							m_ErrorCode = (streamAnalyserErrorCode_t)1;
							goto end_IL_0079;
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
						end_IL_0079:;
					}
					finally
					{
						global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
					}
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmByteDataHeaderAnalysisResult*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmByteDataHeaderAnalysisResult_002E_007Bdtor_007D), &cmByteDataHeaderAnalysisResult);
				throw;
			}
			global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D((NmTimeStamp.CmTimeStamp*)(&cmByteDataHeaderAnalysisResult));
		}
		else
		{
			m_ErrorCode = (streamAnalyserErrorCode_t)1;
		}
		GC.KeepAlive(byteData);
		GC.KeepAlive(analysisResult);
		GC.KeepAlive(this);
		return flag;
	}

	public int GetErrorCode()
	{
		return (int)m_ErrorCode;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007EByteDataHeaderAnalyzer();
			return;
		}
		try
		{
			_0021ByteDataHeaderAnalyzer();
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

	~ByteDataHeaderAnalyzer()
	{
		Dispose(A_0: false);
	}
}
