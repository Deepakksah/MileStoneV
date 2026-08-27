using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.EventServer.Communication.Status.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IStatusCommandCredentialsChannel : IStatusCommandCredentials, IClientChannel, IContextChannel, IChannel, ICommunicationObject, IExtensibleObject<IContextChannel>, IDisposable
{
}
