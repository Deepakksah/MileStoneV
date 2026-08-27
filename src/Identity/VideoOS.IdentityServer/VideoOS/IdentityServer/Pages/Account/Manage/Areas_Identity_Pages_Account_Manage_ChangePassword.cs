using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

[RazorCompiledItemMetadata("Identifier", "/Areas/Identity/Pages/Account/Manage/ChangePassword.cshtml")]
[CreateNewOnMetadataUpdate]
public sealed class Areas_Identity_Pages_Account_Manage_ChangePassword : Page
{
	private static readonly TagHelperAttribute __tagHelperAttribute_0 = new TagHelperAttribute("class", new HtmlString("form-label"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_1 = new TagHelperAttribute("id", new HtmlString("Input_CurrentPassword"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_2 = new TagHelperAttribute("class", new HtmlString("form-input"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_3 = new TagHelperAttribute("autocomplete", new HtmlString("off"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_4 = new TagHelperAttribute("class", new HtmlString("inline-error-text"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_5 = new TagHelperAttribute("id", new HtmlString("Input_NewPassword"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_6 = new TagHelperAttribute("id", new HtmlString("Input_ConfirmPassword"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_7 = new TagHelperAttribute("asp-page-handler", "Cancel", HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_8 = new TagHelperAttribute("class", new HtmlString("button-secondary"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_9 = new TagHelperAttribute("class", new HtmlString("form"), HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_10 = new TagHelperAttribute("method", "post", HtmlAttributeValueStyle.DoubleQuotes);

	private static readonly TagHelperAttribute __tagHelperAttribute_11 = new TagHelperAttribute("id", new HtmlString("change-password-form"), HtmlAttributeValueStyle.DoubleQuotes);

	private TagHelperExecutionContext __tagHelperExecutionContext;

	private TagHelperRunner __tagHelperRunner = new TagHelperRunner();

	private string __tagHelperStringValueBuffer;

	private TagHelperScopeManager __backed__tagHelperScopeManager;

	private FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;

	private RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;

	private LabelTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper;

	private InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;

	private ValidationMessageTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper;

	private FormActionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper;

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
	public IHtmlHelper<ChangePasswordModel> Html { get; private set; }

	public ViewDataDictionary<ChangePasswordModel> ViewData => (ViewDataDictionary<ChangePasswordModel>)(base.PageContext?.ViewData);

	public ChangePasswordModel Model => ViewData.Model;

	public override async Task ExecuteAsync()
	{
		ViewData["Title"] = Localizer.GetString("ChangePasswordTitle");
		WriteLiteral("\r\n<div class=\"brand-header\">\r\n    <div class=\"brand-logo\">\r\n        ");
		Write(Html.Raw(IOemResources.FormHeaderImage.Value));
		WriteLiteral("\r\n    </div>\r\n</div>\r\n\r\n<div class=\"page-section\">\r\n    <span class=\"page-title\">");
		Write(Localizer.GetString("ChangePasswordPageTitle"));
		WriteLiteral("</span>\r\n\r\n    ");
		__tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", TagMode.StartTagAndEndTag, "0db5f6a4f3bd4c302880e5e75545d599122093b1f6006e2d35ef2d3f6280dff410151", async delegate
		{
			WriteLiteral("\r\n        <div class=\"form-group\">\r\n            <div class=\"form-label-wrapper\">\r\n                ");
			__tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", TagMode.StartTagAndEndTag, "0db5f6a4f3bd4c302880e5e75545d599122093b1f6006e2d35ef2d3f6280dff410538", async delegate
			{
			});
			__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<LabelTagHelper>();
			__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
			__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, (ChangePasswordModel __model) => __model.Input.CurrentPassword);
			__tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For, HtmlAttributeValueStyle.DoubleQuotes);
			await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
			if (!__tagHelperExecutionContext.Output.IsContentModified)
			{
				await __tagHelperExecutionContext.SetOutputContentAsync();
			}
			Write(__tagHelperExecutionContext.Output);
			__tagHelperExecutionContext = __tagHelperScopeManager.End();
			WriteLiteral("\r\n            </div>\r\n            <div");
			BeginWriteAttribute("class", " class=\"", 806, "\"", 909, 2);
			WriteAttributeValue("", 814, "form-field", 814, 10, isLiteral: true);
			ModelStateEntry? modelStateEntry = ViewData.ModelState["Input.CurrentPassword"];
			WriteAttributeValue(" ", 824, (modelStateEntry != null && modelStateEntry.Errors.Count > 0) ? "has-error" : "", 825, 84, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(">\r\n                <img");
			BeginWriteAttribute("src", " src=\"", 933, "\"", 976, 1);
			WriteAttributeValue("", 939, Url.Content("~/images/Lock_off.svg"), 939, 37, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(" alt=\"User Icon\" class=\"form-icon-user\" draggable=\"false\">\r\n                ");
			__tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", TagMode.SelfClosing, "0db5f6a4f3bd4c302880e5e75545d599122093b1f6006e2d35ef2d3f6280dff413515", async delegate
			{
			});
			__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<InputTagHelper>();
			__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
			__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, (ChangePasswordModel __model) => __model.Input.CurrentPassword);
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
			WriteLiteral("\r\n                <div class=\"form-icon-button\">\r\n                    <img");
			BeginWriteAttribute("src", " src=\"", 1233, "\"", 1277, 1);
			WriteAttributeValue("", 1239, Url.Content("~/images/Mask_Hide.svg"), 1239, 38, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral("\r\n                         alt=\"Show Password\"\r\n                         class=\"form-password-toggle\"\r\n                         draggable=\"false\"\r\n                         data-hide=\"");
			Write(Url.Content("~/images/Mask_Hide.svg"));
			WriteLiteral("\"\r\n                         data-show=\"");
			Write(Url.Content("~/images/Mask_Show.svg"));
			WriteLiteral("\">\r\n                </div>\r\n            </div>\r\n            <div class=\"inline-message\">\r\n                ");
			__tagHelperExecutionContext = __tagHelperScopeManager.Begin("span", TagMode.StartTagAndEndTag, "0db5f6a4f3bd4c302880e5e75545d599122093b1f6006e2d35ef2d3f6280dff417016", async delegate
			{
			});
			__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper = CreateTagHelper<ValidationMessageTagHelper>();
			__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper);
			__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, (ChangePasswordModel __model) => __model.Input.CurrentPassword);
			__tagHelperExecutionContext.AddTagHelperAttribute("asp-validation-for", __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For, HtmlAttributeValueStyle.DoubleQuotes);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
			await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
			if (!__tagHelperExecutionContext.Output.IsContentModified)
			{
				await __tagHelperExecutionContext.SetOutputContentAsync();
			}
			Write(__tagHelperExecutionContext.Output);
			__tagHelperExecutionContext = __tagHelperScopeManager.End();
			WriteLiteral("\r\n                <div class=\"inline-error-icon\">\r\n                    <img");
			BeginWriteAttribute("src", " src=\"", 1839, "\"", 1920, 1);
			WriteAttributeValue("", 1845, Url.Content("~/images/messageBar/Status_Error_Exclamation_Mark_Color.svg"), 1845, 75, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(" alt=\"Error\" draggable=\"false\" />\r\n                </div>\r\n            </div>\r\n        </div>\r\n\r\n        <div class=\"page-instruction\">\r\n            <span class=\"page-instruction-heading\">");
			Write(Localizer.GetString("SetPasswordTitle"));
			WriteLiteral("</span>\r\n            <span class=\"page-instruction-text\">");
			Write(Localizer.GetString("PasswordInfo"));
			WriteLiteral("</span>\r\n            <ul class=\"password-bullet-list\">\r\n");
			foreach (string passwordRequirement in Model.PasswordRequirements)
			{
				WriteLiteral("                    <li class=\"password-list-item\">\r\n                        <span class=\"page-instruction-text\">");
				Write(passwordRequirement);
				WriteLiteral("</span>\r\n                    </li>\r\n");
			}
			WriteLiteral("            </ul>\r\n        </div>\r\n\r\n        <div class=\"form-group\">\r\n            <div class=\"form-label-wrapper\">\r\n                ");
			__tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", TagMode.StartTagAndEndTag, "0db5f6a4f3bd4c302880e5e75545d599122093b1f6006e2d35ef2d3f6280dff421715", async delegate
			{
			});
			__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<LabelTagHelper>();
			__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
			__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, (ChangePasswordModel __model) => __model.Input.NewPassword);
			__tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For, HtmlAttributeValueStyle.DoubleQuotes);
			await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
			if (!__tagHelperExecutionContext.Output.IsContentModified)
			{
				await __tagHelperExecutionContext.SetOutputContentAsync();
			}
			Write(__tagHelperExecutionContext.Output);
			__tagHelperExecutionContext = __tagHelperScopeManager.End();
			WriteLiteral("\r\n            </div>\r\n            <div");
			BeginWriteAttribute("class", " class=\"", 2850, "\"", 2949, 2);
			WriteAttributeValue("", 2858, "form-field", 2858, 10, isLiteral: true);
			ModelStateEntry? modelStateEntry2 = ViewData.ModelState["Input.NewPassword"];
			WriteAttributeValue(" ", 2868, (modelStateEntry2 != null && modelStateEntry2.Errors.Count > 0) ? "has-error" : "", 2869, 80, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(">\r\n                <img");
			BeginWriteAttribute("src", " src=\"", 2973, "\"", 3016, 1);
			WriteAttributeValue("", 2979, Url.Content("~/images/Lock_off.svg"), 2979, 37, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(" alt=\"User Icon\" class=\"form-icon-user\" draggable=\"false\">\r\n                ");
			__tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", TagMode.SelfClosing, "0db5f6a4f3bd4c302880e5e75545d599122093b1f6006e2d35ef2d3f6280dff424694", async delegate
			{
			});
			__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<InputTagHelper>();
			__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
			__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, (ChangePasswordModel __model) => __model.Input.NewPassword);
			__tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, HtmlAttributeValueStyle.DoubleQuotes);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
			await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
			if (!__tagHelperExecutionContext.Output.IsContentModified)
			{
				await __tagHelperExecutionContext.SetOutputContentAsync();
			}
			Write(__tagHelperExecutionContext.Output);
			__tagHelperExecutionContext = __tagHelperScopeManager.End();
			WriteLiteral("\r\n                <div class=\"form-icon-button\">\r\n                    <img");
			BeginWriteAttribute("src", " src=\"", 3265, "\"", 3309, 1);
			WriteAttributeValue("", 3271, Url.Content("~/images/Mask_Hide.svg"), 3271, 38, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral("\r\n                         alt=\"Show Password\"\r\n                         class=\"form-password-toggle\"\r\n                         draggable=\"false\"\r\n                         data-hide=\"");
			Write(Url.Content("~/images/Mask_Hide.svg"));
			WriteLiteral("\"\r\n                         data-show=\"");
			Write(Url.Content("~/images/Mask_Show.svg"));
			WriteLiteral("\">\r\n                </div>\r\n            </div>\r\n            <div class=\"inline-message\">\r\n                ");
			__tagHelperExecutionContext = __tagHelperScopeManager.Begin("span", TagMode.StartTagAndEndTag, "0db5f6a4f3bd4c302880e5e75545d599122093b1f6006e2d35ef2d3f6280dff428191", async delegate
			{
			});
			__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper = CreateTagHelper<ValidationMessageTagHelper>();
			__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper);
			__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, (ChangePasswordModel __model) => __model.Input.NewPassword);
			__tagHelperExecutionContext.AddTagHelperAttribute("asp-validation-for", __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For, HtmlAttributeValueStyle.DoubleQuotes);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
			await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
			if (!__tagHelperExecutionContext.Output.IsContentModified)
			{
				await __tagHelperExecutionContext.SetOutputContentAsync();
			}
			Write(__tagHelperExecutionContext.Output);
			__tagHelperExecutionContext = __tagHelperScopeManager.End();
			WriteLiteral("\r\n                <div class=\"inline-error-icon\">\r\n                    <img");
			BeginWriteAttribute("src", " src=\"", 3867, "\"", 3948, 1);
			WriteAttributeValue("", 3873, Url.Content("~/images/messageBar/Status_Error_Exclamation_Mark_Color.svg"), 3873, 75, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(" alt=\"Error\" draggable=\"false\" />\r\n                </div>\r\n            </div>\r\n        </div>\r\n\r\n        <div class=\"form-group\">\r\n            <div class=\"form-label-wrapper\">\r\n                ");
			__tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", TagMode.StartTagAndEndTag, "0db5f6a4f3bd4c302880e5e75545d599122093b1f6006e2d35ef2d3f6280dff430802", async delegate
			{
			});
			__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<LabelTagHelper>();
			__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
			__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, (ChangePasswordModel __model) => __model.Input.ConfirmPassword);
			__tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For, HtmlAttributeValueStyle.DoubleQuotes);
			await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
			if (!__tagHelperExecutionContext.Output.IsContentModified)
			{
				await __tagHelperExecutionContext.SetOutputContentAsync();
			}
			Write(__tagHelperExecutionContext.Output);
			__tagHelperExecutionContext = __tagHelperScopeManager.End();
			WriteLiteral("\r\n            </div>\r\n            <div");
			BeginWriteAttribute("class", " class=\"", 4246, "\"", 4349, 2);
			WriteAttributeValue("", 4254, "form-field", 4254, 10, isLiteral: true);
			ModelStateEntry? modelStateEntry3 = ViewData.ModelState["Input.ConfirmPassword"];
			WriteAttributeValue(" ", 4264, (modelStateEntry3 != null && modelStateEntry3.Errors.Count > 0) ? "has-error" : "", 4265, 84, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(">\r\n                <img");
			BeginWriteAttribute("src", " src=\"", 4373, "\"", 4416, 1);
			WriteAttributeValue("", 4379, Url.Content("~/images/Lock_off.svg"), 4379, 37, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(" alt=\"User Icon\" class=\"form-icon-user\" draggable=\"false\">\r\n                ");
			__tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", TagMode.SelfClosing, "0db5f6a4f3bd4c302880e5e75545d599122093b1f6006e2d35ef2d3f6280dff433789", async delegate
			{
			});
			__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<InputTagHelper>();
			__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
			__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, (ChangePasswordModel __model) => __model.Input.ConfirmPassword);
			__tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, HtmlAttributeValueStyle.DoubleQuotes);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
			await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
			if (!__tagHelperExecutionContext.Output.IsContentModified)
			{
				await __tagHelperExecutionContext.SetOutputContentAsync();
			}
			Write(__tagHelperExecutionContext.Output);
			__tagHelperExecutionContext = __tagHelperScopeManager.End();
			WriteLiteral("\r\n                <div class=\"form-icon-button\">\r\n                    <img");
			BeginWriteAttribute("src", " src=\"", 4673, "\"", 4717, 1);
			WriteAttributeValue("", 4679, Url.Content("~/images/Mask_Hide.svg"), 4679, 38, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral("\r\n                         alt=\"Show Password\"\r\n                         class=\"form-password-toggle\"\r\n                         draggable=\"false\"\r\n                         data-hide=\"");
			Write(Url.Content("~/images/Mask_Hide.svg"));
			WriteLiteral("\"\r\n                         data-show=\"");
			Write(Url.Content("~/images/Mask_Show.svg"));
			WriteLiteral("\">\r\n                </div>\r\n            </div>\r\n            <div class=\"inline-message\">\r\n                ");
			__tagHelperExecutionContext = __tagHelperScopeManager.Begin("span", TagMode.StartTagAndEndTag, "0db5f6a4f3bd4c302880e5e75545d599122093b1f6006e2d35ef2d3f6280dff437290", async delegate
			{
			});
			__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper = CreateTagHelper<ValidationMessageTagHelper>();
			__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper);
			__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, (ChangePasswordModel __model) => __model.Input.ConfirmPassword);
			__tagHelperExecutionContext.AddTagHelperAttribute("asp-validation-for", __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For, HtmlAttributeValueStyle.DoubleQuotes);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
			await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
			if (!__tagHelperExecutionContext.Output.IsContentModified)
			{
				await __tagHelperExecutionContext.SetOutputContentAsync();
			}
			Write(__tagHelperExecutionContext.Output);
			__tagHelperExecutionContext = __tagHelperScopeManager.End();
			WriteLiteral("\r\n                <div class=\"inline-error-icon\">\r\n                    <img");
			BeginWriteAttribute("src", " src=\"", 5279, "\"", 5360, 1);
			WriteAttributeValue("", 5285, Url.Content("~/images/messageBar/Status_Error_Exclamation_Mark_Color.svg"), 5285, 75, isLiteral: false);
			EndWriteAttribute();
			WriteLiteral(" alt=\"Error\" draggable=\"false\" />\r\n                </div>\r\n            </div>\r\n        </div>\r\n\r\n        <div class=\"page-action-group page-action-group--end\">\r\n            <div class=\"page-action-button page-action-button--narrow\" style=\"order: 2;\">\r\n                <button type=\"submit\" class=\"button-primary\">\r\n                    <span class=\"button-text\">");
			Write(Localizer.GetString("ButtonSave"));
			WriteLiteral("</span>\r\n                </button>\r\n            </div>\r\n            <div class=\"page-action-button page-action-button--narrow\" style=\"order: 1;\">\r\n                ");
			__tagHelperExecutionContext = __tagHelperScopeManager.Begin("button", TagMode.StartTagAndEndTag, "0db5f6a4f3bd4c302880e5e75545d599122093b1f6006e2d35ef2d3f6280dff440607", async delegate
			{
				WriteLiteral("\r\n                    <span class=\"button-text\">");
				Write(Localizer.GetString("ButtonCancel"));
				WriteLiteral("</span>\r\n                ");
			});
			__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper = CreateTagHelper<FormActionTagHelper>();
			__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper);
			__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.PageHandler = (string)__tagHelperAttribute_7.Value;
			__tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
			BeginWriteTagHelperAttribute();
			__tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
			__tagHelperExecutionContext.AddHtmlAttribute("formnovalidate", Html.Raw(__tagHelperStringValueBuffer), HtmlAttributeValueStyle.Minimized);
			__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_8);
			await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
			if (!__tagHelperExecutionContext.Output.IsContentModified)
			{
				await __tagHelperExecutionContext.SetOutputContentAsync();
			}
			Write(__tagHelperExecutionContext.Output);
			__tagHelperExecutionContext = __tagHelperScopeManager.End();
			WriteLiteral("\r\n            </div>\r\n        </div>\r\n    ");
		});
		__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<FormTagHelper>();
		__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
		__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<RenderAtEndOfFormTagHelper>();
		__tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
		__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_9);
		__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_10.Value;
		__tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_10);
		__tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_11);
		await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
		if (!__tagHelperExecutionContext.Output.IsContentModified)
		{
			await __tagHelperExecutionContext.SetOutputContentAsync();
		}
		Write(__tagHelperExecutionContext.Output);
		__tagHelperExecutionContext = __tagHelperScopeManager.End();
		WriteLiteral("\r\n</div>\r\n<script");
		BeginWriteAttribute("src", " src=\"", 6223, "\"", 6267, 1);
		WriteAttributeValue("", 6229, Url.Content("~/js/changePassword.js"), 6229, 38, isLiteral: false);
		EndWriteAttribute();
		WriteLiteral("></script>");
	}
}
