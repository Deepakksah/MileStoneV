using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NmToolkit;
using std;

namespace VideoOS.Toolkit;

public unsafe class Image(ImImage* image) : MediaData((ImMediaData*)image), IImage
{
	private unsafe ImImage* m_Image = image;

	public unsafe virtual int Height
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_0084: Expected I, but got I8
			//IL_005a: Expected I, but got I8
			//IL_00d5: Expected I, but got I8
			//IL_00ab: Expected I, but got I8
			//IL_002e: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			if (m_Image == null)
			{
				throw new ArgumentNullException("Internal native image is uninitialized.");
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				ImImage* image = m_Image;
				return (int)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong>)(*(ulong*)(*(long*)image + 104)))((nint)image);
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new ToolkitException(error);
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
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new StdCppException(error2);
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
			return 0;
		}
	}

	public unsafe virtual int Width
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_0084: Expected I, but got I8
			//IL_005a: Expected I, but got I8
			//IL_00d5: Expected I, but got I8
			//IL_00ab: Expected I, but got I8
			//IL_002e: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			if (m_Image == null)
			{
				throw new ArgumentNullException("Internal native image is uninitialized.");
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				ImImage* image = m_Image;
				return (int)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong>)(*(ulong*)(*(long*)image + 96)))((nint)image);
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new ToolkitException(error);
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
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new StdCppException(error2);
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
			return 0;
		}
	}

	public unsafe virtual double PixelAspectRatio
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_0087: Expected I, but got I8
			//IL_005d: Expected I, but got I8
			//IL_00d8: Expected I, but got I8
			//IL_00ae: Expected I, but got I8
			//IL_002e: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			if (m_Image == null)
			{
				throw new ArgumentNullException("Internal native image is uninitialized.");
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				ImImage* image = m_Image;
				return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, double>)(*(ulong*)(*(long*)image + 88)))((nint)image);
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new ToolkitException(error);
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
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new StdCppException(error2);
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
			return 0.0;
		}
	}

	public unsafe virtual IImage.ImageColorSpaceDetail ColorSpaceDetail
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_0089: Expected I, but got I8
			//IL_005e: Expected I, but got I8
			//IL_00da: Expected I, but got I8
			//IL_00b0: Expected I, but got I8
			//IL_002e: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			if (m_Image == null)
			{
				throw new ArgumentNullException("Internal native image is uninitialized.");
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				ImImage* image = m_Image;
				ImImage.color_space_detail_t color_space_detail_t = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImImage.color_space_detail_t>)(*(ulong*)(*(long*)image + 80)))((nint)image);
				IImage.ImageColorSpaceDetail imageColorSpaceDetail = (IImage.ImageColorSpaceDetail)color_space_detail_t;
				return (IImage.ImageColorSpaceDetail)color_space_detail_t;
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new ToolkitException(error);
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
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new StdCppException(error2);
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
			return IImage.ImageColorSpaceDetail.NotApplicable;
		}
	}

	public unsafe virtual IImage.ImageFormat Format
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_0089: Expected I, but got I8
			//IL_005e: Expected I, but got I8
			//IL_00da: Expected I, but got I8
			//IL_00b0: Expected I, but got I8
			//IL_002e: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			if (m_Image == null)
			{
				throw new ArgumentNullException("Internal native image is uninitialized.");
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				ImImage* image = m_Image;
				ImImage.image_format_t image_format_t = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImImage.image_format_t>)(*(ulong*)(*(long*)image + 72)))((nint)image);
				IImage.ImageFormat imageFormat = (IImage.ImageFormat)image_format_t;
				return (IImage.ImageFormat)image_format_t;
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new ToolkitException(error);
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
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new StdCppException(error2);
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
			return IImage.ImageFormat.Gray;
		}
	}

	public unsafe virtual int GetPlaneWidth(int planeIndex)
	{
		//IL_0008: Expected I8, but got I
		//IL_008d: Expected I, but got I8
		//IL_0062: Expected I, but got I8
		//IL_00de: Expected I, but got I8
		//IL_00b4: Expected I, but got I8
		//IL_0036: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_Image == null)
		{
			throw new ArgumentNullException("Internal native image is uninitialized.");
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
		try
		{
			ImImage* image = m_Image;
			long num2 = *(long*)(*(long*)image + 112);
			return (int)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, ulong>)num2)((nint)image, (ulong)planeIndex);
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new ToolkitException(error);
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
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new StdCppException(error2);
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
		return 0;
	}

	public unsafe virtual int GetPlaneHeight(int planeIndex)
	{
		//IL_0008: Expected I8, but got I
		//IL_008d: Expected I, but got I8
		//IL_0062: Expected I, but got I8
		//IL_00de: Expected I, but got I8
		//IL_00b4: Expected I, but got I8
		//IL_0036: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_Image == null)
		{
			throw new ArgumentNullException("Internal native image is uninitialized.");
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
		try
		{
			ImImage* image = m_Image;
			long num2 = *(long*)(*(long*)image + 120);
			return (int)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, ulong>)num2)((nint)image, (ulong)planeIndex);
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new ToolkitException(error);
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
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new StdCppException(error2);
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
		return 0;
	}

	public unsafe virtual int GetPlaneStride(int planeIndex)
	{
		//IL_0008: Expected I8, but got I
		//IL_0090: Expected I, but got I8
		//IL_0065: Expected I, but got I8
		//IL_00e1: Expected I, but got I8
		//IL_00b7: Expected I, but got I8
		//IL_0039: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_Image == null)
		{
			throw new ArgumentNullException("Internal native image is uninitialized.");
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
		try
		{
			ImImage* image = m_Image;
			long num2 = *(long*)(*(long*)image + 128);
			return (int)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, ulong>)num2)((nint)image, (ulong)planeIndex);
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new ToolkitException(error);
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
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new StdCppException(error2);
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
		return 0;
	}

	public unsafe virtual byte[] GetPlane(int planeIndex)
	{
		//IL_0008: Expected I8, but got I
		//IL_0107: Expected I, but got I8
		//IL_00dc: Expected I, but got I8
		//IL_0159: Expected I, but got I8
		//IL_012e: Expected I, but got I8
		//IL_003e: Expected I, but got I8
		//IL_008c: Expected I, but got I8
		//IL_0096: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_Image == null)
		{
			throw new ArgumentNullException("Internal native image is uninitialized.");
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
		try
		{
			ImImage* image = m_Image;
			long num2 = *(long*)(*(long*)image + 136);
			ulong num3 = (ulong)planeIndex;
			byte* ptr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, byte*>)num2)((nint)image, num3);
			if (ptr == null)
			{
				return null;
			}
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = new IntPtr(ptr);
			zero = intPtr;
			ImImage* image2 = m_Image;
			ImImage* ptr2 = image2;
			long num4 = *(long*)image2;
			long num5 = *(long*)(num4 + 120);
			long num6 = *(long*)(num4 + 128);
			int num7 = (int)(((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, ulong>)num6)((nint)image2, num3) * ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, ulong>)num5)((nint)ptr2, num3));
			byte[] array = new byte[num7];
			Marshal.Copy(intPtr, array, 0, num7);
			return array;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
		{
			uint num8 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new ToolkitException(error);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num8 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num8 != 0;
				}).Invoke())
				{
				}
				if (num8 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num8);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
		{
			uint num8 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new StdCppException(error2);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num8 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num8 != 0;
				}).Invoke())
				{
				}
				if (num8 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num8);
			}
		}
		return null;
	}

	public unsafe virtual IntPtr GetPlanePointer(int planeIndex)
	{
		//IL_0008: Expected I8, but got I
		//IL_00a5: Expected I, but got I8
		//IL_007a: Expected I, but got I8
		//IL_00f7: Expected I, but got I8
		//IL_00cc: Expected I, but got I8
		//IL_0039: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_Image == null)
		{
			throw new ArgumentNullException("Internal native image is uninitialized.");
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
		try
		{
			ImImage* image = m_Image;
			long num2 = *(long*)(*(long*)image + 136);
			byte* value = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, byte*>)num2)((nint)image, (ulong)planeIndex);
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = new IntPtr(value);
			zero = intPtr;
			return intPtr;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new ToolkitException(error);
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
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new StdCppException(error2);
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
		System.Runtime.CompilerServices.Unsafe.SkipInit(out IntPtr result);
		return result;
	}
}
