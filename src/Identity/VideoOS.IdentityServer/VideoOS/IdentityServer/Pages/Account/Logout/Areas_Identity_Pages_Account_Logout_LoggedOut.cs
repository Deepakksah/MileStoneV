using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Hosting;
using VideoOS.IdentityServer.Areas.Identity.Pages.Account.Logout;
using VideoOS.IdentityServer.Localization;
using VideoOS.IdentityServer.Oem;

namespace VideoOS.IdentityServer.Pages.Account.Logout;

[RazorCompiledItemMetadata("Identifier", "/Areas/Identity/Pages/Account/Logout/LoggedOut.cshtml")]
[CreateNewOnMetadataUpdate]
public sealed class Areas_Identity_Pages_Account_Logout_LoggedOut : Page
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
	public IHtmlHelper<LoggedOutModel> Html { get; private set; }

	public ViewDataDictionary<LoggedOutModel> ViewData => (ViewDataDictionary<LoggedOutModel>)(base.PageContext?.ViewData);

	public LoggedOutModel Model => ViewData.Model;

	public override async Task ExecuteAsync()
	{
		ViewData["Title"] = Localizer.GetString("LoggedOutTitle");
		WriteLiteral("\r\n<div class=\"brand-header\">\r\n    <div class=\"brand-logo\">\r\n        ");
		Write(Html.Raw(IOemResources.FormHeaderImage.Value));
		WriteLiteral("\r\n    </div>\r\n</div>\r\n\r\n<div class=\"page-section\">\r\n    <div class=\"page-status-icon\">\r\n        <img");
		BeginWriteAttribute("src", " src=\"", 469, "\"", 540, 1);
		WriteAttributeValue("", 475, Url.Content("~/images/Status_Confirmation_Check_Mark_Color.svg"), 475, 65, isLiteral: false);
		EndWriteAttribute();
		WriteLiteral(" alt=\"Information\" draggable=\"false\" />\r\n                  </div>\r\n    <span class=\"page-title\">");
		Write(Localizer.GetString("MessageLoggedOut"));
		WriteLiteral("</span>\r\n\r\n");
		if (Model.View.PostLogoutRedirectUri != null)
		{
			WriteLiteral("        <div class=\"page-action-group page-action-group--center\">\r\n            <div class=\"page-action-button page-action-button--large\">\r\n                <a");
			BeginWriteAttribute("href", " href=\"", 927, "\"", 967, 1);
			WriteAttributeValue("", 934, Model.View.PostLogoutRedirectUri, 934, 33, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(" class=\"button-primary\">\r\n                    <span class=\"button-text\" draggable=\"false\">\r\n                        ");
			Write(Localizer.GetString("ButtonReturnTo", Model.View.ClientName));
			WriteLiteral("\r\n                    </span>\r\n                </a>\r\n            </div>\r\n        </div>\r\n");
		}
		WriteLiteral("</div>");
	}
}
