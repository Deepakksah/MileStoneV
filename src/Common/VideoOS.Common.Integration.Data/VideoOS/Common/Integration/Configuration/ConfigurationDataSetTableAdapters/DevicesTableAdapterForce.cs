using System.Data;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public class DevicesTableAdapterForce : DevicesTableAdapter
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
			ConfigurationDataSet.DevicesRow devicesRow = (ConfigurationDataSet.DevicesRow)dataRows[i];
			switch (devicesRow.RowState)
			{
			case DataRowState.Added:
				num += Update(devicesRow);
				break;
			case DataRowState.Deleted:
			{
				string iDDevice = devicesRow.GetIDDevice(DataRowVersion.Original);
				int num2;
				if ((num2 = ForceDelete(iDDevice)) == 0)
				{
					throw new DBConcurrencyException("Concurrency violation: the DeleteCommand affected 0 of the expected 1 records.", null, new DataRow[1] { devicesRow });
				}
				num += num2;
				break;
			}
			case DataRowState.Modified:
			{
				string presets = (devicesRow.IsPresetsNull() ? null : devicesRow.Presets);
				string settings = (devicesRow.IsSettingsNull() ? null : devicesRow.Settings);
				int num2;
				if ((num2 = ForceUpdate(devicesRow.LastModified, devicesRow.IDHardware, devicesRow.Name, devicesRow.Description, devicesRow.Location, devicesRow.DeviceType, devicesRow.Enabled, settings, presets, devicesRow.Shortcut, devicesRow.MulticastEnabled, devicesRow.Streams, devicesRow.DeviceIndex, devicesRow.SubType, devicesRow.FailoverLiveOnly, devicesRow.DisableOnFailover, devicesRow.PtzManualSessionTimeout, devicesRow.PtzReservedSessionTimeout, devicesRow.PtzExtendedManualSessionTimeout, devicesRow.GisPointString, devicesRow.ShortName, devicesRow.Icon, devicesRow.CoverageDirection, devicesRow.CoverageDepth, devicesRow.CoverageFieldOfView, devicesRow.IDDevice)) == 0)
				{
					throw new DBConcurrencyException("Concurrency violation: the UpdateCommand affected 0 of the expected 1 records.", null, new DataRow[1] { devicesRow });
				}
				num += num2;
				break;
			}
			}
		}
		return num;
	}
}
