using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VideoOS.Common.Interfaces.Communication;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

public sealed class LicenseActivationService : ILicenseActivationService
{
	private readonly ILogger _logger;

	private readonly Uri _activationUrl;

	private readonly int _timeout;

	private readonly IHttpPost _httpPost;

	public LicenseActivationService(IHttpPost httpPost, ILogger logger, Uri activationUrl, int timeout)
	{
		_httpPost = httpPost ?? throw new ArgumentNullException("httpPost");
		_logger = logger ?? throw new ArgumentNullException("logger");
		if (activationUrl == null)
		{
			throw new ArgumentNullException("activationUrl");
		}
		if (!activationUrl.IsAbsoluteUri)
		{
			throw new ArgumentException("Must contain an absolute Uri", "activationUrl");
		}
		if (timeout < 1)
		{
			throw new ArgumentOutOfRangeException("timeout");
		}
		_activationUrl = activationUrl;
		_timeout = timeout;
	}

	public byte[] AcquireLicense(string userName, string password, byte[] request)
	{
		NameValueCollection nameValueCollection = new NameValueCollection();
		string text = Convert.ToBase64String(Encoding.ASCII.GetBytes(FormattableString.Invariant($"{userName}:{password}")));
		nameValueCollection.Add("Authorization", "Basic " + text);
		nameValueCollection.Add("VMS-Version", AssemblyProductInfo.Major + "." + AssemblyProductInfo.Minor);
		LicenseActivationContainer licenseActivationContainer = new LicenseActivationContainer(Convert.ToBase64String(request));
		LicenseResponseContainer licenseResponseContainer = JsonConvert.DeserializeObject<LicenseResponseContainer>(_httpPost.Post(_activationUrl, JsonConvert.SerializeObject((object)licenseActivationContainer), _timeout, nameValueCollection, "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0)", "application/json"));
		LoggerExtensions.LogInformation(_logger, "Online activation - CreateLicense result: {LicenseResponseStatus}", new object[1] { licenseResponseContainer.Status });
		return licenseResponseContainer.Status switch
		{
			"Success" => Convert.FromBase64String(licenseResponseContainer.LicenseFile), 
			"InternalServerError" => throw new LicenseException("Internal server error.", LicenseErrorNumber.GeneralLicenseError), 
			"RequestFormatInvalid" => throw new LicenseException("The format of the activation request was invalid.", LicenseErrorNumber.GeneralLicenseError), 
			"AccessDenied" => throw NewAccessDeniedLicenseException(licenseResponseContainer), 
			"LicenseNotAllowed" => throw NewLicenseNotAllowedLicenseException(licenseResponseContainer), 
			_ => throw new LicenseException("Could not obtain a new license.", LicenseErrorNumber.GeneralLicenseError), 
		};
	}

	private static LicenseException NewLicenseNotAllowedLicenseException(LicenseResponseContainer licenseResponse)
	{
		switch (licenseResponse.ErrorMessage)
		{
		case "TooManyCamerasRequested":
		case "ToManyCamerasRequested":
			return new LicenseException("Too many licenses for camera feeds requested.", LicenseErrorNumber.TooManyCamerasRequested);
		case "ToManyHardwareDevicesRequested":
		case "TooManyHardwareDevicesRequested":
			return new LicenseException("Too many licenses for hardware requested.", LicenseErrorNumber.TooManyHardwareDevicesRequested);
		case "ToManyInterconnectedCamerasRequested":
		case "TooManyInterconnectedCamerasRequested":
			return new LicenseException(string.Format(CultureInfo.CurrentCulture, "Too many licenses for {0} camera feeds requested.", OemResources.Instance.Server.ICPApplicationName), LicenseErrorNumber.TooManyInterconnectedCamerasRequested);
		case "ToManyRecordersRequested":
		case "TooManyRecordersRequested":
			return new LicenseException("Too many recording server licenses requested.", LicenseErrorNumber.TooManyRecordersRequested);
		case "FeatureNotRegistered":
			return new LicenseException("The SLC for an installed product was not registered.", LicenseErrorNumber.SlcNotRegistered);
		case "FeatureAlreadyInUse":
			return new LicenseException("The SLC for an installed product was already registered on another system.", LicenseErrorNumber.SlcAlreadyInUse);
		case "ActivationLimitExceeded":
		case "DlkRemovalExceeded":
			return new LicenseException("Too much hardware has been replaced.", LicenseErrorNumber.ActivationLimitExceeded);
		default:
			return new LicenseException("The requested license could not be granted.", LicenseErrorNumber.GeneralLicenseError);
		}
	}

	private static LicenseException NewAccessDeniedLicenseException(LicenseResponseContainer licenseResponse)
	{
		return licenseResponse.ErrorMessage switch
		{
			"UsernameAndPasswordNotFound" => new LicenseException("The specified user name or password was not correct.", LicenseErrorNumber.UserNameAndPasswordNotFound), 
			"SlcNotFound" => new LicenseException("The SLC was not registered.", LicenseErrorNumber.SlcNotFound), 
			"SlcRegisteredOnOtherLogin" => new LicenseException("The specified user is not allowed to activate this system.", LicenseErrorNumber.SlcRegisteredOnOtherLogOn), 
			"MismatchOnComputerSID" => new LicenseException("The license has been activated on multiple systems.", LicenseErrorNumber.MismatchOnComputerSid), 
			"CustomerIsNotCdsValidated" => new LicenseException("Cannot activate your licenses. You need to register your SLC first.", LicenseErrorNumber.CustomerIsNotCdsValidated), 
			_ => new LicenseException("Access denied.", LicenseErrorNumber.AccessDenied), 
		};
	}
}
