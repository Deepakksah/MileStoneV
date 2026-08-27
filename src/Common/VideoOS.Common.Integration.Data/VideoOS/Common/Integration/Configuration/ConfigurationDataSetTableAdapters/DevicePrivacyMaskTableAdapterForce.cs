using System;
using System.Data;
using System.Globalization;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public class DevicePrivacyMaskTableAdapterForce : DevicePrivacyMaskTableAdapter
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
			ConfigurationDataSet.DevicePrivacyMaskRow devicePrivacyMaskRow = (ConfigurationDataSet.DevicePrivacyMaskRow)dataRows[i];
			switch (devicePrivacyMaskRow.RowState)
			{
			case DataRowState.Added:
				num += Update(devicePrivacyMaskRow);
				break;
			case DataRowState.Deleted:
			case DataRowState.Modified:
				throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "The table adapter '{0}' does not support forcing updates (modifiy or delete) - but the corresponding table contains changes.", GetType().Name));
			}
		}
		return num;
	}
}
