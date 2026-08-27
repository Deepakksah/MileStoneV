using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

public class ValueTypeInformation
{
	public string TranslationId { get; set; }

	public string Name { get; set; }

	public string Value { get; set; }

	public ValueTypeInformation(ValueTypeInfo valueTypeInfo)
	{
		TranslationId = valueTypeInfo.TranslationId;
		Name = valueTypeInfo.Name;
		Value = valueTypeInfo.Value;
	}
}
