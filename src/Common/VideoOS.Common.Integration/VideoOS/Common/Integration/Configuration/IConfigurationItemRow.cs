using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public interface IConfigurationItemRow
{
	DataRow DataRow { get; }
}
