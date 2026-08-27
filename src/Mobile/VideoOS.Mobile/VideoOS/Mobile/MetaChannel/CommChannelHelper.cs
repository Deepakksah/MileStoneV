using System.Collections.Generic;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Platform;

namespace VideoOS.Mobile.MetaChannel;

public class CommChannelHelper
{
	public static bool CheckUserRights(Item item, string rights)
	{
		bool flag = item != null;
		if (flag)
		{
			Dictionary<string, string> authorization = item.Authorization;
			if (authorization != null && authorization.ContainsKey(rights))
			{
				flag = authorization[rights] == "Yes";
			}
		}
		return flag;
	}

	public static bool CheckUserRightsSpeak(Item item)
	{
		return CheckUserRights(item, "Speak");
	}

	public static bool CheckUserRightsLive(Item item)
	{
		return CheckUserRights(item, "Live");
	}

	public static bool CheckUserRightsPlayback(Item item)
	{
		return CheckUserRights(item, "Playback");
	}

	public static bool CheckUserRightsPtz(Item item)
	{
		return CheckUserRights(item, "PTZ");
	}

	public static bool CheckUserRightsPreset(Item item)
	{
		return CheckUserRights(item, "Preset");
	}

	public static bool CheckUserRightsStream(Item item, StreamParamsHelper.SignalType signalType)
	{
		bool result = false;
		switch (signalType)
		{
		case StreamParamsHelper.SignalType.Live:
			result = CheckUserRightsLive(item);
			break;
		case StreamParamsHelper.SignalType.Playback:
			result = CheckUserRightsPlayback(item);
			break;
		}
		return result;
	}

	public static void CheckAuthorizationParams(Dictionary<string, string> outParams, Item item)
	{
		string[] authorStrings = new string[12]
		{
			"Live", "Playback", "PTZ", "Preset", "ExportAvi", "ExportDatabase", "ExportJpeg", "Speak", "BookmarkAdd", "BookmarkEdit",
			"BookmarkView", "BookmarkDelete"
		};
		CheckParams(outParams, item.Authorization, authorStrings);
		CopyParams(outParams, "Playback", "Sequences");
		CheckParams(outParams, item.Authorization, "Sequences", "GetSequences");
	}

	public static void CheckExportParams(Dictionary<string, string> outParams, bool exportEnabled)
	{
		string value = (exportEnabled ? "Yes" : "No");
		Dictionary<string, string> itemParams = new Dictionary<string, string> { { "Exports", value } };
		string[] array = new string[3] { "ExportAvi", "ExportDatabase", "ExportJpeg" };
		foreach (string authorOut in array)
		{
			CheckParams(outParams, itemParams, authorOut, "Exports");
		}
	}

	public static void CheckCameraParams(Dictionary<string, string> outParams, Item item)
	{
		string[] authorStrings = new string[2] { "PTZ", "Preset" };
		CheckParams(outParams, item.Properties, authorStrings);
		CheckParams(outParams, item.Properties, "Preset", "PTZ");
	}

	public static void CheckParams(Dictionary<string, string> outParams, Dictionary<string, string> itemParams, string[] authorStrings)
	{
		if (itemParams != null && outParams != null && authorStrings != null)
		{
			foreach (string text in authorStrings)
			{
				CheckParams(outParams, itemParams, text, text);
			}
		}
	}

	public static void CheckParams(Dictionary<string, string> outParams, Dictionary<string, string> itemParams, string authorOut, string autorItem)
	{
		if (itemParams.ContainsKey(autorItem))
		{
			string text = itemParams[autorItem];
			string text2 = "Yes";
			if (outParams.ContainsKey(authorOut))
			{
				text2 = outParams[authorOut];
			}
			if (text2 != text)
			{
				text = "No";
			}
			outParams[authorOut] = text;
		}
	}

	public static void CopyParams(Dictionary<string, string> outParams, string paramNameFrom, string paramNameTo)
	{
		if (outParams != null && !string.IsNullOrWhiteSpace(paramNameFrom) && !string.IsNullOrWhiteSpace(paramNameTo) && outParams.ContainsKey(paramNameFrom))
		{
			outParams[paramNameTo] = outParams[paramNameFrom];
		}
	}

	public static void AddPositioningInfo(Dictionary<string, string> outParams, PositioningInformation positioningInfo)
	{
		if (positioningInfo != null && positioningInfo.IsPointDefined)
		{
			outParams.Add("Latitude", positioningInfo.Latitude.ToString());
			outParams.Add("Longitude", positioningInfo.Longitude.ToString());
			outParams.Add("CoverageDirection", positioningInfo.CoverageDirection.ToString());
			outParams.Add("CoverageDepth", positioningInfo.CoverageDepth.ToString());
			outParams.Add("CoverageFieldOfView", positioningInfo.CoverageFieldOfView.ToString());
		}
	}
}
