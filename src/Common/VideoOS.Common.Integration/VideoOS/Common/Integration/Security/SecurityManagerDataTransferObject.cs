using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Security;

[Serializable]
public class SecurityManagerDataTransferObject : ISerializable
{
	private byte[] _serializedDataSet;

	private SecurityDataSet _securityDataSet;

	public SecurityDataSet DataSet
	{
		get
		{
			if (_securityDataSet == null)
			{
				_securityDataSet = UnzipSerializableObject(_serializedDataSet) as SecurityDataSet;
				_serializedDataSet = null;
			}
			return _securityDataSet;
		}
	}

	public Version Version { get; private set; }

	public long LastUpdate { get; set; }

	public long NextUpdate { get; set; }

	public SecurityManagerDataTransferObject(SecurityDataSet ds)
	{
		Version = new Version(1, 0, 0, 0);
		_securityDataSet = ds;
	}

	protected SecurityManagerDataTransferObject(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		_serializedDataSet = (byte[])info.GetValue("ZipSerializedDataSet", typeof(byte[]));
		_securityDataSet = null;
		MemoryStream stream = new MemoryStream((byte[])info.GetValue("Version", typeof(byte[])));
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(Version));
		Version = xmlSerializer.Deserialize(stream) as Version;
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
			info.AddValue("ZipSerializedDataSet", ZipSerializableObject(_securityDataSet));
		}
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(Version));
		MemoryStream memoryStream = new MemoryStream();
		xmlSerializer.Serialize(memoryStream, Version);
		info.AddValue("Version", memoryStream.ToArray());
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
		BinaryFormatter binaryFormatter = new BinaryFormatter().Wrap(typeof(SecurityDataSet), typeof(Version));
		using DeflateStream serializationStream = new DeflateStream(stream, CompressionMode.Decompress);
		return binaryFormatter.Deserialize(serializationStream);
	}
}
