using System.Collections.Generic;

namespace VideoOS.Common.Login;

public interface IOfflineConnectionInfo : IConnectionInfo
{
	AsyncLoginCallback<IOfflineConnectionInfo> AsyncLoginCallback { get; }

	string ProjectName { get; set; }

	string ProjectPassword { get; set; }

	IList<OfflinePasswordSourceInfo> PasswordSourceInfos { get; }
}
