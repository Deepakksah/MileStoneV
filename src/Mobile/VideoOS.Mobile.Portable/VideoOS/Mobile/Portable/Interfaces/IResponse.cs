namespace VideoOS.Mobile.Portable.Interfaces;

public interface IResponse
{
	int Read(byte[] buffer, int nOffset, int nCount);
}
