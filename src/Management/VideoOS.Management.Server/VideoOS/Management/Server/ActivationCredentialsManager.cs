using System;
using System.Xml;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class ActivationCredentialsManager : ISystemSettingManager
{
	public string ConvertToDatabaseValue(IGeminiRequestContext requestContext, string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			try
			{
				ActivationCredentials activationCredentials = ActivationCredentials.Parse(value);
				if (string.IsNullOrEmpty(activationCredentials.Password))
				{
					throw new ArgumentException("Password in 'ActivationCredentials' cannot be empty.");
				}
				string encryptedValue = "";
				if (requestContext.GetEncryptionService().TryDecrypt(activationCredentials.Password, out encryptedValue))
				{
					activationCredentials.Password = encryptedValue;
				}
				return requestContext.GetEncryptionService().Encrypt(activationCredentials.Xml);
			}
			catch (FormatException)
			{
				throw new SystemSettingInvalidValueException(SupportedSystemSettings.ActivationCredentials, value);
			}
		}
		return string.Empty;
	}

	public string ConvertToInternalServerValue(IGeminiRequestContext requestContext, string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}
		return requestContext.GetEncryptionService().Decrypt(value);
	}

	public string ConvertToClientValue(IGeminiRequestContext requestContext, string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}
		string text = requestContext.GetEncryptionService().Decrypt(value);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode lastChild = xmlDocument.LastChild.LastChild;
		if (requestContext.GetEncryptionService().TryDecrypt(lastChild.InnerText))
		{
			return text;
		}
		lastChild.InnerText = requestContext.GetEncryptionService().Encrypt(lastChild.InnerText);
		return xmlDocument.OuterXml;
	}

	public void CheckGetPermission(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
	}

	public void CheckSetPermission(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
	}
}
