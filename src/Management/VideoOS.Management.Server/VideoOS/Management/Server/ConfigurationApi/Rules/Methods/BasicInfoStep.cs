using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Methods;

public class BasicInfoStep : IWizardStep
{
	private const string NamePropertyKey = "Name";

	private const string DescriptionPropertyKey = "Description";

	public bool IsComplete(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		if (item.IsPropertyValuePresent("Name"))
		{
			return item.IsPropertyPresent("Description");
		}
		return false;
	}

	public List<Property> GetCurrentProperties(IGeminiRequestContext requestContext, ConfigurationItem item, out string lastPropertyKey)
	{
		List<Property> list = new List<Property>();
		lastPropertyKey = null;
		if (item.TryGetProperty("Id", out Guid result))
		{
			list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(result));
		}
		if (item.TryGetProperty("Name", out var result2))
		{
			list.Add(GeneralPropertyDefinitions.GetNameProperty(result2, isSettable: true));
		}
		if (item.TryGetProperty("Description", out result2))
		{
			list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(result2, isSettable: true));
		}
		return list;
	}

	public List<Property> GetNextProperties(IGeminiRequestContext requestContext, ConfigurationItem item, string lastPropertyKey)
	{
		List<Property> list = new List<Property>();
		if (!item.IsPropertyPresent("Name"))
		{
			list.Add(GeneralPropertyDefinitions.GetNameProperty(string.Empty, isSettable: true));
		}
		if (!item.IsPropertyPresent("Description"))
		{
			list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(string.Empty, isSettable: true));
		}
		return list;
	}
}
