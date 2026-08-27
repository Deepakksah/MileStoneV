#define DEBUG
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NmBuffers;
using NmGenericByteData;
using NmTimeStamp;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public sealed class GenericMetaDataHelpers
{
	private static ReadOnlySharedBuffer RestrictedMetadataPacketTemplate;

	unsafe static GenericMetaDataHelpers()
	{
		System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj);
		global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bctor_007D(&obj, (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_0KC_0040GPLPPJAH_0040_003F_0024DM_003F_0024DPxml_003F5version_003F_0024DN_003F_0024CC1_003F40_003F_0024CC_003F5encoding_003F_0024DN_003F_0024CCU_0040));
		try
		{
			sbyte* value = global::_003CModule_003E.std_002E_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002E_Myptr((std._String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E*)(&obj));
			IntPtr data = new IntPtr(value);
			ulong length = System.Runtime.CompilerServices.Unsafe.As<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 16));
			RestrictedMetadataPacketTemplate = new ReadOnlySharedBuffer(data, length);
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
			return;
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std._Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E*, void>)(&global::_003CModule_003E.std_002E_Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E_002E_007Bdtor_007D), &obj);
			throw;
		}
	}

	public unsafe static ReadOnlySharedBuffer CreateMetaDataStreamPacket(DateTime timeStamp, ushort sequenceNumber, GenericMetaDataStreamPacketTypes metaDataPacketType, IReadOnlyBuffer byteData)
	{
		//IL_0009: Expected I8, but got I
		//IL_017b: Expected I, but got I8
		//IL_0143: Expected I, but got I8
		//IL_0154: Expected I, but got I8
		//IL_0081: Expected I8, but got I
		//IL_00ee: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		byte condition = (((uint)sequenceNumber <= 65535u) ? ((byte)1) : ((byte)0));
		Debug.Assert(condition != 0);
		if (byteData == null)
		{
			throw new ArgumentNullException("byteData");
		}
		if (byteData.Data == null)
		{
			throw new ArgumentException("The underlying pointer is null", "byteData");
		}
		try
		{
			ulong length = byteData.Length;
			byte* data = byteData.Data;
			ushort num2 = Convert.ToUInt16(sequenceNumber);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
			NmTimeStamp.CmTimeStamp* ptr = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_002Cclass_0020System_003A_003ADateTime_003E(&cmTimeStamp, &timeStamp);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out unique_ptr_003CNmGenericByteData_003A_003ACmGenericMetaDataStreamPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericMetaDataStreamPacket_003E_0020_003E obj);
			try
			{
				CmGenericMetaDataStreamPacket* ptr2 = global::_003CModule_003E.NmGenericByteData_002EBuildGenericMetaDataStreamPacket(ptr, num2, (_GenericMetaDataStreamStreamPacket_Types_e)metaDataPacketType, data, length);
				CmGenericMetaDataStreamPacket** ptr3 = &ptr2;
				*(long*)(&obj) = (nint)ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
				throw;
			}
			ReadOnlySharedBuffer result;
			try
			{
				global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
				GC.KeepAlive(byteData);
				int num3;
				if (*(long*)(&obj) != 0L)
				{
					byte b = 1;
					byte b2 = b;
					num3 = 0;
				}
				else
				{
					byte b2 = 0;
					num3 = 1;
				}
				byte b3 = (byte)num3;
				byte b4 = b3;
				if (b3 != 0)
				{
					result = null;
					goto IL_00d4;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<unique_ptr_003CNmGenericByteData_003A_003ACmGenericMetaDataStreamPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericMetaDataStreamPacket_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericMetaDataStreamPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericMetaDataStreamPacket_003E_0020_003E_002E_007Bdtor_007D), &obj);
				throw;
			}
			ReadOnlySharedBuffer result2;
			try
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmUniqueBuffer cmUniqueBuffer);
				CmUniqueBuffer* buffer = global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002ETakeMemoryOwnership((NmGenericByteData.CmGenericByteData*)(*(ulong*)(&obj)), &cmUniqueBuffer);
				try
				{
					result2 = new ReadOnlySharedBuffer((CmUniqueBuffer*)buffer);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmUniqueBuffer*, void>)(&global::_003CModule_003E.NmBuffers_002ECmUniqueBuffer_002E_007Bdtor_007D), &cmUniqueBuffer);
					throw;
				}
				global::_003CModule_003E.std_002Eunique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E_002E_007Bdtor_007D((std.unique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 16)));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<unique_ptr_003CNmGenericByteData_003A_003ACmGenericMetaDataStreamPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericMetaDataStreamPacket_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericMetaDataStreamPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericMetaDataStreamPacket_003E_0020_003E_002E_007Bdtor_007D), &obj);
				throw;
			}
			global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericMetaDataStreamPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericMetaDataStreamPacket_003E_0020_003E_002E_007Bdtor_007D(&obj);
			return result2;
			IL_00d4:
			global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericMetaDataStreamPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericMetaDataStreamPacket_003E_0020_003E_002E_007Bdtor_007D(&obj);
			return result;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, null) != 0)
		{
			uint num4 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					return null;
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
		return null;
	}

	public static ReadOnlySharedBuffer CreateRestrictedMetaDataPacket(DateTime timeStamp, ushort sequenceNumber)
	{
		return CreateMetaDataStreamPacket(timeStamp, sequenceNumber, GenericMetaDataStreamPacketTypes.onvif, RestrictedMetadataPacketTemplate);
	}
}
