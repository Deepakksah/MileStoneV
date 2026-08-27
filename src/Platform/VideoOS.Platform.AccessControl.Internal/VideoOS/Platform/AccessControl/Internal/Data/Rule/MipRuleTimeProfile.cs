using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public class MipRuleTimeProfile : RuleTimeProfileBase
{
	public FQID ItemId { get; private set; }

	public MipRuleTimeProfile(RuleConfigurationSerializeData.MipRuleTimeProfileData mipRuleTimeProfileData)
	{
		ItemId = mipRuleTimeProfileData.ItemId;
	}

	public MipRuleTimeProfile(FQID timeProfileId)
	{
		ItemId = timeProfileId;
	}

	public override object ToDataClass()
	{
		return new RuleConfigurationSerializeData.MipRuleTimeProfileData
		{
			ItemId = ItemId
		};
	}
}
