using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.WCF.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ServerCommandClient : ClientBase<IServerCommand>, IServerCommand
{
	public ServerCommandClient()
	{
	}

	public ServerCommandClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ServerCommandClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ServerCommandClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ServerCommandClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public GetVersionResp GetVersion(string token, GetVersionParams parameters)
	{
		return base.Channel.GetVersion(token, parameters);
	}

	public Task<GetVersionResp> GetVersionAsync(string token, GetVersionParams parameters)
	{
		return base.Channel.GetVersionAsync(token, parameters);
	}

	public GetStatusResp GetStatus(string token, GetStatusParams parameters)
	{
		return base.Channel.GetStatus(token, parameters);
	}

	public Task<GetStatusResp> GetStatusAsync(string token, GetStatusParams parameters)
	{
		return base.Channel.GetStatusAsync(token, parameters);
	}

	public GetIntradaCountryModulesResp GetIntradaCountryModules(string token, GetIntradaCountryModulesParams parameters)
	{
		return base.Channel.GetIntradaCountryModules(token, parameters);
	}

	public Task<GetIntradaCountryModulesResp> GetIntradaCountryModulesAsync(string token, GetIntradaCountryModulesParams parameters)
	{
		return base.Channel.GetIntradaCountryModulesAsync(token, parameters);
	}

	public GetDetectedLicensePlatesResp GetDetectedLicensePlates(string token, GetDetectedLicensePlatesParams parameters)
	{
		return base.Channel.GetDetectedLicensePlates(token, parameters);
	}

	public Task<GetDetectedLicensePlatesResp> GetDetectedLicensePlatesAsync(string token, GetDetectedLicensePlatesParams parameters)
	{
		return base.Channel.GetDetectedLicensePlatesAsync(token, parameters);
	}

	public SystemConfigurationHasChangedResp SystemConfigurationHasChanged(string token, SystemConfigurationHasChangedParams parameters)
	{
		return base.Channel.SystemConfigurationHasChanged(token, parameters);
	}

	public Task<SystemConfigurationHasChangedResp> SystemConfigurationHasChangedAsync(string token, SystemConfigurationHasChangedParams parameters)
	{
		return base.Channel.SystemConfigurationHasChangedAsync(token, parameters);
	}

	public AnalyzeJpegResp AnalyzeJpeg(string token, AnalyzeJpegParams parameters)
	{
		return base.Channel.AnalyzeJpeg(token, parameters);
	}

	public Task<AnalyzeJpegResp> AnalyzeJpegAsync(string token, AnalyzeJpegParams parameters)
	{
		return base.Channel.AnalyzeJpegAsync(token, parameters);
	}
}
