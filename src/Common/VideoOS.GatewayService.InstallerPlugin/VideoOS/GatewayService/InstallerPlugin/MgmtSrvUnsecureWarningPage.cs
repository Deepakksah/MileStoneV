using System;
using System.Collections.Generic;
using VideoOS.GatewayService.Utilities;
using VideoOS.Installer.Interfaces.Content;

namespace VideoOS.GatewayService.InstallerPlugin;

public class MgmtSrvUnsecureWarningPage : IWizardCommonPageSubscription<WarningPageContent>
{
	private readonly int _rank = 301;

	public Guid Id => new Guid("52934999-5985-4E17-807D-E6129E1C56FF");

	public bool BeforeActivate(Dictionary<string, string> parameters)
	{
		if (parameters != null)
		{
			if ((parameters.ContainsKey("IS_UPGRADE") && parameters["IS_UPGRADE"].Equals("true", StringComparison.OrdinalIgnoreCase)) || (parameters.ContainsKey("TRECK_ID") && parameters["TRECK_ID"].Equals("modify", StringComparison.OrdinalIgnoreCase)))
			{
				return false;
			}
			if (parameters.ContainsKey("MANAGEMENTSERVERURI"))
			{
				string text = parameters["MANAGEMENTSERVERURI"];
				if (!string.IsNullOrEmpty(text))
				{
					return text.StartsWith("http://", StringComparison.Ordinal);
				}
			}
		}
		return false;
	}

	public Dictionary<string, string> CollectParameters(WarningPageContent content)
	{
		return null;
	}

	public WarningPageContent GetContent()
	{
		GatewayServiceLocalizedResource gatewayServiceLocalizedResource = new GatewayServiceLocalizedResource();
		ContentLinkText details = new ContentLinkText(gatewayServiceLocalizedResource.MgmtSrvUnsecureWarningDetails);
		ContentLinkText subsection = new ContentLinkText(gatewayServiceLocalizedResource.MgmtSrvUnsecureWarningSubsection);
		return new WarningPageContent(gatewayServiceLocalizedResource.Warning, details, gatewayServiceLocalizedResource.WhatTodo, subsection, _rank);
	}
}
