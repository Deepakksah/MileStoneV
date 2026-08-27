using System;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public interface IPcmAudioData : ISourceData
{
	TimeSpan Duration { get; }

	DateTime EndDateTime { get; }

	IPcmSourceSettings PcmSourceSettings { get; }
}
