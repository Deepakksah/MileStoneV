using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmGenericByteData;
using NmMediaProcessing;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class VideoSizeKeyFrameAnalyzer : IDisposable
{
	protected unsafe CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmVideoSizeKeyFrameAnalysisTraits_003E* m_pAnalyzer;

	public unsafe VideoSizeKeyFrameAnalyzer()
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_00b0: Expected I, but got I8
		//IL_0085: Expected I, but got I8
		//IL_0049: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pAnalyzer = null;
		base._002Ector();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmVideoSizeKeyFrameAnalysisTraits_003E* ptr = (CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmVideoSizeKeyFrameAnalysisTraits_003E*)global::_003CModule_003E.@new(16uL);
			CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmVideoSizeKeyFrameAnalysisTraits_003E* ptr3;
			try
			{
				if (ptr != null)
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out _Zero_then_variadic_args_t zero_then_variadic_args_t2);
					_Zero_then_variadic_args_t zero_then_variadic_args_t = zero_then_variadic_args_t2;
					*(long*)ptr = 0L;
					try
					{
						*(int*)((ulong)(nint)ptr + 8uL) = 0;
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<unique_ptr_003CNmMediaProcessing_003A_003AImVideoAnalyzer_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003AImVideoAnalyzer_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Eunique_ptr_003CNmMediaProcessing_003A_003AImVideoAnalyzer_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003AImVideoAnalyzer_003E_0020_003E_002E_007Bdtor_007D), ptr);
						throw;
					}
					CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmVideoSizeKeyFrameAnalysisTraits_003E* ptr2 = ptr;
					ptr3 = ptr;
				}
				else
				{
					ptr3 = null;
				}
				CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmVideoSizeKeyFrameAnalysisTraits_003E* ptr4 = ptr3;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 16uL);
				throw;
			}
			m_pAnalyzer = ptr3;
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

	private void _007EVideoSizeKeyFrameAnalyzer()
	{
		_0021VideoSizeKeyFrameAnalyzer();
	}

	private unsafe void _0021VideoSizeKeyFrameAnalyzer()
	{
		//IL_0021: Expected I, but got I8
		CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmVideoSizeKeyFrameAnalysisTraits_003E* pAnalyzer = m_pAnalyzer;
		if (pAnalyzer != null)
		{
			global::_003CModule_003E.std_002Eunique_ptr_003CNmMediaProcessing_003A_003AImVideoAnalyzer_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003AImVideoAnalyzer_003E_0020_003E_002E_007Bdtor_007D((unique_ptr_003CNmMediaProcessing_003A_003AImVideoAnalyzer_002Cstd_003A_003Adefault_delete_003CNmMediaProcessing_003A_003AImVideoAnalyzer_003E_0020_003E*)pAnalyzer);
			global::_003CModule_003E.delete(pAnalyzer, 16uL);
		}
		m_pAnalyzer = null;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool Analyze(GenericByteData keyFramePacket, out int width, out int height)
	{
		//IL_0008: Expected I8, but got I
		//IL_0083: Expected I, but got I8
		//IL_0059: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (keyFramePacket == null)
		{
			throw new ArgumentNullException();
		}
		bool flag = false;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmVideoSizeAnalysisResult cmVideoSizeAnalysisResult);
		global::_003CModule_003E.NmMediaProcessing_002ECmVideoSizeAnalysisResult_002E_007Bctor_007D(&cmVideoSizeAnalysisResult);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmVideoSizeKeyFrameAnalysisTraits_003E* native = GetNative();
			NmGenericByteData.CmGenericByteData* native2 = keyFramePacket.GetNative();
			flag = global::_003CModule_003E.NmMediaProcessing_002ECmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmVideoSizeKeyFrameAnalysisTraits_003E_002EAnalyze(native, native2, &cmVideoSizeAnalysisResult);
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
		if (flag)
		{
			width = (int)(*(long*)(&cmVideoSizeAnalysisResult));
			height = (int)System.Runtime.CompilerServices.Unsafe.As<CmVideoSizeAnalysisResult, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmVideoSizeAnalysisResult, 8));
		}
		GC.KeepAlive(keyFramePacket);
		GC.KeepAlive(this);
		return flag;
	}

	public unsafe CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmVideoSizeKeyFrameAnalysisTraits_003E* GetNative()
	{
		CmStreamAnalyzer_003CNmMediaProcessing_003A_003ASmVideoSizeKeyFrameAnalysisTraits_003E* pAnalyzer = m_pAnalyzer;
		if (pAnalyzer == null)
		{
			throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.VideoSizeKeyFrameAnalyzer");
		}
		return pAnalyzer;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_0021VideoSizeKeyFrameAnalyzer();
			return;
		}
		try
		{
			_0021VideoSizeKeyFrameAnalyzer();
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

	~VideoSizeKeyFrameAnalyzer()
	{
		Dispose(A_0: false);
	}
}
