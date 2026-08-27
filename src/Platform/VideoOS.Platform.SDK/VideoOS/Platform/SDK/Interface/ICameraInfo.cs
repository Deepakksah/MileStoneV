namespace VideoOS.Platform.SDK.Interface;

public interface ICameraInfo : IMediaDeviceInfo, IDeviceInfo
{
	double MaxFPS { get; set; }

	bool BrowsableStream { get; set; }

	IStreamInfo[] Streams { get; set; }

	ICameraSecurityInfo CameraSecurity { get; set; }

	IPtzInfo PtzSettings { get; set; }

	IIpixInfo IpixSettings { get; set; }

	IPanoramicLensInfo PanoramicLensSettings { get; set; }
}
