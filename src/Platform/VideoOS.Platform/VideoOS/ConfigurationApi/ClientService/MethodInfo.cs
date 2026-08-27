using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationApi.ClientService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MethodInfo", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.ConfigurationAPI")]
public class MethodInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private byte[] BitmapField;

	private string DisplayNameField;

	private string MethodIdField;

	private string TranslationIdField;

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
	public byte[] Bitmap
	{
		get
		{
			return BitmapField;
		}
		set
		{
			BitmapField = value;
		}
	}

	[DataMember]
	public string DisplayName
	{
		get
		{
			return DisplayNameField;
		}
		set
		{
			DisplayNameField = value;
		}
	}

	[DataMember]
	public string MethodId
	{
		get
		{
			return MethodIdField;
		}
		set
		{
			MethodIdField = value;
		}
	}

	[DataMember]
	public string TranslationId
	{
		get
		{
			return TranslationIdField;
		}
		set
		{
			TranslationIdField = value;
		}
	}
}
