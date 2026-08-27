using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NmToolkit;
using std;

namespace VideoOS.Toolkit;

public class PrivacyMaskProperty : Property, IPrivacyMaskProperty
{
	public static long UnknownId = -1L;

	public unsafe virtual PrivacyMaskGridSize GridSize
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_0089: Expected I, but got I8
			//IL_005f: Expected I, but got I8
			//IL_00da: Expected I, but got I8
			//IL_00b0: Expected I, but got I8
			//IL_002a: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				void* intPtr = global::_003CModule_003E.__RTDynamicCast(m_Property, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImPrivacyMaskProperty_0040NmToolkit_0040_0040_00408), 0);
				ImPrivacyMaskProperty.mask_size_t result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImPrivacyMaskProperty.mask_size_t>)(*(ulong*)(*(long*)intPtr + 8)))((nint)intPtr);
				GC.KeepAlive(this);
				return (PrivacyMaskGridSize)result;
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
			return PrivacyMaskGridSize.Size0;
		}
	}

	public unsafe virtual long Id
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_0082: Expected I, but got I8
			//IL_0058: Expected I, but got I8
			//IL_00d3: Expected I, but got I8
			//IL_00a9: Expected I, but got I8
			//IL_0027: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				void* intPtr = global::_003CModule_003E.__RTDynamicCast(m_Property, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImPrivacyMaskProperty_0040NmToolkit_0040_0040_00408), 0);
				long result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, long>)(*(ulong*)(*(ulong*)intPtr)))((nint)intPtr);
				GC.KeepAlive(this);
				return result;
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
			return 0L;
		}
	}

	public unsafe PrivacyMaskProperty(ImPrivacyMaskProperty* property, object ownerRef)
	{
		//IL_0018: Expected I, but got I8
		//IL_0006: Expected I, but got I8
		ImProperty* property2;
		if (property == null)
		{
			property2 = null;
		}
		else
		{
			property2 = (ImProperty*)((long)(nint)property + (long)(*(int*)(*(long*)((ulong)(nint)property + 8uL) + 4)) + 8);
		}
		base._002Ector(property2, ownerRef);
	}

	public unsafe virtual IEnumerable<PrivacyMaskCellInformation> GetCellInformation()
	{
		//IL_0008: Expected I8, but got I
		//IL_0146: Expected I, but got I8
		//IL_011b: Expected I, but got I8
		//IL_0198: Expected I, but got I8
		//IL_016d: Expected I, but got I8
		//IL_002e: Expected I, but got I8
		//IL_0064: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
		try
		{
			ImPrivacyMaskProperty* ptr = (ImPrivacyMaskProperty*)global::_003CModule_003E.__RTDynamicCast(m_Property, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImPrivacyMaskProperty_0040NmToolkit_0040_0040_00408), 0);
			byte b = (byte)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImPrivacyMaskProperty.mask_size_t>)(*(ulong*)(*(long*)ptr + 8)))((nint)ptr);
			List<PrivacyMaskCellInformation> list = new List<PrivacyMaskCellInformation>(b);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImPrivacyMaskProperty.SmCellData smCellData);
			for (byte b2 = 0; (uint)b2 < (uint)b; b2++)
			{
				for (byte b3 = 0; (uint)b3 < (uint)b; b3++)
				{
					long num2 = *(long*)(*(long*)ptr + 16);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte, byte, ImPrivacyMaskProperty.SmCellData*, void>)num2)((nint)ptr, b2, b3, &smCellData);
					switch ((int)(*(byte*)(&smCellData)))
					{
					default:
						throw new InvalidOperationException("Unsupported cell method returned.");
					case 2:
					{
						BlurCellData data2 = new BlurCellData(System.Runtime.CompilerServices.Unsafe.As<ImPrivacyMaskProperty.SmCellData, ImPrivacyMaskProperty.SmBlurCellParameters>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smCellData, 2)), System.Runtime.CompilerServices.Unsafe.As<ImPrivacyMaskProperty.SmCellData, bool>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smCellData, 1)));
						PrivacyMaskCellInformation item2 = new PrivacyMaskCellInformation(b2, b3, data2);
						list.Add(item2);
						break;
					}
					case 1:
					{
						SolidCellData data = new SolidCellData(System.Runtime.CompilerServices.Unsafe.As<ImPrivacyMaskProperty.SmCellData, ImPrivacyMaskProperty.SmSolidCellParameters>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smCellData, 2)), System.Runtime.CompilerServices.Unsafe.As<ImPrivacyMaskProperty.SmCellData, bool>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smCellData, 1)));
						PrivacyMaskCellInformation item = new PrivacyMaskCellInformation(b2, b3, data);
						list.Add(item);
						break;
					}
					case 0:
						break;
					}
				}
			}
			GC.KeepAlive(this);
			return list;
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
		return null;
	}

	public unsafe virtual PrivacyMaskCellData GetCellData(byte row, byte column)
	{
		//IL_0008: Expected I8, but got I
		//IL_0119: Expected I, but got I8
		//IL_00ee: Expected I, but got I8
		//IL_016b: Expected I, but got I8
		//IL_0140: Expected I, but got I8
		//IL_002e: Expected I, but got I8
		//IL_0067: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
		try
		{
			ImPrivacyMaskProperty* ptr = (ImPrivacyMaskProperty*)global::_003CModule_003E.__RTDynamicCast(m_Property, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImPrivacyMaskProperty_0040NmToolkit_0040_0040_00408), 0);
			byte b = (byte)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImPrivacyMaskProperty.mask_size_t>)(*(ulong*)(*(long*)ptr + 8)))((nint)ptr);
			if ((uint)row >= (uint)b)
			{
				throw new IndexOutOfRangeException("The value 'row' is out of range.");
			}
			if ((uint)column >= (uint)b)
			{
				throw new IndexOutOfRangeException("The value 'column' is out of range.");
			}
			long num2 = *(long*)(*(long*)ptr + 16);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImPrivacyMaskProperty.SmCellData smCellData);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte, byte, ImPrivacyMaskProperty.SmCellData*, void>)num2)((nint)ptr, row, column, &smCellData);
			GC.KeepAlive(this);
			switch ((int)(*(byte*)(&smCellData)))
			{
			default:
				throw new InvalidOperationException("Unsupported cell method returned.");
			case 2:
				return new BlurCellData(System.Runtime.CompilerServices.Unsafe.As<ImPrivacyMaskProperty.SmCellData, ImPrivacyMaskProperty.SmBlurCellParameters>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smCellData, 2)), System.Runtime.CompilerServices.Unsafe.As<ImPrivacyMaskProperty.SmCellData, bool>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smCellData, 1)));
			case 1:
				return new SolidCellData(System.Runtime.CompilerServices.Unsafe.As<ImPrivacyMaskProperty.SmCellData, ImPrivacyMaskProperty.SmSolidCellParameters>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smCellData, 2)), System.Runtime.CompilerServices.Unsafe.As<ImPrivacyMaskProperty.SmCellData, bool>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smCellData, 1)));
			case 0:
			{
				EmptyCellData instance = EmptyCellData.Instance;
				GC.KeepAlive(this);
				return instance;
			}
			}
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
		return null;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe virtual bool IsEmpty([MarshalAs(UnmanagedType.U1)] bool ignoreRemovableMasks)
	{
		//IL_0008: Expected I8, but got I
		//IL_0092: Expected I, but got I8
		//IL_0067: Expected I, but got I8
		//IL_00e3: Expected I, but got I8
		//IL_00b9: Expected I, but got I8
		//IL_0032: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
		try
		{
			ImPrivacyMaskProperty* ptr = (ImPrivacyMaskProperty*)global::_003CModule_003E.__RTDynamicCast(m_Property, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImPrivacyMaskProperty_0040NmToolkit_0040_0040_00408), 0);
			long num2 = *(long*)(*(long*)ptr + 24);
			bool result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte, byte>)num2)((nint)ptr, ignoreRemovableMasks ? ((byte)1) : ((byte)0)) != 0;
			GC.KeepAlive(this);
			return result;
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
		return false;
	}
}
