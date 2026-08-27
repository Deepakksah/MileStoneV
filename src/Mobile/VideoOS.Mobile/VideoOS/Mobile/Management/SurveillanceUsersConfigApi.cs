using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Platform;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Util.Security;

namespace VideoOS.Mobile.Management;

public static class SurveillanceUsersConfigApi
{
	public static List<SurveillanceUser> GetUsers(bool excludeExternalUsers = false)
	{
		return (from i in GetUsersByRole(excludeExternalUsers)?.SelectMany((KeyValuePair<Role, IEnumerable<SurveillanceUser>> kvp) => kvp.Value).AsEnumerable().DistinctBy((SurveillanceUser user) => user.Sid)
			orderby i.Name
			select i).ToList();
	}

	public static IDictionary<Role, IEnumerable<SurveillanceUser>> GetUsersByRole(bool excludeExternalUsers = false)
	{
		IDictionary<Role, IEnumerable<SurveillanceUser>> result = null;
		try
		{
			VideoOS.Platform.ConfigurationItems.ManagementServer managementServer = new VideoOS.Platform.ConfigurationItems.ManagementServer(EnvironmentManager.Instance.MasterSite);
			ICollection<BasicUser> basicUsers = managementServer.BasicUserFolder?.BasicUsers;
			result = managementServer.RoleFolder?.Roles.Where((Role role) => role.UserFolder?.Users != null).Select(delegate(Role role)
			{
				IEnumerable<SurveillanceUser> item = (from user in role.UserFolder.Users
					where !string.IsNullOrWhiteSpace(user.Sid) && !string.IsNullOrEmpty(user.IdentityType) && !string.IsNullOrWhiteSpace(user.AccountName) && !user.AccountName.EndsWith("$") && (!excludeExternalUsers || !user.IdentityType.Equals(IdentityType.BasicUser.ToString()) || basicUsers?.FirstOrDefault((BasicUser basicUser) => basicUser.Sid.Equals(user.Sid, StringComparison.CurrentCultureIgnoreCase))?.IsExternal != true)
					select new SurveillanceUser(user)).ToList().AsEnumerable();
				return (role: role, users: item);
			}).ToDictionary(((Role role, IEnumerable<SurveillanceUser> users) el) => el.role, ((Role role, IEnumerable<SurveillanceUser> users) el) => el.users);
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		return result;
	}
}
