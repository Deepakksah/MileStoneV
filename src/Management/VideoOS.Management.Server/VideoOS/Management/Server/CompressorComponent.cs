using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class CompressorComponent : GeminiResourceComponent, ICompressorComponent, IDisposable
{
	private static class CompressorColumns
	{
		private static DatabaseColumnBinder Handler = new DatabaseColumnBinder("Handler");

		private static DatabaseColumnBinder IDCodec = new DatabaseColumnBinder("IDCodec");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder Flags = new DatabaseColumnBinder("Flags");

		private static DatabaseColumnBinder Quality = new DatabaseColumnBinder("Quality");

		private static DatabaseColumnBinder DataRate = new DatabaseColumnBinder("DataRate");

		private static DatabaseColumnBinder KeyFrames = new DatabaseColumnBinder("KeyFrames");

		private static DatabaseColumnBinder Configdata = new DatabaseColumnBinder("ConfigData");

		public static Compressors Bind(DatabaseDataReader reader)
		{
			return new Compressors
			{
				LastModified = LastModified.GetDateTime(reader),
				IdCodec = new Guid(IDCodec.GetString(reader, allowNulls: false)),
				Handler = Handler.GetInt32(reader),
				Flags = Flags.GetInt32(reader),
				Quality = Quality.GetInt32(reader),
				DataRate = DataRate.GetInt32(reader),
				KeyFrames = KeyFrames.GetInt32(reader),
				ConfigData = Configdata.GetBinary(reader, allowNulls: true)
			};
		}
	}

	public void DeleteCompressor(long handler)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_compressor");
			BindLong("@handler", handler);
			ExecuteNonQuery();
		});
	}

	public Compressors QueryCompressor(long handler)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_compressor");
			BindLong("@handler", handler);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			Compressors result = null;
			if (databaseDataReader.Read())
			{
				result = CompressorColumns.Bind(databaseDataReader);
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public Compressors[] QueryCompressors()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_all_compressor");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Compressors> list = new List<Compressors>();
			while (databaseDataReader.Read())
			{
				list.Add(CompressorColumns.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public void UpdateCompressor(long handler, Guid idCodec, int flags, int quality, int keyFrames, int dataRate, byte[] configData)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_compressor");
			BindLong("@handler", handler);
			BindGuid("@idCodec", idCodec);
			BindInt("@flags", flags);
			BindInt("@quality", quality);
			BindInt("@keyFrames", keyFrames);
			BindInt("@dataRate", dataRate);
			BindByteArray("@configdata", configData, allowNull: false);
			ExecuteNonQuery();
		});
	}
}
