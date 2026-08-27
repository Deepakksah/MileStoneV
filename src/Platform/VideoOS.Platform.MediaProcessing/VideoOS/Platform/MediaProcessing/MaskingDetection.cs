using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmMediaProcessing;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class MaskingDetection : RegionMotionDetection
{
	public unsafe MaskingDetection(double noiseSensitivity, double alarmThreshold)
	{
		//IL_0009: Expected I8, but got I
		//IL_011c: Expected I, but got I8
		//IL_00f0: Expected I, but got I8
		//IL_0033: Expected I, but got I8
		//IL_002b: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		//IL_007f: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		base._002Ector();
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmRegionMotionDetection* pDetector = m_pDetector;
				if (pDetector != null)
				{
					long num2 = *(long*)(*(ulong*)pDetector);
					void* ptr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num2)((nint)pDetector, 1u);
				}
				else
				{
					void* ptr = null;
				}
				CmMaskingDetection* ptr2 = (CmMaskingDetection*)global::_003CModule_003E.@new(112uL);
				CmMaskingDetection* ptr3;
				try
				{
					ptr3 = ((ptr2 == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmMaskingDetection_002E_007Bctor_007D(ptr2, noiseSensitivity, alarmThreshold));
					CmMaskingDetection* ptr4 = ptr3;
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.delete(ptr2, 112uL);
					throw;
				}
				m_pDetector = (CmRegionMotionDetection*)ptr3;
				CmMotionChecker* ptr5 = (CmMotionChecker*)global::_003CModule_003E.@new(80uL);
				CmMotionChecker* ptr6;
				try
				{
					ptr6 = ((ptr5 == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmMotionChecker_002E_007Bctor_007D(ptr5));
					CmMotionChecker* ptr7 = ptr6;
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.delete(ptr5, 80uL);
					throw;
				}
				m_pChecker = ptr6;
				global::_003CModule_003E.NmMediaProcessing_002ECmBaseMotionCheck_002ESetOnLevel((CmBaseMotionCheck*)ptr6, 0.5);
				global::_003CModule_003E.NmMediaProcessing_002ECmBaseMotionCheck_002ESetOffLevel((CmBaseMotionCheck*)m_pChecker, 0.5);
				global::_003CModule_003E.NmMediaProcessing_002ECmRegionMotionDetection_002EAddCheckWithOwnership(m_pDetector, (ImMotionCheck*)m_pChecker);
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
			GC.KeepAlive(this);
			return;
		}
		catch
		{
			//try-fault
			base.Dispose(A_0: true);
			throw;
		}
	}

	private void _007EMaskingDetection()
	{
	}

	private void _0021MaskingDetection()
	{
	}

	[HandleProcessCorruptedStateExceptions]
	protected override void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			try
			{
				return;
			}
			finally
			{
				base.Dispose(A_0: true);
			}
		}
		try
		{
		}
		finally
		{
			base.Dispose(A_0: false);
		}
	}
}
