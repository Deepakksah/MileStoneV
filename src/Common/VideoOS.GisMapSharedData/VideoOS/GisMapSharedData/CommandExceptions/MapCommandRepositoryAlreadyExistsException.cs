using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("RepositoryAlreadyExists")]
public class MapCommandRepositoryAlreadyExistsException : MapCommandException
{
	private const string REPOSITORY_ID_IDENTIFIER = "RepositoryId";

	public Guid RepositoryId
	{
		get
		{
			if (TryGetData("RepositoryId", out var value) && Guid.TryParse(value, out var result))
			{
				return result;
			}
			return default(Guid);
		}
	}

	public MapCommandRepositoryAlreadyExistsException()
		: base(MapCommandErrorReason.RepositoryAlreadyExists, MapCommandErrorReason.RepositoryAlreadyExists)
	{
	}

	public MapCommandRepositoryAlreadyExistsException(Guid repositoryId)
		: this()
	{
		AddData("RepositoryId", repositoryId.ToString());
	}

	public MapCommandRepositoryAlreadyExistsException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.RepositoryAlreadyExists)
	{
	}

	public MapCommandRepositoryAlreadyExistsException(string errorDescription, Guid repositoryId)
		: this(errorDescription)
	{
		AddData("RepositoryId", repositoryId.ToString());
	}

	public MapCommandRepositoryAlreadyExistsException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.RepositoryAlreadyExists, innerException)
	{
	}

	public MapCommandRepositoryAlreadyExistsException(string errorDescription, Guid repositoryId, Exception innerException)
		: this(errorDescription, innerException)
	{
		AddData("RepositoryId", repositoryId.ToString());
	}

	protected MapCommandRepositoryAlreadyExistsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
