using System;
using System.Data;
using System.Globalization;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public class HardwareDefinitionTableAdapterForce : HardwareDefinitionTableAdapter
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
			ConfigurationDataSet.HardwareDefinitionRow hardwareDefinitionRow = (ConfigurationDataSet.HardwareDefinitionRow)dataRows[i];
			switch (hardwareDefinitionRow.RowState)
			{
			case DataRowState.Added:
				ForceInsert(hardwareDefinitionRow.C18NDigest, hardwareDefinitionRow.Definition);
				num++;
				break;
			case DataRowState.Deleted:
				throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "The table adapter '{0}' does not support forcing updates - but the corresponding table contains changes.", GetType().Name));
			case DataRowState.Modified:
				throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "The table adapter '{0}' does not support forcing updates - but the corresponding table contains changes.", GetType().Name));
			}
		}
		return num;
	}
}
