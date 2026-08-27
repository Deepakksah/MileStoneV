using System;
using System.Collections;

namespace VideoOS.Common.Integration.Configuration;

public class Compressors : DictionaryBase
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.CompressorsDataTable _table;

	public Compressor this[Guid codecId] => (Compressor)base.Dictionary[codecId];

	public Compressor this[int handler]
	{
		get
		{
			foreach (Compressor value in Values)
			{
				if (value.Handler == handler)
				{
					return value;
				}
			}
			return null;
		}
	}

	public ICollection Keys => base.Dictionary.Keys;

	public ICollection Values => base.Dictionary.Values;

	public Compressors(ConfigurationManager manager)
	{
		if (manager == null)
		{
			throw new ArgumentNullException("manager");
		}
		_configurationManager = manager;
		_table = _configurationManager.Dataset.Compressors;
		_configurationManager.EnsureTableDataLoaded(_table);
		foreach (ConfigurationDataSet.CompressorsRow item in _table)
		{
			Add(item);
		}
	}

	public Compressor Add(ConfigurationDataSet.CompressorsRow row)
	{
		Compressor compressor = new Compressor(_configurationManager, row);
		base.Dictionary.Add(compressor.IDCompressor, compressor);
		return compressor;
	}

	public Compressor Add(Guid idCompressor, int handler, int quality, int keyFrames, int dataRate, byte[] configData)
	{
		Compressor compressor = new Compressor(_configurationManager, _table.AddCompressorsRow(DateTime.UtcNow, idCompressor.ToString(), handler, 0, quality, keyFrames, dataRate, configData));
		base.Dictionary.Add(compressor.IDCompressor, compressor);
		return compressor;
	}
}
