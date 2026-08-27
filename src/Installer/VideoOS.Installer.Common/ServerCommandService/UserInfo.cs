using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "UserInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class UserInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string nameField;

	private string sidField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public string name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	[DataMember]
	public string sid
	{
		get
		{
			return sidField;
		}
		set
		{
			sidField = value;
		}
	}
}
