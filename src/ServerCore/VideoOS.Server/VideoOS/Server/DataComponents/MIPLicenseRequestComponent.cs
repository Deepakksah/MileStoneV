using System;
using System.Text;

namespace VideoOS.Server.DataComponents;

public sealed class MIPLicenseRequestComponent : VmoResourceComponent, IMIPLicenseRequestComponent, IDisposable
{
	public MIPLicenseRequestComponent()
	{
	}

	public string QueryMIPLicenseRequest()
	{
		string sql = "SELECT [Data] FROM [License] WHERE [IDLicense] = 'B1CB8385-EC7E-492B-B046-1B84E378CB71'";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			if (!(ExecuteScalar() is byte[] value))
			{
				return string.Empty;
			}
			byte[] bytes = Server.Instance.CorporateEncryptionSymmetric.DecryptSequence(value);
			return Encoding.UTF8.GetString(bytes);
		});
	}
}
