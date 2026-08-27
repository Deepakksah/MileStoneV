using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmDatabase.Utility;
using NmError;
using msclr.interop;
using std;

namespace VideoOS.DatabaseUtility.MediaStorage;

public class MediaStorageSignatureVerification : IDisposable
{
	private unsafe CmMPMStoSignatureVerification* m_NativeVerify;

	public unsafe int TotalProgress
	{
		get
		{
			int result = global::_003CModule_003E.NmDatabase_002EUtility_002ECmMPMStoSignatureVerification_002EGetTotalProgress(m_NativeVerify);
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe int TableProgress
	{
		get
		{
			int result = global::_003CModule_003E.NmDatabase_002EUtility_002ECmMPMStoSignatureVerification_002EGetTableProgress(m_NativeVerify);
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe string TablePath
	{
		get
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj);
			std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* from_obj = global::_003CModule_003E.NmDatabase_002EUtility_002ECmMPMStoSignatureVerification_002EGetTablePath(m_NativeVerify, &obj);
			string result;
			try
			{
				result = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003AString_0020_005E_002Cclass_0020std_003A_003Abasic_string_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E_0020_003E(from_obj);
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
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe MediaStorageSignatureVerification(IList<TableInfo> tablesToVerify, string password)
	{
		//IL_0083: Expected I, but got I8
		uint num = 0u;
		base._002Ector();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E obj);
		global::_003CModule_003E.ConvertFromManagedList_003Cclass_0020NmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cclass_0020VideoOS_003A_003ADatabaseUtility_003A_003AMediaStorage_003A_003ATableInfo_0020_005E_003E(&obj, tablesToVerify);
		try
		{
			CmMPMStoSignatureVerification* ptr = (CmMPMStoSignatureVerification*)global::_003CModule_003E.@new(216uL);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out msclr.interop.utf8string_t utf8string_t2);
			CmMPMStoSignatureVerification* nativeVerify;
			try
			{
				if (ptr != null)
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj2);
					std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* pThis = &obj2;
					msclr.interop.utf8string_t* right = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020msclr_003A_003Ainterop_003A_003Autf8string_t_002Cclass_0020System_003A_003AString_0020_005E_003E(&utf8string_t2, &password);
					try
					{
						num = 1u;
						std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* ptr2 = global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bctor_007D(&obj2, (std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)right);
						list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* ptr4;
						try
						{
							System.Runtime.CompilerServices.Unsafe.SkipInit(out list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E obj3);
							list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* ptr3 = &obj3;
							ptr4 = global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_007Bctor_007D(&obj3, &obj);
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), pThis);
							throw;
						}
						nativeVerify = global::_003CModule_003E.NmDatabase_002EUtility_002ECmMPMStoSignatureVerification_002E_007Bctor_007D(ptr, ptr4, ptr2);
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
					nativeVerify = null;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 216uL);
				throw;
			}
			try
			{
				m_NativeVerify = nativeVerify;
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
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_007Bdtor_007D), &obj);
			throw;
		}
		global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_Tidy(&obj);
	}

	public unsafe MediaStorageSignatureVerification(IList<TableInfo> tablesToVerify)
	{
		//IL_005b: Expected I, but got I8
		System.Runtime.CompilerServices.Unsafe.SkipInit(out list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E obj);
		global::_003CModule_003E.ConvertFromManagedList_003Cclass_0020NmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cclass_0020VideoOS_003A_003ADatabaseUtility_003A_003AMediaStorage_003A_003ATableInfo_0020_005E_003E(&obj, tablesToVerify);
		try
		{
			CmMPMStoSignatureVerification* ptr = (CmMPMStoSignatureVerification*)global::_003CModule_003E.@new(216uL);
			CmMPMStoSignatureVerification* nativeVerify;
			try
			{
				if (ptr != null)
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj2);
					std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* pThis = &obj2;
					std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* ptr2 = global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bctor_007D(&obj2, (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_00CNPNBAHC_0040_0040));
					list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* ptr4;
					try
					{
						System.Runtime.CompilerServices.Unsafe.SkipInit(out list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E obj3);
						list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* ptr3 = &obj3;
						ptr4 = global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_007Bctor_007D(&obj3, &obj);
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), pThis);
						throw;
					}
					nativeVerify = global::_003CModule_003E.NmDatabase_002EUtility_002ECmMPMStoSignatureVerification_002E_007Bctor_007D(ptr, ptr4, ptr2);
				}
				else
				{
					nativeVerify = null;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 216uL);
				throw;
			}
			m_NativeVerify = nativeVerify;
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_007Bdtor_007D), &obj);
			throw;
		}
		global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_Tidy(&obj);
	}

	private void _007EMediaStorageSignatureVerification()
	{
		_0021MediaStorageSignatureVerification();
	}

	private unsafe void _0021MediaStorageSignatureVerification()
	{
		CmMPMStoSignatureVerification* nativeVerify = m_NativeVerify;
		if (nativeVerify != null)
		{
			global::_003CModule_003E.NmDatabase_002EUtility_002ECmMPMStoSignatureVerification_002E_007Bdtor_007D(nativeVerify);
			global::_003CModule_003E.delete(nativeVerify, 216uL);
		}
		GC.KeepAlive(this);
	}

	public unsafe void Cancel()
	{
		global::_003CModule_003E.NmDatabase_002EUtility_002ECmMPMStoSignatureVerification_002ECancel(m_NativeVerify);
		GC.KeepAlive(this);
	}

	public unsafe VerificationResult Verify()
	{
		//Discarded unreachable code: IL_0149
		//IL_0008: Expected I8, but got I
		//IL_00f9: Expected I, but got I8
		//IL_00a9: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out SmSignatureVerificationResult smSignatureVerificationResult);
		global::_003CModule_003E.NmDatabase_002EUtility_002ESmSignatureVerificationResult_002ESmResultItem_002E_007Bctor_007D((SmSignatureVerificationResult.SmResultItem*)(&smSignatureVerificationResult));
		try
		{
			global::_003CModule_003E.NmDatabase_002EUtility_002ESmSignatureVerificationResult_002ESmResultItem_002E_007Bctor_007D((SmSignatureVerificationResult.SmResultItem*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smSignatureVerificationResult, 24)));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<SmSignatureVerificationResult.SmResultItem*, void>)(&global::_003CModule_003E.NmDatabase_002EUtility_002ESmSignatureVerificationResult_002ESmResultItem_002E_007Bdtor_007D), &smSignatureVerificationResult);
			throw;
		}
		VerificationResult result;
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMediaStorageUtilityError* ptr2);
			try
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out SmSignatureVerificationResult smSignatureVerificationResult2);
				SmSignatureVerificationResult* a_ = global::_003CModule_003E.NmDatabase_002EUtility_002ECmMPMStoSignatureVerification_002EVerify(m_NativeVerify, &smSignatureVerificationResult2);
				try
				{
					global::_003CModule_003E.NmDatabase_002EUtility_002ESmSignatureVerificationResult_002E_003D(&smSignatureVerificationResult, a_);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<SmSignatureVerificationResult*, void>)(&global::_003CModule_003E.NmDatabase_002EUtility_002ESmSignatureVerificationResult_002E_007Bdtor_007D), &smSignatureVerificationResult2);
					throw;
				}
				try
				{
					SmSignatureVerificationResult.SmResultItem* ptr = (SmSignatureVerificationResult.SmResultItem*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smSignatureVerificationResult2, 24));
					global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_003E_0020_003E_002E_Tidy((list_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smSignatureVerificationResult2, 32)));
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<SmSignatureVerificationResult.SmResultItem*, void>)(&global::_003CModule_003E.NmDatabase_002EUtility_002ESmSignatureVerificationResult_002ESmResultItem_002E_007Bdtor_007D), &smSignatureVerificationResult2);
					throw;
				}
				global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_003E_0020_003E_002E_Tidy((list_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smSignatureVerificationResult2, 8)));
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVCmMediaStorageUtilityError_0040Utility_0040NmDatabase_0040_0040_00408), 9, &ptr2) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj2);
						std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* from_obj = global::_003CModule_003E.NmError_002ECmError_002EGetErrorDescription((CmError*)ptr2, &obj2);
						try
						{
							throw new MediaStorageUtilityException(global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003AString_0020_005E_002Cclass_0020std_003A_003Abasic_string_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E_0020_003E(from_obj), (MediaStorageUtilityException.ErrorCode)global::_003CModule_003E.NmDatabase_002EUtility_002ECmMediaStorageUtilityError_002EGetErrorCode(ptr2));
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
			result = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cstruct_0020VideoOS_003A_003ADatabaseUtility_003A_003AMediaStorage_003A_003AVerificationResult_002Cstruct_0020NmDatabase_003A_003AUtility_003A_003ASmSignatureVerificationResult_003E(&smSignatureVerificationResult);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<SmSignatureVerificationResult*, void>)(&global::_003CModule_003E.NmDatabase_002EUtility_002ESmSignatureVerificationResult_002E_007Bdtor_007D), &smSignatureVerificationResult);
			throw;
		}
		try
		{
			SmSignatureVerificationResult.SmResultItem* ptr3 = (SmSignatureVerificationResult.SmResultItem*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smSignatureVerificationResult, 24));
			global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_003E_0020_003E_002E_Tidy((list_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smSignatureVerificationResult, 32)));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<SmSignatureVerificationResult.SmResultItem*, void>)(&global::_003CModule_003E.NmDatabase_002EUtility_002ESmSignatureVerificationResult_002ESmResultItem_002E_007Bdtor_007D), &smSignatureVerificationResult);
			throw;
		}
		global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_003E_0020_003E_002E_Tidy((list_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref smSignatureVerificationResult, 8)));
		GC.KeepAlive(this);
		return result;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_0021MediaStorageSignatureVerification();
			return;
		}
		try
		{
			_0021MediaStorageSignatureVerification();
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

	~MediaStorageSignatureVerification()
	{
		Dispose(A_0: false);
	}
}
