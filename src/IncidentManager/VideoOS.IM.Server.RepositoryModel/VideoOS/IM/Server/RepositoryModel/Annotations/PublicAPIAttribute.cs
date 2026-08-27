using System;

namespace VideoOS.IM.Server.RepositoryModel.Annotations;

[MeansImplicitUse]
public sealed class PublicAPIAttribute : Attribute
{
	public PublicAPIAttribute()
	{
	}

	public PublicAPIAttribute(string comment)
	{
	}
}
