using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.EventServer.Communication.AlarmClient;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IAlarmCommandTokenChannel : IAlarmCommandToken, IClientChannel, IContextChannel, IChannel, ICommunicationObject, IExtensibleObject<IContextChannel>, IDisposable
{
}
