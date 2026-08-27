using System;
using System.Data;
using System.Globalization;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public class HardwareDriverRecorderTableAdapterForce : HardwareDriverRecorderTableAdapter
{
	public override int Update(DataRow[] dataRows)
	{
		throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "The table adapter '{0}' does not support forcing updates - but the corresponding table contains changes.", GetType().Name));
	}
}
