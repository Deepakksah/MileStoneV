using System;

namespace VideoOS.Common.Integration.Configuration;

public interface IConfigurationItem : IConfigurationItemRow
{
	Guid ID { get; }
}
