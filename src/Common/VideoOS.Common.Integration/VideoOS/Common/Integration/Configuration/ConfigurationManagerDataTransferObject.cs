using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace VideoOS.Common.Integration.Configuration;

[Serializable]
public class ConfigurationManagerDataTransferObject : ISerializable
{
	private byte[] _serializedDataSet;

	private ConfigurationDataSet _configurationDataSet;

	public ConfigurationDataSet DataSet
	{
		get
		{
			if (_configurationDataSet == null)
			{
				_configurationDataSet = UnzipSerializableObject(_serializedDataSet) as ConfigurationDataSet;
				_serializedDataSet = null;
			}
			return _configurationDataSet;
		}
	}

	public long LastUpdate { get; set; }

	public long NextUpdate { get; set; }

	public ConfigurationManagerDataTransferObject(ConfigurationDataSet ds)
	{
		_configurationDataSet = ds;
	}

	protected ConfigurationManagerDataTransferObject(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		_serializedDataSet = (byte[])info.GetValue("ZipSerializedDataSet", typeof(byte[]));
		_configurationDataSet = null;
		LastUpdate = info.GetInt64("LastUpdate");
		NextUpdate = info.GetInt64("NextUpdate");
	}

	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		if (_serializedDataSet != null)
		{
			info.AddValue("ZipSerializedDataSet", _serializedDataSet);
		}
		else
		{
			info.AddValue("ZipSerializedDataSet", ZipSerializableObject(_configurationDataSet));
		}
		info.AddValue("LastUpdate", LastUpdate);
		info.AddValue("NextUpdate", NextUpdate);
	}

	private static byte[] ZipSerializableObject(object ds)
	{
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			using DeflateStream serializationStream = new DeflateStream(memoryStream, CompressionMode.Compress);
			binaryFormatter.Serialize(serializationStream, ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new Exception("Error getting bytes", innerException);
		}
		return memoryStream.ToArray();
	}

	private static object UnzipSerializableObject(byte[] data)
	{
		MemoryStream stream = new MemoryStream(data);
		BinaryFormatter binaryFormatter = new BinaryFormatter().Wrap(typeof(ConfigurationDataSet), typeof(Version));
		using DeflateStream serializationStream = new DeflateStream(stream, CompressionMode.Decompress);
		return binaryFormatter.Deserialize(serializationStream);
	}
}
