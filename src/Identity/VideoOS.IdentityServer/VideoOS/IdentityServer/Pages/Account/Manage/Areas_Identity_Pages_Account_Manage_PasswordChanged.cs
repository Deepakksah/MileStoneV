using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Hosting;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using VideoOS.IdentityServer.Areas.Identity.Pages.Account.Manage;
using VideoOS.IdentityServer.Localization;
using VideoOS.IdentityServer.Oem;

namespace VideoOS.IdentityServer.Pages.Account.Manage;

[RazorCompiledItemMetadata("Identifier", "/Areas/Identity/Pages/Account/Manage/PasswordChanged.cshtml")]
[CreateNewOnMetadataUpdate]
public sealed class Areas_Identity_Pages_Account_Manage_PasswordChanged : Page
{
	private static readonly TagHelperAttribute __tagHelperAttribute_0 = new TagHelperAttribute("class", new HtmlString("form"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_1 = new TagHelperAttribute("method", "get", HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_2 = new TagHelperAttribute("asp-page", "/Account/Login", HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_3 = new TagHelperAttribute("id", new HtmlString("password-changed-form"), HtmlAttributeValueStyle.DoubleQuotes);

	private TagHelperExecutionContext __tagHelperExecutionContext;

	private TagHelperRunner __tagHelperRunner = new TagHelperRunner();

	private string __tagHelperStringValueBuffer;

	private TagHelperScopeManager __backed__tagHelperScopeManager;

	private FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;

	private RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;

	private TagHelperScopeManager __tagHelperScopeManager
	{
		get
		{
			if (__backed__tagHelperScopeManager == null)
			{
				__backed__tagHelperScopeManager = new TagHelperScopeManager(base.StartTagHelperWritingScope, base.EndTagHelperWritingScope);
			}
			return __backed__tagHelperScopeManager;
		}
	}

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
	public IHtmlHelper<PasswordChangedModel> Html { get; private set; }

	public ViewDataDictionary<PasswordChangedModel> ViewData => (ViewDataDictionary<PasswordChangedModel>)(base.PageContext?.ViewData);

	public PasswordChangedModel Model => ViewData.Model;

	public override async Task ExecuteAsync()
	{
		ViewData["Title"] = Localizer.GetString("PasswordChangedTitle");
		WriteLiteral("\r\n<div class=\"brand-header\">\r\n    <div class=\"brand-logo\">\r\n        ");
		Write(Html.Raw(IOemResources.FormHeaderImage.Value));
		WriteLiteral("\r\n    </div>\r\n</div>\r\n\r\n<div class=\"page-section\">\r\n    <div class=\"page-status-icon\">\r\n        <img");
		BeginWriteAttribute("src", " src=\"", 481, "\"", 552, 1);
		WriteAttributeValue("", 487, Url.Content("~/images/Status_Confirmation_Check_Mark_Color.svg"), 487, 65, isLiteral: false);
		EndWriteAttribute();
		WriteLiteral(" alt=\"Information\" draggable=\"false\" />\r\n    </div>\r\n    <span class=\"page-title\">");
		Write(Localizer.GetString("PasswordChangedPageTitle"));
		WriteLiteral("</span>\r\n\r\n    ");
		__tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", TagMode.StartTagAndEndTag, "da564f4d40e292e0c369087575822a9e218619fceb936379b294c9b8428ddf397387", async delegate
		{
			WriteLiteral("\r\n        <div class=\"page-action-group page-action-group--center\">\r\n            <div class=\"page-action-button page-action-button--large\">\r\n                <button type=\"submit\" class=\"button-primary\">\r\n                    <span class=\"button-text\">");
			Write(Localizer.GetString("ButtonContinueToLogin"));
			WriteLiteral("</span>\r\n                </button>\r\n            </div>\r\n        </div>\r\n    ");
		});
		__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<FormTagHelper>();
		__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
		__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<RenderAtEndOfFormTagHelper>();
		__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
		__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
		__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
		__tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
		__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Page = (string)__tagHelperAttribute_2.Value;
		__tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
		__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
		await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
		if (!__tagHelperExecutionContext.Output.IsContentModified)
		{
			await __tagHelperExecutionContext.SetOutputContentAsync();
		}
		Write(__tagHelperExecutionContext.Output);
		__tagHelperExecutionContext = __tagHelperScopeManager.End();
		WriteLiteral("\r\n</div>");
	}
}
