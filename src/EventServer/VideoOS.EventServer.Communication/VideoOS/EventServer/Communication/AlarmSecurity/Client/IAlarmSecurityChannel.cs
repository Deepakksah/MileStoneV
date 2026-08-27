using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.EventServer.Communication.AlarmSecurity.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IAlarmSecurityChannel : IAlarmSecurity, IClientChannel, IContextChannel, IChannel, ICommunicationObject, IExtensibleObject<IContextChannel>, IDisposable
{
}
