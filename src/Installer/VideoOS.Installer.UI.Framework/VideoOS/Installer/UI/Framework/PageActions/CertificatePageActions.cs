using System;
using System.Collections.Generic;
using VideoOS.Installer.Common;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.UI.Framework.PageActions;

public class CertificatePageActions : PageSetupActionsBase
{
	public override string PageName => "CertificateCommonPage";

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		bool num = base.BeforeLeavePage(parameters);
		if (num && parameters.GetValueAsBoolean("QUIET-MODE"))
		{
			bool? flag = null;
			if (parameters.ContainsKey("SERVERTOSERVERENABLEENCRYPTION"))
			{
				flag = parameters.GetValueAsBoolean("SERVERTOSERVERENABLEENCRYPTION") && parameters.TryGetValue("SERVERTOSERVERCERTIFICATETHUMBPRINT", out var value) && !string.IsNullOrEmpty(value);
			}
			if ((!parameters.ContainsKey("ISCORPORATEINSTALLER") || !(parameters["ISCORPORATEINSTALLER"] == "true")) && flag.HasValue && parameters.ContainsKey("MANAGEMENTSERVERURI") && !string.IsNullOrEmpty(parameters["MANAGEMENTSERVERURI"]))
			{
				Uri uri = new Uri(parameters["MANAGEMENTSERVERURI"]);
				if (!flag.Value && uri.Scheme == Uri.UriSchemeHttps)
				{
					throw new InvalidOperationException("The argument MANAGEMENTSERVERURI is set to 'https', but the 'Server To Server' encryption parameters have not been set accordingly.");
				}
				if (flag.Value && uri.Scheme == Uri.UriSchemeHttp)
				{
					throw new InvalidOperationException("The argument MANAGEMENTSERVERURI is set to 'http', but the 'Server To Server' encryption parameters have been set to enable encryption.");
				}
			}
			string text = ((!flag.HasValue) ? null : (flag.Value ? parameters.GetValueOrNull("MANAGEMENTAPPNAMESECURE") : parameters.GetValueOrNull("MANAGEMENTAPPNAME")));
			if (text != null)
			{
				GlobalData.SetValue("MANAGEMENTSERVERURI", text, "VideoOS.Installer.UI.Framework.PageActions.CertificatePageActions.BeforeLeavePage");
				if (string.IsNullOrEmpty(parameters.GetValueOrNull("IDPADDRESS")))
				{
					GlobalData.SetValue("IDPADDRESS", new UriBuilder(new Uri(text))
					{
						Path = "IDP"
					}.Uri.ToString(), "VideoOS.Installer.UI.Framework.PageActions.CertificatePageActions.BeforeLeavePage");
				}
			}
		}
		return num;
	}
}
