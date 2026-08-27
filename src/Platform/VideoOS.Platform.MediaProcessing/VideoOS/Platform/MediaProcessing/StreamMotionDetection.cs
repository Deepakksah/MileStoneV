using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmCommon;
using NmGenericByteData;
using NmMediaProcessing;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class StreamMotionDetection : IDisposable
{
	private unsafe CmStreamMotionDetection* m_pDetection;

	private StreamMotionDetectionParameters m_parameters;

	private List<RegionMotionDetection> m_regions;

	public StreamMotionDetectionParameters Parameters => m_parameters;

	protected unsafe void OnParametersChanged()
	{
		//IL_0008: Expected I8, but got I
		//IL_007f: Expected I, but got I8
		//IL_0055: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_parameters == null)
		{
			throw new InvalidOperationException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmStreamMotionDetection* native = GetNative();
			CmStreamMotionDetectionParameters* native2 = m_parameters.GetNative();
			global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetection_002ESetParameters(native, native2);
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
		GC.KeepAlive(m_parameters);
		GC.KeepAlive(this);
	}

	protected unsafe CmMotionDetectionResult.decoderImplementation_t GetLastDecoderImplementation()
	{
		CmMotionDetectionResult.decoderImplementation_t result = global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetection_002EGetDecoderImplementation(m_pDetection) switch
		{
			(CmVideoDecoderParameters.decoderImplementation_t)7 => (CmMotionDetectionResult.decoderImplementation_t)7, 
			(CmVideoDecoderParameters.decoderImplementation_t)6 => (CmMotionDetectionResult.decoderImplementation_t)5, 
			(CmVideoDecoderParameters.decoderImplementation_t)5 => (CmMotionDetectionResult.decoderImplementation_t)6, 
			(CmVideoDecoderParameters.decoderImplementation_t)4 => (CmMotionDetectionResult.decoderImplementation_t)2, 
			_ => (CmMotionDetectionResult.decoderImplementation_t)0, 
		};
		GC.KeepAlive(this);
		return result;
	}

	public unsafe StreamMotionDetection()
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_00c4: Expected I, but got I8
		//IL_0099: Expected I, but got I8
		//IL_005a: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pDetection = null;
		m_parameters = null;
		m_regions = new List<RegionMotionDetection>();
		base._002Ector();
		m_parameters = new StreamMotionDetectionParameters(OnParametersChanged);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmStreamMotionDetection* ptr = (CmStreamMotionDetection*)global::_003CModule_003E.@new(704uL);
			CmStreamMotionDetection* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetection_002E_007Bctor_007D(ptr));
				CmStreamMotionDetection* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 704uL);
				throw;
			}
			m_pDetection = ptr2;
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

	private void _007EStreamMotionDetection()
	{
		((IDisposable)m_parameters)?.Dispose();
		m_parameters = null;
		_0021StreamMotionDetection();
	}

	private unsafe void _0021StreamMotionDetection()
	{
		//IL_002d: Expected I, but got I8
		//IL_0024: Expected I, but got I8
		m_regions = null;
		m_parameters = null;
		CmStreamMotionDetection* pDetection = m_pDetection;
		if (pDetection != null)
		{
			long num = *(long*)(*(ulong*)pDetection);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)pDetection, 1u);
		}
		m_pDetection = null;
	}

	public unsafe void AddRegion(RegionMotionDetection region)
	{
		//IL_0008: Expected I8, but got I
		//IL_00a7: Expected I, but got I8
		//IL_007d: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (region == null)
		{
			throw new ArgumentNullException();
		}
		List<RegionMotionDetection> regions = m_regions;
		if (regions == null)
		{
			throw new InvalidOperationException();
		}
		regions.Add(region);
		if (region.IsOwned())
		{
			throw new ArgumentException("The region has already been added to another detection object");
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmStreamMotionDetection* native = GetNative();
			CmRegionMotionDetection* native2 = region.GetNative();
			global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetection_002EAddRegionWithOwnership(native, native2);
			region.SetOwned();
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

	public unsafe void SetNoiseSensitivity(double noiseSensitivity)
	{
		//IL_0008: Expected I8, but got I
		//IL_0064: Expected I, but got I8
		//IL_003a: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetection_002ESetNoiseSensitivity(GetNative(), noiseSensitivity);
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

	public void SetMotionOnLevel(double motionLevel)
	{
		List<RegionMotionDetection>.Enumerator enumerator = m_regions.GetEnumerator();
		if (enumerator.MoveNext())
		{
			do
			{
				enumerator.Current.onLevel = motionLevel;
			}
			while (enumerator.MoveNext());
		}
		GC.KeepAlive(this);
	}

	public unsafe MotionDetectionResult CheckMotionLevel(GenericByteData byteData)
	{
		//Discarded unreachable code: IL_0115
		//IL_0008: Expected I8, but got I
		//IL_00ac: Expected I, but got I8
		//IL_0081: Expected I, but got I8
		//IL_00c0: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (byteData == null)
		{
			throw new ArgumentNullException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMotionDetectionResult cmMotionDetectionResult);
		*(long*)(&cmMotionDetectionResult) = 0L;
		System.Runtime.CompilerServices.Unsafe.As<CmMotionDetectionResult, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmMotionDetectionResult, 8)) = 0L;
		System.Runtime.CompilerServices.Unsafe.As<CmMotionDetectionResult, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmMotionDetectionResult, 16)) = 0L;
		MotionDetectionResult result;
		try
		{
			List<RegionMotionDetection> regions = m_regions;
			Action<RegionMotionDetection> action = new CmResultContainerSetter(&cmMotionDetectionResult).SetResultContainer;
			regions.ForEach(action);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmStreamMotionDetection* native = GetNative();
				NmGenericByteData.CmGenericByteData* native2 = byteData.GetNative();
				global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetection_002ECheckMotionLevel(native, native2);
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
				List<RegionMotionDetection> regions2 = m_regions;
				Action<RegionMotionDetection> action2 = new CmResultContainerSetter(null).SetResultContainer;
				regions2.ForEach(action2);
			}
			global::_003CModule_003E.NmMediaProcessing_002ECmMotionDetectionResult_002ESetDecoderImplementation(&cmMotionDetectionResult, GetLastDecoderImplementation());
			GC.KeepAlive(byteData);
			GC.KeepAlive(this);
			result = new MotionDetectionResult(&cmMotionDetectionResult, GetNative());
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

	public unsafe MotionDetectionResult CheckMotionLevelAndGetMask(GenericByteData byteData, SimpleMotionDetector.maskType_t maskType, out Image mask)
	{
		//Discarded unreachable code: IL_0188
		//IL_0008: Expected I8, but got I
		//IL_0116: Expected I, but got I8
		//IL_00eb: Expected I, but got I8
		//IL_012a: Expected I, but got I8
		//IL_0092: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (byteData == null)
		{
			throw new ArgumentNullException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMotionDetectionResult cmMotionDetectionResult);
		*(long*)(&cmMotionDetectionResult) = 0L;
		System.Runtime.CompilerServices.Unsafe.As<CmMotionDetectionResult, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmMotionDetectionResult, 8)) = 0L;
		System.Runtime.CompilerServices.Unsafe.As<CmMotionDetectionResult, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmMotionDetectionResult, 16)) = 0L;
		MotionDetectionResult result;
		try
		{
			List<RegionMotionDetection> regions = m_regions;
			Action<RegionMotionDetection> action = new CmResultContainerSetter(&cmMotionDetectionResult).SetResultContainer;
			regions.ForEach(action);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmStreamMotionDetection* native = GetNative();
				NmGenericByteData.CmGenericByteData* native2 = byteData.GetNative();
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmImage cmImage);
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmFlags_003Cenum_0020NmMediaProcessing_003A_003ACmSimpleRegionDetector_003A_003AmaskFlag_t_003E obj);
				global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetection_002ECheckMotionLevelAndGetMask(native, &cmImage, native2, *SimpleMotionDetector.ConvertMaskType(&obj, maskType));
				try
				{
					CmImage* ptr = &cmImage;
					if (global::_003CModule_003E.NmMediaProcessing_002ECmBaseImage_002EIsValid((CmBaseImage*)(&cmImage)))
					{
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
						mask = new Image(ptr3);
					}
					else
					{
						mask = null;
					}
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
				List<RegionMotionDetection> regions2 = m_regions;
				Action<RegionMotionDetection> action2 = new CmResultContainerSetter(null).SetResultContainer;
				regions2.ForEach(action2);
			}
			global::_003CModule_003E.NmMediaProcessing_002ECmMotionDetectionResult_002ESetDecoderImplementation(&cmMotionDetectionResult, GetLastDecoderImplementation());
			GC.KeepAlive(byteData);
			GC.KeepAlive(mask);
			GC.KeepAlive(this);
			result = new MotionDetectionResult(&cmMotionDetectionResult, GetNative());
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

	public unsafe Image GetLastImage()
	{
		//IL_0009: Expected I8, but got I
		//IL_00c2: Expected I, but got I8
		//IL_0096: Expected I, but got I8
		//IL_0042: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		Image result = null;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmImage cmImage);
			global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetection_002EGetLastImage(GetNative(), &cmImage);
			try
			{
				CmImage* ptr = &cmImage;
				if (global::_003CModule_003E.NmMediaProcessing_002ECmBaseImage_002EIsValid((CmBaseImage*)(&cmImage)))
				{
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
		return result;
	}

	public unsafe MotionDetectionResult EndStream()
	{
		//Discarded unreachable code: IL_00ef
		//IL_0008: Expected I8, but got I
		//IL_0099: Expected I, but got I8
		//IL_006e: Expected I, but got I8
		//IL_00ad: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMotionDetectionResult cmMotionDetectionResult);
		*(long*)(&cmMotionDetectionResult) = 0L;
		System.Runtime.CompilerServices.Unsafe.As<CmMotionDetectionResult, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmMotionDetectionResult, 8)) = 0L;
		System.Runtime.CompilerServices.Unsafe.As<CmMotionDetectionResult, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmMotionDetectionResult, 16)) = 0L;
		MotionDetectionResult result;
		try
		{
			List<RegionMotionDetection> regions = m_regions;
			Action<RegionMotionDetection> action = new CmResultContainerSetter(&cmMotionDetectionResult).SetResultContainer;
			regions.ForEach(action);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetection_002EEndStream(GetNative());
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
				List<RegionMotionDetection> regions2 = m_regions;
				Action<RegionMotionDetection> action2 = new CmResultContainerSetter(null).SetResultContainer;
				regions2.ForEach(action2);
			}
			GC.KeepAlive(this);
			result = new MotionDetectionResult(&cmMotionDetectionResult, GetNative());
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

	public unsafe void ResetDecoder()
	{
		//IL_0008: Expected I8, but got I
		//IL_0063: Expected I, but got I8
		//IL_0039: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetection_002EResetDecoder(GetNative());
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

	public unsafe void Reset()
	{
		//IL_0008: Expected I8, but got I
		//IL_0063: Expected I, but got I8
		//IL_0039: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			global::_003CModule_003E.NmMediaProcessing_002ECmStreamMotionDetection_002EReset(GetNative());
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

	public unsafe CmStreamMotionDetection* GetNative()
	{
		CmStreamMotionDetection* pDetection = m_pDetection;
		if (pDetection == null)
		{
			throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.StreamMotionDetection");
		}
		return pDetection;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007EStreamMotionDetection();
			return;
		}
		try
		{
			_0021StreamMotionDetection();
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

	~StreamMotionDetection()
	{
		Dispose(A_0: false);
	}
}
