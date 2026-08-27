using System;
using System.Globalization;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server;

public class SendEventRestResource
{
	private string _restResourcePath;

	public SendEventRestResource(Guid id, string itemType)
	{
		string arg = new NameConvert().ItemTypeToResourcePlural(itemType);
		_restResourcePath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", arg, id);
	}

	public SendEventRestResource(Guid id, string itemType, Guid parentId, string parentItemType)
	{
		NameConvert nameConvert = new NameConvert();
		string text = nameConvert.ItemTypeToResourcePlural(itemType);
		string text2 = nameConvert.ItemTypeToResourcePlural(parentItemType);
		_restResourcePath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}/{3}", text2, parentId, text, id);
	}

	public override string ToString()
	{
		return _restResourcePath;
	}
}
