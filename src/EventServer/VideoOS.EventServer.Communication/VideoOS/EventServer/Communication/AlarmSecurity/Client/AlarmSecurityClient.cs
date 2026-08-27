using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.Management.Common;

namespace VideoOS.EventServer.Communication.AlarmSecurity.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class AlarmSecurityClient : ClientBase<IAlarmSecurity>, IAlarmSecurity
{
	public AlarmSecurityClient()
	{
	}

	public AlarmSecurityClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public AlarmSecurityClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AlarmSecurityClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AlarmSecurityClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public Identity[] GetUsersPermittedByObject(string objectId)
	{
		return base.Channel.GetUsersPermittedByObject(objectId);
	}

	public Identity[] GetAllUsers()
	{
		return base.Channel.GetAllUsers();
	}
}
