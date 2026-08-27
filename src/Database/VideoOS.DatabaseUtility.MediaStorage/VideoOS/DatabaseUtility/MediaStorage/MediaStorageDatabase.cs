using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmDatabase.Utility;
using NmDatabaseUtility;
using NmError;
using NmTimeStamp;
using msclr.interop;
using std;

namespace VideoOS.DatabaseUtility.MediaStorage;

public class MediaStorageDatabase : IDatabaseMetaData
{
	private unsafe ImDatabaseMetaData* m_Database;

	public unsafe MediaStorageDatabase(TableInfo[] tables, [MarshalAs(UnmanagedType.U1)] bool includeSiblingArchives, string masterPassword)
	{
		//IL_006d: Expected I, but got I8
		uint num = 0u;
		CmMPMStoDatabase* ptr = (CmMPMStoDatabase*)global::_003CModule_003E.@new(208uL);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out msclr.interop.utf8string_t utf8string_t2);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E obj);
		CmMPMStoDatabase* database;
		try
		{
			if (ptr != null)
			{
				msclr.interop.utf8string_t* ptr2 = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020msclr_003A_003Ainterop_003A_003Autf8string_t_002Cclass_0020System_003A_003AString_0020_005E_003E(&utf8string_t2, &masterPassword);
				try
				{
					num = 1u;
					list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* ptr3 = global::_003CModule_003E.ConvertFromManagedArray_003Cclass_0020NmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cclass_0020VideoOS_003A_003ADatabaseUtility_003A_003AMediaStorage_003A_003ATableInfo_0020_005E_003E(&obj, tables);
					try
					{
						num = 3u;
						database = global::_003CModule_003E.NmDatabaseUtility_002ECmMPMStoDatabase_002E_007Bctor_007D(ptr, ptr3, includeSiblingArchives, (std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)ptr2);
					}
					catch
					{
						//try-fault
						if ((num & 2) != 0)
						{
							num &= 0xFFFFFFFDu;
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_007Bdtor_007D), &obj);
						}
						throw;
					}
				}
				catch
				{
					//try-fault
					if ((num & 1) != 0)
					{
						num &= 0xFFFFFFFEu;
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<msclr.interop.utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t2);
					}
					throw;
				}
			}
			else
			{
				database = null;
			}
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr, 208uL);
			throw;
		}
		try
		{
			try
			{
				m_Database = (ImDatabaseMetaData*)database;
			}
			catch
			{
				//try-fault
				if ((num & 2) != 0)
				{
					num &= 0xFFFFFFFDu;
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_007Bdtor_007D), &obj);
				}
				throw;
			}
			if ((num & 2) != 0)
			{
				num &= 0xFFFFFFFDu;
				global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_Tidy(&obj);
			}
		}
		catch
		{
			//try-fault
			if ((num & 1) != 0)
			{
				num &= 0xFFFFFFFEu;
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<msclr.interop.utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t2);
			}
			throw;
		}
		if ((num & 1) != 0)
		{
			num &= 0xFFFFFFFEu;
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
		}
		base._002Ector();
		GC.KeepAlive(masterPassword);
		GC.KeepAlive(tables);
		GC.KeepAlive(this);
	}

	public unsafe MediaStorageDatabase(TableInfo table, [MarshalAs(UnmanagedType.U1)] bool includeSiblingArchives)
	{
		//IL_0045: Expected I, but got I8
		uint num = 0u;
		CmMPMStoDatabase* ptr = (CmMPMStoDatabase*)global::_003CModule_003E.@new(208uL);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmTableInfo cmTableInfo);
		CmMPMStoDatabase* database;
		try
		{
			if (ptr != null)
			{
				CmTableInfo* ptr2 = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cclass_0020VideoOS_003A_003ADatabaseUtility_003A_003AMediaStorage_003A_003ATableInfo_0020_005E_003E(&cmTableInfo, &table);
				try
				{
					num = 1u;
					database = global::_003CModule_003E.NmDatabaseUtility_002ECmMPMStoDatabase_002E_007Bctor_007D(ptr, ptr2, includeSiblingArchives);
				}
				catch
				{
					//try-fault
					if ((num & 1) != 0)
					{
						num &= 0xFFFFFFFEu;
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTableInfo*, void>)(&global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bdtor_007D), &cmTableInfo);
					}
					throw;
				}
			}
			else
			{
				database = null;
			}
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr, 208uL);
			throw;
		}
		try
		{
			m_Database = (ImDatabaseMetaData*)database;
		}
		catch
		{
			//try-fault
			if ((num & 1) != 0)
			{
				num &= 0xFFFFFFFEu;
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTableInfo*, void>)(&global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bdtor_007D), &cmTableInfo);
			}
			throw;
		}
		if ((num & 1) != 0)
		{
			num &= 0xFFFFFFFEu;
			global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bdtor_007D(&cmTableInfo);
		}
		base._002Ector();
	}

	public unsafe MediaStorageDatabase(TableInfo table)
	{
		//IL_0045: Expected I, but got I8
		uint num = 0u;
		CmMPMStoDatabase* ptr = (CmMPMStoDatabase*)global::_003CModule_003E.@new(208uL);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmTableInfo cmTableInfo);
		CmMPMStoDatabase* database;
		try
		{
			if (ptr != null)
			{
				CmTableInfo* ptr2 = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cclass_0020VideoOS_003A_003ADatabaseUtility_003A_003AMediaStorage_003A_003ATableInfo_0020_005E_003E(&cmTableInfo, &table);
				try
				{
					num = 1u;
					database = global::_003CModule_003E.NmDatabaseUtility_002ECmMPMStoDatabase_002E_007Bctor_007D(ptr, ptr2, true);
				}
				catch
				{
					//try-fault
					if ((num & 1) != 0)
					{
						num &= 0xFFFFFFFEu;
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTableInfo*, void>)(&global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bdtor_007D), &cmTableInfo);
					}
					throw;
				}
			}
			else
			{
				database = null;
			}
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr, 208uL);
			throw;
		}
		try
		{
			m_Database = (ImDatabaseMetaData*)database;
		}
		catch
		{
			//try-fault
			if ((num & 1) != 0)
			{
				num &= 0xFFFFFFFEu;
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTableInfo*, void>)(&global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bdtor_007D), &cmTableInfo);
			}
			throw;
		}
		if ((num & 1) != 0)
		{
			num &= 0xFFFFFFFEu;
			global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bdtor_007D(&cmTableInfo);
		}
		base._002Ector();
	}

	private void _007EMediaStorageDatabase()
	{
		_0021MediaStorageDatabase();
	}

	private unsafe void _0021MediaStorageDatabase()
	{
		//IL_0016: Expected I, but got I8
		ImDatabaseMetaData* database = m_Database;
		if (database != null)
		{
			long num = *(long*)(*(ulong*)database);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)database, 1u);
		}
		GC.KeepAlive(this);
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe virtual bool Open()
	{
		//IL_0008: Expected I8, but got I
		//IL_0095: Expected I, but got I8
		//IL_0045: Expected I, but got I8
		//IL_001b: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		bool result = true;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMediaStorageUtilityError* ptr);
		try
		{
			ImDatabaseMetaData* database = m_Database;
			result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte>)(*(ulong*)(*(long*)database + 8)))((nint)database) != 0;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVCmMediaStorageUtilityError_0040Utility_0040NmDatabase_0040_0040_00408), 9, &ptr) != 0)
		{
			uint num2 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj2);
					std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* from_obj = global::_003CModule_003E.NmError_002ECmError_002EGetErrorDescription((CmError*)ptr, &obj2);
					try
					{
						throw new MediaStorageUtilityException(global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003AString_0020_005E_002Cclass_0020std_003A_003Abasic_string_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E_0020_003E(from_obj), (MediaStorageUtilityException.ErrorCode)global::_003CModule_003E.NmDatabase_002EUtility_002ECmMediaStorageUtilityError_002EGetErrorCode(ptr));
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj2);
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
		GC.KeepAlive(this);
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe virtual bool IsOpen()
	{
		//IL_0008: Expected I8, but got I
		//IL_0096: Expected I, but got I8
		//IL_0046: Expected I, but got I8
		//IL_001c: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		bool result = true;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMediaStorageUtilityError* ptr);
		try
		{
			ImDatabaseMetaData* database = m_Database;
			result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte>)(*(ulong*)(*(long*)database + 16)))((nint)database) != 0;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVCmMediaStorageUtilityError_0040Utility_0040NmDatabase_0040_0040_00408), 9, &ptr) != 0)
		{
			uint num2 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj2);
					std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* from_obj = global::_003CModule_003E.NmError_002ECmError_002EGetErrorDescription((CmError*)ptr, &obj2);
					try
					{
						throw new MediaStorageUtilityException(global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003AString_0020_005E_002Cclass_0020std_003A_003Abasic_string_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E_0020_003E(from_obj), (MediaStorageUtilityException.ErrorCode)global::_003CModule_003E.NmDatabase_002EUtility_002ECmMediaStorageUtilityError_002EGetErrorCode(ptr));
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj2);
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
		GC.KeepAlive(this);
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe virtual bool Close()
	{
		//IL_0008: Expected I8, but got I
		//IL_0096: Expected I, but got I8
		//IL_0046: Expected I, but got I8
		//IL_001c: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		bool result = true;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMediaStorageUtilityError* ptr);
		try
		{
			ImDatabaseMetaData* database = m_Database;
			result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte>)(*(ulong*)(*(long*)database + 24)))((nint)database) != 0;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVCmMediaStorageUtilityError_0040Utility_0040NmDatabase_0040_0040_00408), 9, &ptr) != 0)
		{
			uint num2 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj2);
					std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* from_obj = global::_003CModule_003E.NmError_002ECmError_002EGetErrorDescription((CmError*)ptr, &obj2);
					try
					{
						throw new MediaStorageUtilityException(global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003AString_0020_005E_002Cclass_0020std_003A_003Abasic_string_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E_0020_003E(from_obj), (MediaStorageUtilityException.ErrorCode)global::_003CModule_003E.NmDatabase_002EUtility_002ECmMediaStorageUtilityError_002EGetErrorCode(ptr));
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj2);
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
		GC.KeepAlive(this);
		return result;
	}

	public unsafe virtual TimelineResponse[] GetTimeLineInformation(MediaType mediaType, TimeLineInformationType activityType, DateTime beginTime, TimeSpan resolution, int count)
	{
		//IL_0009: Expected I8, but got I
		//IL_0145: Expected I, but got I8
		//IL_00f4: Expected I, but got I8
		//IL_004f: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMediaStorageUtilityError* ptr5);
		try
		{
			ImDatabaseMetaData* database = m_Database;
			ImDatabaseMetaData* ptr = database;
			long num2 = *(long*)(*(long*)database + 32);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmTimeSpan cmTimeSpan);
			CmTimeSpan* ptr2 = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeSpan_002Cclass_0020System_003A_003ATimeSpan_003E(&cmTimeSpan, &resolution);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out list_003CNmDatabaseUtility_003A_003ASmTimelineResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmTimelineResponse_003E_0020_003E obj);
			try
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
				NmTimeStamp.CmTimeStamp* ptr3 = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_002Cclass_0020System_003A_003ADateTime_003E(&cmTimeStamp, &beginTime);
				try
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, list_003CNmDatabaseUtility_003A_003ASmTimelineResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmTimelineResponse_003E_0020_003E*, NmDatabaseUtility.MediaType, NmDatabaseUtility.TimeLineInformationType, NmTimeStamp.CmTimeStamp, CmTimeSpan, int, list_003CNmDatabaseUtility_003A_003ASmTimelineResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmTimelineResponse_003E_0020_003E*>)num2)((nint)ptr, &obj, (NmDatabaseUtility.MediaType)mediaType, (NmDatabaseUtility.TimeLineInformationType)activityType, *ptr3, *ptr2, count);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
					throw;
				}
				try
				{
					global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<list_003CNmDatabaseUtility_003A_003ASmTimelineResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmTimelineResponse_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Elist_003CNmDatabaseUtility_003A_003ASmTimelineResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmTimelineResponse_003E_0020_003E_002E_007Bdtor_007D), &obj);
					throw;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTimeSpan*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D), &cmTimeSpan);
				throw;
			}
			TimelineResponse[] array;
			try
			{
				global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D(&cmTimeSpan);
				System.Runtime.CompilerServices.Unsafe.SkipInit(out list_003CNmDatabaseUtility_003A_003ASmTimelineResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmTimelineResponse_003E_0020_003E obj2);
				list_003CNmDatabaseUtility_003A_003ASmTimelineResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmTimelineResponse_003E_0020_003E* ptr4 = &obj2;
				list_003CNmDatabaseUtility_003A_003ASmTimelineResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmTimelineResponse_003E_0020_003E* listToConvert = global::_003CModule_003E.std_002Elist_003CNmDatabaseUtility_003A_003ASmTimelineResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmTimelineResponse_003E_0020_003E_002E_007Bctor_007D(&obj2, &obj);
				array = global::_003CModule_003E.ConvertToManagedArray_003Cstruct_0020NmDatabaseUtility_003A_003ASmTimelineResponse_002Cclass_0020VideoOS_003A_003ADatabaseUtility_003A_003ATimelineResponse_0020_005E_003E(listToConvert);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<list_003CNmDatabaseUtility_003A_003ASmTimelineResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmTimelineResponse_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Elist_003CNmDatabaseUtility_003A_003ASmTimelineResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmTimelineResponse_003E_0020_003E_002E_007Bdtor_007D), &obj);
				throw;
			}
			global::_003CModule_003E.std_002Elist_003CNmDatabaseUtility_003A_003ASmTimelineResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmTimelineResponse_003E_0020_003E_002E_Tidy(&obj);
			TimelineResponse[] array2 = array;
			GC.KeepAlive(this);
			return array;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVCmMediaStorageUtilityError_0040Utility_0040NmDatabase_0040_0040_00408), 9, &ptr5) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj4);
					std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* from_obj = global::_003CModule_003E.NmError_002ECmError_002EGetErrorDescription((CmError*)ptr5, &obj4);
					try
					{
						throw new MediaStorageUtilityException(global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003AString_0020_005E_002Cclass_0020std_003A_003Abasic_string_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E_0020_003E(from_obj), (MediaStorageUtilityException.ErrorCode)global::_003CModule_003E.NmDatabase_002EUtility_002ECmMediaStorageUtilityError_002EGetErrorCode(ptr5));
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

	public unsafe virtual SequencesResponse[] SequencesGetAroundWithSpan(SequenceType sequenceType, DateTime centerTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter)
	{
		//IL_0009: Expected I8, but got I
		//IL_0180: Expected I, but got I8
		//IL_012f: Expected I, but got I8
		//IL_0062: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMediaStorageUtilityError* ptr6);
		try
		{
			ImDatabaseMetaData* database = m_Database;
			ImDatabaseMetaData* ptr = database;
			long num2 = *(long*)(*(long*)database + 40);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmTimeSpan cmTimeSpan);
			CmTimeSpan* ptr2 = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeSpan_002Cclass_0020System_003A_003ATimeSpan_003E(&cmTimeSpan, &maxTimeAfter);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out list_003CNmDatabaseUtility_003A_003ASmSequencesResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmSequencesResponse_003E_0020_003E obj);
			try
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmTimeSpan cmTimeSpan2);
				CmTimeSpan* ptr3 = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeSpan_002Cclass_0020System_003A_003ATimeSpan_003E(&cmTimeSpan2, &maxTimeBefore);
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
					NmTimeStamp.CmTimeStamp* ptr4 = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_002Cclass_0020System_003A_003ADateTime_003E(&cmTimeStamp, &centerTime);
					try
					{
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, list_003CNmDatabaseUtility_003A_003ASmSequencesResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmSequencesResponse_003E_0020_003E*, NmDatabaseUtility.SequenceType, NmTimeStamp.CmTimeStamp, CmTimeSpan, int, CmTimeSpan, int, list_003CNmDatabaseUtility_003A_003ASmSequencesResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmSequencesResponse_003E_0020_003E*>)num2)((nint)ptr, &obj, (NmDatabaseUtility.SequenceType)sequenceType, *ptr4, *ptr3, maxCountBefore, *ptr2, maxCountAfter);
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
						throw;
					}
					try
					{
						global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<list_003CNmDatabaseUtility_003A_003ASmSequencesResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmSequencesResponse_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Elist_003CNmDatabaseUtility_003A_003ASmSequencesResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmSequencesResponse_003E_0020_003E_002E_007Bdtor_007D), &obj);
						throw;
					}
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTimeSpan*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D), &cmTimeSpan2);
					throw;
				}
				try
				{
					global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D(&cmTimeSpan2);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<list_003CNmDatabaseUtility_003A_003ASmSequencesResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmSequencesResponse_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Elist_003CNmDatabaseUtility_003A_003ASmSequencesResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmSequencesResponse_003E_0020_003E_002E_007Bdtor_007D), &obj);
					throw;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTimeSpan*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D), &cmTimeSpan);
				throw;
			}
			SequencesResponse[] array;
			try
			{
				global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D(&cmTimeSpan);
				System.Runtime.CompilerServices.Unsafe.SkipInit(out list_003CNmDatabaseUtility_003A_003ASmSequencesResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmSequencesResponse_003E_0020_003E obj2);
				list_003CNmDatabaseUtility_003A_003ASmSequencesResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmSequencesResponse_003E_0020_003E* ptr5 = &obj2;
				list_003CNmDatabaseUtility_003A_003ASmSequencesResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmSequencesResponse_003E_0020_003E* listToConvert = global::_003CModule_003E.std_002Elist_003CNmDatabaseUtility_003A_003ASmSequencesResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmSequencesResponse_003E_0020_003E_002E_007Bctor_007D(&obj2, &obj);
				array = global::_003CModule_003E.ConvertToManagedArray_003Cstruct_0020NmDatabaseUtility_003A_003ASmSequencesResponse_002Cclass_0020VideoOS_003A_003ADatabaseUtility_003A_003ASequencesResponse_0020_005E_003E(listToConvert);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<list_003CNmDatabaseUtility_003A_003ASmSequencesResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmSequencesResponse_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Elist_003CNmDatabaseUtility_003A_003ASmSequencesResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmSequencesResponse_003E_0020_003E_002E_007Bdtor_007D), &obj);
				throw;
			}
			global::_003CModule_003E.std_002Elist_003CNmDatabaseUtility_003A_003ASmSequencesResponse_002Cstd_003A_003Aallocator_003CNmDatabaseUtility_003A_003ASmSequencesResponse_003E_0020_003E_002E_Tidy(&obj);
			SequencesResponse[] array2 = array;
			GC.KeepAlive(this);
			return array;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVCmMediaStorageUtilityError_0040Utility_0040NmDatabase_0040_0040_00408), 9, &ptr6) != 0)
		{
			uint num3 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj4);
					std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* from_obj = global::_003CModule_003E.NmError_002ECmError_002EGetErrorDescription((CmError*)ptr6, &obj4);
					try
					{
						throw new MediaStorageUtilityException(global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003AString_0020_005E_002Cclass_0020std_003A_003Abasic_string_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E_0020_003E(from_obj), (MediaStorageUtilityException.ErrorCode)global::_003CModule_003E.NmDatabase_002EUtility_002ECmMediaStorageUtilityError_002EGetErrorCode(ptr6));
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

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_0021MediaStorageDatabase();
			return;
		}
		try
		{
			_0021MediaStorageDatabase();
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

	~MediaStorageDatabase()
	{
		Dispose(A_0: false);
	}
}
