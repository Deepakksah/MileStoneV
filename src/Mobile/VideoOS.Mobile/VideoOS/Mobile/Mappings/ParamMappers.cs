using System;
using System.Collections.Generic;
using System.Reflection;
using VideoOS.Mobile.Helpers;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Platform;

namespace VideoOS.Mobile.Mappings;

public class ParamMappers
{
	public static string MaskValue(Dictionary<string, string> arg1, int arg2, string arg3)
	{
		return "********";
	}

	public static string CameraMapper(Dictionary<string, string> properties, int index, string cameraId)
	{
		string g = cameraId;
		cameraId = null;
		try
		{
			cameraId = VideoOS.Platform.Configuration.Instance.GetItem(new Guid(g), Kind.Camera).Name.SanitizedCustom("<>'");
		}
		catch (Exception ex)
		{
			Log.Debug(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
		return cameraId;
	}
}
