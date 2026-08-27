using System;
using System.Text;

namespace VideoOS.Platform.Metadata;

public class MetadataContent
{
	private readonly string _metadataXml;

	private readonly Lazy<MetadataStream> _deserializedMetadata;

	private readonly MetadataDeserializer _deserializer = new MetadataDeserializer();

	public MetadataContent(byte[] data)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		_metadataXml = Encoding.UTF8.GetString(data);
		_deserializedMetadata = new Lazy<MetadataStream>(DeserializeMetadata, isThreadSafe: true);
	}

	public string GetMetadataString()
	{
		return _metadataXml;
	}

	public MetadataStream GetMetadataStream()
	{
		return _deserializedMetadata.Value;
	}

	private MetadataStream DeserializeMetadata()
	{
		return _deserializer.ParseMetadataXml(_metadataXml);
	}
}
