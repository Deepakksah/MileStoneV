using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class Sequences
{
	public const string SequencesName = "Sequence";

	[JsonProperty("Sequence")]
	[XmlElement(ElementName = "Sequence")]
	public List<Sequence> SequenceList { get; set; }

	public Sequences()
	{
		InternalInit();
	}

	private void InternalInit()
	{
		SequenceList = new List<Sequence>();
	}
}
