using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Common.Integration;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.RecorderPlugins.Properties;

namespace VideoOS.Installer.RecorderPlugins;

public class RetentionWarningPageSubscriber : IWizardCommonPageSubscription<WarningPageContent>
{
	private IGlobalData _globalData;

	public Guid Id => new Guid("D0F30BBB-48AC-41DD-BA0D-CF8EADD7E7E2");

	public RetentionWarningPageSubscriber()
	{
		_globalData = new GlobalDataWrapper();
	}

	public RetentionWarningPageSubscriber(IGlobalData globalData)
	{
		if (globalData == null)
		{
			throw new ArgumentNullException("globalData");
		}
		_globalData = globalData;
	}

	public bool BeforeActivate(Dictionary<string, string> parameters)
	{
		bool num = _globalData.HasParameter("HARDWARE-MANUFACTURER-MODE") && _globalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE");
		bool flag = _globalData.HasParameter("IS_UPGRADE") && _globalData.GetValueAsBoolean("IS_UPGRADE");
		if (!num)
		{
			return !flag;
		}
		return false;
	}

	public WarningPageContent GetContent()
	{
		Uri result;
		return new WarningPageContent(details: (!Uri.TryCreate(OemResources.Instance.App.PrivacyGuideUrl, UriKind.RelativeOrAbsolute, out result)) ? new ContentLinkText(string.Format(CultureInfo.InvariantCulture, Resources.WarningMessage_GdprRetention, OemResources.Instance.App.ProductNameWithoutCompany, string.Empty)) : new ContentLinkText(string.Format(CultureInfo.InvariantCulture, Resources.WarningMessage_GdprRetention, OemResources.Instance.App.ProductNameWithoutCompany, "{0}"), result), heading: Resources.WarningPageHeadingDetails, subheading: null, subsection: null, rank: 20);
	}

	public Dictionary<string, string> CollectParameters(WarningPageContent content)
	{
		return null;
	}
}
