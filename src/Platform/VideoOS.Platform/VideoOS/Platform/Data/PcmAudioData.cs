using System;
using VideoOS.Platform.Util;
using VideoOS.Platform.Util.Pcm;

namespace VideoOS.Platform.Data;

public class PcmAudioData : GenericData, IPcmAudioData, ISourceData
{
	public TimeSpan Duration => TimeSpan.FromMilliseconds(PcmHelper.BytesCountToMilliseconds(base.Data.Length, PcmSourceSettings));

	public DateTime EndDateTime => base.DateTime + Duration;

	public IPcmSourceSettings PcmSourceSettings { get; set; }
}
