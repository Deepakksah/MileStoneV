using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public class DeviceGroupsTableAdapterWithSortedUpdate : DeviceGroupsTableAdapter
{
	public override int Update(DataRow[] dataRows)
	{
		int result = 0;
		if (dataRows == null)
		{
			throw new ArgumentNullException("dataRows");
		}
		if (dataRows.Length == 0)
		{
			return result;
		}
		dataRows = DeviceGroupsTableAdapter.PrepareForUpdate(dataRows);
		return base.Update(dataRows);
	}
}
