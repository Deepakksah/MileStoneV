namespace VideoOS.Common.Integration.Drivers;

public interface IBrowseMedia
{
	IDriverMediaStorageReader GetMediaBrowser(RequestMediaDataType[] mediaDataType);
}
