using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Platform.Data;
using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.LPR.Common.Communication;

public class LicensePlateEvent
{
	public EventLine EventLine { get; private set; }

	public LicensePlateInfo LicensePlateInfo { get; private set; }

	public LicensePlateList LicensePlateList { get; private set; }

	public AnalyticsEvent AnalyticsEvent { get; private set; }

	public LicensePlateEvent(EventLine eventLine, LicensePlateInfo licensePlateInfo, LicensePlateList licensePlateList, AnalyticsEvent analyticsEvent)
	{
		EventLine = eventLine;
		LicensePlateInfo = licensePlateInfo;
		LicensePlateList = licensePlateList;
		AnalyticsEvent = analyticsEvent;
	}
}
