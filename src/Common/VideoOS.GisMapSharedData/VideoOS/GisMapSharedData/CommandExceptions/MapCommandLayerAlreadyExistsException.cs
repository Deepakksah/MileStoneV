using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("LayerAlreadyExists")]
public class MapCommandLayerAlreadyExistsException : MapCommandException
{
	private const string LAYER_ID_IDENTIFIER = "LayerId";

	public Guid LayerId
	{
		get
		{
			if (TryGetData("LayerId", out var value) && Guid.TryParse(value, out var result))
			{
				return result;
			}
			return default(Guid);
		}
	}

	public MapCommandLayerAlreadyExistsException()
		: base(MapCommandErrorReason.LayerAlreadyExists, MapCommandErrorReason.LayerAlreadyExists)
	{
	}

	public MapCommandLayerAlreadyExistsException(Guid layerId)
		: this()
	{
		AddData("LayerId", layerId.ToString());
	}

	public MapCommandLayerAlreadyExistsException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.LayerAlreadyExists)
	{
	}

	public MapCommandLayerAlreadyExistsException(string errorDescription, Guid layerId)
		: this(errorDescription)
	{
		AddData("LayerId", layerId.ToString());
	}

	public MapCommandLayerAlreadyExistsException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.LayerAlreadyExists, innerException)
	{
	}

	public MapCommandLayerAlreadyExistsException(string errorDescription, Guid layerId, Exception innerException)
		: this(errorDescription, innerException)
	{
		AddData("LayerId", layerId.ToString());
	}

	protected MapCommandLayerAlreadyExistsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
