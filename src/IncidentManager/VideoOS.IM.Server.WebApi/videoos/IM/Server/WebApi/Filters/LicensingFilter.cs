using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using VideoOS.IM.Server.Service.Licenses.Validators;
using VideoOS.IM.Server.Service.OEM;

namespace VideoOS.IM.Server.WebApi.Filters;

public class LicensingFilter : AuthorizationFilterAttribute
{
	public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
	{
		if (!LicenseValidationStatusService.Instance.IsLicenseValid)
		{
			HttpResponseMessage val = actionContext.Request.CreateResponse(HttpStatusCode.PaymentRequired);
			val.ReasonPhrase = "The current license is not compatible with '" + OemResourceService.Instance.IncidentManagerApplicationNameLong + "'. Check the logs for more details.";
			actionContext.Response = val;
		}
		else
		{
			await base.OnAuthorizationAsync(actionContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
