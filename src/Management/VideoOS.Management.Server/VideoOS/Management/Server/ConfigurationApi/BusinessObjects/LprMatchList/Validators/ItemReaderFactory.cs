using VideoOS.ConfigurationAPI;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Validators;

public delegate IItemReader ItemReaderFactory(ConfigurationItem item, LicensePlateList[] licensePlateLists);
