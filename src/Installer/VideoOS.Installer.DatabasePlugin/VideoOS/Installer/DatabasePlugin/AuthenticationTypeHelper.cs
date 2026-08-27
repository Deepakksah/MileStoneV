using System.Globalization;
using VideoOS.Installer.DatabasePlugin.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.DatabasePlugin;

public static class AuthenticationTypeHelper
{
	public static string GetAuthenticationTypeResource(AuthenticationType authenticationType)
	{
		return authenticationType switch
		{
			AuthenticationType.IntegratedSecurity => string.Format(CultureInfo.InvariantCulture, Resources.DatabaseAdvancedSetup_WindowsAuthentication), 
			AuthenticationType.IntegratedSecurityTrusted => string.Format(CultureInfo.InvariantCulture, Resources.DatabaseAdvancedSetup_WindowsAuthenticationTrusted), 
			AuthenticationType.MicrosoftEntraIntegrated => string.Format(CultureInfo.InvariantCulture, Resources.DatabaseAdvancedSetup_MicrosoftEntraIntegrated), 
			AuthenticationType.MicrosoftEntraManagedIdentity => string.Format(CultureInfo.InvariantCulture, Resources.DatabaseAdvancedSetup_MicrosoftEntraManagedIdentity), 
			_ => null, 
		};
	}
}
