using System;
using System.Collections.Generic;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class UpdateAlarmValuesRequestObject : RequestObject
{
	public Guid Id { get; }

	public KeyValuePair<string, string>[] Updates { get; }

	public UpdateAlarmValuesRequestObject(int requestId, Guid id, KeyValuePair<string, string>[] updates)
		: base(requestId)
	{
		Id = id;
		Updates = updates;
	}
}
