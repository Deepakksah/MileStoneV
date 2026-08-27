using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server.RestApi;

public static class JObjectUtil
{
	public static DateTime GetTimeValue(JObject content, string propertyName, DateTime? defaultTime)
	{
		if (content != null && content.TryGetValue(propertyName, out JToken value))
		{
			if (value.Type == JTokenType.Date)
			{
				return value.Value<DateTime>();
			}
			if (value.Type == JTokenType.String && DateTime.TryParse(value.Value<string>(), out var result))
			{
				return result;
			}
			throw new ArgumentException("Invalid time format");
		}
		if (defaultTime.HasValue)
		{
			return defaultTime.Value;
		}
		throw new PropertyInvalidException("property missing", propertyName);
	}

	public static string GetStringValue(JObject content, string propertyName, bool allowDefault)
	{
		if (content != null && content.TryGetValue(propertyName, out JToken value))
		{
			return value.Value<string>();
		}
		if (allowDefault)
		{
			return null;
		}
		throw new PropertyInvalidException("property missing", propertyName);
	}

	public static TimeSpan GetTimeSpanValue(JObject content, string propertyName)
	{
		TimeSpan result = new TimeSpan(1, 0, 0);
		if (content != null && content.TryGetValue(propertyName, out JToken value) && !TimeSpan.TryParse(value.ToString(), out result))
		{
			throw new PropertyInvalidException("Invalid timeSpan format", propertyName);
		}
		return result;
	}

	public static int GetNumberValue(JObject content, string propertyName)
	{
		if (content != null && content.TryGetValue(propertyName, out JToken value))
		{
			if (value.Type == JTokenType.Integer)
			{
				return value.Value<int>();
			}
			throw new PropertyInvalidException("Invalid number format", propertyName);
		}
		return 0;
	}

	public static bool GetBoolValue(JObject content, string propertyName, bool defaultValue = false)
	{
		if (content != null && content.TryGetValue(propertyName, out JToken value))
		{
			if (value.Type == JTokenType.Boolean)
			{
				return value.Value<bool>();
			}
			throw new PropertyInvalidException("Invalid boolean format", propertyName);
		}
		return defaultValue;
	}

	public static string[] GetStringListValue(JObject content, string propertyName, bool allowDefault)
	{
		if (content == null || !content.TryGetValue(propertyName, out JToken value))
		{
			if (allowDefault)
			{
				return null;
			}
			throw new PropertyInvalidException("Property missing", propertyName);
		}
		if (value.Type == JTokenType.Array)
		{
			List<string> list = new List<string>();
			foreach (JToken item in value.Children())
			{
				if (!string.IsNullOrEmpty(item.Value<string>()))
				{
					list.Add(item.Value<string>());
				}
			}
			return list.ToArray();
		}
		return null;
	}

	public static bool GetRetentionOptions(JObject content, out int units, out string retentionType)
	{
		if (content.TryGetValue("retentionOption", out JToken value))
		{
			JToken jToken = value.SelectToken("retentionType");
			JToken jToken2 = value.SelectToken("retentionUnits");
			if (jToken != null && jToken2 != null)
			{
				units = jToken2.Value<int>();
				retentionType = jToken.Value<string>();
				return true;
			}
		}
		units = 0;
		retentionType = null;
		return false;
	}

	public static void FetchDeviceIdAndTypes(JObject content, out Guid[] deviceIds, out MediaDeviceType[] mediaTypes, out Dictionary<Guid, string> deviceTypeIndex)
	{
		List<Guid> list = new List<Guid>();
		HashSet<MediaDeviceType> hashSet = new HashSet<MediaDeviceType>();
		deviceTypeIndex = new Dictionary<Guid, string>();
		if (content.TryGetValue("devicePaths", out JToken value))
		{
			if (value.Type != JTokenType.Array)
			{
				throw new ArgumentException("devicePaths should be an array");
			}
			foreach (JToken item3 in value.Children())
			{
				JToken jToken = item3.SelectToken("id");
				JToken jToken2 = item3.SelectToken("type");
				if (jToken == null || jToken2 == null)
				{
					throw new ArgumentException("Incorrect formatted path in devicePaths");
				}
				if (!Guid.TryParse(jToken.Value<string>(), out var result))
				{
					throw new ArgumentException("id is not a GUID");
				}
				MediaDeviceType item = ConvertResourceToMediaType(jToken2.Value<string>());
				list.Add(result);
				hashSet.Add(item);
				deviceTypeIndex[result] = item.ToString();
			}
		}
		if (content.TryGetValue("resourceTypes", out JToken value2))
		{
			if (value2.Type != JTokenType.Array)
			{
				throw new ArgumentException("resourceTypes should be an array");
			}
			foreach (JToken item4 in value2.Children())
			{
				MediaDeviceType item2 = ConvertResourceToMediaType(item4.Value<string>());
				hashSet.Add(item2);
			}
		}
		deviceIds = list.ToArray();
		mediaTypes = hashSet.ToArray();
	}

	public static Guid VerifyDeviceId(JObject content)
	{
		if (content != null && content.TryGetValue("devicePath", out JToken value))
		{
			JToken? jToken = value.SelectToken("id");
			JToken jToken2 = value.SelectToken("type");
			if (jToken == null || jToken2 == null)
			{
				throw new PropertyInvalidException("Incorrect formatted", "devicePath");
			}
			if (!Guid.TryParse(jToken.Value<string>(), out var result))
			{
				throw new PropertyInvalidException("id is not a GUID", "devicePath");
			}
			return result;
		}
		throw new PropertyInvalidException("Missing or incorrect formatted", "devicePath");
	}

	public static string[] VerifyDeviceIds(PathDataModel[] paths)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < paths.Length; i++)
		{
			string id = paths[i].Id;
			list.Add(id);
		}
		return list.ToArray();
	}

	private static MediaDeviceType ConvertResourceToMediaType(string deviceType)
	{
		return deviceType switch
		{
			"cameras" => MediaDeviceType.Camera, 
			"microphones" => MediaDeviceType.Microphone, 
			"speakers" => MediaDeviceType.Speaker, 
			_ => throw new ArgumentException("Unknown resourceType: " + deviceType), 
		};
	}
}
