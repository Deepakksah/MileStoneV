using System;

namespace VideoOS.Platform.Data;

public interface IInternalCommandServiceImplementer<TDataType>
{
	Func<object, FQID, DateTime, TDataType> GetNearest { get; }

	Func<object, FQID, DateTime, TDataType> GetAt { get; }

	Func<object, FQID, DateTime, TDataType> GetAtOrBefore { get; }

	Func<object, FQID, DateTime, TDataType> GetAfter { get; }

	Func<object, FQID, DateTime, TDataType> GetBefore { get; }

	Func<object, string, TDataType> Command { get; }

	Action<TDataType> OnData { get; }

	Func<TDataType, ISourceData> Convertor { get; }
}
