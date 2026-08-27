using System;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public class AccessControlOperationableInstanceRuleSource : RuleSourceBase
{
	public Guid OperationableInstanceId { get; private set; }

	public AccessControlOperationableInstanceRuleSource(RuleConfigurationSerializeData.AccessControlOperationableInstanceRuleSourceData accessControlOperationableInstanceRuleSourceData)
	{
		OperationableInstanceId = accessControlOperationableInstanceRuleSourceData.OperationableInstanceId;
	}

	public AccessControlOperationableInstanceRuleSource(Guid operationableInstanceId)
	{
		OperationableInstanceId = operationableInstanceId;
	}

	public override object ToDataClass()
	{
		return new RuleConfigurationSerializeData.AccessControlOperationableInstanceRuleSourceData
		{
			OperationableInstanceId = OperationableInstanceId
		};
	}
}
