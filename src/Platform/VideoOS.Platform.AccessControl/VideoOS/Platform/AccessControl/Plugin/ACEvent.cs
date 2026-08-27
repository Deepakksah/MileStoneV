using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACEvent
{
	public string Id { get; private set; }

	public string EventTypeId { get; private set; }

	public string SourceOperationableInstanceId { get; private set; }

	public DateTime TimeStamp { get; private set; }

	public string Message { get; private set; }

	public string Reason { get; private set; }

	public IEnumerable<string> RelatedCredentialHolderIds { get; private set; }

	public IEnumerable<string> RelatedElementIds { get; private set; }

	public IEnumerable<ACProperty> Properties { get; private set; }

	public ACEvent(string id, string eventTypeId, string sourceOperationableInstanceId, DateTime timeStamp, string message, string reason, IEnumerable<string> relatedCredentialHolderIds, IEnumerable<string> relatedElementIds, IEnumerable<ACProperty> properties)
	{
		Id = id;
		EventTypeId = eventTypeId;
		SourceOperationableInstanceId = sourceOperationableInstanceId;
		TimeStamp = timeStamp;
		Message = message;
		Reason = reason;
		RelatedCredentialHolderIds = new ReadOnlyCollection<string>((relatedCredentialHolderIds != null) ? relatedCredentialHolderIds.OfType<string>().ToList() : new List<string>());
		RelatedElementIds = new ReadOnlyCollection<string>((relatedElementIds != null) ? relatedElementIds.ToList() : new List<string>());
		Properties = new ReadOnlyCollection<ACProperty>((properties != null) ? properties.ToList() : new List<ACProperty>());
	}

	public override string ToString()
	{
		return $"Id: {Id}, EventTypeId: {EventTypeId}, SourceOperationableInstanceId: {SourceOperationableInstanceId}, TimeStamp: {TimeStamp}, Message: {Message}, RelatedCredentialHolderIds: {RelatedCredentialHolderIds.ToList()}, RelatedElementIds: {RelatedElementIds.ToList()}";
	}
}
