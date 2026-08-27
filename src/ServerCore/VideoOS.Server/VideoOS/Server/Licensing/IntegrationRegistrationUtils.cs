using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Server.DataComponents;

namespace VideoOS.Server.Licensing;

public static class IntegrationRegistrationUtils
{
	public static List<IntegrationRegistration> ParseRows(IEnumerable<IntegrationRegistrationRow> rows)
	{
		List<IntegrationRegistration> list = new List<IntegrationRegistration>();
		if (rows == null || !rows.Any())
		{
			return list;
		}
		List<IGrouping<Guid, IntegrationRegistrationRow>> list2 = (from i in rows
			where Enum.TryParse<IntegrationType>(i.IntegrationType, out var _)
			group i by i.IntegrationId).ToList();
		foreach (IGrouping<Guid, IntegrationRegistrationRow> item in list2)
		{
			IntegrationRegistrationRow integrationRegistrationRow = item.OrderByDescending((IntegrationRegistrationRow i) => i.RegistrationTime).First();
			IEnumerable<string> versions = item.Select((IntegrationRegistrationRow x) => x.IntegrationVersion).Distinct();
			Dictionary<IntegrationType, int> typeToInstanceCount = (from x in item
				group x by x.IntegrationType into x
				select new KeyValuePair<IntegrationType, int>((IntegrationType)Enum.Parse(typeof(IntegrationType), x.Key), x.Sum((IntegrationRegistrationRow y) => y.VersionCount))).ToDictionary((KeyValuePair<IntegrationType, int> x) => x.Key, (KeyValuePair<IntegrationType, int> x) => x.Value);
			list.Add(new IntegrationRegistration(item.Key, integrationRegistrationRow.IntegrationName, integrationRegistrationRow.ManufacturerName, versions, typeToInstanceCount, integrationRegistrationRow.AllowIntegrationRegistration));
		}
		return list;
	}
}
