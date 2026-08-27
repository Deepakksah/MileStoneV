using System;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class MatrixMonitor
{
	private Guid _guid;

	private string _name;

	public Guid Guid => _guid;

	public string Name => _name;

	public MatrixMonitor(IMatrixMonitorInfo matrixMonitorInfo)
	{
		_guid = matrixMonitorInfo.MatrixMonitorId;
		_name = matrixMonitorInfo.DisplayName;
	}
}
