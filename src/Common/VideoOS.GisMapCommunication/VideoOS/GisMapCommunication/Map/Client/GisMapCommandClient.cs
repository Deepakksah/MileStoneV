using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GisMapCommandClient : ClientBase<IGisMapCommand>, IGisMapCommand
{
	public GisMapCommandClient()
	{
	}

	public GisMapCommandClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public GisMapCommandClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public GisMapCommandClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public GisMapCommandClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public AddMapResp AddMap(AddMapParams parameters)
	{
		return base.Channel.AddMap(parameters);
	}

	public Task<AddMapResp> AddMapAsync(AddMapParams parameters)
	{
		return base.Channel.AddMapAsync(parameters);
	}

	public UpdateMapResp UpdateMap(UpdateMapParams parameters)
	{
		return base.Channel.UpdateMap(parameters);
	}

	public Task<UpdateMapResp> UpdateMapAsync(UpdateMapParams parameters)
	{
		return base.Channel.UpdateMapAsync(parameters);
	}

	public BrowseMapsResp BrowseMaps(BrowseMapsParams parameters)
	{
		return base.Channel.BrowseMaps(parameters);
	}

	public Task<BrowseMapsResp> BrowseMapsAsync(BrowseMapsParams parameters)
	{
		return base.Channel.BrowseMapsAsync(parameters);
	}

	public GetMapResp GetMap(GetMapParams parameters)
	{
		return base.Channel.GetMap(parameters);
	}

	public Task<GetMapResp> GetMapAsync(GetMapParams parameters)
	{
		return base.Channel.GetMapAsync(parameters);
	}

	public DeleteMapResp DeleteMap(DeleteMapParams parameters)
	{
		return base.Channel.DeleteMap(parameters);
	}

	public Task<DeleteMapResp> DeleteMapAsync(DeleteMapParams parameters)
	{
		return base.Channel.DeleteMapAsync(parameters);
	}
}
