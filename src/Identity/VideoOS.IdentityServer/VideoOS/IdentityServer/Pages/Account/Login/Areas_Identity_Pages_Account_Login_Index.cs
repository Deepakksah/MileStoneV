using System;
using System.Linq;
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
using VideoOS.IdentityServer.Areas.Identity.Pages.Account.Login;
using VideoOS.IdentityServer.Localization;
using VideoOS.IdentityServer.Oem;

namespace VideoOS.IdentityServer.Pages.Account.Login;

[RazorCompiledItemMetadata("Identifier", "/Areas/Identity/Pages/Account/Login/Index.cshtml")]
[CreateNewOnMetadataUpdate]
public sealed class Areas_Identity_Pages_Account_Login_Index : Page
{
	private static readonly TagHelperAttribute __tagHelperAttribute_0 = new TagHelperAttribute("class", new HtmlString("form-label"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_1 = new TagHelperAttribute("id", new HtmlString("Input_UserName"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_2 = new TagHelperAttribute("class", new HtmlString("form-input"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_3 = new TagHelperAttribute("autocomplete", new HtmlString("off"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_4 = new TagHelperAttribute("value", "", HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_5 = new TagHelperAttribute("class", new HtmlString("form"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_6 = new TagHelperAttribute("method", "post", HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_7 = new TagHelperAttribute("id", new HtmlString("account"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_8 = new TagHelperAttribute("asp-page", "/ExternalLogin/WindowsChallenge", HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_9 = new TagHelperAttribute("class", new HtmlString("button-secondary"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_10 = new TagHelperAttribute("draggable", new HtmlString("false"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_11 = new TagHelperAttribute("asp-page", "/ExternalLogin/Challenge", HtmlAttributeValueStyle.DoubleQuotes);

	private TagHelperExecutionContext __tagHelperExecutionContext;

	private TagHelperRunner __tagHelperRunner = new TagHelperRunner();

	private string __tagHelperStringValueBuffer;

	private TagHelperScopeManager __backed__tagHelperScopeManager;

	private FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;

	private RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;

	private LabelTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper;

	private InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;

	private AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;

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
	public IHtmlHelper<LoginModel> Html { get; private set; }

	public ViewDataDictionary<LoginModel> ViewData => (ViewDataDictionary<LoginModel>)(base.PageContext?.ViewData);

	public LoginModel Model => ViewData.Model;

	public override async Task ExecuteAsync()
	{
		ViewData["Title"] = Localizer.GetString("LoginTitle");
		WriteLiteral("\r\n<div class=\"brand-header\">\r\n    <div class=\"brand-logo\">\r\n        ");
		Write(Html.Raw(IOemResources.FormHeaderImage.Value));
		WriteLiteral("\r\n    </div>\r\n");
		if (Model.View.IsClientInitiatedFlow)
		{
			WriteLiteral("        <span class=\"brand-client\">");
			Write(Model.View.ClientName);
			WriteLiteral("</span>\r\n");
		}
		WriteLiteral("</div>\r\n<div class=\"page-section\">\r\n");
		if (Model.View.IsClientInitiatedFlow)
		{
			WriteLiteral("        <span class=\"page-title\">");
			Write(Localizer.GetString("ClientInitiatedLoginPageTitle"));
			WriteLiteral("</span>\r\n");
		}
		else
		{
			WriteLiteral("        <span class=\"page-title\">");
			Write(Localizer.GetString("DirectLoginPageTitle"));
			WriteLiteral("</span>\r\n");
		}
		WriteLiteral("\r\n");
		if (Model.UserSessionIsExpired || Model.CancelUserSession)
		{
			WriteLiteral("        <div class=\"message-bar info\">\r\n            <div class=\"message-bar-container\">\r\n                <div class=\"message-bar-body\">\r\n                    <div class=\"message-bar-content\">\r\n                        <div class=\"message-bar-icon\">\r\n                            <img");
			BeginWriteAttribute("src", " src=\"", 1197, "\"", 1272, 1);
			WriteAttributeValue("", 1203, Url.Content("~/images/messageBar/Status_Information_Mark_Color.svg"), 1203, 69, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(" alt=\"Information\" draggable=\"false\" />\r\n                        </div>\r\n");
			if (Model.UserSessionIsExpired)
			{
				WriteLiteral("                            <div class=\"message-bar-text\">");
				Write(Localizer.GetString("ExpiredLoginSessionMessage"));
				WriteLiteral("</div>\r\n");
			}
			else
			{
				WriteLiteral("                            <div class=\"message-bar-text\">");
				Write(Localizer.GetString("CancelLoginSessionMessage"));
				WriteLiteral("</div>\r\n");
			}
			WriteLiteral("                    </div>\r\n                </div>\r\n            </div>\r\n            <div class=\"message-bar-controls\">\r\n                <button class=\"message-bar-close\" title=\"Close\"></button>\r\n                <button class=\"message-bar-expand\" title=\"Expand\"></button>\r\n            </div>\r\n        </div>\r\n");
		}
		WriteLiteral("\r\n");
		if (!string.IsNullOrEmpty(Model.ErrorMessage))
		{
			WriteLiteral("        <div class=\"message-bar error\">\r\n            <div class=\"message-bar-container\">\r\n                <div class=\"message-bar-body\">\r\n                    <div class=\"message-bar-content\">\r\n                        <div class=\"message-bar-icon\">\r\n                            <img");
			BeginWriteAttribute("src", " src=\"", 2477, "\"", 2558, 1);
			WriteAttributeValue("", 2483, Url.Content("~/images/messageBar/Status_Error_Exclamation_Mark_Color.svg"), 2483, 75, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(" alt=\"Error\" draggable=\"false\" />\r\n                        </div>\r\n                        <div class=\"message-bar-text\">");
			Write(Model.ErrorMessage);
			WriteLiteral("</div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n            <div class=\"message-bar-controls\">\r\n                <button class=\"message-bar-close\" title=\"Close\"></button>\r\n                <button class=\"message-bar-expand\" title=\"Expand\"></button>\r\n            </div>\r\n        </div>\r\n");
		}
		WriteLiteral("\r\n");
		if (!Model.View.IsClientInitiatedFlow)
		{
			WriteLiteral("        <div class=\"page-instruction\">\r\n            <span class=\"page-instruction-heading\">");
			Write(Localizer.GetString("CurrentCredentialsTitle"));
			WriteLiteral("</span>\r\n            <span class=\"page-instruction-text\">");
			Write(Localizer.GetString("CurrentCredentialsSubtitle"));
			WriteLiteral("</span>\r\n        </div>\r\n");
		}
		WriteLiteral("\r\n    ");
		__tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", TagMode.StartTagAndEndTag, "3ea635fde839402390135aeb869b18e6e87438996990e55c12be0d414173cfa718965", async delegate
		{
			WriteLiteral("\r\n        <div class=\"form-group\">\r\n            <div class=\"form-label-wrapper\">\r\n                ");
			__tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", TagMode.StartTagAndEndTag, "3ea635fde839402390135aeb869b18e6e87438996990e55c12be0d414173cfa719352", async delegate
			{
			});
			__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<LabelTagHelper>();
			__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
			__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, (LoginModel __model) => __model.Input.UserName);
			__tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For, HtmlAttributeValueStyle.DoubleQuotes);
			await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
			if (!__tagHelperExecutionContext.Output.IsContentModified)
			{
				await __tagHelperExecutionContext.SetOutputContentAsync();
			}
			Write(__tagHelperExecutionContext.Output);
			__tagHelperExecutionContext = __tagHelperScopeManager.End();
			WriteLiteral("\r\n            </div>\r\n            <div");
			BeginWriteAttribute("class", " class=\"", 3646, "\"", 3728, 2);
			WriteAttributeValue("", 3654, "form-field", 3654, 10, isLiteral: true);
			WriteAttributeValue(" ", 3664, (!string.IsNullOrEmpty(Model.ErrorMessage)) ? "has-error" : "", 3665, 63, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(">\r\n                <img");
			BeginWriteAttribute("src", " src=\"", 3752, "\"", 3791, 1);
			WriteAttributeValue("", 3758, Url.Content("~/images/user.svg"), 3758, 33, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(" alt=\"User Icon\" class=\"form-icon-user\" draggable=\"false\">\r\n                ");
			__tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", TagMode.SelfClosing, "3ea635fde839402390135aeb869b18e6e87438996990e55c12be0d414173cfa722276", async delegate
			{
			});
			__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<InputTagHelper>();
			__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
			__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, (LoginModel __model) => __model.Input.UserName);
			__tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, HtmlAttributeValueStyle.DoubleQuotes);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
			await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
			if (!__tagHelperExecutionContext.Output.IsContentModified)
			{
				await __tagHelperExecutionContext.SetOutputContentAsync();
			}
			Write(__tagHelperExecutionContext.Output);
			__tagHelperExecutionContext = __tagHelperScopeManager.End();
			WriteLiteral("\r\n            </div>\r\n        </div>\r\n        <div class=\"form-group\">\r\n            <div class=\"form-label-wrapper\">\r\n                ");
			__tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", TagMode.StartTagAndEndTag, "3ea635fde839402390135aeb869b18e6e87438996990e55c12be0d414173cfa724288", async delegate
			{
			});
			__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<LabelTagHelper>();
			__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
			__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, (LoginModel __model) => __model.Input.Password);
			__tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For, HtmlAttributeValueStyle.DoubleQuotes);
			await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
			if (!__tagHelperExecutionContext.Output.IsContentModified)
			{
				await __tagHelperExecutionContext.SetOutputContentAsync();
			}
			Write(__tagHelperExecutionContext.Output);
			__tagHelperExecutionContext = __tagHelperScopeManager.End();
			WriteLiteral("\r\n            </div>\r\n            <div");
			BeginWriteAttribute("class", " class=\"", 4191, "\"", 4273, 2);
			WriteAttributeValue("", 4199, "form-field", 4199, 10, isLiteral: true);
			WriteAttributeValue(" ", 4209, (!string.IsNullOrEmpty(Model.ErrorMessage)) ? "has-error" : "", 4210, 63, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(">\r\n                <img");
			BeginWriteAttribute("src", " src=\"", 4297, "\"", 4340, 1);
			WriteAttributeValue("", 4303, Url.Content("~/images/Lock_off.svg"), 4303, 37, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(" alt=\"Lock Icon\" class=\"form-icon-password\" draggable=\"false\">\r\n                ");
			__tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", TagMode.SelfClosing, "3ea635fde839402390135aeb869b18e6e87438996990e55c12be0d414173cfa727220", async delegate
			{
			});
			__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<InputTagHelper>();
			__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
			__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, (LoginModel __model) => __model.Input.Password);
			__tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, HtmlAttributeValueStyle.DoubleQuotes);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
			__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.Value = (string)__tagHelperAttribute_4.Value;
			__tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
			await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
			if (!__tagHelperExecutionContext.Output.IsContentModified)
			{
				await __tagHelperExecutionContext.SetOutputContentAsync();
			}
			Write(__tagHelperExecutionContext.Output);
			__tagHelperExecutionContext = __tagHelperScopeManager.End();
			WriteLiteral("\r\n                <div class=\"form-icon-button\">\r\n                    <img");
			BeginWriteAttribute("src", " src=\"", 4576, "\"", 4620, 1);
			WriteAttributeValue("", 4582, Url.Content("~/images/Mask_Hide.svg"), 4582, 38, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral("\r\n                         alt=\"Show Password\"\r\n                         class=\"form-password-toggle\"\r\n                         draggable=\"false\"\r\n                         data-hide=\"");
			Write(Url.Content("~/images/Mask_Hide.svg"));
			WriteLiteral("\"\r\n                         data-show=\"");
			Write(Url.Content("~/images/Mask_Show.svg"));
			WriteLiteral("\">\r\n                </div>\r\n            </div>\r\n        </div>\r\n\r\n        <div");
			BeginWriteAttribute("class", " class=\"", 4997, "\"", 5089, 2);
			WriteAttributeValue("", 5005, "form-actions", 5005, 12, isLiteral: true);
			WriteAttributeValue(" ", 5017, (!Model.View.IsClientInitiatedFlow) ? "form-actions--auto-height" : "", 5018, 71, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(">\r\n");
			if (Model.View.IsClientInitiatedFlow)
			{
				WriteLiteral("                <button type=\"submit\" class=\"button-primary\">\r\n                    <span class=\"button-text\">");
				Write(Localizer.GetString("ButtonSignin"));
				WriteLiteral("</span>\r\n                </button>\r\n");
			}
			else
			{
				WriteLiteral("                <button type=\"submit\" class=\"button-primary\">\r\n                    <span class=\"button-text\">");
				Write(Localizer.GetString("ButtonNext"));
				WriteLiteral("</span>\r\n                </button>\r\n");
			}
			WriteLiteral("        </div>\r\n\r\n    ");
		});
		__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<FormTagHelper>();
		__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
		__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<RenderAtEndOfFormTagHelper>();
		__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
		__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
		__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_6.Value;
		__tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
		__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_7);
		await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
		if (!__tagHelperExecutionContext.Output.IsContentModified)
		{
			await __tagHelperExecutionContext.SetOutputContentAsync();
		}
		Write(__tagHelperExecutionContext.Output);
		__tagHelperExecutionContext = __tagHelperScopeManager.End();
		WriteLiteral("\r\n");
		if (!Model.View.IsClientInitiatedFlow)
		{
			WriteLiteral("        <span class=\"login-additional-info\">\r\n            ");
			Write(Localizer.GetString("Clarification"));
			WriteLiteral("\r\n        </span>\r\n");
		}
		if (Model.View.ExternalProviders.Any())
		{
			WriteLiteral("        <div class=\"login-alternative-methods\">\r\n            <div class=\"login-divider-group\">\r\n                <div class=\"login-divider\"></div>\r\n                <span class=\"login-divider-text\" draggable=\"false\">or</span>\r\n                <div class=\"login-divider\"></div>\r\n            </div>\r\n            <div class=\"login-external-providers\">\r\n");
			foreach (LoginViewModel.ExternalProvider provider in Model.View.ExternalProviders)
			{
				if (provider.AuthenticationScheme.Equals("Windows", StringComparison.OrdinalIgnoreCase))
				{
					WriteLiteral("                        ");
					__tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", TagMode.StartTagAndEndTag, "3ea635fde839402390135aeb869b18e6e87438996990e55c12be0d414173cfa736949", async delegate
					{
						WriteLiteral("\r\n                            <span class=\"button-text\">");
						Write(Localizer.GetString("ButtonContinueWith", "Windows").Value);
						WriteLiteral("</span>\r\n                        ");
					});
					__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<AnchorTagHelper>();
					__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
					__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_8.Value;
					__tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_8);
					if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
					{
						throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-returnUrl", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
					}
					BeginWriteTagHelperAttribute();
					WriteLiteral(Model.Input.ReturnUrl);
					__tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
					__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["returnUrl"] = __tagHelperStringValueBuffer;
					__tagHelperExecutionContext.AddTagHelperAttribute("asp-route-returnUrl", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["returnUrl"], HtmlAttributeValueStyle.DoubleQuotes);
					__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_9);
					__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_10);
					await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
					if (!__tagHelperExecutionContext.Output.IsContentModified)
					{
						await __tagHelperExecutionContext.SetOutputContentAsync();
					}
					Write(__tagHelperExecutionContext.Output);
					__tagHelperExecutionContext = __tagHelperScopeManager.End();
					WriteLiteral("\r\n");
					continue;
				}
				WriteLiteral("                        ");
				__tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", TagMode.StartTagAndEndTag, "3ea635fde839402390135aeb869b18e6e87438996990e55c12be0d414173cfa740184", async delegate
				{
					WriteLiteral("\r\n                            <span class=\"button-text\">");
					Write(Localizer.GetString("ButtonContinueWith", provider.DisplayName));
					WriteLiteral("</span>\r\n                        ");
				});
				__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<AnchorTagHelper>();
				__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
				__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_11.Value;
				__tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_11);
				if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
				{
					throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-scheme", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
				}
				BeginWriteTagHelperAttribute();
				WriteLiteral(provider.AuthenticationScheme);
				__tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
				__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["scheme"] = __tagHelperStringValueBuffer;
				__tagHelperExecutionContext.AddTagHelperAttribute("asp-route-scheme", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["scheme"], HtmlAttributeValueStyle.DoubleQuotes);
				BeginWriteTagHelperAttribute();
				WriteLiteral(Model.Input.ReturnUrl);
				__tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
				__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["returnUrl"] = __tagHelperStringValueBuffer;
				__tagHelperExecutionContext.AddTagHelperAttribute("asp-route-returnUrl", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["returnUrl"], HtmlAttributeValueStyle.DoubleQuotes);
				__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_9);
				__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_10);
				await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
				if (!__tagHelperExecutionContext.Output.IsContentModified)
				{
					await __tagHelperExecutionContext.SetOutputContentAsync();
				}
				Write(__tagHelperExecutionContext.Output);
				__tagHelperExecutionContext = __tagHelperScopeManager.End();
				WriteLiteral("\r\n");
			}
			WriteLiteral("            </div>\r\n        </div>\r\n");
		}
		WriteLiteral("</div>");
	}
}
