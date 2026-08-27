using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.GisMapCommunication.Layer.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IGisLayerCommandChannel : IGisLayerCommand, IClientChannel, IContextChannel, IChannel, ICommunicationObject, IExtensibleObject<IContextChannel>, IDisposable
{
}
