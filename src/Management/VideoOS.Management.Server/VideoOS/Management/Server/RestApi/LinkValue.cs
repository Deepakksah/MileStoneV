using System;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

public class LinkValue
{
	public string Relation { get; set; }

	public string ResourcePlural { get; set; }

	public string Id { get; set; }

	public INameConvert NameConvert { get; set; } = new NameConvert();

	public LinkValue()
	{
	}

	public LinkValue(string relation, string itemType, string id)
	{
		if (string.IsNullOrEmpty(relation))
		{
			throw new ArgumentException("Must have a relation", "relation");
		}
		if (itemType == "System" && string.IsNullOrEmpty(id))
		{
			id = RestApiCache.SiteId.ToString();
		}
		if (string.IsNullOrEmpty(id))
		{
			throw new ArgumentException("Empty id", "id");
		}
		Relation = relation;
		ResourcePlural = NameConvert.ItemTypeToResourcePlural(itemType);
		Id = CorpJsonBuilder.IdToLower(id);
	}

	public LinkValue(string itemType, string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			throw new ArgumentException("Empty id", "id");
		}
		ResourcePlural = ((itemType == "any") ? itemType : NameConvert.ItemTypeToResourcePlural(itemType));
		Id = CorpJsonBuilder.IdToLower(id);
	}

	public string Serialize()
	{
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		jsonBuilder.AppendKeyString("type", ResourcePlural);
		jsonBuilder.AppendKeyString("id", Id);
		return jsonBuilder.ToJson();
	}
}
