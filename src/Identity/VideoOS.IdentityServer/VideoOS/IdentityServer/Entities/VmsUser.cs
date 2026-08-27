using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace VideoOS.IdentityServer.Entities;

public class VmsUser : IdentityUser, IModifiedAware
{
	private readonly List<VmsUserClaim> _claims;

	private readonly List<IdentityUserLogin<string>> _logins;

	public override string UserName
	{
		get
		{
			return base.UserName ?? throw new InvalidOperationException("Username cannot be null");
		}
		set
		{
			ArgumentException.ThrowIfNullOrEmpty(value, "UserName");
			base.UserName = value;
		}
	}

	public bool Active { get; private set; }

	public string Description { get; set; }

	public bool CanChangePassword { get; set; }

	public bool ForcePasswordChange { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime ModifiedDate { get; set; }

	public DateTime? DeactivatedAtUtc { get; private set; }

	public virtual ICollection<VmsUserClaim> Claims => _claims;

	public virtual ICollection<IdentityUserLogin<string>> Logins => _logins;

	private VmsUser()
	{
		_claims = new List<VmsUserClaim>();
		_logins = new List<IdentityUserLogin<string>>();
		Active = true;
	}

	public static VmsUser Create(string userName, string description, bool canChangePassword, bool forcePasswordChange, DateTimeOffset? lockoutEnd, bool active)
	{
		if (string.IsNullOrEmpty(userName))
		{
			throw new ArgumentNullException("userName");
		}
		VmsUser vmsUser = new VmsUser
		{
			UserName = userName,
			Description = description,
			CanChangePassword = canChangePassword,
			ForcePasswordChange = forcePasswordChange
		};
		if (active)
		{
			vmsUser.Activate();
		}
		else
		{
			vmsUser.Deactivate();
		}
		vmsUser.SetLockoutEnd(lockoutEnd);
		return vmsUser;
	}

	public void Activate()
	{
		if (!Active)
		{
			Active = true;
			DeactivatedAtUtc = null;
		}
	}

	public void Deactivate()
	{
		if (Active)
		{
			Active = false;
			DeactivatedAtUtc = DateTime.UtcNow;
		}
	}

	public void SetLockoutEnd(DateTimeOffset? endDate)
	{
		LockoutEnd = endDate;
		AccessFailedCount = 0;
	}

	public void AddLogin(IdentityUserLogin<string> login)
	{
		_logins.Add(login);
	}

	public void AddClaim(VmsUserClaim claim)
	{
		_claims.Add(claim);
	}
}
