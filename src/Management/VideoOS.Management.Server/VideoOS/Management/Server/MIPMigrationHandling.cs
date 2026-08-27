#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Platform;
using VideoOS.Platform.Util;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public static class MIPMigrationHandling
{
	private static Guid _configServiceId = new Guid("7BA35BDA-87DA-4D9B-9403-A1EDAE42DAA8");

	private static Guid _configServiceKind = new Guid("E05DDF5B-EB39-41F3-A0EF-420277AB6C39");

	private const string EnabledPropertySpecialFormat = "EnabledPropertySpecialFormat";

	private const string BooleanProperty = "Boolean";

	public static CustomSettingInternal GetMIPItems(IGeminiRequestContext requestContext, Guid pluginId)
	{
		if (pluginId == _configServiceId)
		{
			return BuildIndex(requestContext);
		}
		List<MIPKind> kinds = MIPKind.GetKinds(requestContext, pluginId);
		if (kinds == null || !kinds.Any() || kinds.FirstOrDefault((MIPKind m) => m.KindType == "ITEM") == null)
		{
			return null;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(CreateInitialXml(pluginId));
		XmlNode xmlNode = xmlDocument.SelectSingleNode("/PlatformConfiguration");
		int num = 0;
		DateTime dateTime = DateTime.MinValue;
		string lastModifiedUser = "";
		foreach (MIPKind item2 in kinds)
		{
			if (item2.KindType != "ITEM")
			{
				continue;
			}
			if (item2.DataVersion > num)
			{
				num = item2.DataVersion;
			}
			if (item2.LastModified > dateTime)
			{
				dateTime = item2.LastModified;
			}
			foreach (Item item3 in MIPItem.GetItems(requestContext, item2.Id))
			{
				Item item = item3;
				if (item.Properties.ContainsKey("EnabledPropertySpecialFormat") && item.Properties["EnabledPropertySpecialFormat"] == "Boolean")
				{
					item.Properties["Enabled"] = item.Enabled.ToString(CultureInfo.InvariantCulture);
					item.Properties.Remove("EnabledPropertySpecialFormat");
				}
				if (item2.Id == MIPItemConfigurationApi.LicenseKindId)
				{
					if (item.FQID.ObjectId == MIPItemConfigurationApi.LicensesObjectId)
					{
						item = MIPItemConfigurationApi.EncryptNonFipsProperty(item, MIPItemConfigurationProperties.Licenses);
						item.FQID = new FQID(item.FQID.ServerId, item.FQID.ParentId, MIPItemConfigurationApi.LegacyLicensesObjectId, item.FQID.FolderType, item.FQID.Kind);
					}
					else if (item.FQID.ObjectId == MIPItemConfigurationApi.RegistrationsObjectId)
					{
						item = MIPItemConfigurationApi.EncryptNonFipsProperty(item, MIPItemConfigurationProperties.Registrations);
						item.FQID = new FQID(item.FQID.ServerId, item.FQID.ParentId, MIPItemConfigurationApi.LegacyRegistrationsObjectId, item.FQID.FolderType, item.FQID.Kind);
					}
				}
				ConfigurationUtil.SaveItemConfiguration(xmlNode, null, item);
			}
			lastModifiedUser = item2.ModifiedUser;
		}
		return new CustomSettingInternal
		{
			CustomSettingData = new CustomSettingInternalData
			{
				CustomSettingDataXml = xmlNode.OuterXml,
				DataVersion = num
			},
			CustomSettingInfo = new CustomSettingInfo
			{
				Id = pluginId,
				LastModifiedTime = dateTime,
				LastModifiedUser = lastModifiedUser
			}
		};
	}

	public static bool SetMIPItems(IGeminiRequestContext requestContext, Guid pluginId, CustomSettingInternalData customSettingInternalData, out SetCustomSettingResultType result)
	{
		if (pluginId == _configServiceId)
		{
			result = SetCustomSettingResultType.Success;
			return true;
		}
		List<MIPKind> kinds = MIPKind.GetKinds(requestContext, pluginId);
		if (kinds != null && kinds.Any() && kinds.FirstOrDefault((MIPKind m) => m.KindType == "ITEM") != null)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(customSettingInternalData.CustomSettingDataXml);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("PlatformConfiguration");
			if (xmlNode == null)
			{
				result = SetCustomSettingResultType.Success;
				return false;
			}
			foreach (XmlNode item2 in xmlNode.SelectNodes("Configurations"))
			{
				foreach (XmlNode childNode in item2.ChildNodes)
				{
					try
					{
						Item item = ConfigurationUtil.DeserializeItem(childNode);
						if (item.Properties.ContainsKey("Enabled") && bool.TryParse(item.Properties["Enabled"], out var _))
						{
							item.Properties["EnabledPropertySpecialFormat"] = "Boolean";
						}
						MIPItem.SaveItem(requestContext, item);
					}
					catch (Exception ex)
					{
						Trace.WriteLine("SetMIPItems ignoring one item:" + ex.Message);
					}
				}
			}
			result = SetCustomSettingResultType.Success;
			return true;
		}
		result = SetCustomSettingResultType.Success;
		return false;
	}

	private static CustomSettingInternal BuildIndex(IGeminiRequestContext requestContext)
	{
		List<MIPKind> kinds = MIPKind.GetKinds(requestContext);
		if (kinds == null || kinds.Count == 0)
		{
			return null;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(CreateInitialXml(_configServiceId));
		XmlNode xmlNode = xmlDocument.SelectSingleNode("/PlatformConfiguration");
		foreach (MIPKind item2 in kinds)
		{
			Item item = new Item(new ServerId("N/A", "N/A", 0, _configServiceId), Guid.Empty, item2.Id, "Update", FolderType.No, _configServiceKind);
			item.Properties["DataVersion"] = item2.DataVersion.ToString(CultureInfo.InvariantCulture);
			item.Properties["PluginId"] = item2.PluginId.ToString() ?? "";
			item.Properties["PluginName"] = item2.PluginName ?? "";
			ConfigurationUtil.SaveItemConfiguration(xmlNode, null, item);
		}
		return new CustomSettingInternal
		{
			CustomSettingData = new CustomSettingInternalData
			{
				CustomSettingDataXml = xmlNode.OuterXml,
				DataVersion = 1
			},
			CustomSettingInfo = new CustomSettingInfo
			{
				Id = _configServiceId,
				LastModifiedTime = DateTime.UtcNow
			}
		};
	}

	private static string CreateInitialXml(Guid platformDefinitionId)
	{
		return "<PlatformConfiguration ID=\"" + platformDefinitionId.ToString() + "\"><Configurations /></PlatformConfiguration>";
	}
}
