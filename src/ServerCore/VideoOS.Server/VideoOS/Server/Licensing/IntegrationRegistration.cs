using System;
using System.Collections.Generic;

namespace VideoOS.Server.Licensing;

public sealed class IntegrationRegistration
{
	public Guid Id { get; }

	public string Name { get; }

	public HashSet<string> Versions { get; }

	public string Manufacturer { get; }

	public Dictionary<IntegrationType, int> TypeToInstanceCount { get; }

	private bool AllowIntegrationRegistration { get; }

	public IntegrationRegistration(Guid id, string name, string manufacturer, IEnumerable<string> versions, Dictionary<IntegrationType, int> typeToInstanceCount, bool allowIntegrationRegistration)
	{
		Id = id;
		Name = name;
		Manufacturer = manufacturer;
		Versions = new HashSet<string>(versions);
		TypeToInstanceCount = typeToInstanceCount;
		AllowIntegrationRegistration = allowIntegrationRegistration;
	}

	public IntegrationRegistration(Guid id, string name, string manufacturer, string version, Dictionary<IntegrationType, int> typeToInstanceCount, bool allowIntegrationRegistration)
		: this(id, name, manufacturer, new List<string> { version }, typeToInstanceCount, allowIntegrationRegistration)
	{
	}
}
