using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.Server.ServiceChannel;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class SCHMessageCreatorManagerClient : ClientBase<ISCHMessageCreatorManager>, ISCHMessageCreatorManager
{
	public SCHMessageCreatorManagerClient()
	{
	}

	public SCHMessageCreatorManagerClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public SCHMessageCreatorManagerClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public SCHMessageCreatorManagerClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public SCHMessageCreatorManagerClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public CreateMessagesMsgOut CreateMessages(SCHmessage[] messages)
	{
		return base.Channel.CreateMessages(messages);
	}
}
