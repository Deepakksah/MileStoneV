using System.Data;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public class DeviceLinksTableAdapterForce : DeviceLinksTableAdapter
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
			ConfigurationDataSet.DeviceLinksRow deviceLinksRow = (ConfigurationDataSet.DeviceLinksRow)dataRows[i];
			switch (deviceLinksRow.RowState)
			{
			case DataRowState.Added:
				num += Update(deviceLinksRow);
				break;
			case DataRowState.Deleted:
			{
				string iDDevice = deviceLinksRow.GetIDDevice(DataRowVersion.Original);
				string iDDeviceLinked = deviceLinksRow.GetIDDeviceLinked(DataRowVersion.Original);
				int num2;
				if ((num2 = ForceDelete(iDDevice, iDDeviceLinked)) == 0)
				{
					throw new DBConcurrencyException("Concurrency violation: the DeleteCommand affected 0 of the expected 1 records.", null, new DataRow[1] { deviceLinksRow });
				}
				num += num2;
				break;
			}
			case DataRowState.Modified:
			{
				int num2;
				if ((num2 = ForceUpdate(deviceLinksRow.LastModified, deviceLinksRow.Priority, deviceLinksRow.IDDevice, deviceLinksRow.IDDeviceLinked)) == 0)
				{
					throw new DBConcurrencyException("Concurrency violation: the UpdateCommand affected 0 of the expected 1 records.", null, new DataRow[1] { deviceLinksRow });
				}
				num += num2;
				break;
			}
			}
		}
		return num;
	}
}
