using System;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class Site
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public static Site QueryHomeSite(IDataComponentFactory componentFactory)
	{
		using ISiteConfigDataComponent siteConfigDataComponent = componentFactory.CreateSiteConfigDataComponent();
		return siteConfigDataComponent.QueryHomeSite();
	}
}
