using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.EventLogs;
using VideoOS.IdentityServer.Filters;
using VideoOS.IdentityServer.Helper;
using VideoOS.IdentityServer.Repositories;
using VideoOS.IdentityServer.Resources;
using VideoOS.IdentityServer.Services;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.Areas.Identity.Pages.Account.Manage;

[Authorize(Policy = "IdpChangePasswordPolicy")]
[IdentityPageExceptionFilter]
public class ChangePasswordModel : PageModel
{
	private readonly VmsUserManager _userManager;

	private readonly SignInManager<VmsUser> _signInManager;

	private readonly ILogger<ChangePasswordModel> _logger;

	private readonly IStringLocalizer _identityLocalizer;

	private readonly IPasswordRequirements _passwordRequirements;

	private readonly IIdentityServerInteractionService _interaction;

	private readonly IEventLogService _eventLogService;

	[BindProperty]
	public InputModel Input { get; set; }

	public string ErrorMessage { get; set; }

	public bool IsForceChange => base.User.Identity?.AuthenticationType == "IDP.ForcePasswordChange";

	public List<string> PasswordRequirements { get; set; }

	public ChangePasswordModel(VmsUserManager userManager, SignInManager<VmsUser> signInManager, ILogger<ChangePasswordModel> logger, IPasswordRequirements passwordRequirements, IStringLocalizerFactory factory, IIdentityServerInteractionService interaction, IEventLogService eventLogService)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_logger = logger;
		_identityLocalizer = factory.Create(typeof(IdentityResource));
		_passwordRequirements = passwordRequirements;
		_interaction = interaction;
		_eventLogService = eventLogService;
		InitPasswordRequirements();
	}

	public IActionResult OnGetAsync()
	{
		return Page();
	}

	public async Task<IActionResult> OnPostCancel()
	{
		base.HttpContext.Session.SetSessionIntValue("CancelSession", 1);
		await SignOutAsync();
		return RedirectToPage("/Account/Login/Index");
	}

	public async Task<IActionResult> OnPostAsync(string returnUrl = null)
	{
		if (!base.ModelState.IsValid)
		{
			return Page();
		}
		VmsUser user = await _userManager.GetUserAsync(base.User);
		if (user == null)
		{
			return RedirectToPage("/Error", NotFound());
		}
		if (_userManager.IsUserLockedOut(user) || !user.Active)
		{
			ErrorMessage = _identityLocalizer["ErrorLockAccount"];
			base.ModelState.AddModelError("Input.CurrentPassword", ErrorMessage);
			return Page();
		}
		IdentityResult identityResult = ((!user.ForcePasswordChange) ? (await _userManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword)) : (await _userManager.ChangePasswordAndResetForcePasswordChangeAsync(user, Input.CurrentPassword, Input.NewPassword)));
		if (!identityResult.Succeeded)
		{
			if (identityResult.Errors.FirstOrDefault((IdentityError e) => e.Code == "PasswordMismatch") != null)
			{
				await _userManager.AccessFailedAsync(user);
				ErrorMessage = _identityLocalizer["PasswordMismatch"];
				base.ModelState.AddModelError("Input.CurrentPassword", ErrorMessage);
				return Page();
			}
			if (identityResult.Errors.FirstOrDefault((IdentityError e) => e.Code == "ErrorStrongPassword") != null)
			{
				_logger.LogTrace("A user tried to set a password with insufficient complexity.");
				ErrorMessage = _identityLocalizer["ErrorStrongPassword"];
				base.ModelState.AddModelError("Input.NewPassword", ErrorMessage);
				return Page();
			}
			_logger.LogError("Unable to find expected error from change password result.");
			await SignOutAsync();
			_logger.LogDebug("User logged out.");
			return RedirectToPage("/Error");
		}
		await _userManager.ResetAccessFailedCountAsync(user);
		_logger.LogDebug("User changed their password successfully.");
		if (IsForceChange)
		{
			await SignOutAsync();
			_logger.LogDebug("User completed forced password change successfully.");
			if (!base.Url.IsLocalUrl(returnUrl))
			{
				return RedirectToPage("/Account/Manage/PasswordChanged");
			}
			IActionResult result;
			if (await _interaction.GetAuthorizationContextAsync(returnUrl) != null)
			{
				IActionResult actionResult = Redirect(returnUrl);
				result = actionResult;
			}
			else
			{
				IActionResult actionResult = RedirectToPage("/Account/Manage/PasswordChanged");
				result = actionResult;
			}
			return result;
		}
		await SignOutAsync();
		return RedirectToPage("/Account/Manage/PasswordChanged");
	}

	private async Task SignOutAsync()
	{
		await _signInManager.SignOutAsync();
		await base.HttpContext.SignOutAsync("IDP.ForcePasswordChange");
		if (base.User.Identity?.IsAuthenticated ?? false)
		{
			await _eventLogService.Log(new UserLogoutSuccess(base.User.Identity.Name, base.User.Claims.First((Claim c) => c.Type.Equals("sub", StringComparison.Ordinal)).Value));
		}
	}

	private void InitPasswordRequirements()
	{
		PasswordRequirements = new List<string>();
		if (_passwordRequirements.Settings.RequiredLength > 0)
		{
			PasswordRequirements.Add(string.Format(_identityLocalizer["PasswordRequirementMinPasswordLength"], _passwordRequirements.Settings.RequiredLength));
		}
		if (_passwordRequirements.Settings.MaxLength > 0)
		{
			PasswordRequirements.Add(string.Format(_identityLocalizer["PasswordRequireementMaxLength"], _passwordRequirements.Settings.MaxLength));
		}
		if (_passwordRequirements.Settings.RequireLowercase)
		{
			PasswordRequirements.Add(_identityLocalizer["PasswordRequirementLowercaseLetters"]);
		}
		if (_passwordRequirements.Settings.RequireUppercase)
		{
			PasswordRequirements.Add(_identityLocalizer["PasswordRequirementUppercaseLetters"]);
		}
		if (_passwordRequirements.Settings.RequireNonAlphanumeric)
		{
			PasswordRequirements.Add(_identityLocalizer["PasswordRequirementSpecialCharacters"]);
		}
		if (_passwordRequirements.Settings.RequireDigit)
		{
			PasswordRequirements.Add(_identityLocalizer["PasswordRequirementDigits"]);
		}
		if (_passwordRequirements.Settings.RequiredUniqueChars > 0)
		{
			PasswordRequirements.Add(string.Format(_identityLocalizer["PasswordRequirementMinimumUnique"], _passwordRequirements.Settings.RequiredUniqueChars));
		}
	}
}
