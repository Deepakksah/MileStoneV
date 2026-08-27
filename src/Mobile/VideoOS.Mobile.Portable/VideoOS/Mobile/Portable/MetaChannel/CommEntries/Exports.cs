using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class Exports
{
	public const string ExportName = "Export";

	[JsonProperty("Export")]
	[XmlElement(ElementName = "Export")]
	public List<Export> ExportsList { get; set; }

	public Exports()
	{
		InternalInit();
	}

	public Exports(IEnumerable<object> datas)
	{
		InternalInit();
		InitFrom(datas);
	}

	private void InternalInit()
	{
		ExportsList = new List<Export>();
	}

	public void InitFrom(IEnumerable<object> datas)
	{
		foreach (IExportEntry data in datas)
		{
			ExportsList.Add(new Export(data));
		}
	}
}
