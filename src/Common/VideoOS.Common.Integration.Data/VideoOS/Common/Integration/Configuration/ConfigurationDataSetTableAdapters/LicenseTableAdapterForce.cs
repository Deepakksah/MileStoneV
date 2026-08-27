using System;
using System.Data;
using System.Globalization;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public class LicenseTableAdapterForce : LicenseTableAdapter
{
	public override int Update(DataRow[] dataRows)
	{
		throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "The table adapter '{0}' does not support forcing updates - but the corresponding table contains changes.", GetType().Name));
	}

	public virtual int FillByLicenseIds2(ConfigurationDataSet.LicenseDataTable dataTable, object ids)
	{
		base.Adapter.SelectCommand = base.CommandCollection[2];
		if (ids == null)
		{
			base.Adapter.SelectCommand.Parameters[1].Value = DBNull.Value;
		}
		else
		{
			base.Adapter.SelectCommand.Parameters[1].Value = ids;
		}
		if (base.ClearBeforeFill)
		{
			((DataTable)(object)dataTable).Clear();
		}
		return base.Adapter.Fill((DataTable)(object)dataTable);
	}
}
