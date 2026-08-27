using System;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public sealed class IntegrationRegistration
{
	private const string ManufacturerNameToExclude = "Sample Manufacturer";

	private IntegrationRegistration()
	{
	}

	public static void CreateIntegrationRegistration(IGeminiRequestContext requestContext, string instanceId, Guid integrationId, IntegrationType integrationType, string integrationVersion, string integrationName, string manufacturerName)
	{
		Validation.CheckStringNotNullOrWhiteSpace(manufacturerName, "manufacturerName");
		if (manufacturerName.Equals("Sample Manufacturer"))
		{
			return;
		}
		Validation.CheckGuidNotNullOrEmpty(integrationId, "integrationId");
		Validation.CheckStringNotNullOrWhiteSpace(instanceId, "instanceId");
		Validation.CheckStringNotNullOrWhiteSpace(integrationType.ToString(), "integrationType");
		Validation.CheckStringNotNullOrWhiteSpace(integrationVersion, "integrationVersion");
		Validation.CheckStringNotNullOrWhiteSpace(integrationName, "integrationName");
		using IIntegrationRegistrationsComponent integrationRegistrationsComponent = requestContext.CreateIntegrationRegistrationsComponent();
		integrationRegistrationsComponent.CreateIntegrationRegistration(instanceId, integrationId, integrationType, integrationVersion, integrationName, manufacturerName, allowIntegrationRegistration: true);
	}

	public static void CreatePluginRegistrations(IGeminiRequestContext requestContext, string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations)
	{
		Validation.CheckStringNotNullOrWhiteSpace(instanceId, "instanceId");
		Validation.CheckStringNotNullOrWhiteSpace(integrationType.ToString(), "integrationType");
		using IIntegrationRegistrationsComponent integrationRegistrationsComponent = requestContext.CreateIntegrationRegistrationsComponent();
		foreach (PluginIntegration pluginIntegration in pluginIntegrations)
		{
			Validation.CheckStringNotNullOrWhiteSpace(pluginIntegration.ManufacturerName, "ManufacturerName");
			if (!pluginIntegration.ManufacturerName.Equals("Sample Manufacturer"))
			{
				Validation.CheckGuidNotNullOrEmpty(pluginIntegration.IntegrationId, "IntegrationId");
				Validation.CheckStringNotNullOrWhiteSpace(pluginIntegration.IntegrationVersion, "IntegrationVersion");
				Validation.CheckStringNotNullOrWhiteSpace(pluginIntegration.IntegrationName, "IntegrationName");
				integrationRegistrationsComponent.CreateIntegrationRegistration(instanceId, pluginIntegration.IntegrationId, integrationType, pluginIntegration.IntegrationVersion, pluginIntegration.IntegrationName, pluginIntegration.ManufacturerName, pluginIntegration.AllowIntegrationRegistration);
			}
		}
	}
}
