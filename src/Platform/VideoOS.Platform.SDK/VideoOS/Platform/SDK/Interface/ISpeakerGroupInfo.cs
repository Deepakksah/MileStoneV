using System;

namespace VideoOS.Platform.SDK.Interface;

public interface ISpeakerGroupInfo : IGroupInfo
{
	ISpeakerGroupInfo[] SpeakerGroups { get; set; }

	Guid[] Speakers { get; set; }
}
