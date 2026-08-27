using System;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public class AccessControlDoorRuleSource : RuleSourceBase
{
	public Guid DoorUnitId { get; private set; }

	public AccessControlDoorRuleSource(RuleConfigurationSerializeData.AccessControlDoorRuleSourceData accessControlDoorRuleSourceData)
	{
		DoorUnitId = accessControlDoorRuleSourceData.DoorUnitId;
	}

	public AccessControlDoorRuleSource(Guid doorUnitId)
	{
		DoorUnitId = doorUnitId;
	}

	public override object ToDataClass()
	{
		return new RuleConfigurationSerializeData.AccessControlDoorRuleSourceData
		{
			DoorUnitId = DoorUnitId
		};
	}
}
