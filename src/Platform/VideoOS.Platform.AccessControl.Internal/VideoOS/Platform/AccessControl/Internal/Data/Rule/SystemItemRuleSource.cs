using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public class SystemItemRuleSource : RuleSourceBase
{
	public FQID ItemId { get; private set; }

	public SystemItemRuleSource(RuleConfigurationSerializeData.SystemItemRuleSourceData systemItemRuleSourceData)
	{
		ItemId = systemItemRuleSourceData.ItemId;
	}

	public SystemItemRuleSource(FQID itemId)
	{
		ItemId = itemId;
	}

	public override object ToDataClass()
	{
		return new RuleConfigurationSerializeData.SystemItemRuleSourceData
		{
			ItemId = ItemId
		};
	}
}
