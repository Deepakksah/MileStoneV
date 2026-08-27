using System.Security.Principal;

namespace VideoOS.Common.Controls;

public class DirectoryObject
{
	public string ClassName { get; public set; }

	public string Name { get; public set; }

	public string UPN { get; public set; }

	public string Path { get; public set; }

	public byte[] BinarySid { get; public set; }

	public string Sid
	{
		get
		{
			if (BinarySid != null)
			{
				return new SecurityIdentifier(BinarySid, 0).Value;
			}
			return null;
		}
	}

	public DirectoryObject(string className, string name, string upn, string path)
	{
		ClassName = className;
		Name = name;
		UPN = upn;
		Path = path;
	}
}
