using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

public class HostingInfo
{
	private sealed class HostingInfoComponent : VmoResourceComponent
	{
		public HostingInfoComponent()
		{
		}

		public int[] QueryHostedHardwareIdsNotAdded()
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				List<int> list = new List<int>();
				PrepareStoredProcedure("query_hosted_hardware_not_added");
				DatabaseDataReader databaseDataReader = ExecuteReader();
				while (databaseDataReader.Read())
				{
					int @int = databaseDataReader.GetInt32(0);
					list.Add(@int);
				}
				return list.ToArray();
			});
		}
	}

	public static int[] QueryHostedHardwareIdsNotAdded()
	{
		using HostingInfoComponent hostingInfoComponent = new HostingInfoComponent();
		return hostingInfoComponent.QueryHostedHardwareIdsNotAdded();
	}
}
