using System.Collections.ObjectModel;
using VideoOS.Platform.JsonHandling;

namespace VideoOS.Management.Server.RestApi;

public class LinkList
{
	public Collection<LinkValue> Links { get; } = new Collection<LinkValue>();

	public string Serialize()
	{
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		for (int i = 0; i < Links.Count; i++)
		{
			LinkValue linkValue = Links[i];
			jsonBuilder.AppendKeyValue(linkValue.Relation, linkValue.Serialize());
		}
		return jsonBuilder.ToJson();
	}
}
