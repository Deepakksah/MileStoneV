using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.LPR.Common.WCF.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IPluginCommandChannel : IPluginCommand, IClientChannel, IContextChannel, IChannel, ICommunicationObject, IExtensibleObject<IContextChannel>, IDisposable
{
}
