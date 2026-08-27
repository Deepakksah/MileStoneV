using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmCommon;
using NmMediaProcessing;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class ImageMotionDetection : IDisposable
{
	private unsafe CmImageMotionDetection* m_pDetector;

	private List<RegionMotionDetection> m_regions;

	public unsafe ImageMotionDetection()
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_00a0: Expected I, but got I8
		//IL_0075: Expected I, but got I8
		//IL_0039: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pDetector = null;
		m_regions = new List<RegionMotionDetection>();
		base._002Ector();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmImageMotionDetection* ptr = (CmImageMotionDetection*)global::_003CModule_003E.@new(24uL);
			CmImageMotionDetection* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmImageMotionDetection_002E_007Bctor_007D(ptr));
				CmImageMotionDetection* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 24uL);
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

	private void _007EImageMotionDetection()
	{
		_0021ImageMotionDetection();
	}

	private unsafe void _0021ImageMotionDetection()
	{
		//IL_0028: Expected I, but got I8
		m_regions = null;
		CmImageMotionDetection* pDetector = m_pDetector;
		if (pDetector != null)
		{
			global::_003CModule_003E.NmMediaProcessing_002ECmImageMotionDetection_002E_007Bdtor_007D(pDetector);
			global::_003CModule_003E.delete(pDetector, 24uL);
		}
		m_pDetector = null;
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
			CmImageMotionDetection* native = GetNative();
			CmRegionMotionDetection* native2 = region.GetNative();
			global::_003CModule_003E.NmMediaProcessing_002ECmImageMotionDetection_002EAddRegionWithOwnership(native, native2);
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

	public unsafe MotionDetectionResult Check(Image oldImage, Image newImage)
	{
		//Discarded unreachable code: IL_011c
		//IL_0008: Expected I8, but got I
		//IL_00b9: Expected I, but got I8
		//IL_008e: Expected I, but got I8
		//IL_00cd: Expected I, but got I8
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
				List<RegionMotionDetection> regions = m_regions;
				Action<RegionMotionDetection> action = new CmResultContainerSetter(&cmMotionDetectionResult).SetResultContainer;
				regions.ForEach(action);
				System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
				try
				{
					CmImageMotionDetection* native = GetNative();
					CmImage* native2 = newImage.GetNative();
					CmImage* native3 = oldImage.GetNative();
					global::_003CModule_003E.NmMediaProcessing_002ECmImageMotionDetection_002ECheckMotionLevel(native, native3, native2);
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

	public unsafe Image GetMask(Image oldImage, Image newImage, SimpleMotionDetector.maskType_t maskType)
	{
		//IL_0009: Expected I8, but got I
		//IL_00ce: Expected I, but got I8
		//IL_00a2: Expected I, but got I8
		//IL_002f: Expected I, but got I8
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
				CmImageMotionDetection* native = GetNative();
				CmImage* native2 = image.GetNative();
				CmImage* native3 = newImage.GetNative();
				CmImage* native4 = oldImage.GetNative();
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmFlags_003Cenum_0020NmMediaProcessing_003A_003ACmSimpleRegionDetector_003A_003AmaskFlag_t_003E obj);
				global::_003CModule_003E.NmMediaProcessing_002ECmImageMotionDetection_002EGetMotionMask(native, native4, native3, native2, *SimpleMotionDetector.ConvertMaskType(&obj, maskType));
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
			GC.KeepAlive(oldImage);
			GC.KeepAlive(newImage);
			GC.KeepAlive(image);
			GC.KeepAlive(this);
			return image;
		}
		throw new ArgumentNullException();
	}

	public unsafe CmImageMotionDetection* GetNative()
	{
		CmImageMotionDetection* pDetector = m_pDetector;
		if (pDetector == null)
		{
			throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.ImageMotionDetection");
		}
		return pDetector;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007EImageMotionDetection();
			return;
		}
		try
		{
			_0021ImageMotionDetection();
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

	~ImageMotionDetection()
	{
		Dispose(A_0: false);
	}
}
