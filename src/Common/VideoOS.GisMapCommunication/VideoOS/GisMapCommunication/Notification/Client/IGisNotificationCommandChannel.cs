using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.GisMapCommunication.Notification.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IGisNotificationCommandChannel : IGisNotificationCommand, IClientChannel, IContextChannel, IChannel, ICommunicationObject, IExtensibleObject<IContextChannel>, IDisposable
{
}
