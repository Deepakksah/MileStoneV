namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

public interface IBinaryStructHelper
{
	T ReadStruct<T>(byte[] buffer);

	byte[] WriteStruct<T>(T tObject, int sizeOfT);
}
