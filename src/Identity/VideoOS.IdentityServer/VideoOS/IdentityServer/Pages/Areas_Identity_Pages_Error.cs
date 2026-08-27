using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Hosting;
using VideoOS.IdentityServer.Areas.Identity.Pages;
using VideoOS.IdentityServer.Localization;
using VideoOS.IdentityServer.Oem;

namespace VideoOS.IdentityServer.Pages;

[RazorCompiledItemMetadata("RouteTemplate", "/Error")]
[RazorCompiledItemMetadata("Identifier", "/Areas/Identity/Pages/Error.cshtml")]
[CreateNewOnMetadataUpdate]
public sealed class Areas_Identity_Pages_Error : Page
{
	[RazorInject]
	public IOemResources IOemResources { get; private set; }

	[RazorInject]
	public StringLocalizer Localizer { get; private set; }

	[RazorInject]
	public IModelExpressionProvider ModelExpressionProvider { get; private set; }

	[RazorInject]
	public IUrlHelper Url { get; private set; }

	[RazorInject]
	public IViewComponentHelper Component { get; private set; }

	[RazorInject]
	public IJsonHelper Json { get; private set; }

	[RazorInject]
	public IHtmlHelper<ErrorModel> Html { get; private set; }

	public ViewDataDictionary<ErrorModel> ViewData => (ViewDataDictionary<ErrorModel>)(base.PageContext?.ViewData);

	public ErrorModel Model => ViewData.Model;

	public override async Task ExecuteAsync()
	{
		ViewData["Title"] = Localizer.GetString("UnexpectedError");
		WriteLiteral("\r\n<div class=\"brand-header\">\r\n    <div class=\"brand-logo\">\r\n        ");
		Write(Html.Raw(IOemResources.FormHeaderImage.Value));
		WriteLiteral("\r\n    </div>\r\n</div>\r\n\r\n<div class=\"page-section\">\r\n    <div class=\"page-status-icon\">\r\n        <img");
		BeginWriteAttribute("src", " src=\"", 460, "\"", 530, 1);
		WriteAttributeValue("", 466, Url.Content("~/images/Status_Error_Exclamation_Mark_Color.svg"), 466, 64, isLiteral: false);
		EndWriteAttribute();
		WriteLiteral(" alt=\"Information\" draggable=\"false\" />\r\n    </div>\r\n    <span class=\"page-title\">");
		Write(Localizer.GetString(Model.ErrorMessageCode));
		WriteLiteral("</span>\r\n\r\n");
		if (Model.ShowFooter)
		{
			WriteLiteral("        <div class=\"page-instruction page-instruction--center\">\r\n            <div class=\"page-instruction-text page-instruction-text--centered\">\r\n                ");
			Write(Localizer.GetString("UnexpectedErrorFooter"));
			WriteLiteral("\r\n            </div>\r\n        </div>\r\n");
		}
		WriteLiteral("</div>");
	}
}
