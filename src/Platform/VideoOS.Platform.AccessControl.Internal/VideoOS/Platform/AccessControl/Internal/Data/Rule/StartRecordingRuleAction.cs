using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public class StartRecordingRuleAction : RuleActionBase
{
	public IEnumerable<FQID> CameraIds { get; set; }

	public TimeSpan Duration { get; set; }

	public StartRecordingRuleAction(RuleConfigurationSerializeData.StartRecordingRuleActionData startRecordingRuleActionData)
	{
		CameraIds = startRecordingRuleActionData.CameraIds;
		Duration = new TimeSpan(startRecordingRuleActionData.DurationTicks);
	}

	public StartRecordingRuleAction()
	{
	}

	public override object ToDataClass()
	{
		return new RuleConfigurationSerializeData.StartRecordingRuleActionData
		{
			CameraIds = ((CameraIds != null) ? CameraIds.ToList() : new List<FQID>()),
			DurationTicks = Duration.Ticks
		};
	}
}
