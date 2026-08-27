using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class SynchronizationManager
{
	public DateTime _currentUtcTime;

	private Dictionary<string, object> _synchedGroups;

	private HashSet<string> _verifiedDomains;

	private HashSet<string> _failingDomains;

	private SynchronizationManager()
	{
		_currentUtcTime = DateTime.Now;
		_synchedGroups = new Dictionary<string, object>();
		_verifiedDomains = new HashSet<string>();
		_failingDomains = new HashSet<string>();
	}

	public static bool FullSynchronizaionWithIdentityStore(IGeminiRequestContext requestContext)
	{
		bool flag = false;
		GeminiTrace.Info(TraceType.Security, "Starting Active Directory synchronization for all roles");
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			Role[] array = roleComponent.QueryRoles();
			SynchronizationManager synchronizationManager = new SynchronizationManager();
			Role[] array2 = array;
			foreach (Role role in array2)
			{
				Identity identity = new Identity();
				identity.Sid = role.Id.ToString();
				identity.Type = IdentityType.Role;
				identity.DisplayName = role.Name;
				flag = synchronizationManager.SynchronizeRole(requestContext, identity) || flag;
			}
		}
		GeminiTrace.Info(TraceType.Security, "Finished Active Directory synchronization for all roles");
		return flag;
	}

	public static bool SynchronizeWindowsGroupWithIdentityStore(IGeminiRequestContext requestContext, Identity identity)
	{
		if (identity == null || identity.Type != IdentityType.WindowsGroup)
		{
			throw new ArgumentException("Identity needs to of type windows group");
		}
		return new SynchronizationManager().SynchronizeWindowsGroup(requestContext, identity);
	}

	private bool SynchronizeRole(IGeminiRequestContext requestContext, Identity identity)
	{
		identity = new RoleAccessor(identity.Sid).QueryIdentity(requestContext, includeMembers: true);
		if (identity != null && identity.Members != null)
		{
			Tuple<bool, List<Identity>> tuple = UpdateMembers(requestContext, identity, identity.Members, SidUtility.IsCorporateIdentity);
			bool flag = tuple.Item1;
			{
				foreach (Identity item in tuple.Item2)
				{
					if (item.Type == IdentityType.WindowsGroup)
					{
						flag = SynchronizeWindowsGroup(requestContext, item) || flag;
					}
				}
				return flag;
			}
		}
		return false;
	}

	private bool SynchronizeWindowsGroup(IGeminiRequestContext requestContext, Identity identity)
	{
		bool result = false;
		try
		{
			GeminiTrace.Verbose(TraceType.Security, "Now synchronizing {0}", identity.ToString());
			if (!_synchedGroups.ContainsKey(identity.Sid))
			{
				_synchedGroups.Add(identity.Sid, null);
				IdentityAccessor accessor = IdentityAccessor.GetAccessor(identity);
				if (accessor == null)
				{
					using (IIdentityCacheComponent identityCacheComponent = requestContext.CreateIdentityCacheComponent())
					{
						identityCacheComponent.DeleteIdentity(identity.Sid, isAdSync: true);
					}
					return true;
				}
				if (accessor is IdleAccessor)
				{
					GeminiTrace.Info("Idle Accessor returned.");
					return false;
				}
				if (accessor is SAMAccessor)
				{
					result = SynchronizeSAM(requestContext, identity, accessor as SAMAccessor);
				}
				else if (accessor is ActiveDirectoryAccessor)
				{
					result = SynchronizeADGroup(requestContext, identity, accessor as ActiveDirectoryAccessor);
				}
			}
			GeminiTrace.Info(TraceType.Security, "Finished synchronizing {0}", identity.ToString());
			return result;
		}
		catch (Exception ex)
		{
			GeminiTrace.Error(TraceType.Security, "A system error occurred synchronizing Active Directory. Exception message: {0}", ex.Message);
			GeminiTrace.TraceException(ex);
			return false;
		}
	}

	private bool SynchronizeSAM(IGeminiRequestContext requestContext, Identity identity, SAMAccessor accessor)
	{
		identity = accessor.QueryIdentity(requestContext, includeMembers: true);
		if (identity != null && identity.Members != null)
		{
			Tuple<bool, List<Identity>> tuple = UpdateMembers(requestContext, identity, identity.Members, (string sid) => false);
			bool flag = tuple.Item1;
			{
				foreach (Identity item in tuple.Item2)
				{
					if (item.Type == IdentityType.WindowsGroup)
					{
						flag = SynchronizeWindowsGroup(requestContext, item) || flag;
					}
				}
				return flag;
			}
		}
		return false;
	}

	private bool SynchronizeADGroup(IGeminiRequestContext requestContext, Identity identity, ActiveDirectoryAccessor accessor)
	{
		List<Identity> list = new List<Identity>();
		bool flag = false;
		try
		{
			if (!_verifiedDomains.Any((string f) => f.Equals(accessor.Domain, StringComparison.OrdinalIgnoreCase)) && !_failingDomains.Any((string f) => f.Equals(accessor.Domain, StringComparison.OrdinalIgnoreCase)))
			{
				accessor.InitializeDomainControllerConnection();
				_verifiedDomains.Add(accessor.Domain);
			}
		}
		catch (InvalidDomainException)
		{
			_failingDomains.Add(accessor.Domain);
		}
		try
		{
			using IIdentityCacheComponent identityCacheComponent = requestContext.CreateIdentityCacheComponent();
			ActiveDirectoryIdentityAccessor activeDirectoryIdentityAccessor = new ActiveDirectoryIdentityAccessor(identity, list);
			accessor.QueryIdentity(includeMembers: true, activeDirectoryIdentityAccessor.ADIdentityFound, activeDirectoryIdentityAccessor.ProcessADIdentity, _failingDomains);
			flag = activeDirectoryIdentityAccessor.Submit(identityCacheComponent, _currentUtcTime, _failingDomains);
		}
		catch (Exception ex2)
		{
			GeminiTrace.Error(TraceType.Security, "An error occurred when synchronizing the following Active Directory identity: {0}", identity.DisplayName);
			GeminiTrace.TraceException(TraceType.Security, ex2);
			return false;
		}
		foreach (Identity item in list)
		{
			if (item.Type == IdentityType.WindowsGroup)
			{
				flag = SynchronizeWindowsGroup(requestContext, item) || flag;
			}
		}
		return flag;
	}

	private Tuple<bool, List<Identity>> UpdateMembers(IGeminiRequestContext requestContext, Identity identity, IEnumerable<string> members, Func<string, bool> filterMember)
	{
		Dictionary<string, Identity> dictionary = new Dictionary<string, Identity>();
		List<Identity> list = new List<Identity>();
		List<Dictionary<string, Identity>> list2 = new List<Dictionary<string, Identity>>();
		list2.Add(dictionary);
		bool flag = false;
		bool initialCall = true;
		bool flag2 = false;
		try
		{
			if (members != null)
			{
				foreach (string member in members)
				{
					if (!filterMember(member))
					{
						bool flag3 = ReadIdentityFromAccessor(requestContext, member, dictionary, list);
						flag = flag || flag3;
						if (dictionary.Count >= 1000)
						{
							dictionary = new Dictionary<string, Identity>();
							list2.Add(dictionary);
						}
					}
				}
			}
			using IIdentityCacheComponent identityCacheComponent = requestContext.CreateIdentityCacheComponent();
			foreach (Dictionary<string, Identity> item in list2)
			{
				flag2 = identityCacheComponent.UpdateIdentity(identity, updateMembers: true, item.Values, _currentUtcTime, finalCall: true, ref initialCall, _failingDomains);
				flag = flag || flag2;
			}
			return new Tuple<bool, List<Identity>>(flag, list);
		}
		catch (Exception ex)
		{
			GeminiTrace.Error(TraceType.Security, "An error occurred when synchronizing the following role: {0}", identity.DisplayName);
			GeminiTrace.TraceException(ex);
			return new Tuple<bool, List<Identity>>(item1: false, list);
		}
	}

	private bool ReadIdentityFromAccessor(IGeminiRequestContext requestContext, string memberSid, Dictionary<string, Identity> membersList, List<Identity> identitiesToSynchronize)
	{
		Identity identity = null;
		try
		{
			IdentityAccessor accessor = IdentityAccessor.GetAccessor(requestContext, memberSid);
			if (accessor is ActiveDirectoryAccessor activeDirectoryAccessor)
			{
				try
				{
					if (!_verifiedDomains.Any((string f) => f.Equals(accessor.Domain, StringComparison.OrdinalIgnoreCase)) && !_failingDomains.Any((string f) => f.Equals(accessor.Domain, StringComparison.OrdinalIgnoreCase)))
					{
						activeDirectoryAccessor.InitializeDomainControllerConnection();
						_verifiedDomains.Add(accessor.Domain);
					}
				}
				catch (InvalidDomainException)
				{
					_failingDomains.Add(activeDirectoryAccessor.Domain);
				}
				identity = activeDirectoryAccessor.QueryIdentity(requestContext, includeMembers: false, _failingDomains);
			}
			else
			{
				if (accessor == null)
				{
					using (IIdentityCacheComponent identityCacheComponent = requestContext.CreateIdentityCacheComponent())
					{
						identityCacheComponent.DeleteIdentity(memberSid, isAdSync: true);
					}
					GeminiTrace.Verbose(TraceType.Security, "ReadIdentityFromAccessor was unable to accessor for sid {0}", memberSid);
					return true;
				}
				identity = accessor.QueryIdentity(requestContext, includeMembers: false);
			}
			if (identity != null)
			{
				if (string.IsNullOrEmpty(identity.Sid) || identity.Type == IdentityType.InvalidIdentity)
				{
					GeminiTrace.Error(TraceType.Security, "The identity {0} ({1}), is invalid and will not be synchronized", identity.DisplayName, identity.Sid);
					return false;
				}
				GeminiTrace.Verbose(TraceType.Security, "ReadIdentityFromAccessor successfully retrieved identity {0}", identity.ToString());
				identitiesToSynchronize.Add(identity);
			}
			else
			{
				GeminiTrace.Verbose(TraceType.Security, "ReadIdentityFromAccessor was unable to retrieve identity for sid {0}", memberSid);
			}
		}
		catch (ActiveDirectoryAccessException)
		{
			identity = IdentityCache.ReadIdentityFromCache(requestContext, memberSid, QueryMembership.Direct);
		}
		if (identity != null)
		{
			membersList[memberSid] = identity;
		}
		return false;
	}
}
