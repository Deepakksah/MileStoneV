using System;

namespace VideoOS.IM.Server.Repository.EntityFramework.Properties;

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
