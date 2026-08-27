using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmCommon;
using NmMediaProcessing;
using NmTimeStamp;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class RegionMotionDetection : IDisposable
{
	protected unsafe CmRegionMotionDetection* m_pDetector;

	protected unsafe CmMotionChecker* m_pChecker;

	protected bool m_bOwned;

	public unsafe TimeSpan offDelay
	{
		set
		{
			//IL_0008: Expected I8, but got I
			//IL_0089: Expected I, but got I8
			//IL_005f: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmMotionChecker* checker = GetChecker();
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmTimeSpan cmTimeSpan);
				CmTimeSpan* ptr = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeSpan_002Cclass_0020System_003A_003ATimeSpan_003E(&cmTimeSpan, &value);
				try
				{
					global::_003CModule_003E.NmMediaProcessing_002ECmBaseMotionCheck_002ESetOffDelay((CmBaseMotionCheck*)checker, ptr);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTimeSpan*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D), &cmTimeSpan);
					throw;
				}
				global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D(&cmTimeSpan);
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
	}

	public unsafe TimeSpan onDelay
	{
		set
		{
			//IL_0008: Expected I8, but got I
			//IL_0089: Expected I, but got I8
			//IL_005f: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmMotionChecker* checker = GetChecker();
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmTimeSpan cmTimeSpan);
				CmTimeSpan* ptr = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeSpan_002Cclass_0020System_003A_003ATimeSpan_003E(&cmTimeSpan, &value);
				try
				{
					global::_003CModule_003E.NmMediaProcessing_002ECmBaseMotionCheck_002ESetOnDelay((CmBaseMotionCheck*)checker, ptr);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTimeSpan*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D), &cmTimeSpan);
					throw;
				}
				global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D(&cmTimeSpan);
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
	}

	public unsafe double offLevel
	{
		set
		{
			//IL_0008: Expected I8, but got I
			//IL_0064: Expected I, but got I8
			//IL_003a: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				global::_003CModule_003E.NmMediaProcessing_002ECmBaseMotionCheck_002ESetOffLevel((CmBaseMotionCheck*)GetChecker(), value);
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
	}

	public unsafe double onLevel
	{
		set
		{
			//IL_0008: Expected I8, but got I
			//IL_0064: Expected I, but got I8
			//IL_003a: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				global::_003CModule_003E.NmMediaProcessing_002ECmBaseMotionCheck_002ESetOnLevel((CmBaseMotionCheck*)GetChecker(), value);
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
	}

	public unsafe RegionMotionDetection()
	{
		//IL_0009: Expected I8, but got I
		//IL_0011: Expected I, but got I8
		//IL_0019: Expected I, but got I8
		//IL_00e5: Expected I, but got I8
		//IL_00b9: Expected I, but got I8
		//IL_0041: Expected I, but got I8
		//IL_0070: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pDetector = null;
		m_pChecker = null;
		m_bOwned = false;
		base._002Ector();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmRegionMotionDetection* ptr = (CmRegionMotionDetection*)global::_003CModule_003E.@new(56uL);
			CmRegionMotionDetection* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmRegionMotionDetection_002E_007Bctor_007D(ptr));
				CmRegionMotionDetection* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 56uL);
				throw;
			}
			m_pDetector = ptr2;
			CmMotionChecker* ptr4 = (CmMotionChecker*)global::_003CModule_003E.@new(80uL);
			CmMotionChecker* ptr5;
			try
			{
				ptr5 = ((ptr4 == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmMotionChecker_002E_007Bctor_007D(ptr4));
				CmMotionChecker* ptr6 = ptr5;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr4, 80uL);
				throw;
			}
			m_pChecker = ptr5;
			global::_003CModule_003E.NmMediaProcessing_002ECmRegionMotionDetection_002EAddCheckWithOwnership(m_pDetector, (ImMotionCheck*)ptr5);
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

	private void _007ERegionMotionDetection()
	{
		_0021RegionMotionDetection();
	}

	private unsafe void _0021RegionMotionDetection()
	{
		//IL_0008: Expected I, but got I8
		//IL_002f: Expected I, but got I8
		//IL_0026: Expected I, but got I8
		m_pChecker = null;
		if (!m_bOwned)
		{
			CmRegionMotionDetection* pDetector = m_pDetector;
			if (pDetector != null)
			{
				long num = *(long*)(*(ulong*)pDetector);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)pDetector, 1u);
			}
		}
		m_pDetector = null;
	}

	public unsafe void AddSimpleRegion(SimpleMotionDetector region)
	{
		//IL_0008: Expected I8, but got I
		//IL_0075: Expected I, but got I8
		//IL_004b: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (region == null)
		{
			throw new ArgumentNullException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmRegionMotionDetection* native = GetNative();
			CmSimpleRegionDetector* native2 = region.GetNative();
			global::_003CModule_003E.NmMediaProcessing_002ECmRegionMotionDetection_002EAddSimpleRegion(native, native2);
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

	public unsafe MotionDetectionResult Check(Image oldImage, Image newImage)
	{
		//Discarded unreachable code: IL_010c
		//IL_0008: Expected I8, but got I
		//IL_00b0: Expected I, but got I8
		//IL_0085: Expected I, but got I8
		//IL_00cf: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (oldImage != null && newImage != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMotionDetectionResult cmMotionDetectionResult);
			*(long*)(&cmMotionDetectionResult) = 0L;
			System.Runtime.CompilerServices.Unsafe.As<CmMotionDetectionResult, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmMotionDetectionResult, 8)) = 0L;
			System.Runtime.CompilerServices.Unsafe.As<CmMotionDetectionResult, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmMotionDetectionResult, 16)) = 0L;
			MotionDetectionResult result;
			try
			{
				CmMotionChecker* pChecker = m_pChecker;
				if (pChecker == null)
				{
					throw new InvalidOperationException();
				}
				global::_003CModule_003E.NmMediaProcessing_002ECmMotionChecker_002ESetResultContainer(pChecker, &cmMotionDetectionResult);
				System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
				try
				{
					CmRegionMotionDetection* native = GetNative();
					CmImage* native2 = newImage.GetNative();
					CmImage* native3 = oldImage.GetNative();
					global::_003CModule_003E.NmMediaProcessing_002ECmRegionMotionDetection_002ECheckMotionLevel(native, native3, native2);
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
					pChecker = m_pChecker;
					if (pChecker == null)
					{
						throw new InvalidOperationException();
					}
					global::_003CModule_003E.NmMediaProcessing_002ECmMotionChecker_002ESetResultContainer(pChecker, null);
				}
				GC.KeepAlive(oldImage);
				GC.KeepAlive(newImage);
				GC.KeepAlive(this);
				result = new MotionDetectionResult(&cmMotionDetectionResult);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmMotionDetectionResult*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmMotionDetectionResult_002E_007Bdtor_007D), &cmMotionDetectionResult);
				throw;
			}
			global::_003CModule_003E.std_002Evector_003CNmMediaProcessing_003A_003ACmMotionDetectionResult_003A_003ASmMotionDetectionEvent_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMotionDetectionResult_003A_003ASmMotionDetectionEvent_003E_0020_003E_002E_Tidy((vector_003CNmMediaProcessing_003A_003ACmMotionDetectionResult_003A_003ASmMotionDetectionEvent_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMotionDetectionResult_003A_003ASmMotionDetectionEvent_003E_0020_003E*)(&cmMotionDetectionResult));
			return result;
		}
		throw new ArgumentNullException();
	}

	public unsafe MotionDetectionResult CheckAndGetMask(Image oldImage, Image newImage, SimpleMotionDetector.maskType_t maskType, Image* mask)
	{
		//Discarded unreachable code: IL_01ab
		//IL_0009: Expected I8, but got I
		//IL_0147: Expected I, but got I8
		//IL_011b: Expected I, but got I8
		//IL_0166: Expected I, but got I8
		//IL_00be: Expected I, but got I8
		//IL_005e: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (oldImage != null && newImage != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMotionDetectionResult cmMotionDetectionResult);
			*(long*)(&cmMotionDetectionResult) = 0L;
			System.Runtime.CompilerServices.Unsafe.As<CmMotionDetectionResult, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmMotionDetectionResult, 8)) = 0L;
			System.Runtime.CompilerServices.Unsafe.As<CmMotionDetectionResult, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmMotionDetectionResult, 16)) = 0L;
			MotionDetectionResult result;
			try
			{
				CmMotionChecker* pChecker = m_pChecker;
				if (pChecker == null)
				{
					throw new InvalidOperationException();
				}
				global::_003CModule_003E.NmMediaProcessing_002ECmMotionChecker_002ESetResultContainer(pChecker, &cmMotionDetectionResult);
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
					CmRegionMotionDetection* native = GetNative();
					long num2 = *(long*)(*(long*)native + 16);
					CmImage* native2 = mask->GetNative();
					CmImage* native3 = newImage.GetNative();
					CmImage* native4 = oldImage.GetNative();
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CmFlags_003Cenum_0020NmMediaProcessing_003A_003ACmSimpleRegionDetector_003A_003AmaskFlag_t_003E obj);
					double num3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, CmImage*, CmImage*, CmImage*, CmFlags_003Cenum_0020NmMediaProcessing_003A_003ACmSimpleRegionDetector_003A_003AmaskFlag_t_003E, double>)num2)((nint)native, native4, native3, native2, *SimpleMotionDetector.ConvertMaskType(&obj, maskType));
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CmBasicMotionSourceInformation cmBasicMotionSourceInformation);
					global::_003CModule_003E.NmMediaProcessing_002ECmBasicMotionSourceInformation_002E_007Bctor_007D(&cmBasicMotionSourceInformation, newImage.GetNative());
					try
					{
						global::_003CModule_003E.NmMediaProcessing_002ECmRegionMotionDetection_002ECheckMotionLevel(GetNative(), num3, (ImBasicMotionSourceInformation*)(&cmBasicMotionSourceInformation));
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmBasicMotionSourceInformation*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmBasicMotionSourceInformation_002E_007Bdtor_007D), &cmBasicMotionSourceInformation);
						throw;
					}
					global::_003CModule_003E.NmMediaProcessing_002ECmBasicMotionSourceInformation_002E_007Bdtor_007D(&cmBasicMotionSourceInformation);
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
				finally
				{
					pChecker = m_pChecker;
					if (pChecker == null)
					{
						throw new InvalidOperationException();
					}
					global::_003CModule_003E.NmMediaProcessing_002ECmMotionChecker_002ESetResultContainer(pChecker, null);
				}
				GC.KeepAlive(oldImage);
				GC.KeepAlive(newImage);
				GC.KeepAlive(*mask);
				GC.KeepAlive(this);
				result = new MotionDetectionResult(&cmMotionDetectionResult);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmMotionDetectionResult*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmMotionDetectionResult_002E_007Bdtor_007D), &cmMotionDetectionResult);
				throw;
			}
			global::_003CModule_003E.std_002Evector_003CNmMediaProcessing_003A_003ACmMotionDetectionResult_003A_003ASmMotionDetectionEvent_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMotionDetectionResult_003A_003ASmMotionDetectionEvent_003E_0020_003E_002E_Tidy((vector_003CNmMediaProcessing_003A_003ACmMotionDetectionResult_003A_003ASmMotionDetectionEvent_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMotionDetectionResult_003A_003ASmMotionDetectionEvent_003E_0020_003E*)(&cmMotionDetectionResult));
			return result;
		}
		throw new ArgumentNullException();
	}

	public unsafe Image GetMask(Image oldImage, Image newImage, SimpleMotionDetector.maskType_t maskType)
	{
		//IL_0009: Expected I8, but got I
		//IL_00df: Expected I, but got I8
		//IL_00b3: Expected I, but got I8
		//IL_002f: Expected I, but got I8
		//IL_008b: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (oldImage != null && newImage != null)
		{
			Image image = null;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
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
				image = new Image(ptr2);
				CmRegionMotionDetection* native = GetNative();
				long num2 = *(long*)(*(long*)native + 16);
				CmImage* native2 = image.GetNative();
				CmImage* native3 = newImage.GetNative();
				CmImage* native4 = oldImage.GetNative();
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmFlags_003Cenum_0020NmMediaProcessing_003A_003ACmSimpleRegionDetector_003A_003AmaskFlag_t_003E obj);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, CmImage*, CmImage*, CmImage*, CmFlags_003Cenum_0020NmMediaProcessing_003A_003ACmSimpleRegionDetector_003A_003AmaskFlag_t_003E, double>)num2)((nint)native, native4, native3, native2, *SimpleMotionDetector.ConvertMaskType(&obj, maskType));
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
			GC.KeepAlive(image);
			GC.KeepAlive(this);
			return image;
		}
		throw new ArgumentNullException();
	}

	public unsafe CmRegionMotionDetection* GetNative()
	{
		CmRegionMotionDetection* pDetector = m_pDetector;
		if (pDetector == null)
		{
			throw new InvalidOperationException();
		}
		return pDetector;
	}

	public unsafe CmMotionChecker* GetChecker()
	{
		CmMotionChecker* pChecker = m_pChecker;
		if (pChecker == null)
		{
			throw new InvalidOperationException();
		}
		return pChecker;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public bool IsOwned()
	{
		return m_bOwned;
	}

	public void SetOwned()
	{
		m_bOwned = true;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007ERegionMotionDetection();
			return;
		}
		try
		{
			_0021RegionMotionDetection();
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

	~RegionMotionDetection()
	{
		Dispose(A_0: false);
	}
}
