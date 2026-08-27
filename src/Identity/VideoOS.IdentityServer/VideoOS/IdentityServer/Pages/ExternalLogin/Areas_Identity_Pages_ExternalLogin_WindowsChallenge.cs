using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Hosting;
using VideoOS.IdentityServer.Areas.Identity.Pages.ExternalLogin;

namespace VideoOS.IdentityServer.Pages.ExternalLogin;

[RazorCompiledItemMetadata("Identifier", "/Areas/Identity/Pages/ExternalLogin/WindowsChallenge.cshtml")]
[CreateNewOnMetadataUpdate]
public sealed class Areas_Identity_Pages_ExternalLogin_WindowsChallenge : Page
{
	[RazorInject]
	public IModelExpressionProvider ModelExpressionProvider { get; private set; }

	[RazorInject]
	public IUrlHelper Url { get; private set; }

	[RazorInject]
	public IViewComponentHelper Component { get; private set; }

	[RazorInject]
	public IJsonHelper Json { get; private set; }

	[RazorInject]
	public IHtmlHelper<WindowsChallengeModel> Html { get; private set; }

	public ViewDataDictionary<WindowsChallengeModel> ViewData => (ViewDataDictionary<WindowsChallengeModel>)(base.PageContext?.ViewData);

	public WindowsChallengeModel Model => ViewData.Model;

	public override async Task ExecuteAsync()
	{
	}
}
