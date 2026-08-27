using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.GisMapCommunication.Map.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IGisMapCommandChannel : IGisMapCommand, IClientChannel, IContextChannel, IChannel, ICommunicationObject, IExtensibleObject<IContextChannel>, IDisposable
{
}
