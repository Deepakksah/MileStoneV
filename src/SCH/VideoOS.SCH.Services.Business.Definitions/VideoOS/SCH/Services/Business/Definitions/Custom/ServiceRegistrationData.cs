using System;

namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class ServiceRegistrationData
{
	public Guid RegistrationType;

	public Guid RegistrationInstance;

	public string[] uris;

	public string RegistrationName;

	public string RegistrationDescription;

	public string CustomData;
}
