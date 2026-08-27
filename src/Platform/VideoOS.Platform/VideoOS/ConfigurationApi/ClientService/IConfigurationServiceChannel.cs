using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.ConfigurationApi.ClientService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IConfigurationServiceChannel : IConfigurationService, IClientChannel, IContextChannel, IChannel, ICommunicationObject, IExtensibleObject<IContextChannel>, IDisposable
{
}
