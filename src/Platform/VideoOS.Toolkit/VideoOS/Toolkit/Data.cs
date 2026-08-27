using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmTimeStamp;
using NmToolkit;
using msclr.interop;
using std;

namespace VideoOS.Toolkit;

public unsafe class Data(ImData* data) : IData
{
	private unsafe ImData* m_Data = data;

	public unsafe virtual DateTime EndTimeStamp
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_00c0: Expected I, but got I8
			//IL_0095: Expected I, but got I8
			//IL_0111: Expected I, but got I8
			//IL_00e7: Expected I, but got I8
			//IL_002e: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			if (m_Data == null)
			{
				throw new ArgumentNullException("Internal native data is uninitialized.");
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				ImData* data = m_Data;
				long num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, long>)(*(ulong*)(*(long*)data + 16)))((nint)data);
				GC.KeepAlive(this);
				System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
				NmTimeStamp.CmTimeStamp* stamp = global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bctor_007D(&cmTimeStamp, num2);
				DateTime dateTime;
				try
				{
					dateTime = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003ADateTime_002Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_003E(stamp);
					DateTime dateTime2 = dateTime;
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
					throw;
				}
				global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
				GC.KeepAlive(this);
				return dateTime;
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
			System.Runtime.CompilerServices.Unsafe.SkipInit(out DateTime result);
			return result;
		}
	}

	public unsafe virtual DateTime BeginTimeStamp
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_00bf: Expected I, but got I8
			//IL_0094: Expected I, but got I8
			//IL_0110: Expected I, but got I8
			//IL_00e6: Expected I, but got I8
			//IL_002d: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			if (m_Data == null)
			{
				throw new ArgumentNullException("Internal native data is uninitialized.");
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				ImData* data = m_Data;
				long num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, long>)(*(ulong*)(*(long*)data + 8)))((nint)data);
				GC.KeepAlive(this);
				System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
				NmTimeStamp.CmTimeStamp* stamp = global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bctor_007D(&cmTimeStamp, num2);
				DateTime dateTime;
				try
				{
					dateTime = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003ADateTime_002Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_003E(stamp);
					DateTime dateTime2 = dateTime;
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
					throw;
				}
				global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
				GC.KeepAlive(this);
				return dateTime;
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
			System.Runtime.CompilerServices.Unsafe.SkipInit(out DateTime result);
			return result;
		}
	}

	private void _007EData()
	{
		_0021Data();
	}

	private unsafe void _0021Data()
	{
		//IL_001f: Expected I, but got I8
		//IL_0016: Expected I, but got I8
		ImData* data = m_Data;
		if (data != null)
		{
			long num = *(long*)(*(ulong*)data);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)data, 1u);
		}
		m_Data = null;
	}

	public unsafe virtual byte[] GetData()
	{
		//IL_0008: Expected I8, but got I
		//IL_00d2: Expected I, but got I8
		//IL_00a7: Expected I, but got I8
		//IL_0124: Expected I, but got I8
		//IL_00f9: Expected I, but got I8
		//IL_002e: Expected I, but got I8
		//IL_0062: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_Data == null)
		{
			throw new ArgumentNullException("Internal native data is uninitialized.");
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
		try
		{
			ImData* data = m_Data;
			byte* ptr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte*>)(*(ulong*)(*(long*)data + 32)))((nint)data);
			if (ptr == null)
			{
				return null;
			}
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = new IntPtr(ptr);
			zero = intPtr;
			ImData* data2 = m_Data;
			int num2 = (int)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong>)(*(ulong*)(*(long*)data2 + 24)))((nint)data2);
			byte[] array = new byte[num2];
			Marshal.Copy(intPtr, array, 0, num2);
			GC.KeepAlive(this);
			return array;
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

	public unsafe virtual IntPtr GetNativeData(out ulong length)
	{
		//IL_0023: Expected I, but got I8
		//IL_0038: Expected I, but got I8
		ImData* data = m_Data;
		if (data == null)
		{
			throw new ArgumentNullException("Internal native data is uninitialized.");
		}
		ImData* intPtr = data;
		length = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong>)(*(ulong*)(*(long*)intPtr + 24)))((nint)intPtr);
		data = m_Data;
		ImData* intPtr2 = data;
		byte* ptr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte*>)(*(ulong*)(*(long*)intPtr2 + 32)))((nint)intPtr2);
		if (ptr == null)
		{
			length = 0uL;
		}
		GC.KeepAlive(this);
		IntPtr result = new IntPtr(ptr);
		GC.KeepAlive(this);
		return result;
	}

	public unsafe virtual IProperty GetProperty(string propertyName)
	{
		//IL_0008: Expected I8, but got I
		//IL_0299: Expected I, but got I8
		//IL_026e: Expected I, but got I8
		//IL_02eb: Expected I, but got I8
		//IL_02c0: Expected I, but got I8
		//IL_0047: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_Data == null)
		{
			throw new ArgumentNullException("Internal native data is uninitialized.");
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out utf8string_t utf8string_t2);
			global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020msclr_003A_003Ainterop_003A_003Autf8string_t_002Cclass_0020System_003A_003AString_0020_005E_003E(&utf8string_t2, &propertyName);
			Property property;
			try
			{
				std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* ptr = (std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)(&utf8string_t2);
				ImData* data = m_Data;
				long num2 = *(long*)(*(long*)data + 40);
				ImProperty* ptr2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, ImProperty*>)num2)((nint)data, (std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)(&utf8string_t2));
				property = null;
				if (ptr2 != null)
				{
					property = ((global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImUTCTimeProperty_0040NmToolkit_0040_0040_00408), 0) != null) ? new UTCTimeProperty((ImUTCTimeProperty*)global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImUTCTimeProperty_0040NmToolkit_0040_0040_00408), 0), this) : ((global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImInt32Property_0040NmToolkit_0040_0040_00408), 0) != null) ? new Int32Property((ImInt32Property*)global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImInt32Property_0040NmToolkit_0040_0040_00408), 0), this) : ((global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImInt64Property_0040NmToolkit_0040_0040_00408), 0) != null) ? new Int64Property((ImInt64Property*)global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImInt64Property_0040NmToolkit_0040_0040_00408), 0), this) : ((global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImDoubleProperty_0040NmToolkit_0040_0040_00408), 0) != null) ? new DoubleProperty((ImDoubleProperty*)global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImDoubleProperty_0040NmToolkit_0040_0040_00408), 0), this) : ((global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImPrivacyMaskProperty_0040NmToolkit_0040_0040_00408), 0) != null) ? new PrivacyMaskProperty((ImPrivacyMaskProperty*)global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImPrivacyMaskProperty_0040NmToolkit_0040_0040_00408), 0), this) : ((global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImStringProperty_0040NmToolkit_0040_0040_00408), 0) != null) ? new StringProperty((ImStringProperty*)global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImStringProperty_0040NmToolkit_0040_0040_00408), 0), this) : ((global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImBooleanProperty_0040NmToolkit_0040_0040_00408), 0) != null) ? new BooleanProperty((ImBooleanProperty*)global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImBooleanProperty_0040NmToolkit_0040_0040_00408), 0), this) : ((global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImByteArrayProperty_0040NmToolkit_0040_0040_00408), 0) != null) ? new ByteArrayProperty((ImByteArrayProperty*)global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImByteArrayProperty_0040NmToolkit_0040_0040_00408), 0), this) : ((global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImDataProperty_0040NmToolkit_0040_0040_00408), 0) == null) ? new Property(ptr2, this) : new DataProperty((ImDataProperty*)global::_003CModule_003E.__RTDynamicCast(ptr2, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImDataProperty_0040NmToolkit_0040_0040_00408), 0), this))))))))));
				}
				GC.KeepAlive(this);
				IProperty property2 = property;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t2);
				throw;
			}
			try
			{
				global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_Tidy_deallocate((std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)(&utf8string_t2));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std._Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E*, void>)(&global::_003CModule_003E.std_002E_Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E_002E_007Bdtor_007D), &utf8string_t2);
				throw;
			}
			return property;
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

	public unsafe virtual IData Clone()
	{
		//IL_0008: Expected I8, but got I
		//IL_008e: Expected I, but got I8
		//IL_0064: Expected I, but got I8
		//IL_00df: Expected I, but got I8
		//IL_00b5: Expected I, but got I8
		//IL_002e: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_Data == null)
		{
			throw new ArgumentNullException("Internal native data is uninitialized.");
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
		try
		{
			ImData* data = m_Data;
			IData result = DataFactory.CreateFromImData(((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImData*>)(*(ulong*)(*(long*)data + 48)))((nint)data));
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
		return null;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007EData();
			return;
		}
		try
		{
			_0021Data();
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

	~Data()
	{
		Dispose(A_0: false);
	}
}
