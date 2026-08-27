using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class MatrixMonitorCollection
{
	private List<MatrixMonitor> _list = new List<MatrixMonitor>();

	private Dictionary<Guid, MatrixMonitor> _matrixMonitors = new Dictionary<Guid, MatrixMonitor>();

	private ISystemInfo _systemInfo;

	public MatrixMonitor this[Guid guid]
	{
		get
		{
			if (_matrixMonitors.TryGetValue(guid, out var value))
			{
				return value;
			}
			return null;
		}
	}

	public MatrixMonitorCollection()
	{
	}

	public MatrixMonitorCollection(ISystemInfo systemInfo)
	{
		_systemInfo = systemInfo;
		if (_systemInfo.ConfigurationInfo != null)
		{
			CreateMatrixCollection();
		}
	}

	private void CreateMatrixCollection()
	{
		IMatrixMonitorInfo[] matrixMonitors = _systemInfo.ConfigurationInfo.MatrixMonitors;
		for (int i = 0; i < matrixMonitors.Length; i++)
		{
			MatrixMonitor matrixMonitor = new MatrixMonitor(matrixMonitors[i]);
			_list.Add(matrixMonitor);
			_matrixMonitors[matrixMonitor.Guid] = matrixMonitor;
		}
	}

	public bool ContainsKey(Guid id)
	{
		return _matrixMonitors.ContainsKey(id);
	}

	public IEnumerator<MatrixMonitor> GetEnumerator()
	{
		return _list.GetEnumerator();
	}
}
