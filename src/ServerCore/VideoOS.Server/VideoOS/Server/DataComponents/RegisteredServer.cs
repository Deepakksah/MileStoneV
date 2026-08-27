using System;
using System.Collections.Generic;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.DataComponents;

public sealed class RegisteredServer
{
	private sealed class RegisteredServerComponent : VmoResourceComponent
	{
		private static class RegisteredServerColumns
		{
			private static DatabaseColumnBinder IDServer = new DatabaseColumnBinder("IDServer");

			private static DatabaseColumnBinder Type = new DatabaseColumnBinder("Type");

			private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

			private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

			private static DatabaseColumnBinder Addresses = new DatabaseColumnBinder("Addresses");

			private static DatabaseColumnBinder Trusted = new DatabaseColumnBinder("Trusted");

			private static DatabaseColumnBinder CustomData = new DatabaseColumnBinder("CustomData");

			private static DatabaseColumnBinder Enabled = new DatabaseColumnBinder("Enabled");

			public static RegisteredServer Bind(DatabaseDataReader reader)
			{
				return new RegisteredServer
				{
					IDServer = IDServer.GetStringAsGuid(reader),
					Type = Type.GetStringAsGuid(reader),
					Name = Name.GetString(reader),
					Description = Description.GetString(reader),
					Addresses = ServerAddresses.GetFromXml(Addresses.GetString(reader)).ToArray(),
					Trusted = Trusted.GetBoolean(reader),
					CustomData = CustomData.GetString(reader),
					Enabled = Enabled.GetBoolean(reader)
				};
			}
		}

		public RegisteredServer[] QueryRegisteredServersByType(Guid serviceType)
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareSqlBatch();
				AddStatement("SELECT [IDServer], [Type], [Name], [Description], [Addresses], [Trusted], [CustomData], [Enabled] FROM [RegisteredServers] WHERE [Type] = @serviceType AND [Enabled] = 1");
				BindGuidType("@serviceType", serviceType);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<RegisteredServer> list = new List<RegisteredServer>();
				while (databaseDataReader.Read())
				{
					list.Add(RegisteredServerColumns.Bind(databaseDataReader));
				}
				return list.ToArray();
			});
		}
	}

	public Guid IDServer { get; set; }

	public Guid Type { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public ServerAddress[] Addresses { get; set; }

	public bool Trusted { get; set; }

	public string CustomData { get; set; }

	public bool Enabled { get; set; }

	public static RegisteredServer[] QueryRegisteredServersByType(Guid serviceType)
	{
		using RegisteredServerComponent registeredServerComponent = new RegisteredServerComponent();
		return registeredServerComponent.QueryRegisteredServersByType(serviceType);
	}
}
