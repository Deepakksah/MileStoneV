namespace VideoOS.Platform;

public interface IVideoSourceData : ISourceData
{
	string HardwareDecodingStatus { get; set; }
}
