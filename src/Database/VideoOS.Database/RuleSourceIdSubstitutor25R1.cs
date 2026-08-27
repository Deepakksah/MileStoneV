using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using VideoOS.Common.Integration.Definition;
using VideoOS.Rules.Interfaces;

public static class RuleSourceIdSubstitutor25R1
{
	private static Guid serverGuid = SourceGuid.Server;

	private static Guid failoverServerStartedGuid = BuiltInEventTypes.EventIDs.ManagementServerFailoverNodeStarted;

	private static Guid anySource = SourceGuid.AnySource;

	private static IEnumerable<XElement> SourcesToSubstitute(XDocument doc)
	{
		return from e in doc.Descendants()
			where e.Name.LocalName == "SourceId" && e.Value.ToLower() == serverGuid.ToString().ToLower()
			where e.Parent?.Parent?.Parent != null && e.Parent.Parent.Parent.Name.LocalName == "RuleType" && e.Parent.Parent.Parent.Attributes().First().Value == "Event" && e.Parent.Parent.Parent.Descendants().Any((XElement d) => d.Name.LocalName == "EventType" && d.Value.ToLower() == failoverServerStartedGuid.ToString().ToLower())
			select e;
	}

	public static bool AnySourceIdsToSubstitute(string xml)
	{
		return SourcesToSubstitute(XDocument.Parse(xml)).Any();
	}

	public static string SubstituteSourceIds(string xml, bool formatOutput = false)
	{
		XDocument xDocument = XDocument.Parse(xml);
		foreach (XElement item in SourcesToSubstitute(xDocument))
		{
			item.Value = anySource.ToString().ToLower();
		}
		if (!formatOutput)
		{
			return xDocument.ToString(SaveOptions.DisableFormatting);
		}
		return xDocument.ToString();
	}
}
