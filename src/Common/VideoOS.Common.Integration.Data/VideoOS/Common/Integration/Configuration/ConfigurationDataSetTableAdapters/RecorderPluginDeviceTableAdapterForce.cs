using System.Data;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public class RecorderPluginDeviceTableAdapterForce : RecorderPluginDeviceTableAdapter
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
			ConfigurationDataSet.RecorderPluginDeviceRow recorderPluginDeviceRow = (ConfigurationDataSet.RecorderPluginDeviceRow)dataRows[i];
			switch (recorderPluginDeviceRow.RowState)
			{
			case DataRowState.Added:
				num += Update(recorderPluginDeviceRow);
				break;
			case DataRowState.Deleted:
			{
				string iDDevice = recorderPluginDeviceRow.GetIDDevice(DataRowVersion.Original);
				string iDRecorderPlugin = recorderPluginDeviceRow.GetIDRecorderPlugin(DataRowVersion.Original);
				int num2;
				if ((num2 = ForceDelete(iDDevice, iDRecorderPlugin)) == 0)
				{
					throw new DBConcurrencyException("Concurrency violation: the DeleteCommand affected 0 of the expected 1 records.", null, new DataRow[1] { recorderPluginDeviceRow });
				}
				num += num2;
				break;
			}
			case DataRowState.Modified:
			{
				int num2;
				if ((num2 = ForceUpdate(recorderPluginDeviceRow.LastModified, recorderPluginDeviceRow.ProcessOrder, recorderPluginDeviceRow.Settings, recorderPluginDeviceRow.Enabled, recorderPluginDeviceRow.IDDevice, recorderPluginDeviceRow.IDRecorderPlugin)) == 0)
				{
					throw new DBConcurrencyException("Concurrency violation: the UpdateCommand affected 0 of the expected 1 records.", null, new DataRow[1] { recorderPluginDeviceRow });
				}
				num += num2;
				break;
			}
			}
		}
		return num;
	}
}
