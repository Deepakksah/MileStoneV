using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Hosting;
using VideoOS.IdentityServer.Areas.Identity.Pages.Account;
using VideoOS.IdentityServer.Localization;
using VideoOS.IdentityServer.Oem;

namespace VideoOS.IdentityServer.Pages.Account;

[RazorCompiledItemMetadata("Identifier", "/Areas/Identity/Pages/Account/LoginSuccess.cshtml")]
[CreateNewOnMetadataUpdate]
public sealed class Areas_Identity_Pages_Account_LoginSuccess : Page
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
	public IHtmlHelper<LoginSuccessModel> Html { get; private set; }

	public ViewDataDictionary<LoginSuccessModel> ViewData => (ViewDataDictionary<LoginSuccessModel>)(base.PageContext?.ViewData);

	public LoginSuccessModel Model => ViewData.Model;

	public override async Task ExecuteAsync()
	{
		ViewData["Title"] = Localizer.GetString("TitleAuthentication");
		WriteLiteral("\r\n<div class=\"brand-header\">\r\n    <div class=\"brand-logo\">\r\n        ");
		Write(Html.Raw(IOemResources.FormHeaderImage.Value));
		WriteLiteral("\r\n    </div>\r\n</div>\r\n\r\n<div class=\"page-section\">\r\n    <div class=\"page-status-icon\">\r\n        <img");
		BeginWriteAttribute("src", " src=\"", 470, "\"", 541, 1);
		WriteAttributeValue("", 476, Url.Content("~/images/Status_Confirmation_Check_Mark_Color.svg"), 476, 65, isLiteral: false);
		EndWriteAttribute();
		WriteLiteral(" alt=\"Information\" draggable=\"false\" />\r\n                  </div>\r\n    <span class=\"page-title\">");
		Write(Localizer.GetString("SuccessAuthentication"));
		WriteLiteral("</span>\r\n    <div class=\"page-instruction page-instruction--center page-extra-top-spacing\">\r\n        <span class=\"page-instruction-text\">");
		Write(Localizer.GetString("CloseWindow"));
		WriteLiteral("</span>\r\n    </div>\r\n</div>");
	}
}
