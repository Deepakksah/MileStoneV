using System;
using System.Collections.Generic;

namespace VideoOS.Common.SharedComponent.ClientProfile.Handlers;

public sealed class SmartMapClientProfileSettingSpecificationNameHandler : IClientProfileSettingSpecificationNameHandler
{
	private static Dictionary<string, string> OemClientProfileSettingSpecificationsMapping { get; } = new Dictionary<string, string>();

	private IClientProfileSettingSpecificationNameHandler NextNameHandler { get; }

	public SmartMapClientProfileSettingSpecificationNameHandler(IClientProfileSettingSpecificationNameHandler nextNameHandler)
	{
		NextNameHandler = nextNameHandler ?? throw new ArgumentNullException("nextNameHandler");
	}

	public string Handle(string featureId, string clientProfileSettingSpecificationLocalizationId)
	{
		if (clientProfileSettingSpecificationLocalizationId == null)
		{
			throw new ArgumentNullException("clientProfileSettingSpecificationLocalizationId");
		}
		if (CanHandle(featureId, clientProfileSettingSpecificationLocalizationId))
		{
			if (OemClientProfileSettingSpecificationsMapping.TryGetValue(clientProfileSettingSpecificationLocalizationId, out var value))
			{
				string oemResource = ResourcesUtility.GetOemResource(value);
				if (string.IsNullOrEmpty(oemResource))
				{
					return string.Empty;
				}
				return oemResource;
			}
			throw new InvalidOperationException(FormattableString.Invariant($"You're missing an entry in the OemClientProfileSettingSpecificationsMapping dictionary for the key: {clientProfileSettingSpecificationLocalizationId}"));
		}
		return NextNameHandler.Handle(featureId, clientProfileSettingSpecificationLocalizationId);
	}

	public bool CanHandle(string featureId, string clientProfileSettingSpecificationLocalizationId)
	{
		if (clientProfileSettingSpecificationLocalizationId == null)
		{
			throw new ArgumentNullException("clientProfileSettingSpecificationLocalizationId");
		}
		if (featureId == "SmartMap")
		{
			return OemClientProfileSettingSpecificationsMapping.ContainsKey(clientProfileSettingSpecificationLocalizationId);
		}
		return false;
	}
}
