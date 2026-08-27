namespace VideoOS.Platform.UI.Automation.Serialization;

public class AutomationObjectSerializationOptions
{
	public bool IncludeHashCode { get; set; } = true;

	public bool IncludeTypeName { get; set; }

	public bool IncludeAssemblyQualifiedTypeName { get; set; }

	public bool IncludeToStringValue { get; set; }

	public int ToStringValueMaxCharacterCount { get; set; } = 100;
}
