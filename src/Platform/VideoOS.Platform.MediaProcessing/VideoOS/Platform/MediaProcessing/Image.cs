using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using _003CCppImplementationDetails_003E;
using NmCommon;
using NmMediaProcessing;
using NmTimeStamp;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class Image : ICloneable, IDisposable
{
	public enum creation_t
	{
		blackRGB,
		uninitializedRGB
	}

	protected unsafe CmImage* m_pImage;

	public unsafe int Height
	{
		get
		{
			int num = (int)global::_003CModule_003E.NmMediaProcessing_002ECmBaseImage_002EGetHeight((CmBaseImage*)GetNative());
			GC.KeepAlive(this);
			if (num <= 0)
			{
				throw new InvalidOperationException("No image");
			}
			return num;
		}
	}

	public unsafe int Width
	{
		get
		{
			int num = (int)global::_003CModule_003E.NmMediaProcessing_002ECmBaseImage_002EGetWidth((CmBaseImage*)GetNative());
			GC.KeepAlive(this);
			if (num <= 0)
			{
				throw new InvalidOperationException("No image");
			}
			return num;
		}
	}

	public unsafe DateTime TimeStamp
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_0074: Expected I, but got I8
			//IL_004a: Expected I, but got I8
			//IL_0023: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			DateTime result = default(DateTime);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				result = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003ADateTime_002Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_003E((NmTimeStamp.CmTimeStamp*)((ulong)(nint)global::_003CModule_003E.NmMediaProcessing_002ECmImage_002EGetAttributes(GetNative()) + 8uL));
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
		set
		{
			//IL_0008: Expected I8, but got I
			//IL_0095: Expected I, but got I8
			//IL_006b: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmImage* native = GetNative();
				System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
				NmTimeStamp.CmTimeStamp* ptr = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_002Cclass_0020System_003A_003ADateTime_003E(&cmTimeStamp, &value);
				try
				{
					NmTimeStamp.CmTimeStamp* ptr2 = ptr;
					System.Runtime.CompilerServices.Unsafe.SkipInit(out _0024PTMType_0024PEQCmStandardImageAttributes_0040NmMediaProcessing_0040_0040VCmTimeStamp_0040NmTimeStamp_0040_0040 _0024PTMType_0024PEQCmStandardImageAttributes_0040NmMediaProcessing_0040_0040VCmTimeStamp_0040NmTimeStamp_0040_00402);
					*(int*)(&_0024PTMType_0024PEQCmStandardImageAttributes_0040NmMediaProcessing_0040_0040VCmTimeStamp_0040NmTimeStamp_0040_00402) = 8;
					// IL cpblk instruction
					System.Runtime.CompilerServices.Unsafe.CopyBlockUnaligned((long)(nint)native + 56L, ptr, 8);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
					throw;
				}
				global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
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

	public unsafe Image(CmImage* pImage)
	{
		m_pImage = pImage;
		base._002Ector();
		if (m_pImage == null)
		{
			throw new ArgumentNullException();
		}
	}

	public unsafe Image()
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_0095: Expected I, but got I8
		//IL_006a: Expected I, but got I8
		//IL_002e: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pImage = null;
		base._002Ector();
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
			m_pImage = ptr2;
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

	private unsafe void _007EImage()
	{
		//IL_001f: Expected I, but got I8
		//IL_0016: Expected I, but got I8
		CmImage* pImage = m_pImage;
		if (pImage != null)
		{
			long num = *(long*)(*(ulong*)pImage);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)pImage, 1u);
		}
		m_pImage = null;
	}

	private unsafe void _0021Image()
	{
		//IL_0031: Expected I, but got I8
		//IL_0028: Expected I, but got I8
		if (m_pImage != null)
		{
			global::_003CModule_003E.OutputDebugStringA((sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_0EM_0040GHLKCGMC_0040Finalization_003F5warning_003F3_003F5VideoOS_003F4P_0040));
			CmImage* pImage = m_pImage;
			if (pImage != null)
			{
				long num = *(long*)(*(ulong*)pImage);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)pImage, 1u);
			}
			m_pImage = null;
		}
	}

	public unsafe virtual object Clone()
	{
		//IL_0008: Expected I8, but got I
		//IL_0074: Expected I, but got I8
		//IL_004a: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		Image image = new Image();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmImage* native = GetNative();
			global::_003CModule_003E.NmMediaProcessing_002ECmImage_002E_003D(image.GetNative(), native);
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
		GC.KeepAlive(image);
		GC.KeepAlive(this);
		return image;
	}

	public unsafe void Normalize()
	{
		//IL_0008: Expected I8, but got I
		//IL_00c3: Expected I, but got I8
		//IL_0099: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmImage* native = GetNative();
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmNormalizeImage cmNormalizeImage);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmOrientation cmOrientation);
			CmNormalizeImage* ptr = global::_003CModule_003E.NmMediaProcessing_002ECmNormalizeImage_002E_007Bctor_007D(&cmNormalizeImage, true, global::_003CModule_003E.NmMediaProcessing_002ECmOrientation_002E_007Bctor_007D(&cmOrientation));
			try
			{
				global::_003CModule_003E.NmMediaProcessing_002ECmImage_002ETransform(native, (ImImageTransformation*)ptr);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmNormalizeImage*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmNormalizeImage_002E_007Bdtor_007D), &cmNormalizeImage);
				throw;
			}
			global::_003CModule_003E.NmMediaProcessing_002ECmNormalizeImage_002E_007Bdtor_007D(&cmNormalizeImage);
			CmImage* native2 = GetNative();
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmImageColorFormatConvert cmImageColorFormatConvert);
			CmImageColorFormatConvert* ptr2 = global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bctor_007D(&cmImageColorFormatConvert, (CmBaseImage.colorFormat_t)4, 4uL);
			try
			{
				global::_003CModule_003E.NmMediaProcessing_002ECmImage_002ETransform(native2, (ImImageTransformation*)ptr2);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmImageColorFormatConvert*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bdtor_007D), &cmImageColorFormatConvert);
				throw;
			}
			global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bdtor_007D(&cmImageColorFormatConvert);
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

	public unsafe Image GetResized(int width, int height, [MarshalAs(UnmanagedType.U1)] bool keepAspectRatio, [MarshalAs(UnmanagedType.U1)] bool allowUpsizing)
	{
		//IL_0009: Expected I8, but got I
		//IL_0240: Expected I, but got I8
		//IL_0210: Expected I, but got I8
		//IL_0049: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (width >= 1 && height >= 1)
		{
			if (m_pImage == null)
			{
				throw new ObjectDisposedException("VideoOS.Platfom.MediaProcessing.Image");
			}
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
				CmImage* native = GetNative();
				CmImage* native2 = image.GetNative();
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmNormalizeImage cmNormalizeImage);
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmOrientation cmOrientation);
				CmNormalizeImage* ptr4 = global::_003CModule_003E.NmMediaProcessing_002ECmNormalizeImage_002E_007Bctor_007D(&cmNormalizeImage, true, global::_003CModule_003E.NmMediaProcessing_002ECmOrientation_002E_007Bctor_007D(&cmOrientation));
				try
				{
					global::_003CModule_003E.NmMediaProcessing_002ECmImage_002ETransform(native, (ImImageTransformation*)ptr4, native2);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmNormalizeImage*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmNormalizeImage_002E_007Bdtor_007D), &cmNormalizeImage);
					throw;
				}
				global::_003CModule_003E.NmMediaProcessing_002ECmNormalizeImage_002E_007Bdtor_007D(&cmNormalizeImage);
				double num2 = (double)width / (double)image.Width;
				double num3 = (double)height / (double)image.Height;
				if (image.Width == width && image.Height == height)
				{
					Image result = image;
					image = null;
					return result;
				}
				if (keepAspectRatio)
				{
					if (!allowUpsizing && (num2 < 1.0 || num3 < 1.0))
					{
						int num4 = 0;
						int num5 = 0;
						num4 = ((!(num3 < num2)) ? width : ((int)((double)image.Width * num3)));
						num5 = ((!(num2 < num3)) ? height : ((int)((double)image.Height * num2)));
						Image resized = image.GetResized(num4, num5);
						GC.KeepAlive(this);
						return resized;
					}
					if (num2 > num3)
					{
						Image image2 = image;
						Image resized2 = image2.GetResized((int)((double)image2.Width * num3 + 0.5), height);
						GC.KeepAlive(this);
						return resized2;
					}
					Image resized3 = image.GetResized(width, (int)((double)image.Height * num2 + 0.5));
					GC.KeepAlive(this);
					return resized3;
				}
				if (allowUpsizing)
				{
					Image resized4 = image.GetResized(width, height);
					GC.KeepAlive(this);
					return resized4;
				}
				int height2 = ((height <= Height) ? height : Height);
				int width2 = ((width <= Width) ? width : Width);
				Image resized5 = image.GetResized(width2, height2);
				GC.KeepAlive(this);
				return resized5;
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
			{
				uint num6 = 0u;
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
						num6 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
						return (byte)num6 != 0;
					}).Invoke())
					{
					}
					if (num6 != 0)
					{
						throw;
					}
				}
				finally
				{
					global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num6);
				}
			}
			finally
			{
				((IDisposable)image)?.Dispose();
			}
			GC.KeepAlive(image);
			GC.KeepAlive(this);
			return null;
		}
		throw new ArgumentException();
	}

	public Image GetResized(double factor)
	{
		if (factor <= 0.0)
		{
			throw new ArgumentException();
		}
		int height = (int)((double)Height * factor + 0.5);
		int width = (int)((double)Width * factor + 0.5);
		return GetResized(width, height);
	}

	public unsafe Image GetResized(int width, int height)
	{
		//IL_0009: Expected I8, but got I
		//IL_00cd: Expected I, but got I8
		//IL_00a1: Expected I, but got I8
		//IL_0023: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
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
			CmImage* native = GetNative();
			CmImage* native2 = image.GetNative();
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmImageResize cmImageResize);
			CmImageResize* ptr4 = global::_003CModule_003E.NmMediaProcessing_002ECmImageResize_002E_007Bctor_007D(&cmImageResize, (ulong)width, (ulong)height, true, true);
			try
			{
				global::_003CModule_003E.NmMediaProcessing_002ECmImage_002ETransform(native, (ImImageTransformation*)ptr4, native2);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmImageResize*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmImageResize_002E_007Bdtor_007D), &cmImageResize);
				throw;
			}
			global::_003CModule_003E.NmMediaProcessing_002ECmImageResize_002E_007Bdtor_007D(&cmImageResize);
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
		GC.KeepAlive(image);
		GC.KeepAlive(this);
		return image;
	}

	public unsafe void Overlay(Image image)
	{
		//IL_0008: Expected I8, but got I
		//IL_0156: Expected I, but got I8
		//IL_012c: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (image == null)
		{
			throw new ArgumentNullException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			global::_003CModule_003E._Init_thread_header_m((int*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003FA0x3c0b68cf_002E_003F_0024TSS0_0040_003F5_003F_003FOverlay_0040Image_0040MediaProcessing_0040Platform_0040VideoOS_0040_0040QE_0024AAMXPE_0024AAV2345_0040_0040Z_00404HA));
			if (global::_003CModule_003E._003FA0x3c0b68cf_002E_003F_0024TSS0_0040_003F5_003F_003FOverlay_0040Image_0040MediaProcessing_0040Platform_0040VideoOS_0040_0040QE_0024AAMXPE_0024AAV2345_0040_0040Z_00404HA == -1)
			{
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CmFlags_003Cenum_0020NmMediaProcessing_003A_003ACmPositionParameters_003A_003ApositionFlagEnumeration_t_003E obj);
					CmFlags_003Cenum_0020NmMediaProcessing_003A_003ACmPositionParameters_003A_003ApositionFlagEnumeration_t_003E* ptr = &obj;
					*(int*)(&obj) = 16;
					CmFlags_003Cenum_0020NmMediaProcessing_003A_003ACmPositionParameters_003A_003ApositionFlagEnumeration_t_003E* ptr2 = &obj;
					global::_003CModule_003E.NmMediaProcessing_002ECmPositionParameters_002E_007Bctor_007D((CmPositionParameters*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003FA0x3c0b68cf_002E_003Fposition_0040_003F5_003F_003FOverlay_0040Image_0040MediaProcessing_0040Platform_0040VideoOS_0040_0040QE_0024AAMXPE_0024AAV2345_0040_0040Z_00404VCmPositionParameters_0040NmMediaProcessing_0040_0040B), obj);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<int*, void>)(&global::_003CModule_003E._Init_thread_abort_m), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003FA0x3c0b68cf_002E_003F_0024TSS0_0040_003F5_003F_003FOverlay_0040Image_0040MediaProcessing_0040Platform_0040VideoOS_0040_0040QE_0024AAMXPE_0024AAV2345_0040_0040Z_00404HA));
					throw;
				}
				global::_003CModule_003E._Init_thread_footer_m((int*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003FA0x3c0b68cf_002E_003F_0024TSS0_0040_003F5_003F_003FOverlay_0040Image_0040MediaProcessing_0040Platform_0040VideoOS_0040_0040QE_0024AAMXPE_0024AAV2345_0040_0040Z_00404HA));
			}
			if (global::_003CModule_003E.NmMediaProcessing_002ECmBaseImage_002EGetColorFormat((CmBaseImage*)image.GetNative()) != (CmBaseImage.colorFormat_t)9 && global::_003CModule_003E.NmMediaProcessing_002ECmBaseImage_002EGetColorFormat((CmBaseImage*)image.GetNative()) != (CmBaseImage.colorFormat_t)8)
			{
				CmImage* native = GetNative();
				CmImage* native2 = image.GetNative();
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmColorKeyOverlay cmColorKeyOverlay);
				CmColorKeyOverlay* ptr3 = global::_003CModule_003E.NmMediaProcessing_002ECmColorKeyOverlay_002E_007Bctor_007D(&cmColorKeyOverlay, native2, global::_003CModule_003E._003FA0x3c0b68cf_002E_003Fposition_0040_003F5_003F_003FOverlay_0040Image_0040MediaProcessing_0040Platform_0040VideoOS_0040_0040QE_0024AAMXPE_0024AAV2345_0040_0040Z_00404VCmPositionParameters_0040NmMediaProcessing_0040_0040B, 0, 0, 0, 0);
				try
				{
					global::_003CModule_003E.NmMediaProcessing_002ECmImage_002ETransform(native, (ImImageTransformation*)ptr3);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmColorKeyOverlay*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmColorKeyOverlay_002E_007Bdtor_007D), &cmColorKeyOverlay);
					throw;
				}
				global::_003CModule_003E.NmMediaProcessing_002ECmColorKeyOverlay_002E_007Bdtor_007D(&cmColorKeyOverlay);
			}
			else
			{
				CmImage* native3 = GetNative();
				CmImage* native4 = image.GetNative();
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmAlphaBlendOverlay cmAlphaBlendOverlay);
				CmAlphaBlendOverlay* ptr4 = global::_003CModule_003E.NmMediaProcessing_002ECmAlphaBlendOverlay_002E_007Bctor_007D(&cmAlphaBlendOverlay, native4, global::_003CModule_003E._003FA0x3c0b68cf_002E_003Fposition_0040_003F5_003F_003FOverlay_0040Image_0040MediaProcessing_0040Platform_0040VideoOS_0040_0040QE_0024AAMXPE_0024AAV2345_0040_0040Z_00404VCmPositionParameters_0040NmMediaProcessing_0040_0040B);
				try
				{
					global::_003CModule_003E.NmMediaProcessing_002ECmImage_002ETransform(native3, (ImImageTransformation*)ptr4);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmAlphaBlendOverlay*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmAlphaBlendOverlay_002E_007Bdtor_007D), &cmAlphaBlendOverlay);
					throw;
				}
				global::_003CModule_003E.NmMediaProcessing_002ECmAlphaBlendOverlay_002E_007Bdtor_007D(&cmAlphaBlendOverlay);
			}
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
		GC.KeepAlive(image);
		GC.KeepAlive(this);
	}

	public unsafe void ApplyPrivacyMask(IPrivacyMaskSource source)
	{
		//IL_0008: Expected I8, but got I
		//IL_018c: Expected I, but got I8
		//IL_0162: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (null == source)
		{
			throw new ArgumentNullException();
		}
		byte b;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		switch (source.Size)
		{
		case IPrivacyMaskSource.GridSize.Size_1x1:
			b = 1;
			goto IL_0050;
		case IPrivacyMaskSource.GridSize.Size_8x8:
			b = 8;
			goto IL_0050;
		case IPrivacyMaskSource.GridSize.Size_16x16:
			b = 16;
			goto IL_0050;
		case IPrivacyMaskSource.GridSize.Size_32x32:
			b = 32;
			goto IL_0050;
		case IPrivacyMaskSource.GridSize.Size_64x64:
			b = 64;
			goto IL_0050;
		default:
			throw new ArgumentException();
		case IPrivacyMaskSource.GridSize.Size_0:
			break;
			IL_0050:
			try
			{
				ulong num2 = b;
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmPrivacyMaskGridDefinition cmPrivacyMaskGridDefinition);
				global::_003CModule_003E.NmMediaProcessing_002ECmPrivacyMaskGridDefinition_002E_007Bctor_007D(&cmPrivacyMaskGridDefinition, num2, num2);
				try
				{
					if (!global::_003CModule_003E._003FA0x3c0b68cf_002E_003FA0x3c0b68cf_002EConvertPrivacyMask(source, &cmPrivacyMaskGridDefinition, b))
					{
						GC.KeepAlive(this);
						goto IL_0083;
					}
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmPrivacyMaskGridDefinition*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmPrivacyMaskGridDefinition_002E_007Bdtor_007D), &cmPrivacyMaskGridDefinition);
					throw;
				}
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CmPrivacyMaskTransform cmPrivacyMaskTransform);
					global::_003CModule_003E.NmMediaProcessing_002ECmPrivacyMaskTransform_002E_007Bctor_007D(&cmPrivacyMaskTransform, &cmPrivacyMaskGridDefinition);
					try
					{
						global::_003CModule_003E.NmMediaProcessing_002ECmImage_002ETransform(GetNative(), (ImImageTransformation*)(&cmPrivacyMaskTransform));
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmPrivacyMaskTransform*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmPrivacyMaskTransform_002E_007Bdtor_007D), &cmPrivacyMaskTransform);
						throw;
					}
					global::_003CModule_003E.NmMediaProcessing_002ECmPrivacyMaskTransform_002E_007Bdtor_007D(&cmPrivacyMaskTransform);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmPrivacyMaskGridDefinition*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmPrivacyMaskGridDefinition_002E_007Bdtor_007D), &cmPrivacyMaskGridDefinition);
					throw;
				}
				try
				{
					CmPrivacyMaskGridDefinition.Grid* ptr = (CmPrivacyMaskGridDefinition.Grid*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmPrivacyMaskGridDefinition, 24));
					map_003Cunsigned_0020__int64_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_002Cstd_003A_003Aless_003Cunsigned_0020__int64_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020__int64_0020const_0020_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_0020_003E_0020_003E_0020_003E* ptr2 = (map_003Cunsigned_0020__int64_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_002Cstd_003A_003Aless_003Cunsigned_0020__int64_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020__int64_0020const_0020_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_0020_003E_0020_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmPrivacyMaskGridDefinition, 40));
					global::_003CModule_003E.std_002E_Tree_003Cstd_003A_003A_Tmap_traits_003Cunsigned_0020__int64_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_002Cstd_003A_003Aless_003Cunsigned_0020__int64_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020__int64_0020const_0020_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_0020_003E_0020_003E_002C0_003E_0020_003E_002E_007Bdtor_007D((_Tree_003Cstd_003A_003A_Tmap_traits_003Cunsigned_0020__int64_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_002Cstd_003A_003Aless_003Cunsigned_0020__int64_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020__int64_0020const_0020_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_0020_003E_0020_003E_002C0_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmPrivacyMaskGridDefinition, 40)));
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<map_003Cunsigned_0020char_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_002Cstd_003A_003Aless_003Cunsigned_0020char_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020char_0020const_0020_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_0020_003E_0020_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Emap_003Cunsigned_0020char_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_002Cstd_003A_003Aless_003Cunsigned_0020char_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020char_0020const_0020_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_0020_003E_0020_003E_0020_003E_002E_007Bdtor_007D), System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmPrivacyMaskGridDefinition, 8)));
					throw;
				}
				map_003Cunsigned_0020char_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_002Cstd_003A_003Aless_003Cunsigned_0020char_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020char_0020const_0020_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_0020_003E_0020_003E_0020_003E* ptr3 = (map_003Cunsigned_0020char_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_002Cstd_003A_003Aless_003Cunsigned_0020char_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020char_0020const_0020_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_0020_003E_0020_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmPrivacyMaskGridDefinition, 8));
				global::_003CModule_003E.std_002E_Tree_003Cstd_003A_003A_Tmap_traits_003Cunsigned_0020char_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_002Cstd_003A_003Aless_003Cunsigned_0020char_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020char_0020const_0020_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_0020_003E_0020_003E_002C0_003E_0020_003E_002E_007Bdtor_007D((_Tree_003Cstd_003A_003A_Tmap_traits_003Cunsigned_0020char_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_002Cstd_003A_003Aless_003Cunsigned_0020char_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020char_0020const_0020_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_0020_003E_0020_003E_002C0_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmPrivacyMaskGridDefinition, 8)));
				goto end_IL_0050;
				IL_0083:
				try
				{
					CmPrivacyMaskGridDefinition.Grid* ptr4 = (CmPrivacyMaskGridDefinition.Grid*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmPrivacyMaskGridDefinition, 24));
					map_003Cunsigned_0020__int64_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_002Cstd_003A_003Aless_003Cunsigned_0020__int64_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020__int64_0020const_0020_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_0020_003E_0020_003E_0020_003E* ptr5 = (map_003Cunsigned_0020__int64_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_002Cstd_003A_003Aless_003Cunsigned_0020__int64_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020__int64_0020const_0020_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_0020_003E_0020_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmPrivacyMaskGridDefinition, 40));
					global::_003CModule_003E.std_002E_Tree_003Cstd_003A_003A_Tmap_traits_003Cunsigned_0020__int64_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_002Cstd_003A_003Aless_003Cunsigned_0020__int64_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020__int64_0020const_0020_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_0020_003E_0020_003E_002C0_003E_0020_003E_002E_007Bdtor_007D((_Tree_003Cstd_003A_003A_Tmap_traits_003Cunsigned_0020__int64_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_002Cstd_003A_003Aless_003Cunsigned_0020__int64_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020__int64_0020const_0020_002Cstd_003A_003Avector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_0020_003E_0020_003E_002C0_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmPrivacyMaskGridDefinition, 40)));
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<map_003Cunsigned_0020char_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_002Cstd_003A_003Aless_003Cunsigned_0020char_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020char_0020const_0020_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_0020_003E_0020_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Emap_003Cunsigned_0020char_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_002Cstd_003A_003Aless_003Cunsigned_0020char_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020char_0020const_0020_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_0020_003E_0020_003E_0020_003E_002E_007Bdtor_007D), System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmPrivacyMaskGridDefinition, 8)));
					throw;
				}
				map_003Cunsigned_0020char_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_002Cstd_003A_003Aless_003Cunsigned_0020char_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020char_0020const_0020_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_0020_003E_0020_003E_0020_003E* ptr6 = (map_003Cunsigned_0020char_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_002Cstd_003A_003Aless_003Cunsigned_0020char_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020char_0020const_0020_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_0020_003E_0020_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmPrivacyMaskGridDefinition, 8));
				global::_003CModule_003E.std_002E_Tree_003Cstd_003A_003A_Tmap_traits_003Cunsigned_0020char_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_002Cstd_003A_003Aless_003Cunsigned_0020char_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020char_0020const_0020_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_0020_003E_0020_003E_002C0_003E_0020_003E_002E_007Bdtor_007D((_Tree_003Cstd_003A_003A_Tmap_traits_003Cunsigned_0020char_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_002Cstd_003A_003Aless_003Cunsigned_0020char_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003Cunsigned_0020char_0020const_0020_002Cstd_003A_003Ashared_ptr_003CNmMediaProcessing_003A_003ACmPrivacyMaskGridDefinition_003A_003AMethod_003E_0020_003E_0020_003E_002C0_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmPrivacyMaskGridDefinition, 8)));
				break;
				end_IL_0050:;
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
			GC.KeepAlive(source);
			GC.KeepAlive(this);
			break;
		}
	}

	public unsafe Bitmap CreateBitmapAsReference()
	{
		//IL_0009: Expected I8, but got I
		//IL_0221: Expected I, but got I8
		//IL_01f5: Expected I, but got I8
		//IL_016e: Expected I, but got I8
		//IL_0170: Expected I8, but got I
		//IL_0185: Expected I, but got I8
		//IL_0185: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			PixelFormat pixelFormat = PixelFormat.Undefined;
			switch (global::_003CModule_003E.NmMediaProcessing_002ECmBaseImage_002EGetColorFormat((CmBaseImage*)GetNative()))
			{
			case (CmBaseImage.colorFormat_t)4:
				pixelFormat = PixelFormat.Format32bppRgb;
				break;
			case (CmBaseImage.colorFormat_t)9:
				pixelFormat = PixelFormat.Format32bppPArgb;
				break;
			case (CmBaseImage.colorFormat_t)2:
				pixelFormat = PixelFormat.Format24bppRgb;
				break;
			case (CmBaseImage.colorFormat_t)3:
			{
				CmImage* native4 = GetNative();
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmImageColorFormatConvert cmImageColorFormatConvert4);
				CmImageColorFormatConvert* ptr4 = global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bctor_007D(&cmImageColorFormatConvert4, (CmBaseImage.colorFormat_t)4, 0uL);
				try
				{
					global::_003CModule_003E.NmMediaProcessing_002ECmImage_002ETransform(native4, (ImImageTransformation*)ptr4);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmImageColorFormatConvert*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bdtor_007D), &cmImageColorFormatConvert4);
					throw;
				}
				global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bdtor_007D(&cmImageColorFormatConvert4);
				pixelFormat = PixelFormat.Format32bppRgb;
				break;
			}
			case (CmBaseImage.colorFormat_t)8:
			{
				CmImage* native3 = GetNative();
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmImageColorFormatConvert cmImageColorFormatConvert3);
				CmImageColorFormatConvert* ptr3 = global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bctor_007D(&cmImageColorFormatConvert3, (CmBaseImage.colorFormat_t)9, 0uL);
				try
				{
					global::_003CModule_003E.NmMediaProcessing_002ECmImage_002ETransform(native3, (ImImageTransformation*)ptr3);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmImageColorFormatConvert*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bdtor_007D), &cmImageColorFormatConvert3);
					throw;
				}
				global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bdtor_007D(&cmImageColorFormatConvert3);
				pixelFormat = PixelFormat.Format32bppPArgb;
				break;
			}
			case (CmBaseImage.colorFormat_t)1:
			{
				CmImage* native2 = GetNative();
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmImageColorFormatConvert cmImageColorFormatConvert2);
				CmImageColorFormatConvert* ptr2 = global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bctor_007D(&cmImageColorFormatConvert2, (CmBaseImage.colorFormat_t)2, 0uL);
				try
				{
					global::_003CModule_003E.NmMediaProcessing_002ECmImage_002ETransform(native2, (ImImageTransformation*)ptr2);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmImageColorFormatConvert*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bdtor_007D), &cmImageColorFormatConvert2);
					throw;
				}
				global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bdtor_007D(&cmImageColorFormatConvert2);
				pixelFormat = PixelFormat.Format24bppRgb;
				break;
			}
			default:
			{
				CmImage* native = GetNative();
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmImageColorFormatConvert cmImageColorFormatConvert);
				CmImageColorFormatConvert* ptr = global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bctor_007D(&cmImageColorFormatConvert, (CmBaseImage.colorFormat_t)2, 4uL);
				try
				{
					global::_003CModule_003E.NmMediaProcessing_002ECmImage_002ETransform(native, (ImImageTransformation*)ptr);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmImageColorFormatConvert*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bdtor_007D), &cmImageColorFormatConvert);
					throw;
				}
				global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bdtor_007D(&cmImageColorFormatConvert);
				pixelFormat = PixelFormat.Format24bppRgb;
				break;
			}
			}
			CmImage* native5 = GetNative();
			long num2 = (nint)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImImageMemoryAllocation*>)(*(ulong*)(*(long*)native5 + 16)))((nint)native5);
			long num3 = *(long*)(*(long*)num2 + 88);
			ImImageMemoryAllocation.CmWriteablePlane* ptr5 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, ImImageMemoryAllocation.CmWriteablePlane*>)num3)((nint)num2, 0uL);
			Bitmap result2;
			try
			{
				Bitmap result = new Bitmap(scan0: new IntPtr(global::_003CModule_003E.NmMediaProcessing_002EImImageMemoryAllocation_002ECmWriteablePlane_002EGetBits(ptr5)), width: (int)global::_003CModule_003E.NmMediaProcessing_002ECmBaseImage_002EGetWidth((CmBaseImage*)GetNative()), height: (int)global::_003CModule_003E.NmMediaProcessing_002ECmBaseImage_002EGetHeight((CmBaseImage*)GetNative()), stride: (int)global::_003CModule_003E.NmMediaProcessing_002EImImageMemoryAllocation_002ECmPlane_002EGetRowStride((ImImageMemoryAllocation.CmPlane*)ptr5), format: pixelFormat);
				GC.KeepAlive(this);
				return result;
			}
			catch (Exception)
			{
				result2 = null;
			}
			return result2;
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
		return null;
	}

	public Bitmap CreateBitmapAsCopy()
	{
		Bitmap bitmap = CreateBitmapAsReference();
		Bitmap result = new Bitmap(bitmap);
		((IDisposable)bitmap)?.Dispose();
		GC.KeepAlive(this);
		return result;
	}

	public unsafe IntPtr GetBufferPointerRGB([MarshalAs(UnmanagedType.U1)] bool bReadOnly)
	{
		//IL_0009: Expected I8, but got I
		//IL_010e: Expected I, but got I8
		//IL_00e2: Expected I, but got I8
		//IL_003b: Expected I, but got I8
		//IL_0052: Expected I, but got I8
		//IL_0095: Expected I, but got I8
		//IL_0096: Expected I8, but got I
		//IL_00a9: Expected I, but got I8
		//IL_00a9: Expected I, but got I8
		//IL_0067: Expected I, but got I8
		//IL_0068: Expected I8, but got I
		//IL_007b: Expected I, but got I8
		//IL_007b: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		IntPtr result = IntPtr.Zero;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			if (global::_003CModule_003E.NmMediaProcessing_002ECmBaseImage_002EGetColorFormat((CmBaseImage*)GetNative()) != (CmBaseImage.colorFormat_t)2)
			{
				throw new InvalidOperationException("Buffer is not in BGR24 format");
			}
			CmImage* native = GetNative();
			if (!global::_003CModule_003E.NmMediaProcessing_002ECmImageBufferHelpers_002EFitsAlignment(((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImImageMemoryAllocation*>)(*(ulong*)(*(long*)native + 16)))((nint)native), 4uL))
			{
				throw new InvalidOperationException("Buffer has incompatible alignment");
			}
			byte* ptr = null;
			if (bReadOnly)
			{
				CmImage* native2 = GetNative();
				long num2 = (nint)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImImageMemoryAllocation*>)(*(ulong*)(*(long*)native2 + 16)))((nint)native2);
				long num3 = *(long*)(*(long*)num2 + 80);
				ptr = global::_003CModule_003E.NmMediaProcessing_002EImImageMemoryAllocation_002ECmPlane_002EGetBits(((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, ImImageMemoryAllocation.CmPlane*>)num3)((nint)num2, 0uL));
			}
			else
			{
				CmImage* native3 = GetNative();
				long num4 = (nint)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImImageMemoryAllocation*>)(*(ulong*)(*(long*)native3 + 16)))((nint)native3);
				long num5 = *(long*)(*(long*)num4 + 88);
				ptr = global::_003CModule_003E.NmMediaProcessing_002EImImageMemoryAllocation_002ECmWriteablePlane_002EGetBits(((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, ImImageMemoryAllocation.CmWriteablePlane*>)num5)((nint)num4, 0uL));
			}
			IntPtr intPtr = new IntPtr(ptr);
			result = intPtr;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
		{
			uint num6 = 0u;
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
					num6 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num6 != 0;
				}).Invoke())
				{
				}
				if (num6 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num6);
			}
		}
		GC.KeepAlive(this);
		return result;
	}

	public unsafe byte[] GetBufferRGB()
	{
		//IL_0009: Expected I8, but got I
		//IL_00b0: Expected I, but got I8
		//IL_0084: Expected I, but got I8
		//IL_001b: Expected I, but got I8
		//IL_001c: Expected I8, but got I
		//IL_002f: Expected I, but got I8
		//IL_002f: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmImage* native = GetNative();
			long num2 = (nint)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImImageMemoryAllocation*>)(*(ulong*)(*(long*)native + 16)))((nint)native);
			long num3 = *(long*)(*(long*)num2 + 80);
			int num4 = (int)((long)global::_003CModule_003E.NmMediaProcessing_002EImImageMemoryAllocation_002ECmPlane_002EGetRowStride(((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, ImImageMemoryAllocation.CmPlane*>)num3)((nint)num2, 0uL)) * (long)Height);
			byte[] array = new byte[num4];
			Marshal.Copy(GetBufferPointerRGB(bReadOnly: true), array, 0, num4);
			GC.KeepAlive(this);
			return array;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
		{
			uint num5 = 0u;
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
					num5 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num5 != 0;
				}).Invoke())
				{
				}
				if (num5 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num5);
			}
		}
		return null;
	}

	public unsafe void ConvertToRGB([MarshalAs(UnmanagedType.U1)] bool bPadding)
	{
		//IL_0008: Expected I8, but got I
		//IL_0099: Expected I, but got I8
		//IL_006f: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			ulong num2 = (ulong)(bPadding ? 4 : 1);
			ulong num3 = num2;
			CmImage* native = GetNative();
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmImageColorFormatConvert cmImageColorFormatConvert);
			CmImageColorFormatConvert* ptr = global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bctor_007D(&cmImageColorFormatConvert, (CmBaseImage.colorFormat_t)2, num2);
			try
			{
				global::_003CModule_003E.NmMediaProcessing_002ECmImage_002ETransform(native, (ImImageTransformation*)ptr);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmImageColorFormatConvert*, void>)(&global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bdtor_007D), &cmImageColorFormatConvert);
				throw;
			}
			global::_003CModule_003E.NmMediaProcessing_002ECmImageColorFormatConvert_002E_007Bdtor_007D(&cmImageColorFormatConvert);
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
	}

	public unsafe void Load(string fileName)
	{
		//IL_0008: Expected I8, but got I
		//IL_00a4: Expected I, but got I8
		//IL_007a: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (fileName == null)
		{
			throw new ArgumentNullException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj);
			global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020std_003A_003Abasic_string_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E_002Cclass_0020System_003A_003AString_0020_005E_003E(&obj, &fileName);
			try
			{
				std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* ptr = &obj;
				global::_003CModule_003E.NmMediaProcessing_002ECmImageFileHelpers_002ELoadFromWindowsBitmap(&obj, GetNative());
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj);
				throw;
			}
			try
			{
				global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_Tidy_deallocate(&obj);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std._Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E*, void>)(&global::_003CModule_003E.std_002E_Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E_002E_007Bdtor_007D), &obj);
				throw;
			}
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

	public unsafe void Save(string fileName)
	{
		//IL_0008: Expected I8, but got I
		//IL_00a4: Expected I, but got I8
		//IL_007a: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (fileName == null)
		{
			throw new ArgumentNullException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj);
			global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020std_003A_003Abasic_string_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E_002Cclass_0020System_003A_003AString_0020_005E_003E(&obj, &fileName);
			try
			{
				std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* ptr = &obj;
				global::_003CModule_003E.NmMediaProcessing_002ECmImageFileHelpers_002ESaveAsWindowsBitmap(&obj, GetNative());
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj);
				throw;
			}
			try
			{
				global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_Tidy_deallocate(&obj);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std._Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E*, void>)(&global::_003CModule_003E.std_002E_Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E_002E_007Bdtor_007D), &obj);
				throw;
			}
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

	public static Image CreateFromBitmapAsReference(Bitmap bitmap)
	{
		throw new NotSupportedException();
	}

	public static Image CreateFromBitmapAsCopy(Bitmap bitmap)
	{
		throw new NotSupportedException();
	}

	public unsafe void Create(int width, int height, creation_t creation)
	{
		//IL_0008: Expected I8, but got I
		//IL_00ca: Expected I, but got I8
		//IL_00a0: Expected I, but got I8
		//IL_0060: Expected I, but got I8
		//IL_0061: Expected I8, but got I
		//IL_0075: Expected I, but got I8
		//IL_0075: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (width > 0 && height > 0)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				switch (creation)
				{
				default:
					throw new NotSupportedException("Unknown creation type");
				case creation_t.uninitializedRGB:
					global::_003CModule_003E.NmMediaProcessing_002ECmBaseImage_002ECreate((CmBaseImage*)GetNative(), (ulong)width, (ulong)height, (CmBaseImage.colorFormat_t)2, 4uL);
					break;
				case creation_t.blackRGB:
				{
					global::_003CModule_003E.NmMediaProcessing_002ECmBaseImage_002ECreate((CmBaseImage*)GetNative(), (ulong)width, (ulong)height, (CmBaseImage.colorFormat_t)2, 4uL);
					CmImage* native = GetNative();
					long num2 = (nint)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImImageMemoryAllocation*>)(*(ulong*)(*(long*)native + 16)))((nint)native);
					long num3 = *(long*)(*(long*)num2 + 88);
					global::_003CModule_003E.NmMediaProcessing_002ECmImageBufferHelpers_002ESetBytes(0, ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, ImImageMemoryAllocation.CmWriteablePlane*>)num3)((nint)num2, 0uL));
					break;
				}
				}
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
			return;
		}
		throw new ArgumentException("Width and height must be positive");
	}

	public unsafe CmImage* GetNative()
	{
		CmImage* pImage = m_pImage;
		if (pImage == null)
		{
			throw new ObjectDisposedException("VideoOS.Platfom.MediaProcessing.Image");
		}
		return pImage;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007EImage();
			return;
		}
		try
		{
			_0021Image();
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

	~Image()
	{
		Dispose(A_0: false);
	}
}
