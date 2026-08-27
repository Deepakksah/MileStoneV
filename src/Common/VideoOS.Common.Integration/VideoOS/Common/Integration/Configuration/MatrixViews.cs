using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class MatrixViews : CollectionBase
{
	public new MatrixView this[Guid id] => base[id] as MatrixView;

	public MatrixViews(ConfigurationManager manager)
		: base(manager, manager.Dataset.Matrix)
	{
	}

	public MatrixView Add(Guid matrixId, string name, string description, string address, int port, string password)
	{
		return Add(_configurationManager.Dataset.Matrix.AddMatrixRow(matrixId.ToString(), name, description, address, port, password, DateTime.UtcNow)) as MatrixView;
	}

	protected override IConfigurationItem GetInstance(DataRow row)
	{
		return new MatrixView(_configurationManager, row as ConfigurationDataSet.MatrixRow);
	}
}
