using System.Data;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public class DeviceGroupMembersTableAdapterForce : DeviceGroupMembersTableAdapter
{
	public override int Update(DataRow[] dataRows)
	{
		int num = 0;
		if (dataRows == null || dataRows.Length == 0)
		{
			return num;
		}
		for (int i = 0; i < dataRows.Length; i++)
		{
			ConfigurationDataSet.DeviceGroupMembersRow deviceGroupMembersRow = (ConfigurationDataSet.DeviceGroupMembersRow)dataRows[i];
			switch (deviceGroupMembersRow.RowState)
			{
			case DataRowState.Added:
				num += Update(deviceGroupMembersRow);
				break;
			case DataRowState.Deleted:
			{
				string iDDevice = deviceGroupMembersRow.GetIDDevice(DataRowVersion.Original);
				string iDDeviceGroup = deviceGroupMembersRow.GetIDDeviceGroup(DataRowVersion.Original);
				int num2;
				if ((num2 = ForceDelete(iDDeviceGroup, iDDevice)) == 0)
				{
					throw new DBConcurrencyException("Concurrency violation: the DeleteCommand affected 0 of the expected 1 records.", null, new DataRow[1] { deviceGroupMembersRow });
				}
				num += num2;
				break;
			}
			case DataRowState.Modified:
			{
				int num2;
				if ((num2 = ForceUpdate(deviceGroupMembersRow.LastModified, deviceGroupMembersRow.IDDeviceGroup, deviceGroupMembersRow.IDDevice)) == 0)
				{
					throw new DBConcurrencyException("Concurrency violation: the UpdateCommand affected 0 of the expected 1 records.", null, new DataRow[1] { deviceGroupMembersRow });
				}
				num += num2;
				break;
			}
			}
		}
		return num;
	}
}
