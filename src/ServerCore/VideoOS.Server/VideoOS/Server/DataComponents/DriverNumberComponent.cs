using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.DataComponents;

public sealed class DriverNumberComponent : VmoResourceComponent, IDriverNumberComponent, IDisposable
{
	public DriverNumberComponent()
	{
	}

	public IDictionary<Guid, int> UpdateDriverNumbers(IDictionary<Guid, int> parameter)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				Dictionary<Guid, int> dictionary = new Dictionary<Guid, int>();
				PrepareStoredProcedure("update_dbo_driver_numbers");
				BindIdTable("@id_drivers", parameter.Keys.Select((Guid x) => x.ToString()));
				DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDDriver");
				DatabaseColumnBinder databaseColumnBinder2 = new DatabaseColumnBinder("DriverNumber");
				DatabaseDataReader databaseDataReader = ExecuteReader();
				while (databaseDataReader.Read())
				{
					Guid guid = databaseColumnBinder.GetGuid2(databaseDataReader);
					int @int = databaseColumnBinder2.GetInt32(databaseDataReader);
					dictionary.Add(guid, @int);
				}
				return dictionary;
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}
}
