using System;

namespace VideoOS.IM.Server.RepositoryModel.Annotations;

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
