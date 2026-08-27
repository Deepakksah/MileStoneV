using Newtonsoft.Json;
using VideoOS.Platform.Proxy.Alarm;
using VideoOS.Utilities.Validation;

namespace VideoOS.LPR.Common.Domain;

public static class LprAnalyticsObjectDataLoader
{
	public static LprAnalyticsObjectData LoadAnalyticsObjectData(EventLine line)
	{
		line.NullCheck("", "LoadAnalyticsObjectData", "C:\\actions-runner\\_work\\XPCO\\XPCO\\LPR\\Components\\Analytics\\VideoOS.LPR.Common\\Domain\\LprAnalyticsObjectDataLoader.cs", 11);
		if (line.ObjectData == null)
		{
			return null;
		}
		try
		{
			return JsonConvert.DeserializeObject<LprAnalyticsObjectData>(line.ObjectData);
		}
		catch (JsonException)
		{
			return null;
		}
	}
}
