using System;
using System.Configuration;
using System.ServiceModel.Configuration;
using VideoOS.Utilities.Validation;

namespace VideoOS.Utilities.Configuration;

public interface IConfigurationFileController
{
	Uri GetUriAppSetting([ValidatedNotNull] string keyName);

	void SetOrUpdateAppSetting([ValidatedNotNull] string keyName, [ValidatedNotNull] string value);

	System.Configuration.Configuration CreateConfiguration();

	ServiceElement GetServiceElementConfiguration([ValidatedNotNull] System.Configuration.Configuration openMappedConfiguration, [ValidatedNotNull] string serviceName);
}
