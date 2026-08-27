using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Repository.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GisRepositoryCommandClient : ClientBase<IGisRepositoryCommand>, IGisRepositoryCommand
{
	public GisRepositoryCommandClient()
	{
	}

	public GisRepositoryCommandClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public GisRepositoryCommandClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public GisRepositoryCommandClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public GisRepositoryCommandClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public GetRepositoryResp GetRepository(GetRepositoryParams parameters)
	{
		return base.Channel.GetRepository(parameters);
	}

	public Task<GetRepositoryResp> GetRepositoryAsync(GetRepositoryParams parameters)
	{
		return base.Channel.GetRepositoryAsync(parameters);
	}

	public AddRepositoryResp AddRepository(AddRepositoryParams parameters)
	{
		return base.Channel.AddRepository(parameters);
	}

	public Task<AddRepositoryResp> AddRepositoryAsync(AddRepositoryParams parameters)
	{
		return base.Channel.AddRepositoryAsync(parameters);
	}

	public UpdateRepositoryResp UpdateRepository(UpdateRepositoryParams parameters)
	{
		return base.Channel.UpdateRepository(parameters);
	}

	public Task<UpdateRepositoryResp> UpdateRepositoryAsync(UpdateRepositoryParams parameters)
	{
		return base.Channel.UpdateRepositoryAsync(parameters);
	}
}
