using System;

namespace VideoOS.IM.Server.Registrars.Properties;

[AttributeUsage(AttributeTargets.Parameter)]
public class PathReferenceAttribute : Attribute
{
	[UsedImplicitly]
	public string BasePath { get; private set; }

	public PathReferenceAttribute()
	{
	}

	[UsedImplicitly]
	public PathReferenceAttribute([PathReference] string basePath)
	{
		BasePath = basePath;
	}
}
