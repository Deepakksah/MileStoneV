using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IOutputGroupInfo : IGroupInfo
{
	IOutputGroupInfo[] OutputGroups { get; set; }

	Guid[] Outputs { get; set; }
}
