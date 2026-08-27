using System;
using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[DataContract(Namespace = "")]
public class ManagementServerAddress : ServerAddressBase
{
}
