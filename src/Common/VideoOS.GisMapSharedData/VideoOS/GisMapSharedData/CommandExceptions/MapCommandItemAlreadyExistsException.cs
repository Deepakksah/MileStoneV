using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("ItemAlreadyExists")]
public class MapCommandItemAlreadyExistsException : MapCommandException
{
	private const string ITEM_ID_IDENTIFIER = "ItemId";

	public Guid ItemId
	{
		get
		{
			if (TryGetData("ItemId", out var value) && Guid.TryParse(value, out var result))
			{
				return result;
			}
			return default(Guid);
		}
	}

	public MapCommandItemAlreadyExistsException()
		: base(MapCommandErrorReason.ItemAlreadyExists, MapCommandErrorReason.ItemAlreadyExists)
	{
	}

	public MapCommandItemAlreadyExistsException(Guid itemId)
		: this()
	{
		AddData("ItemId", itemId.ToString());
	}

	public MapCommandItemAlreadyExistsException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.ItemAlreadyExists)
	{
	}

	public MapCommandItemAlreadyExistsException(string errorDescription, Guid itemId)
		: this(errorDescription)
	{
		AddData("ItemId", itemId.ToString());
	}

	public MapCommandItemAlreadyExistsException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.ItemAlreadyExists, innerException)
	{
	}

	public MapCommandItemAlreadyExistsException(string errorDescription, Guid itemId, Exception innerException)
		: this(errorDescription, innerException)
	{
		AddData("ItemId", itemId.ToString());
	}

	protected MapCommandItemAlreadyExistsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
