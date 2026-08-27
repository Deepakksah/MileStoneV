using System;

namespace VideoOS.IM.Server.Service.Annotations;

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
