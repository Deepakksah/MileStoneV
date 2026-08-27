using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IAlertTypeGroupInfo : IGroupInfo
{
	IAlertTypeGroupInfo[] AlertTypeGroups { get; set; }

	Guid[] AlertTypes { get; set; }
}
