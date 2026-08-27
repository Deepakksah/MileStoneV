using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.LogServer.Communication.LogWriterServiceReference;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface ILogWriterServiceChannel : ILogWriterService, IClientChannel, IContextChannel, IChannel, ICommunicationObject, IExtensibleObject<IContextChannel>, IDisposable
{
}
