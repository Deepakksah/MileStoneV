using System;

namespace VideoOS.IM.Server.Logger;

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
