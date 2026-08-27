using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Layer.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GisLayerCommandClient : ClientBase<IGisLayerCommand>, IGisLayerCommand
{
	public GisLayerCommandClient()
	{
	}

	public GisLayerCommandClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public GisLayerCommandClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public GisLayerCommandClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public GisLayerCommandClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public AddLayerResp AddLayer(AddLayerParams parameters)
	{
		return base.Channel.AddLayer(parameters);
	}

	public Task<AddLayerResp> AddLayerAsync(AddLayerParams parameters)
	{
		return base.Channel.AddLayerAsync(parameters);
	}

	public AddLayersResp AddLayers(AddLayersParams parameters)
	{
		return base.Channel.AddLayers(parameters);
	}

	public Task<AddLayersResp> AddLayersAsync(AddLayersParams parameters)
	{
		return base.Channel.AddLayersAsync(parameters);
	}

	public UpdateLayerResp UpdateLayer(UpdateLayerParams parameters)
	{
		return base.Channel.UpdateLayer(parameters);
	}

	public Task<UpdateLayerResp> UpdateLayerAsync(UpdateLayerParams parameters)
	{
		return base.Channel.UpdateLayerAsync(parameters);
	}

	public GetLayerResp GetLayer(GetLayerParams parameters)
	{
		return base.Channel.GetLayer(parameters);
	}

	public Task<GetLayerResp> GetLayerAsync(GetLayerParams parameters)
	{
		return base.Channel.GetLayerAsync(parameters);
	}

	public GetLayersByMapResp GetLayersByMap(GetLayersByMapParams parameters)
	{
		return base.Channel.GetLayersByMap(parameters);
	}

	public Task<GetLayersByMapResp> GetLayersByMapAsync(GetLayersByMapParams parameters)
	{
		return base.Channel.GetLayersByMapAsync(parameters);
	}

	public DeleteLayerResp DeleteLayer(DeleteLayerParams parameters)
	{
		return base.Channel.DeleteLayer(parameters);
	}

	public Task<DeleteLayerResp> DeleteLayerAsync(DeleteLayerParams parameters)
	{
		return base.Channel.DeleteLayerAsync(parameters);
	}

	public DeleteLayersResp DeleteLayers(DeleteLayersParams parameters)
	{
		return base.Channel.DeleteLayers(parameters);
	}

	public Task<DeleteLayersResp> DeleteLayersAsync(DeleteLayersParams parameters)
	{
		return base.Channel.DeleteLayersAsync(parameters);
	}

	public ReorderLayersResp ReorderLayers(ReorderLayersParams parameters)
	{
		return base.Channel.ReorderLayers(parameters);
	}

	public Task<ReorderLayersResp> ReorderLayersAsync(ReorderLayersParams parameters)
	{
		return base.Channel.ReorderLayersAsync(parameters);
	}
}
