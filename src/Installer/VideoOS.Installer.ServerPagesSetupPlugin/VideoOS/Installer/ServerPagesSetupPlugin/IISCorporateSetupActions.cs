using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Installer.Common;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Framework.IISSetup;
using VideoOS.Server.Admin;
using VideoOS.Server.Admin.Utilities;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

[SuppressMessage("Microsoft.Naming", "CA1709:Identifiers should be cased correctly", Scope = "member", Target = "VideoOS.Installer.ServerPagesSetupPlugin.#IISCorporateSetupActions", Justification = "IIS is the way it is used by Milestone")]
public class IISCorporateSetupActions : IISSetupPageActions
{
	private SiteInfo _defaultSite;

	public override string PageName => "IISSetupCorporatePage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		bool flag = base.BeforeActivatePage(parameters);
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "IisSetupActions, base.BeforeActivatePage returned with result = {0}", flag), TraceEventType.Information);
		Dictionary<string, string> settings = GlobalData.ToDict();
		if (!flag)
		{
			WebsiteHelper.CreateRegistryEntries(settings);
			return false;
		}
		return true;
	}

	public override bool AfterWebSiteSelected(ref SiteInfo websiteToUse)
	{
		if (websiteToUse == null)
		{
			return false;
		}
		bool result = base.AfterWebSiteSelected(ref websiteToUse);
		WebsiteHelper.UpdateManagementAppSettings(websiteToUse.HttpPort, websiteToUse.HttpsPort, "AfterWebSiteSelected");
		return result;
	}

	protected override SiteInfo SelectWebsiteToUse()
	{
		SiteInfo siteInfo = null;
		SiteInfo[] configuredSiteInfos = GetConfiguredSiteInfos(IisHelper.GetIisWebsites((ILogger)(object)new LogComponent(Path.Combine(LogManager.GetMsiLogPath(), "iis.log"))));
		string value = null;
		bool flag = false;
		flag = !GlobalData.TryGetValue("INSTALLATION_MODE", out value) || value == "CustomMode";
		try
		{
			_defaultSite = configuredSiteInfos.Single((SiteInfo site) => site.HttpPort == 80);
		}
		catch (InvalidOperationException)
		{
		}
		siteInfo = _defaultSite;
		if (_defaultSite == null)
		{
			LogManager.Write("IISSetupAction.BeforeActivatePage() Default web site was not found or was not configured correctly");
			siteInfo = ((configuredSiteInfos.Length != 0) ? configuredSiteInfos[0] : null);
		}
		if (flag)
		{
			if (base.Websites.Count() > 1)
			{
				LogManager.Write("IISSetupAction.BeforeActivatePage() More than one website was found");
				siteInfo = null;
			}
			else
			{
				LogManager.Write("IISSetupAction.BeforeActivatePage() First website on the list was selected");
				siteInfo = ((configuredSiteInfos.Length != 0) ? configuredSiteInfos[0] : null);
			}
		}
		return siteInfo;
	}

	private SiteInfo[] GetConfiguredSiteInfos(IisWebsite[] iisWebsite)
	{
		string text = string.Empty;
		List<SiteInfo> list = new List<SiteInfo>();
		foreach (IisWebsite website in iisWebsite)
		{
			SiteInfo siteInfo = base.Websites.SingleOrDefault((SiteInfo siteInfo2) => siteInfo2.Name.ToUpperInvariant() == website.Name.ToUpperInvariant() && website.Status == IisWebsiteStatus.Valid);
			if (siteInfo != null)
			{
				list.Add(siteInfo);
			}
		}
		foreach (SiteInfo item in list)
		{
			text = text + item.Id + ";";
		}
		GlobalData.SetValue("ConfiguredSitesIds", text, "IISSetupActions.SelectWebsiteToUse()");
		return list.ToArray();
	}

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		base.BeforeLeavePage(parameters);
		if (parameters.GetValueAsNullableBoolean("QUIET-MODE") == true)
		{
			string selectedWebsite = null;
			if (parameters.TryGetValue("WebSiteId", out var value))
			{
				selectedWebsite = value;
				WebsiteHelper.CreateRegistryEntries(parameters);
			}
			SiteInfo siteInfo = null;
			siteInfo = ((selectedWebsite != null) ? base.Websites.FirstOrDefault((SiteInfo x) => x.Id.Equals(selectedWebsite, StringComparison.OrdinalIgnoreCase)) : _defaultSite);
			if (siteInfo == null)
			{
				siteInfo = base.Websites.FirstOrDefault() ?? IISSetupPageActions.CreateFakeSiteInfo(parameters);
			}
			if (siteInfo.HttpsPort == 0)
			{
				siteInfo = new SiteInfo(siteInfo.Id, siteInfo.Name, siteInfo.Address, siteInfo.HttpPort, 443, (int)siteInfo.State.State);
			}
			if (siteInfo.HttpPort == 0)
			{
				siteInfo = new SiteInfo(siteInfo.Id, siteInfo.Name, siteInfo.Address, 80, siteInfo.HttpsPort, (int)siteInfo.State.State);
			}
			WebsiteHelper.UpdateManagementAppSettings(siteInfo.HttpPort, siteInfo.HttpsPort, "BeforeLeavePage");
			IISSetupPageActions.UpdateIisValues(siteInfo, "IISCorporateSetupActions.BeforeLeavePage()");
		}
		return true;
	}
}
