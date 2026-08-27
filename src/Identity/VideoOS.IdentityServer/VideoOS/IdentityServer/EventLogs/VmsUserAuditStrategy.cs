using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.EventLogInfrastructure;

namespace VideoOS.IdentityServer.EventLogs;

public sealed class VmsUserAuditStrategy(IEventLogFactory factory) : AuditStrategy<VmsUser>
{
	private const string UserNamePropertyName = "UserName";

	private const string IdPropertyName = "Id";

	private const string PasswordHashPropertyName = "PasswordHash";

	private const string ForcePasswordChangePropertyName = "ForcePasswordChange";

	private const string ActiveChangePropertyName = "Active";

	private const string LockoutEndChangePropertyName = "LockoutEnd";

	private const string ClaimsChangePropertyName = "Claims";

	private const string DescriptionPropertyName = "Description";

	private const string CanChangePasswordPropertyName = "CanChangePassword";

	protected override async Task<IEnumerable<EventLog>> ProcessEntityStateChange(EntityEntry entry, VmsUser entity)
	{
		IEnumerable<EventLog> result;
		switch (entry.State)
		{
		case EntityState.Added:
		{
			EventLog eventLog2 = await factory.Create("UserCreated", entry.CurrentValues["Id"].ToString(), entry.CurrentValues["UserName"].ToString()).ConfigureAwait(continueOnCapturedContext: false);
			result = new EventLog[1] { eventLog2 };
			break;
		}
		case EntityState.Deleted:
		{
			EventLog eventLog = await factory.Create("UserDeleted", entry.CurrentValues["Id"].ToString(), entry.CurrentValues["UserName"].ToString()).ConfigureAwait(continueOnCapturedContext: false);
			result = new EventLog[1] { eventLog };
			break;
		}
		case EntityState.Modified:
			result = await FindModifiedEventLogs(entry, entry.CurrentValues["Id"].ToString(), entry.CurrentValues["UserName"].ToString()).ConfigureAwait(continueOnCapturedContext: false);
			break;
		default:
			result = Array.Empty<EventLog>();
			break;
		}
		return result;
	}

	private async Task<List<EventLog>> FindModifiedEventLogs(EntityEntry entry, string objectId, string objectValue)
	{
		List<EventLog> eventLogs = new List<EventLog>();
		foreach (MemberEntry member in entry.Members)
		{
			if (!member.IsModified)
			{
				continue;
			}
			switch (member.Metadata.Name)
			{
			case "PasswordHash":
			{
				if (member.EntityEntry.OriginalValues.TryGetValue<string>("PasswordHash", out var value5) && member.EntityEntry.CurrentValues.TryGetValue<string>("PasswordHash", out var value6) && value5 != null && !value5.Equals(value6, StringComparison.Ordinal))
				{
					List<EventLog> list = eventLogs;
					list.Add(await factory.Create("UserChangePasswordSuccess", objectId, objectValue).ConfigureAwait(continueOnCapturedContext: false));
				}
				break;
			}
			case "ForcePasswordChange":
			{
				if (member.EntityEntry.OriginalValues.TryGetValue<bool>("ForcePasswordChange", out var value7) && member.EntityEntry.CurrentValues.TryGetValue<bool>("ForcePasswordChange", out var value8) && value7 != value8)
				{
					if (value8)
					{
						List<EventLog> list = eventLogs;
						list.Add(await factory.Create("UserNeedPasswordChange", objectId, objectValue).ConfigureAwait(continueOnCapturedContext: false));
					}
					else
					{
						List<EventLog> list = eventLogs;
						list.Add(await factory.Create("UserNeedPasswordChangeCleared", objectId, objectValue).ConfigureAwait(continueOnCapturedContext: false));
					}
				}
				break;
			}
			case "Active":
			{
				if (member.EntityEntry.OriginalValues.TryGetValue<bool>("Active", out var value9) && member.EntityEntry.CurrentValues.TryGetValue<bool>("Active", out var value10) && value9 != value10)
				{
					if (value10)
					{
						List<EventLog> list = eventLogs;
						list.Add(await factory.Create("UserEnabled", objectId, objectValue).ConfigureAwait(continueOnCapturedContext: false));
					}
					else
					{
						List<EventLog> list = eventLogs;
						list.Add(await factory.Create("UserDisabled", objectId, objectValue).ConfigureAwait(continueOnCapturedContext: false));
					}
				}
				break;
			}
			case "LockoutEnd":
			{
				if (member.EntityEntry.OriginalValues.TryGetValue<DateTimeOffset?>("LockoutEnd", out var value11) && member.EntityEntry.CurrentValues.TryGetValue<DateTimeOffset?>("LockoutEnd", out var value12) && value11 != value12)
				{
					if (value12.HasValue && value12 > DateTime.Now && (!value11.HasValue || value11 < value12))
					{
						List<EventLog> list = eventLogs;
						list.Add(await factory.Create("UserLockedOut", objectId, objectValue).ConfigureAwait(continueOnCapturedContext: false));
					}
					else if (value11.HasValue)
					{
						List<EventLog> list = eventLogs;
						list.Add(await factory.Create("UserLockedOutCleared", objectId, objectValue).ConfigureAwait(continueOnCapturedContext: false));
					}
				}
				break;
			}
			case "Description":
			{
				if (member.EntityEntry.OriginalValues.TryGetValue<string>("Description", out var value3) && member.EntityEntry.CurrentValues.TryGetValue<string>("Description", out var value4) && value3 != value4)
				{
					List<EventLog> list = eventLogs;
					list.Add(await factory.Create("UserDescriptionChanged", objectId, objectValue).ConfigureAwait(continueOnCapturedContext: false));
				}
				break;
			}
			case "CanChangePassword":
			{
				if (member.EntityEntry.OriginalValues.TryGetValue<bool>("CanChangePassword", out var value) && member.EntityEntry.CurrentValues.TryGetValue<bool>("CanChangePassword", out var value2) && value != value2)
				{
					if (value2)
					{
						List<EventLog> list = eventLogs;
						list.Add(await factory.Create("UserCanChangePassword", objectId, objectValue).ConfigureAwait(continueOnCapturedContext: false));
					}
					else
					{
						List<EventLog> list = eventLogs;
						list.Add(await factory.Create("UserCannotChangePassword", objectId, objectValue).ConfigureAwait(continueOnCapturedContext: false));
					}
				}
				break;
			}
			}
		}
		NavigationEntry navigationEntry = entry.Navigation("Claims");
		if (navigationEntry.IsModified)
		{
			List<EventLog> list = eventLogs;
			list.Add(await factory.Create("UserClaimsUpdated", objectId, objectValue).ConfigureAwait(continueOnCapturedContext: false));
		}
		return eventLogs;
	}
}
