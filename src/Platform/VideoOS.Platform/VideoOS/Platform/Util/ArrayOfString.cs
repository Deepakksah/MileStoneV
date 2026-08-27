using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Util;

[CollectionDataContract(Namespace = "http://videoos.net/2/XProtectCSServiceRegistration")]
public class ArrayOfString : List<string>
{
	public ArrayOfString()
	{
	}

	public ArrayOfString(string[] strings)
	{
		foreach (string item in strings)
		{
			Add(item);
		}
	}
}
