namespace VideoOS.GatewayService.Utilities.Oem;

public interface IGatewayServiceOemResource
{
	string CompanyNameShort { get; }

	string ApiGatewayDisplayName { get; }

	string ApiGatewayInstallDir { get; }
}
