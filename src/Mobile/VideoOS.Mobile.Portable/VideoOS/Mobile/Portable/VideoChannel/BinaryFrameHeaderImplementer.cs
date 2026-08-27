using System.Linq;
using System.Runtime.InteropServices;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.VideoChannel.Binary;

namespace VideoOS.Mobile.Portable.VideoChannel;

public class BinaryFrameHeaderImplementer : IBinaryStructHelper
{
	public static BinaryFrameHeaderImplementer Instance { get; } = new BinaryFrameHeaderImplementer();

	private BinaryFrameHeaderImplementer()
	{
	}

	public T ReadStruct<T>(byte[] buffer)
	{
		if (buffer.Count() < Marshal.SizeOf(typeof(T)))
		{
			Logger.Instance.Log(LogContent.Error, "BinaryFrameHeaderImplementer.ReadStruct<T>", "Smaller size of the input array");
			return default(T);
		}
		GCHandle gCHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
		T result = (T)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(T));
		gCHandle.Free();
		return result;
	}

	public byte[] WriteStruct<T>(T tObject, int sizeOfT)
	{
		byte[] array = new byte[Marshal.SizeOf(typeof(T))];
		if (array.Count() != Marshal.SizeOf(typeof(T)))
		{
			Logger.Instance.Log(LogContent.Error, "BinaryFrameHeaderImplementer.WriteStruct<T>", "Different size of the output array");
		}
		GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		Marshal.StructureToPtr(tObject, gCHandle.AddrOfPinnedObject(), fDeleteOld: true);
		gCHandle.Free();
		return array;
	}
}
