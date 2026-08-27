using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IMatrixMonitorInfo
{
	Guid MatrixMonitorId { get; set; }

	string DisplayName { get; set; }
}
