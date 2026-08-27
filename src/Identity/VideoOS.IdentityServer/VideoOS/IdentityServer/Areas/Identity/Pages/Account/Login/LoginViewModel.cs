using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.IdentityServer.Areas.Identity.Pages.Account.Login;

public class LoginViewModel
{
	public class ExternalProvider
	{
		public string AuthenticationScheme { get; set; }

		public string DisplayName { get; set; }
	}

	public bool EnableLocalLogin { get; set; } = true;

	public bool IsClientInitiatedFlow { get; set; }

	public string ClientName { get; set; }

	public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Array.Empty<ExternalProvider>();

	public bool IsExternalLoginOnly
	{
		get
		{
			if (!EnableLocalLogin)
			{
				IEnumerable<ExternalProvider> externalProviders = ExternalProviders;
				if (externalProviders == null)
				{
					return false;
				}
				return externalProviders.Count() == 1;
			}
			return false;
		}
	}

	public string ExternalLoginScheme
	{
		get
		{
			if (!IsExternalLoginOnly)
			{
				return null;
			}
			return ExternalProviders?.SingleOrDefault()?.AuthenticationScheme;
		}
	}
}
