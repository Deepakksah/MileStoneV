using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public class GetDevicePresetsTaskResultDefinition
{
	private const string CameraPathKey = "CameraPath";

	private const string AffectedProfileIdsKey = "AffectedProfileIds";

	public const string SessionDataIdKey = "SessionDataId";

	public static ConfigurationItem[] ConvertTaskResponse(string xml)
	{
		GetDevicePresetsTaskResult getDevicePresetsTaskResult = GetDevicePresetsTaskResult.FromXml(xml);
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(getDevicePresetsTaskResult.PresetXml);
		foreach (XmlNode item in xmlDocument.FirstChild.SelectNodes("preset"))
		{
			string innerText = item["name"].InnerText;
			string innerText2 = item.Attributes["id"].InnerText;
			list.Add(new ConfigurationItem
			{
				DisplayName = innerText,
				ItemType = "PtzPreset",
				ItemCategory = ItemCategories.ChildItem,
				Path = "",
				Properties = new Property[2]
				{
					new Property("Name", innerText)
					{
						IsSettable = false
					},
					new Property("Id", innerText2)
					{
						IsSettable = false
					}
				}
			});
		}
		return list.ToArray();
	}

	public static Property[] CreateGetDevicePresetsResultProperties(Task task)
	{
		List<Property> list = new List<Property>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded && !string.IsNullOrEmpty(task.AsyncStatus.LastException))
		{
			GetDevicePresetsTaskResult getDevicePresetsTaskResult = GetDevicePresetsTaskResult.FromXml(task.AsyncStatus.LastException);
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("Camera", getDevicePresetsTaskResult.DeviceId);
			list.Add(PropertyCameraPath(configurationItemPath.Text));
			list.Add(SessionDataIdProperty(getDevicePresetsTaskResult.SessionDataId));
			list.Add(AffectedProfileIdsProperty(getDevicePresetsTaskResult.AffectedProfileIds));
		}
		return list.ToArray();
	}

	private static Property PropertyCameraPath(string path)
	{
		return new Property("CameraPath", path)
		{
			ValueType = "Path",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyCameraPath,
			ItemReference = true,
			ServerValidation = false,
			TranslationId = "PropertyCameraPath",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	public static Property SessionDataIdProperty(long sessionDataId)
	{
		return new Property("SessionDataId", sessionDataId.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			UIImportance = UIImportance.Summary,
			DisplayName = ConfigurationApiResources.PropertySessionDataId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertySessionDataId",
			ToolTipTranslationId = "PropertySessionDataIdToolTip"
		};
	}

	public static Property AffectedProfileIdsProperty(string idString)
	{
		string text = "";
		string[] array = idString.Split(',');
		foreach (string text2 in array)
		{
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + ConfigurationItemPath.CreateItemPath("PatrollingProfile", text2).Text + ";";
			}
		}
		text = text.TrimEnd(';');
		return new Property("AffectedProfileIds", text)
		{
			ValueType = "PathList",
			IsSettable = false,
			UIImportance = UIImportance.Default,
			DisplayName = ConfigurationApiResources.PropertyAffectedProfileIds,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyAffectedProfileIds",
			ToolTipTranslationId = "PropertyAffectedProfileIdsToolTip"
		};
	}
}
