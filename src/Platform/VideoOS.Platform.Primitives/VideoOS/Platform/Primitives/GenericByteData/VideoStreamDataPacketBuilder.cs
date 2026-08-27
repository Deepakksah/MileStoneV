using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NmBuffers;
using NmGenericByteData;
using NmTimeStamp;
using std;

namespace VideoOS.Platform.Primitives.GenericByteData;

public sealed class VideoStreamDataPacketBuilder
{
	public unsafe static ReadOnlySharedBuffer BuildPacket(VideoStreamDataPacketCodecType codec, IReadOnlyBuffer frameData, DateTime frameTime, [MarshalAs(UnmanagedType.U1)] bool isSyncPoint, DateTime syncTime, int sequenceNumber)
	{
		//Discarded unreachable code: IL_00d3
		//IL_004d: Expected I, but got I8
		//IL_0064: Expected I, but got I8
		if (frameData == null)
		{
			throw new ArgumentNullException("frameData");
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmTimeStamp cmTimeStamp);
		global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_002Cclass_0020System_003A_003ADateTime_003E(&cmTimeStamp, &frameTime);
		ReadOnlySharedBuffer result;
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmTimeStamp cmTimeStamp2);
			global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_002Cclass_0020System_003A_003ADateTime_003E(&cmTimeStamp2, &syncTime);
			try
			{
				ulong length = frameData.Length;
				byte* data = frameData.Data;
				System.Runtime.CompilerServices.Unsafe.SkipInit(out unique_ptr_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_003E_0020_003E obj);
				global::_003CModule_003E.NmGenericByteData_002ECmGenericVideoStreamDataPacketBuilder_002ECreateWithFrameDataCopy(&obj, (_GenericVideoStreamPacket_CodecTypes_e)codec, data, length, &cmTimeStamp, isSyncPoint, &cmTimeStamp2, (ushort)sequenceNumber);
				try
				{
					if (!global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002EGetIsByteDataOwnedByObject((CmGenericByteData*)(*(ulong*)(&obj))))
					{
						throw new Exception("Internal Logic Error: CreateWithFrameDataCopy returned frame packet object that does not own the data bytes");
					}
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CmUniqueBuffer cmUniqueBuffer);
					CmUniqueBuffer* buffer = global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002ETakeMemoryOwnership((CmGenericByteData*)(*(ulong*)(&obj)), &cmUniqueBuffer);
					try
					{
						result = new ReadOnlySharedBuffer(buffer);
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmUniqueBuffer*, void>)(&global::_003CModule_003E.NmBuffers_002ECmUniqueBuffer_002E_007Bdtor_007D), &cmUniqueBuffer);
						throw;
					}
					global::_003CModule_003E.std_002Eunique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E_002E_007Bdtor_007D((unique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 16)));
					GC.KeepAlive(frameData);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<unique_ptr_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_003E_0020_003E_002E_007Bdtor_007D), &obj);
					throw;
				}
				global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_003E_0020_003E_002E_007Bdtor_007D(&obj);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp2);
				throw;
			}
			global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp2);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
			throw;
		}
		global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
		return result;
	}
}
