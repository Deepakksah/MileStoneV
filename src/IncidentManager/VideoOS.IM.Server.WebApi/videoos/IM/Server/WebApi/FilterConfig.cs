using System.Web.Mvc;

namespace VideoOS.IM.Server.WebApi;

public class FilterConfig
{
	public static void RegisterGlobalFilters(GlobalFilterCollection filters)
	{
		filters.Add(new HandleErrorAttribute());
	}
}
