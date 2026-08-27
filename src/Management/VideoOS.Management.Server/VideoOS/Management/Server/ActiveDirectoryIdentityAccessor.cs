using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class ActiveDirectoryIdentityAccessor
{
	private Identity _group;

	private List<Identity> _membersToSync;

	private Dictionary<string, object> _foundDistinguishedNames;

	private List<List<Identity>> _identitiesBulk;

	private List<Identity> _identities;

	private bool _initialCall;

	public ActiveDirectoryIdentityAccessor(Identity groupIdentity, List<Identity> membersToSync)
	{
		_membersToSync = membersToSync;
		_group = groupIdentity;
		_foundDistinguishedNames = new Dictionary<string, object>();
		_identities = new List<Identity>();
		_identitiesBulk = new List<List<Identity>>();
		_initialCall = true;
	}

	public bool ADIdentityFound(string distinguishedName)
	{
		return _foundDistinguishedNames.ContainsKey(distinguishedName);
	}

	public void ProcessADIdentity(Identity identity)
	{
		GeminiTrace.Info(TraceType.Security, "Background synchronization is processing Identity {0}", identity.ToString());
		_foundDistinguishedNames.Add(identity.DistinguishedName, null);
		if (identity.Type == IdentityType.WindowsGroup)
		{
			_membersToSync.Add(identity);
		}
		if (_identities.Count >= 250)
		{
			_identitiesBulk.Add(_identities);
			_identities = new List<Identity>();
		}
		_identities.Add(identity);
	}

	public bool Submit(IIdentityCacheComponent identityCacheComponent, DateTime currentUtcTime, ICollection<string> failingDomains)
	{
		bool changeStatus = false;
		foreach (List<Identity> item in _identitiesBulk)
		{
			changeStatus = EvaluateDatabaseChanged(identityCacheComponent, currentUtcTime, _group, changeStatus, isFinalCall: false, item, failingDomains);
		}
		return EvaluateDatabaseChanged(identityCacheComponent, currentUtcTime, _group, changeStatus, isFinalCall: true, _identities, failingDomains);
	}

	private bool EvaluateDatabaseChanged(IIdentityCacheComponent identityCacheComponent, DateTime currentUtcTime, Identity group, bool changeStatus, bool isFinalCall, ICollection<Identity> members, ICollection<string> failingDomains)
	{
		bool flag = identityCacheComponent.UpdateIdentity(group, updateMembers: true, members, currentUtcTime, isFinalCall, ref _initialCall, failingDomains);
		return changeStatus || flag;
	}
}
