using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Utilities;

public class PrincipalWrapper
{
	public static string GetEmail(string domain, string username)
	{
		string result = string.Empty;
		new Chronometer().Reset();
		using (PrincipalContext context = new PrincipalContext(ContextType.Domain, domain))
		{
			UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username);
			if (userPrincipal != null)
			{
				result = userPrincipal.EmailAddress;
			}
		}
		return result;
	}

	public static bool CheckUserIsOfDomainGroup(string name, string groupName, string domain)
	{
		bool result = false;
		Chronometer chronometer = new Chronometer();
		chronometer.Reset();
		try
		{
			using PrincipalContext context = new PrincipalContext(ContextType.Domain, domain);
			GroupPrincipal groupPrincipal = GroupPrincipal.FindByIdentity(context, IdentityType.Sid, groupName);
			UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.Sid, name);
			if (userPrincipal != null && groupPrincipal != null)
			{
				result = groupPrincipal.Members.Contains(userPrincipal);
			}
		}
		catch (Exception ex)
		{
			Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
		chronometer.MeasureAndPrintTime(MethodBase.GetCurrentMethod().Name, "Name: " + name + ", GroupName: " + groupName + ", domain: " + domain);
		return result;
	}

	public static List<UserData> GetAllUsersOfDomainGroup(string domain, string groupName, string username, string password)
	{
		List<UserData> result = new List<UserData>();
		PrincipalContext context = new PrincipalContext(ContextType.Domain, domain, null, ContextOptions.Negotiate, username, password);
		try
		{
			using GroupPrincipal groupPrincipal = GroupPrincipal.FindByIdentity(context, groupName);
			if (groupPrincipal != null)
			{
				using PrincipalSearchResult<Principal> principalSearchResult = groupPrincipal.GetMembers(recursive: true);
				if (principalSearchResult != null)
				{
					result = principalSearchResult.Select((Principal u) => PrincipalUser(context, groupName, u)).ToList();
				}
			}
		}
		finally
		{
			if (context != null)
			{
				((IDisposable)context).Dispose();
			}
		}
		return result;
	}

	private static UserData PrincipalUser(PrincipalContext context, string groupName, Principal principal)
	{
		UserData userData = new UserData
		{
			GroupSid = groupName,
			Sid = principal.Sid.ToString(),
			Name = principal.Context.Name + "\\" + principal.SamAccountName
		};
		using (UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, userData.Sid))
		{
			if (userPrincipal != null)
			{
				userData.Email = userPrincipal.EmailAddress;
				userData.PhoneNumber = userPrincipal.VoiceTelephoneNumber;
			}
		}
		return userData;
	}

	public static bool CheckUserIsOfWindowsGroup(string name, string groupname, string hostname, string userName = null, string password = null)
	{
		bool result = false;
		Chronometer chronometer = new Chronometer();
		chronometer.Reset();
		try
		{
			result = IsMemberOf(name, groupname, hostname, userName, password);
		}
		catch (Exception ex)
		{
			Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
		chronometer.MeasureAndPrintTime(MethodBase.GetCurrentMethod().Name, "Name: " + name + ", GroupName: " + groupname + ", hostName: " + hostname + ", username: " + userName + ", password: " + password);
		return result;
	}

	private static bool IsMemberOf(string name, string groupname, string hostname, string userName, string password)
	{
		bool result = false;
		using (PrincipalContext context = new PrincipalContext(ContextType.Machine, hostname, null, ContextOptions.Negotiate, userName, password))
		{
			GroupPrincipal groupPrincipal = GroupPrincipal.FindByIdentity(context, IdentityType.Sid, groupname);
			UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.Sid, name);
			if (userPrincipal != null && groupPrincipal != null)
			{
				result = groupPrincipal.Members.Contains(userPrincipal);
			}
		}
		return result;
	}

	public static bool CheckWindowsGroupSid(string hostname, string groupSid)
	{
		bool result = false;
		Chronometer chronometer = new Chronometer();
		chronometer.Reset();
		try
		{
			using PrincipalContext context = new PrincipalContext(ContextType.Machine, hostname);
			result = GroupPrincipal.FindByIdentity(context, IdentityType.Sid, groupSid) != null;
		}
		catch (Exception ex)
		{
			Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
		chronometer.MeasureAndPrintTime(MethodBase.GetCurrentMethod().Name, "hostName: " + hostname + ", GroupSid: " + groupSid);
		return result;
	}

	public static bool CheckDomainGroupSid(string domainName, string groupSid)
	{
		bool result = false;
		Chronometer chronometer = new Chronometer();
		chronometer.Reset();
		try
		{
			using PrincipalContext context = new PrincipalContext(ContextType.Domain, domainName);
			result = GroupPrincipal.FindByIdentity(context, IdentityType.Sid, groupSid) != null;
		}
		catch (Exception ex)
		{
			Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
		chronometer.MeasureAndPrintTime(MethodBase.GetCurrentMethod().Name, "domainName: " + domainName + ", groupSid: " + groupSid);
		return result;
	}

	private static List<UserData> GetAllUsersOfWindowsGroup(string groupSid, Func<PrincipalContext> principleContextFactory)
	{
		List<UserData> result = new List<UserData>();
		using (PrincipalContext context = principleContextFactory())
		{
			using GroupPrincipal groupPrincipal = GroupPrincipal.FindByIdentity(context, groupSid);
			if (groupPrincipal != null)
			{
				using PrincipalSearchResult<Principal> principalSearchResult = groupPrincipal.GetMembers(recursive: true);
				if (principalSearchResult != null)
				{
					result = principalSearchResult.Select((Principal u) => new UserData
					{
						GroupSid = groupSid,
						Sid = u.Sid.ToString(),
						Name = u.Context.Name + "\\" + u.SamAccountName
					}).ToList();
				}
			}
		}
		return result;
	}

	public static List<UserData> GetAllUsersOfWindowsGroup(string hostname, string groupSid)
	{
		return GetAllUsersOfWindowsGroup(groupSid, () => new PrincipalContext(ContextType.Machine, hostname, null, ContextOptions.Negotiate));
	}

	public static List<UserData> GetAllUsersOfWindowsGroupSecured(string hostname, string groupSid, string username, string password)
	{
		return GetAllUsersOfWindowsGroup(groupSid, () => new PrincipalContext(ContextType.Machine, hostname, null, ContextOptions.Negotiate, username, password));
	}

	public static bool IsMemberOf(string userMail, string groupName)
	{
		return IsMemberOf(userMail, (WindowsPrincipal principal) => principal.IsInRole(groupName));
	}

	public static bool IsMemberOf(string userMail, SecurityIdentifier groupSid)
	{
		return IsMemberOf(userMail, (WindowsPrincipal principal) => principal.IsInRole(groupSid));
	}

	private static bool IsMemberOf(string userMail, Func<WindowsPrincipal, bool> isInRole)
	{
		try
		{
			using WindowsIdentity ntIdentity = new WindowsIdentity(userMail);
			WindowsPrincipal arg = new WindowsPrincipal(ntIdentity);
			return isInRole(arg);
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		return false;
	}
}
