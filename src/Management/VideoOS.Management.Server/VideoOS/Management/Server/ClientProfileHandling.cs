using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using VideoOS.Common.SharedComponent.ClientProfile;
using VideoOS.Common.SharedComponent.Options;
using VideoOS.Management.Common;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public static class ClientProfileHandling
{
	private static string _defaultId = "F496E88C-4422-4A72-B1D5-7CCDE2ABC132";

	public static VideoOS.WebService.Common.ServerCommandService.ClientProfile GetClientProfileLegacy(IGeminiRequestContext requestContext, bool withSettings)
	{
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartClientLogin") || !Role.AllowClientLogOn(GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext(), requestContext.AuthenticatedUserSid, ClientType.SmartClient))
		{
			return SerializeClientProfile(GetLockedDownClientProfile(), withSettings, legacy: true);
		}
		if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartClientProfiles"))
		{
			ClientProfile clientProfile = ClientProfile.QueryActiveSmartClientProfile(requestContext);
			string settingsXml = ClientProfileLegacyEncryptionHelper.LegacyClientProfileSettingsEncrypt(clientProfile.SettingsXml);
			clientProfile.SettingsXml = settingsXml;
			return SerializeClientProfile(clientProfile, withSettings, legacy: true);
		}
		return GetClientProfileInternal(requestContext, withSettings);
	}

	public static VideoOS.WebService.Common.ServerCommandService.ClientProfile GetClientProfileLegacy20R3(IGeminiRequestContext requestContext, bool withSettings)
	{
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartClientLogin") || !Role.AllowClientLogOn(GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext(), requestContext.AuthenticatedUserSid, ClientType.SmartClient))
		{
			return SerializeClientProfile(GetLockedDownClientProfile(), withSettings, legacy: true);
		}
		if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartClientProfiles"))
		{
			return SerializeClientProfile(ClientProfile.QueryActiveSmartClientProfile(requestContext), withSettings, legacy: true);
		}
		return GetClientProfileInternal(requestContext, withSettings);
	}

	public static VideoOS.WebService.Common.ServerCommandService.ClientProfile GetClientProfile(IGeminiRequestContext requestContext, bool withSettings)
	{
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartClientLogin") || !Role.AllowClientLogOn(GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext(), requestContext.AuthenticatedUserSid, ClientType.SmartClient))
		{
			return SerializeClientProfile(GetLockedDownClientProfile(), withSettings);
		}
		if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartClientProfiles"))
		{
			return SerializeClientProfile(ClientProfile.QueryActiveSmartClientProfile(requestContext), withSettings);
		}
		return GetClientProfileInternal(requestContext, withSettings);
	}

	private static VideoOS.WebService.Common.ServerCommandService.ClientProfile GetClientProfileInternal(IGeminiRequestContext requestContext, bool withSettings)
	{
		string systemSetting = ServerInformation.GetSystemSetting(requestContext, "SmartClientSettings");
		if (!string.IsNullOrEmpty(systemSetting))
		{
			Identity identity = IdentityCache.ReadIdentityFromCache(requestContext, requestContext.AuthenticatedUserSid, QueryMembership.Expanded);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			ClientSettings fromXml = ClientSettings.GetFromXml(systemSetting);
			Guid[] memberOfRoles = identity.MemberOfRoles;
			foreach (Guid roleId in memberOfRoles)
			{
				ClientSetting clientSetting = fromXml.FirstOrDefault((ClientSetting r) => r.RoleId.Equals(roleId));
				if (clientSetting == null)
				{
					flag = true;
					flag2 = true;
					flag3 = true;
					break;
				}
				flag = clientSetting.Live || flag;
				flag2 = clientSetting.Playback || flag2;
				flag3 = clientSetting.Setup || flag3;
			}
			return SerializeClientProfile(GetDefaultClientProfileAndMerge(flag, flag2, flag3), withSettings);
		}
		return SerializeClientProfile(GetDefaultClientProfileAndMerge(live: true, playback: true, setup: true), withSettings);
	}

	private static ClientProfile GetDefaultClientProfileAndMerge(bool live, bool playback, bool setup)
	{
		ClientProfile clientProfile = new ClientProfile();
		clientProfile.Id = _defaultId;
		clientProfile.Name = "Default";
		clientProfile.LastModified = DateTime.UtcNow;
		clientProfile.IsDefaultProfile = true;
		clientProfile.Profiletype = ClientProfileType.SmartClientProfile;
		ClientProfileSpecification clientProfileSpecification = new ClientProfileSpecification();
		ClientProfileSettings clientProfileSettings = new ClientProfileSettings();
		foreach (ClientProfileSectionSpecification section in clientProfileSpecification.Sections)
		{
			if (section.App != "MC" && section.App != "All")
			{
				continue;
			}
			foreach (ClientProfileSettingSpecification setting in section.Settings)
			{
				VideoOS.Management.Common.ClientProfileSetting item = new VideoOS.Management.Common.ClientProfileSetting(setting.ID, setting.DefaultValue, setting.DefaultLocked);
				clientProfileSettings.Add(item);
			}
		}
		MergeClientSettings(clientProfileSettings, live, playback, setup);
		clientProfile.SettingsXml = ClientProfileSettings.GetAsXml(clientProfileSettings);
		return clientProfile;
	}

	private static void MergeClientSettings(ClientProfileSettings settings, bool live, bool playback, bool setup)
	{
		SetClientProfileSetting(settings[OptionId.PanesLiveTab.ToString()], live);
		SetClientProfileSetting(settings[OptionId.PanesPlaybackTab.ToString()], playback);
		SetClientProfileSetting(settings[OptionId.CentralizedSearchTab.ToString()], playback);
		SetClientProfileSetting(settings[OptionId.PanesSetupTab.ToString()], setup);
	}

	private static void SetClientProfileSetting(VideoOS.Management.Common.ClientProfileSetting setting, bool value)
	{
		if (value)
		{
			setting.Value = OptionAvailable.Available.ToString();
		}
		else
		{
			setting.Value = OptionAvailable.Unavailable.ToString();
		}
	}

	private static ClientProfile GetLockedDownClientProfile()
	{
		ClientProfile clientProfile = new ClientProfile();
		clientProfile.Id = _defaultId;
		clientProfile.Name = "Default";
		clientProfile.LastModified = DateTime.UtcNow;
		clientProfile.IsDefaultProfile = true;
		clientProfile.Profiletype = ClientProfileType.SmartClientProfile;
		ClientProfileSpecification clientProfileSpecification = new ClientProfileSpecification();
		ClientProfileSettings clientProfileSettings = new ClientProfileSettings();
		foreach (ClientProfileSectionSpecification section in clientProfileSpecification.Sections)
		{
			if (section.App != "MC" && section.App != "All")
			{
				continue;
			}
			foreach (ClientProfileSettingSpecification setting in section.Settings)
			{
				VideoOS.Management.Common.ClientProfileSetting clientProfileSetting = new VideoOS.Management.Common.ClientProfileSetting(setting.ID, setting.DefaultValue, setting.DefaultLocked);
				clientProfileSetting.Locked = true;
				if (clientProfileSetting.Key != OptionId.ApplicationExitButton.ToString())
				{
					if (string.Compare(clientProfileSetting.Value, OptionAvailable.Available.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
					{
						clientProfileSetting.Value = OptionAvailable.Unavailable.ToString();
					}
					if (string.Compare(clientProfileSetting.Value, OptionYesNo.Yes.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
					{
						clientProfileSetting.Value = OptionYesNo.No.ToString();
					}
					if (string.Compare(clientProfileSetting.Value, OptionShowHide.Show.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
					{
						clientProfileSetting.Value = OptionShowHide.Hide.ToString();
					}
					if (string.Compare(clientProfileSetting.Value, OptionEnabled.Enabled.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
					{
						clientProfileSetting.Value = OptionEnabled.Disabled.ToString();
					}
					if (string.Compare(clientProfileSetting.Value, OptionStartView.Last.ToString(), StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(clientProfileSetting.Value, OptionStartView.Ask.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
					{
						clientProfileSetting.Value = OptionStartView.None.ToString();
					}
				}
				clientProfileSettings.Add(clientProfileSetting);
			}
		}
		clientProfile.SettingsXml = ClientProfileSettings.GetAsXml(clientProfileSettings);
		return clientProfile;
	}

	private static VideoOS.WebService.Common.ServerCommandService.ClientProfile SerializeClientProfile(ClientProfile profile, bool withSettings, bool legacy = false)
	{
		VideoOS.WebService.Common.ServerCommandService.ClientProfile clientProfile = new VideoOS.WebService.Common.ServerCommandService.ClientProfile();
		clientProfile.Id = new Guid(profile.Id);
		clientProfile.Name = profile.Name;
		clientProfile.Description = profile.Description;
		clientProfile.LastModified = profile.LastModified;
		clientProfile.SettingsLastModified = profile.LastModified;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(profile.SettingsXml);
		XPathNodeIterator xPathNodeIterator = xmlDocument.CreateNavigator().Select("//Settings/Setting");
		List<VideoOS.WebService.Common.ServerCommandService.ClientProfileSetting> list = new List<VideoOS.WebService.Common.ServerCommandService.ClientProfileSetting>();
		if (withSettings)
		{
			foreach (XPathNavigator item in xPathNodeIterator)
			{
				string attribute = item.GetAttribute("key", string.Empty);
				if (!legacy || string.IsNullOrEmpty(attribute) || !attribute.Equals("GisMapGoogleMapSigningSecret", StringComparison.OrdinalIgnoreCase))
				{
					string value = item.SelectSingleNode("Value").Value;
					if (!bool.TryParse(item.SelectSingleNode("Locked").Value, out var result))
					{
						result = false;
					}
					VideoOS.WebService.Common.ServerCommandService.ClientProfileSetting clientProfileSetting = new VideoOS.WebService.Common.ServerCommandService.ClientProfileSetting();
					clientProfileSetting.Key = attribute;
					clientProfileSetting.Value = value;
					clientProfileSetting.Locked = result;
					list.Add(clientProfileSetting);
				}
			}
		}
		clientProfile.ClientProfileSettings = list.ToArray();
		return clientProfile;
	}
}
