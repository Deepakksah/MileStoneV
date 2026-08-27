using System;

namespace VideoOS.Management.VmoClient;

public sealed class ClientTokenSettings : IClientTokenSettings
{
	public const string ManagementClientId = "VmsAdminClient";

	public const string SmartClientId = "VmsClient";

	public const string ApiClientId = "GrantValidatorClient";

	public string ClientId { get; }

	public ClientTokenSettings(string clientId)
	{
		if (string.IsNullOrEmpty(clientId))
		{
			throw new ArgumentNullException("clientId");
		}
		ClientId = clientId;
	}

	public string GetCalculatedClientId(ServerVersion serverVersion)
	{
		if (!(serverVersion < ServerVersions.v22_1))
		{
			return ClientId;
		}
		return "GrantValidatorClient";
	}
}
