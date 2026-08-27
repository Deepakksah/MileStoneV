using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public class DeviceGroupsTableAdapterForce : DeviceGroupsTableAdapter
{
	public override int Update(DataRow[] dataRows)
	{
		int num = 0;
		if (dataRows == null)
		{
			throw new ArgumentNullException("dataRows");
		}
		if (dataRows.Length == 0)
		{
			return num;
		}
		dataRows = DeviceGroupsTableAdapter.PrepareForUpdate(dataRows);
		DataRow[] array = dataRows;
		for (int i = 0; i < array.Length; i++)
		{
			ConfigurationDataSet.DeviceGroupsRow deviceGroupsRow = (ConfigurationDataSet.DeviceGroupsRow)array[i];
			switch (deviceGroupsRow.RowState)
			{
			case DataRowState.Added:
				num += Update(deviceGroupsRow);
				break;
			case DataRowState.Deleted:
			{
				string iDDeviceGroup = deviceGroupsRow.GetIDDeviceGroup(DataRowVersion.Original);
				int num2;
				if ((num2 = ForceDelete(iDDeviceGroup)) == 0)
				{
					throw new DBConcurrencyException("Concurrency violation: the DeleteCommand affected 0 of the expected 1 records.", null, new DataRow[1] { deviceGroupsRow });
				}
				num += num2;
				break;
			}
			case DataRowState.Modified:
			{
				string iDParentDeviceGroup = (deviceGroupsRow.IsIDParentDeviceGroupNull() ? null : deviceGroupsRow.IDParentDeviceGroup);
				int num2;
				if ((num2 = ForceUpdate(deviceGroupsRow.LastModified, iDParentDeviceGroup, deviceGroupsRow.DeviceType, deviceGroupsRow.Name, deviceGroupsRow.Description, deviceGroupsRow.BuiltIn, deviceGroupsRow.AutoInsertNew, deviceGroupsRow.IsUserControlled, deviceGroupsRow.IDDeviceGroup)) == 0)
				{
					throw new DBConcurrencyException("Concurrency violation: the UpdateCommand affected 0 of the expected 1 records.", null, new DataRow[1] { deviceGroupsRow });
				}
				num += num2;
				break;
			}
			}
		}
		return num;
	}
}
