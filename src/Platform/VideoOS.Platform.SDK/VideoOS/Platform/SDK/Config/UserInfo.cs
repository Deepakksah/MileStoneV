using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

namespace VideoOS.Platform.SDK.Config;

public class UserInfo
{
	public string Sid { get; set; }

	public string DisplayName { get; set; }

	public UserInfo(VideoOS.Platform.SDK.Proxy.RecorderServices.Net.UserInfo userInfo)
	{
		Sid = userInfo?.Sid ?? "";
		DisplayName = userInfo?.DisplayName ?? "";
	}
}
