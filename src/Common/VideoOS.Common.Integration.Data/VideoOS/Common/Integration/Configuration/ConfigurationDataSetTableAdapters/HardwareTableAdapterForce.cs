using System.Data;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public class HardwareTableAdapterForce : HardwareTableAdapter
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
			ConfigurationDataSet.HardwareRow hardwareRow = (ConfigurationDataSet.HardwareRow)dataRows[i];
			switch (hardwareRow.RowState)
			{
			case DataRowState.Added:
				num += Update(hardwareRow);
				break;
			case DataRowState.Deleted:
			{
				string iDHardware = hardwareRow.GetIDHardware(DataRowVersion.Original);
				int num2;
				if ((num2 = ForceDelete(iDHardware)) == 0)
				{
					throw new DBConcurrencyException("Concurrency violation: the DeleteCommand affected 0 of the expected 1 records.", null, new DataRow[1] { hardwareRow });
				}
				num += num2;
				break;
			}
			case DataRowState.Modified:
			{
				string eventSettings = (hardwareRow.IsEventSettingsNull() ? null : hardwareRow.EventSettings);
				string settings = (hardwareRow.IsSettingsNull() ? null : hardwareRow.Settings);
				string vendorProductID = (hardwareRow.IsVendorProductIDNull() ? null : hardwareRow.VendorProductID);
				int? hostedHardware_id = (hardwareRow.IsHostedHardware_idNull() ? ((int?)null) : new int?(hardwareRow.HostedHardware_id));
				string cameraData = (hardwareRow.IsCameraDataNull() ? null : hardwareRow.CameraData);
				int num2;
				if ((num2 = ForceUpdate(hardwareRow.LastModified, hardwareRow.IDHardwareDriver, hardwareRow.Name, hardwareRow.Description, hardwareRow.URI, hardwareRow.LoginID, hardwareRow.EncryptedPassword, hardwareRow.IDRecorder, hardwareRow.Enabled, hardwareRow.C18NDigest, settings, eventSettings, vendorProductID, hardwareRow.DriverNumber, hardwareRow.DriverProxyName, hostedHardware_id, cameraData, hardwareRow.EdgeSettings, hardwareRow.SubType, hardwareRow.IDHardware)) == 0)
				{
					throw new DBConcurrencyException("Concurrency violation: the UpdateCommand affected 0 of the expected 1 records.", null, new DataRow[1] { hardwareRow });
				}
				num += num2;
				break;
			}
			}
		}
		return num;
	}
}
