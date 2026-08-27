using System;

namespace VideoOS.Management.VmoClient;

public class CustomViewItem
{
	public Guid Id => new Guid(ObjectProperties.GetPropertyValue<string>("Id"));

	public string DefinitionXml
	{
		get
		{
			return ObjectProperties.GetPropertyValue<string>("DefinitionXml");
		}
		set
		{
			ObjectProperties.SetPropertyValue("DefinitionXml", value);
		}
	}

	public CustomPropertyCollection CustomProperties { get; private set; }

	public VmoPropertyCollection ObjectProperties { get; private set; }

	public CustomViewItem(Guid id)
	{
		ObjectProperties = new VmoPropertyCollection
		{
			{
				"Id",
				id.ToString()
			},
			{
				"DefinitionXml",
				string.Empty
			}
		};
		CustomProperties = new CustomPropertyCollection(null, id.ToString());
	}
}
