using System;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class PcmAudioSource : GenericVideoSource<PcmAudioDataList>
{
	public PcmSourceSettings PcmSourceSettings { get; private set; }

	public PcmAudioSource(Item item)
		: this(item, new PcmSourceSettings(null))
	{
	}

	private PcmAudioSource(Item item, PcmSourceSettings pcmSourceSettings)
		: base(item, new Guid[2]
		{
			Kind.Microphone,
			Kind.Speaker
		}, (IInternalCommandServiceImplementer<PcmAudioDataList>)new InternalCommandServiceImplementerPcmAudioDataList(pcmSourceSettings))
	{
		PcmSourceSettings = pcmSourceSettings;
		new TrackPageViewData(typeof(PcmAudioSource)).Send();
	}

	public override void Init(int width, int height)
	{
		EnvironmentManager.Instance.Log(error: false, "PcmAudioSource:Init", "Width and Height ignored");
		Init();
	}

	public override void Init()
	{
		InitInternal(() => InternalCommandService.Instance.PcmAudioConnect(new InternalMediaService.PcmAudioConnectParams(PcmSourceSettings)
		{
			DeviceFqid = Item.FQID
		}), null);
	}
}
