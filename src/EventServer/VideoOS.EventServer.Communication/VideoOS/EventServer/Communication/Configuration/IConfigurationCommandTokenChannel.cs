using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.EventServer.Communication.Configuration;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IConfigurationCommandTokenChannel : IConfigurationCommandToken, IClientChannel, IContextChannel, IChannel, ICommunicationObject, IExtensibleObject<IContextChannel>, IDisposable
{
}
