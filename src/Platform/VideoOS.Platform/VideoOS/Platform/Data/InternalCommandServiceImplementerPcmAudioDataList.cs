using System;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class InternalCommandServiceImplementerPcmAudioDataList : IInternalCommandServiceImplementer<PcmAudioDataList>
{
	private readonly IPcmSourceSettings _iPcmSourceSettings;

	public Func<object, FQID, DateTime, PcmAudioDataList> GetNearest => InternalCommandService.Instance.PcmAudioGetNearest;

	public Func<object, FQID, DateTime, PcmAudioDataList> GetAt => InternalCommandService.Instance.PcmAudioGetAt;

	public Func<object, FQID, DateTime, PcmAudioDataList> GetAtOrBefore => InternalCommandService.Instance.PcmAudioGetAtOrBefore;

	public Func<object, FQID, DateTime, PcmAudioDataList> GetAfter => null;

	public Func<object, FQID, DateTime, PcmAudioDataList> GetBefore => null;

	public Func<object, string, PcmAudioDataList> Command => InternalCommandService.Instance.PcmAudioCommand;

	public Action<PcmAudioDataList> OnData => delegate
	{
	};

	public Func<PcmAudioDataList, ISourceData> Convertor => ApplyPcmSourceSettings;

	public InternalCommandServiceImplementerPcmAudioDataList(IPcmSourceSettings iPcmSourceSettings)
	{
		_iPcmSourceSettings = iPcmSourceSettings;
	}

	private PcmAudioDataList ApplyPcmSourceSettings(PcmAudioDataList dataList)
	{
		if (dataList != null)
		{
			dataList.PcmSourceSettings = _iPcmSourceSettings;
			foreach (PcmAudioData data in dataList.DataList)
			{
				data.PcmSourceSettings = _iPcmSourceSettings;
			}
		}
		return dataList;
	}
}
