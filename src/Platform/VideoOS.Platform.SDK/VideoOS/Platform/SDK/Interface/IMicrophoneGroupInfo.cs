using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IMicrophoneGroupInfo : IGroupInfo
{
	IMicrophoneGroupInfo[] MicrophoneGroups { get; set; }

	Guid[] Microphones { get; set; }
}
