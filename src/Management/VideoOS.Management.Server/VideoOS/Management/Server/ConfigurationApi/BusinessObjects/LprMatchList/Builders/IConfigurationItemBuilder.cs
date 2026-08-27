using VideoOS.ConfigurationAPI;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Builders;

public interface IConfigurationItemBuilder
{
	ConfigurationItem Build(ConfigurationItemPath path, ConfigurationItemPath parentPath, LicensePlateList list, LicensePlateInfo[] licensePlates, string[] methodIds);
}
