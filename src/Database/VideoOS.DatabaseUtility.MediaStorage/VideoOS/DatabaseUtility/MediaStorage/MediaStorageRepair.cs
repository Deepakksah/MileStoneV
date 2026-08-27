using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmDatabase.Utility;
using NmError;
using std;

namespace VideoOS.DatabaseUtility.MediaStorage;

public class MediaStorageRepair : IDisposable
{
	private unsafe CmMPMStoRepair* m_NativeRepair;

	public unsafe int TotalProgress
	{
		get
		{
			int result = global::_003CModule_003E.NmDatabase_002EUtility_002ECmMPMStoRepair_002EGetTotalProgress(m_NativeRepair);
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe int TableProgress
	{
		get
		{
			int result = global::_003CModule_003E.NmDatabase_002EUtility_002ECmMPMStoRepair_002EGetTableProgress(m_NativeRepair);
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe MediaStorageRepair(IList<TableInfo> tablesToRepair)
	{
		//IL_0035: Expected I, but got I8
		System.Runtime.CompilerServices.Unsafe.SkipInit(out list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E obj);
		global::_003CModule_003E.ConvertFromManagedList_003Cclass_0020NmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cclass_0020VideoOS_003A_003ADatabaseUtility_003A_003AMediaStorage_003A_003ATableInfo_0020_005E_003E(&obj, tablesToRepair);
		try
		{
			CmMPMStoRepair* ptr = (CmMPMStoRepair*)global::_003CModule_003E.@new(112uL);
			CmMPMStoRepair* nativeRepair;
			try
			{
				if (ptr != null)
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E obj2);
					list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* ptr2 = &obj2;
					list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* ptr3 = global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_007Bctor_007D(&obj2, &obj);
					nativeRepair = global::_003CModule_003E.NmDatabase_002EUtility_002ECmMPMStoRepair_002E_007Bctor_007D(ptr, ptr3);
				}
				else
				{
					nativeRepair = null;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 112uL);
				throw;
			}
			m_NativeRepair = nativeRepair;
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_007Bdtor_007D), &obj);
			throw;
		}
		global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_Tidy(&obj);
	}

	private void _007EMediaStorageRepair()
	{
		_0021MediaStorageRepair();
	}

	private unsafe void _0021MediaStorageRepair()
	{
		CmMPMStoRepair* nativeRepair = m_NativeRepair;
		if (nativeRepair != null)
		{
			global::_003CModule_003E.NmDatabase_002EUtility_002ECmMPMStoRepair_002E_007Bdtor_007D(nativeRepair);
			global::_003CModule_003E.delete(nativeRepair, 112uL);
		}
		GC.KeepAlive(this);
	}

	public unsafe void Cancel()
	{
		global::_003CModule_003E.NmDatabase_002EUtility_002ECmMPMStoRepair_002ECancel(m_NativeRepair);
		GC.KeepAlive(this);
	}

	public unsafe void Repair(out IList<TableInfo> repairedTables)
	{
		//IL_0008: Expected I8, but got I
		//IL_00b7: Expected I, but got I8
		//IL_0069: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E obj);
		*(long*)(&obj) = 0L;
		System.Runtime.CompilerServices.Unsafe.As<list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8)) = 0L;
		global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_Alloc_sentinel_and_proxy(&obj);
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMediaStorageUtilityError* ptr2);
			try
			{
				global::_003CModule_003E.NmDatabase_002EUtility_002ECmMPMStoRepair_002ERepair(m_NativeRepair, &obj);
				System.Runtime.CompilerServices.Unsafe.SkipInit(out list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E obj2);
				list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* ptr = &obj2;
				list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* listToConvert = global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_007Bctor_007D(&obj2, &obj);
				repairedTables = global::_003CModule_003E.ConvertToManagedList_003Cclass_0020NmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cclass_0020VideoOS_003A_003ADatabaseUtility_003A_003AMediaStorage_003A_003ATableInfo_0020_005E_003E(listToConvert);
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVCmMediaStorageUtilityError_0040Utility_0040NmDatabase_0040_0040_00408), 9, &ptr2) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj4);
						std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* from_obj = global::_003CModule_003E.NmError_002ECmError_002EGetErrorDescription((CmError*)ptr2, &obj4);
						try
						{
							throw new MediaStorageUtilityException(global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003AString_0020_005E_002Cclass_0020std_003A_003Abasic_string_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E_0020_003E(from_obj), (MediaStorageUtilityException.ErrorCode)global::_003CModule_003E.NmDatabase_002EUtility_002ECmMediaStorageUtilityError_002EGetErrorCode(ptr2));
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj4);
							throw;
						}
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
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_007Bdtor_007D), &obj);
			throw;
		}
		global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_Tidy(&obj);
		GC.KeepAlive(this);
		try
		{
			return;
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_007Bdtor_007D), &obj);
			throw;
		}
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_0021MediaStorageRepair();
			return;
		}
		try
		{
			_0021MediaStorageRepair();
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

	~MediaStorageRepair()
	{
		Dispose(A_0: false);
	}
}
