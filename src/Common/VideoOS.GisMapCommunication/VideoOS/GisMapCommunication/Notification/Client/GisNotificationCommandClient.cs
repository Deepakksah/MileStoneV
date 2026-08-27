using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace VideoOS.GisMapCommunication.Notification.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GisNotificationCommandClient : ClientBase<IGisNotificationCommand>, IGisNotificationCommand
{
	public GisNotificationCommandClient()
	{
	}

	public GisNotificationCommandClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public GisNotificationCommandClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public GisNotificationCommandClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public GisNotificationCommandClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public void Dummy()
	{
		base.Channel.Dummy();
	}

	public Task DummyAsync()
	{
		return base.Channel.DummyAsync();
	}
}
