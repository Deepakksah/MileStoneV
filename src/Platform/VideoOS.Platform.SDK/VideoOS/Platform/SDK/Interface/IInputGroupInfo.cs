using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IInputGroupInfo : IGroupInfo
{
	IInputGroupInfo[] InputGroups { get; set; }

	Guid[] Inputs { get; set; }
}
