using System.Collections.ObjectModel;

namespace VideoOS.Platform.Proxy.RestApi.ConfigApiRelated;

public class LinkList : IRestSerializer
{
	public Collection<LinkValue> Links { get; } = new Collection<LinkValue>();

	public string Serialize(bool noData)
	{
		ConfigApiRelatedJsonBuilder configApiRelatedJsonBuilder = new ConfigApiRelatedJsonBuilder(array: false);
		for (int i = 0; i < Links.Count; i++)
		{
			LinkValue linkValue = Links[i];
			configApiRelatedJsonBuilder.AppendKeyValue(linkValue.Relation, linkValue.Serialize(noData));
		}
		return configApiRelatedJsonBuilder.ToJson();
	}
}
