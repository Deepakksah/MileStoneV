using System;
using System.Runtime.CompilerServices;
using VideoOS.Platform.JsonHandling;

namespace VideoOS.Platform.EventsAndState;

public abstract class Event : State
{
	private Lazy<JsonObject> _jsonData;

	public virtual Guid Id { get; }

	public virtual JsonObject JsonData => _jsonData?.Value;

	public Event(Guid id, Guid type, string source, Guid? stateGroupId, DateTime time, string jsonDataString)
		: base(type, source, stateGroupId, time)
	{
		Id = id;
		if (jsonDataString != null)
		{
			_jsonData = new Lazy<JsonObject>(() => new JsonObject(jsonDataString));
		}
	}

	public abstract T GetData<T>();

	public override string ToString()
	{
		return FormattableString.Invariant(FormattableStringFactory.Create("{0}, ID: {1}{2}", base.ToString(), Id, (JsonData != null) ? (", JsonData: " + JsonData.ToJson()) : string.Empty));
	}
}
