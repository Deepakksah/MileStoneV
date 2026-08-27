using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SoundDataItem", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class SoundDataItem : SoundConfigurationItem
{
	private string SoundXMLField;

	[DataMember]
	public string SoundXML
	{
		get
		{
			return SoundXMLField;
		}
		set
		{
			SoundXMLField = value;
		}
	}
}
