using System;

namespace VideoOS.Platform.SDK.Interface;

public interface ICustomSettingInfo
{
	Guid Id { get; set; }

	DateTime LastModifiedTime { get; set; }

	string LastModifiedUser { get; set; }
}
