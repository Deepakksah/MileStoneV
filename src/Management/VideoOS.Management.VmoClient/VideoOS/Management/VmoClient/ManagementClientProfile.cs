using System;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("ManagementClientProfile")]
public class ManagementClientProfile : ClientProfile
{
	private bool? _isAdministrator;

	public ManagementClientProfile(ManagementServer managementServer)
		: base(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), managementServer.Repositories, isInitialized: true)
	{
	}

	public ManagementClientProfile(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: false)
	{
	}

	public ManagementClientProfile(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public bool IsElementVisible(string elementId)
	{
		if (!_isAdministrator.HasValue)
		{
			_isAdministrator = base.Repositories.ServerCommunication.IsAdministrator();
		}
		if (_isAdministrator.Value)
		{
			return true;
		}
		return !ClientProfileSettings.GetFromXml(base.SettingsXml).Any((ClientProfileSetting f) => string.Equals(f.Key, elementId, StringComparison.Ordinal));
	}
}
