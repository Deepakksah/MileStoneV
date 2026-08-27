using System;

namespace VideoOS.Platform.SDK.Interface;

public interface ICameraGroupInfo : IGroupInfo
{
	ICameraGroupInfo[] CameraGroups { get; set; }

	Guid[] Cameras { get; set; }
}
