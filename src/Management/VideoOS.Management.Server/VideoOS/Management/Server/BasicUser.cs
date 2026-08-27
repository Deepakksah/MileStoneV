using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Model.Responses;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class BasicUser
{
	private static readonly DateTimeOffset InitiallyEnabledOffset = DateTimeOffset.MinValue.AddDays(2.0);

	public const string BasicUserDomainName = "[BASIC]";

	public string Id { get; set; }

	public string Sid { get; set; }

	public string Name { get; set; }

	public string FullName => "[BASIC]\\" + Name;

	public string Description { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public bool CanChangePassword { get; set; }

	public bool ForcePasswordChange { get; set; }

	public BasicUserStatus Status { get; set; }

	public DateTime? LockoutEnd { get; set; }

	public bool Active { get; set; }

	public bool IsExternal { get; set; }

	public IEnumerable<Claim> Claims { get; set; }

	public static BasicUser Map(GetUserModel userModel)
	{
		if (userModel == null)
		{
			return null;
		}
		DateTime? lockoutEnd = null;
		BasicUserStatus status;
		if (!userModel.Active)
		{
			status = BasicUserStatus.LockedOutByAdmin;
		}
		else if (userModel.LockoutEnd > DateTimeOffset.UtcNow)
		{
			status = BasicUserStatus.LockedOutBySystem;
			lockoutEnd = userModel.LockoutEnd.Value.LocalDateTime;
		}
		else
		{
			status = BasicUserStatus.Enabled;
		}
		return new BasicUser
		{
			Id = userModel.Id,
			Sid = userModel.Id,
			Name = userModel.UserName,
			Description = userModel.Description,
			CanChangePassword = userModel.CanChangePassword,
			CreatedDate = userModel.CreatedDate,
			LastModified = userModel.ModifiedDate,
			ForcePasswordChange = userModel.ForcePasswordChange,
			Status = status,
			LockoutEnd = lockoutEnd,
			Active = userModel.Active,
			IsExternal = userModel.IsExternal,
			Claims = (userModel.Claims?.Select((GetUserClaimModel c) => CreateClaim(c)).ToArray() ?? Array.Empty<Claim>())
		};
		static Claim CreateClaim(GetUserClaimModel claim)
		{
			Guid result = Guid.Empty;
			Guid.TryParse(claim.LoginProviderId, out result);
			return new Claim(result, claim.ClaimType, claim.ClaimValue);
		}
	}

	public static BasicUser[] QueryBasicUsers(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		return requestContext.GetService<IdpClientService>().CreateUserIdpComponent().GetAllVmsUsers();
	}

	public static string[] QueryBasicUserIds(IGeminiRequestContext requestContext)
	{
		return (from vmsUser in QueryBasicUsers(requestContext)
			select vmsUser.Id).ToArray();
	}

	public static BasicUser QueryBasicUserById(IGeminiRequestContext requestContext, string id)
	{
		if (string.Compare(requestContext.UserContext.Identifier, id, ignoreCase: true) != 0)
		{
			requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		}
		return requestContext.GetService<IdpClientService>().CreateUserIdpComponent().GetVmsUserById(id);
	}

	public static BasicUser[] QueryBasicUsersByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		return requestContext.GetService<IdpClientService>().CreateUserIdpComponent().GetVmsUserCollection(ids);
	}

	public static BasicUser CreateBasicUser(IGeminiRequestContext requestContext, string id, string name, string description, bool canChangePassword, string password, bool forceChangePassword, BasicUserStatus status)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Validation.CheckName(name, "name");
		Validation.CheckDescription(ref description, "description");
		Validation.CheckName(password, "password");
		id = GeminiResourceComponent.GetGuid(id);
		if (status == BasicUserStatus.LockedOutBySystem)
		{
			throw VmoExceptionFactory.VMO60346_VmoServerServiceException();
		}
		CreateUserModel model = new CreateUserModel
		{
			Id = id,
			UserName = name,
			Password = password,
			CanChangePassword = canChangePassword,
			Description = description,
			ForcePasswordChange = forceChangePassword,
			LockoutEnd = InitiallyEnabledOffset,
			Active = (status == BasicUserStatus.Enabled)
		};
		BasicUser basicUser = requestContext.GetService<IdpClientService>().CreateUserIdpComponent().CreateVmsUser(model);
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogEntryAuditLogger.AuditBasicUserCreated(basicUser.Sid, basicUser.Name.ToUpperInvariant(), requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "BasicUser", id, EventAction.Added, basicUser.FullName);
		return basicUser;
	}

	public static BasicUser UpdateBasicUser(IGeminiRequestContext requestContext, string id, string description, bool? canChangePassword, bool? forceChangePassword, BasicUserStatus status)
	{
		UpdateBasicUserValidation(requestContext, ref id, ref description);
		IUserIdpComponent userIdpComponent = requestContext.GetService<IdpClientService>().CreateUserIdpComponent();
		BasicUser vmsUserById = userIdpComponent.GetVmsUserById(id);
		DateTimeOffset? lockoutEnd = default(DateTimeOffset);
		bool? active = false;
		switch (status)
		{
		case BasicUserStatus.Enabled:
			if (vmsUserById.LockoutEnd > DateTime.UtcNow)
			{
				lockoutEnd = DateTimeOffset.UtcNow;
			}
			active = true;
			break;
		case BasicUserStatus.LockedOutByAdmin:
			active = false;
			break;
		case BasicUserStatus.LockedOutBySystem:
			if (vmsUserById.Status != BasicUserStatus.LockedOutBySystem)
			{
				throw VmoExceptionFactory.VMO60347_VmoServerServiceException();
			}
			break;
		}
		if (vmsUserById.IsExternal)
		{
			if (canChangePassword == true)
			{
				throw VmoExceptionFactory.VMO60344_VmoServerServiceException();
			}
			if (forceChangePassword == true)
			{
				throw VmoExceptionFactory.VMO60345_VmoServerServiceException();
			}
		}
		UpdateUserModel updateUserModel = new UpdateUserModel
		{
			Description = description,
			LockoutEnd = lockoutEnd,
			Active = active
		};
		if (!vmsUserById.IsExternal)
		{
			updateUserModel.CanChangePassword = canChangePassword;
			updateUserModel.ForcePasswordChange = forceChangePassword;
		}
		BasicUser basicUser = userIdpComponent.UpdateBasicUser(id, updateUserModel);
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogEntryAuditLogger.AuditBasicUserEdited(basicUser.Id, basicUser.Name.ToUpperInvariant(), requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			if (vmsUserById.ForcePasswordChange != basicUser.ForcePasswordChange && basicUser.ForcePasswordChange)
			{
				vmoLogManager.LogEntryAuditLogger.AuditBasicUserForcePasswordChange(basicUser.Id, basicUser.Name.ToUpperInvariant(), requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			}
			if (vmsUserById.Status != basicUser.Status)
			{
				if (basicUser.Status == BasicUserStatus.LockedOutByAdmin)
				{
					vmoLogManager.LogEntryAuditLogger.AuditBasicUserLockedOut(basicUser.Id, basicUser.Name.ToUpperInvariant(), requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
				}
				else if (basicUser.Status == BasicUserStatus.Enabled)
				{
					vmoLogManager.LogEntryAuditLogger.AuditBasicUserEnabled(basicUser.Id, basicUser.Name.ToUpperInvariant(), requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
				}
			}
		}
		Integration.FireConfigurationChanged(requestContext, "BasicUser", basicUser.Id, EventAction.Updated, basicUser.FullName);
		return basicUser;
	}

	public static void UpdateBasicUserValidation(IGeminiRequestContext requestContext, ref string id, ref string description)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Validation.CheckObjectId(ref id, "id", allowNullOrEmpty: false);
		Validation.CheckDescription(ref description, "description");
	}

	public static void DeleteBasicUser(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Guid sessionId = Guid.NewGuid();
		BasicUser basicUser = null;
		using (IBasicUserComponent basicUserComponent = requestContext.CreateBasicUserComponent())
		{
			try
			{
				IUserIdpComponent userIdpComponent = requestContext.GetService<IdpClientService>().CreateUserIdpComponent();
				basicUser = userIdpComponent.GetVmsUserById(id);
				if (basicUser == null)
				{
					throw new BasicUserDeleteException();
				}
				_ = "[BASIC]\\" + basicUser.Name;
				basicUserComponent.DeleteBasicUser(basicUser.Id, basicUser.Sid, basicUser.Name, sessionId);
				NotifyServerDeleteTaskCreated();
				try
				{
					int num = 40;
					while (userIdpComponent.GetVmsUserById(id) != null && num-- > 0)
					{
						Thread.Sleep(50);
					}
				}
				catch (BasicUserNotExistException)
				{
				}
			}
			catch (BasicUserNotExistException)
			{
				throw new BasicUserDeleteException();
			}
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	public static void ChangePasswordBasicUser(IGeminiRequestContext requestContext, string id, string newPassword)
	{
		Validation.CheckStringNotNullOrWhiteSpace(newPassword, "newPassword");
		if (string.Compare(requestContext.UserContext.Identifier, id, ignoreCase: true) != 0)
		{
			requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		}
		UpdateUserModel model = new UpdateUserModel
		{
			Password = newPassword
		};
		IUserIdpComponent userIdpComponent = requestContext.GetService<IdpClientService>().CreateUserIdpComponent();
		BasicUser vmsUserById = userIdpComponent.GetVmsUserById(id);
		if (vmsUserById.IsExternal)
		{
			throw VmoExceptionFactory.VMO60343_VmoServerServiceException();
		}
		vmsUserById = userIdpComponent.UpdateBasicUser(id, model);
		ClearLegacyBasicCredentialsCache(vmsUserById.Name);
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogEntryAuditLogger.AuditBasicUserPasswordChanged(vmsUserById.Sid, vmsUserById.Name.ToUpperInvariant(), requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "BasicUser", id, EventAction.Updated, vmsUserById.FullName);
	}

	private static void ClearLegacyBasicCredentialsCache(string username)
	{
		HttpContext.Current.Cache.Remove(BasicAuthenticationModule.CachePrefix + username.ToUpperInvariant());
	}

	private static void NotifyServerDeleteTaskCreated()
	{
		Integration.TryNotifyServerAsync();
	}
}
