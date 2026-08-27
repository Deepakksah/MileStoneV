#define DEBUG
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using NmBuffers;
using NmGenericByteData;
using NmTimeStamp;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public sealed class GenericVideoHelpers
{
	public unsafe static ReadOnlySharedBuffer CreateRestrictedVideoDataPackage(byte[] jpegImage, DateTime timeStamp, ushort sequenceNumber)
	{
		//Discarded unreachable code: IL_0104
		//IL_0084: Expected I, but got I8
		//IL_009b: Expected I, but got I8
		byte condition = (((uint)sequenceNumber <= 65535u) ? ((byte)1) : ((byte)0));
		Debug.Assert(condition != 0);
		if (jpegImage == null)
		{
			throw new ArgumentNullException("jpegImage");
		}
		if ((nint)jpegImage.LongLength >= 3 && jpegImage[0] == byte.MaxValue && jpegImage[1] == 216 && jpegImage[2] == byte.MaxValue)
		{
			fixed (byte* ptr = &jpegImage[0])
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
				global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_002Cclass_0020System_003A_003ADateTime_003E(&cmTimeStamp, &timeStamp);
				ReadOnlySharedBuffer result;
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out std.unique_ptr_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_003E_0020_003E obj);
					global::_003CModule_003E.NmGenericByteData_002ECmGenericVideoStreamDataPacketBuilder_002ECreateWithFrameDataCopy(&obj, (NmGenericByteData._GenericVideoStreamPacket_CodecTypes_e)1, ptr, (ulong)jpegImage.LongLength, &cmTimeStamp, true, &cmTimeStamp, Convert.ToUInt16(sequenceNumber));
					try
					{
						if (!global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002EGetIsByteDataOwnedByObject((NmGenericByteData.CmGenericByteData*)(*(ulong*)(&obj))))
						{
							throw new Exception("Internal Logic Error: CreateWithFrameDataCopy returned frame packet object that does not own the data bytes");
						}
						System.Runtime.CompilerServices.Unsafe.SkipInit(out CmUniqueBuffer cmUniqueBuffer);
						CmUniqueBuffer* buffer = global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002ETakeMemoryOwnership((NmGenericByteData.CmGenericByteData*)(*(ulong*)(&obj)), &cmUniqueBuffer);
						try
						{
							result = new ReadOnlySharedBuffer((CmUniqueBuffer*)buffer);
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmUniqueBuffer*, void>)(&global::_003CModule_003E.NmBuffers_002ECmUniqueBuffer_002E_007Bdtor_007D), &cmUniqueBuffer);
							throw;
						}
						global::_003CModule_003E.std_002Eunique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E_002E_007Bdtor_007D((std.unique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 16)));
						GC.KeepAlive(jpegImage);
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.unique_ptr_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_003E_0020_003E_002E_007Bdtor_007D), &obj);
						throw;
					}
					global::_003CModule_003E.std_002Eunique_ptr_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_002Cstd_003A_003Adefault_delete_003CNmGenericByteData_003A_003ACmGenericVideoStreamDataPacket_003E_0020_003E_002E_007Bdtor_007D(&obj);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
					throw;
				}
				global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
				return result;
			}
		}
		throw new ArgumentException("jpegImage is not a valid JPEG image", "jpegImage");
	}
}
