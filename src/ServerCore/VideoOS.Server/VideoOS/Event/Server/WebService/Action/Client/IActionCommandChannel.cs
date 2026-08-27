using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.Event.Server.WebService.Action.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IActionCommandChannel : IActionCommand, IClientChannel, IContextChannel, IChannel, ICommunicationObject, IExtensibleObject<IContextChannel>, IDisposable
{
}
