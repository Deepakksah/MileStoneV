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

public sealed class GenericAudioHelpers
{
	public unsafe static ReadOnlySharedBuffer CreateSilenceAudioPacket(DateTime timeStamp, ushort sequenceNumber)
	{
		//IL_0008: Expected I8, but got I
		//IL_011f: Expected I, but got I8
		//IL_00ea: Expected I, but got I8
		//IL_00fa: Expected I, but got I8
		//IL_0044: Expected I8, but got I
		//IL_0061: Expected I, but got I8
		//IL_0095: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		byte condition = (((uint)sequenceNumber <= 65535u) ? ((byte)1) : ((byte)0));
		Debug.Assert(condition != 0);
		try
		{
			ushort num2 = Convert.ToUInt16(sequenceNumber);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
			NmTimeStamp.CmTimeStamp* ptr = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_002Cclass_0020System_003A_003ADateTime_003E(&cmTimeStamp, &timeStamp);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out unique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E obj);
			try
			{
				NmGenericByteData.CmGenericByteData* ptr2 = global::_003CModule_003E.NmMediaProcessing_002ECreateSilenceAudioPacket(ptr, num2);
				NmGenericByteData.CmGenericByteData** ptr3 = &ptr2;
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
				NmGenericByteData.CmGenericByteData* ptr4 = (NmGenericByteData.CmGenericByteData*)(*(ulong*)(&obj));
				if (*(long*)(&obj) == 0L)
				{
					result = null;
					goto IL_007b;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<unique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E_002E_007Bdtor_007D), &obj);
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
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<unique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E_002E_007Bdtor_007D), &obj);
				throw;
			}
			global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E_002E_007Bdtor_007D(&obj);
			return result2;
			IL_007b:
			global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericByteData_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericByteData_003E_0020_003E_002E_007Bdtor_007D(&obj);
			return result;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, null) != 0)
		{
			uint num3 = 0u;
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
}
