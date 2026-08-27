using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class SendNotificationCommand : CommonCommandBase, ICommandWithSource
{
	[DataMember]
	public Guid IDNotificationProfile { get; set; }

	[DataMember]
	public Guid EventTypeId { get; set; }

	[DataMember]
	public Guid CameraId { get; set; }

	[DataMember]
	public Guid[] AudioIds { get; set; }

	[DataMember]
	public Guid TriggerId { get; set; }

	[DataMember]
	public Guid RecorderId { get; set; }

	[DataMember]
	public Guid RuleId { get; set; }

	[DataMember]
	public DateTime Time { get; set; }

	Guid ICommandWithSource.SourceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid("AC4B5835-2895-4003-B6B6-183D6A0BF684");
		}
	}

	public SendNotificationCommand(Guid notificationProfileId, Guid ruleId, Guid eventTypeId, Guid triggerId, Guid recorderId, Guid cameraId, Guid[] audioIds, DateTime time)
	{
		IDNotificationProfile = notificationProfileId;
		RuleId = ruleId;
		EventTypeId = eventTypeId;
		TriggerId = triggerId;
		RecorderId = recorderId;
		CameraId = cameraId;
		AudioIds = audioIds;
		Time = time;
	}

	public override void Accept(ICommandVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonCommandVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
