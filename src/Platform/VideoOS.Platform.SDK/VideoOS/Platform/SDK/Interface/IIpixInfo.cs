namespace VideoOS.Platform.SDK.Interface;

public interface IIpixInfo
{
	bool IpixEnabled { get; set; }

	IHemisphereInfo Hemisphere { get; set; }

	IPositionInfo Homeposition { get; set; }

	bool CeilingMounted { get; set; }
}
