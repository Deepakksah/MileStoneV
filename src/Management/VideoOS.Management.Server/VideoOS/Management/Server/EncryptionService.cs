using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Database.KeyManagement;

namespace VideoOS.Management.Server;

public class EncryptionService : IGeminiServerService, IEncryptionService
{
	private static readonly object _encryptionLocker = new object();

	private readonly string _manufacturerName;

	private readonly ILogger _logger;

	private static CorporateEncryptionSymmetric _corporateEncryptionSymmetric = null;

	public ISymmetricEncryption SymmetricEncryption => _corporateEncryptionSymmetric;

	public EncryptionService()
		: this(OemResources.Instance.App.CompanyName, GeminiTraceWrappingLogger.Instance)
	{
	}

	public EncryptionService(string manufacturerName, ILogger logger)
	{
		_manufacturerName = manufacturerName ?? throw new ArgumentNullException("manufacturerName");
		_logger = logger ?? throw new ArgumentNullException("logger");
	}

	public string Encrypt(string value)
	{
		return _corporateEncryptionSymmetric.EncryptToBase64String(value);
	}

	public byte[] Encrypt(byte[] value)
	{
		return _corporateEncryptionSymmetric.EncryptSequence(value);
	}

	public string Decrypt(string encryptedValue)
	{
		return _corporateEncryptionSymmetric.DecryptFromBase64String(encryptedValue);
	}

	public byte[] Decrypt(byte[] encryptedValue)
	{
		return _corporateEncryptionSymmetric.DecryptSequence(encryptedValue);
	}

	public bool TryDecrypt(string value)
	{
		try
		{
			Decrypt(value);
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	public bool TryDecrypt(string value, out string encryptedValue)
	{
		try
		{
			encryptedValue = Decrypt(value);
			return true;
		}
		catch (Exception)
		{
			encryptedValue = "";
			return false;
		}
	}

	public void StartService(IGeminiRequestContext requestContext)
	{
		if (_corporateEncryptionSymmetric != null)
		{
			return;
		}
		lock (_encryptionLocker)
		{
			if (_corporateEncryptionSymmetric == null)
			{
				ISqlConnectionManager sqlConnectionManager = requestContext.GetDatabaseSettings().GetSqlConnectionManager();
				_corporateEncryptionSymmetric = new CorporateEncryptionSymmetric(Convert.ToBase64String(KeyManagerFactory.GetEncryptionKeyReader(_manufacturerName, sqlConnectionManager, _logger).GetUnencryptedKey()));
			}
		}
	}

	public void StopService(IGeminiRequestContext requestContext)
	{
	}
}
