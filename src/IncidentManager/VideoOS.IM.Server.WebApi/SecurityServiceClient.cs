using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class SecurityServiceClient : ClientBase<ISecurityService>, ISecurityService
{
	public SecurityServiceClient()
	{
	}

	public SecurityServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public SecurityServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public SecurityServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public SecurityServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public Task<bool[]> HasPermissionByActionListAsync(string namespaceId, string identity, string[] actionList, string objectId)
	{
		return base.Channel.HasPermissionByActionListAsync(namespaceId, identity, actionList, objectId);
	}
}
