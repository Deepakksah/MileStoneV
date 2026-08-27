using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.Management.Common;

namespace VideoOS.EventServer.Communication.AlarmSecurity.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class AlarmSecurityTokenClient : ClientBase<IAlarmSecurityToken>, IAlarmSecurityToken
{
	public AlarmSecurityTokenClient()
	{
	}

	public AlarmSecurityTokenClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public AlarmSecurityTokenClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AlarmSecurityTokenClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AlarmSecurityTokenClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public Identity[] GetUsersPermittedByObject(string token, string objectId)
	{
		return base.Channel.GetUsersPermittedByObject(token, objectId);
	}

	public Identity[] GetUsersPermittedByObjectList(string token, string[] objectIdList)
	{
		return base.Channel.GetUsersPermittedByObjectList(token, objectIdList);
	}

	public Identity[] GetUsersPermittedByKind(string token, Guid type)
	{
		return base.Channel.GetUsersPermittedByKind(token, type);
	}

	public Identity[] GetUsersPermittedByKindOnServer(string token, Guid type, string[] serverIds)
	{
		return base.Channel.GetUsersPermittedByKindOnServer(token, type, serverIds);
	}

	public Identity[] GetAllUsers(string token)
	{
		return base.Channel.GetAllUsers(token);
	}
}
