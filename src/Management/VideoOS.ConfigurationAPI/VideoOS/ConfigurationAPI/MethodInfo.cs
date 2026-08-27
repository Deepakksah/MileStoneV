using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI;

[DataContract]
public class MethodInfo
{
	[DataMember]
	public string MethodId { get; set; }

	[DataMember]
	public string DisplayName { get; set; }

	[DataMember]
	public string TranslationId { get; set; }

	[DataMember]
	public byte[] Bitmap { get; set; }

	[DataMember]
	public string ToolTipTranslationId { get; set; }

	public MethodInfo()
	{
	}

	public MethodInfo(string methodId, string displayName)
	{
		MethodId = methodId;
		DisplayName = displayName;
	}
}
