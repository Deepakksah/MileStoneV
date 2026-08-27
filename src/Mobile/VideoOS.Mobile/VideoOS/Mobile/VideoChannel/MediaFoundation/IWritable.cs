namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

public interface IWritable
{
	void Write(byte[] buffer, int offset, int count);
}
