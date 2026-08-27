using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Crypto;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.Admin.Tasks;

public class CreateMediaDatabaseCertificateTask
{
	private string certificateIssuerOutput;

	public ILogger LogComponent { get; }

	public CreateMediaDatabaseCertificateTask(ILogger logComponent)
	{
		LogComponent = logComponent;
	}

	public void Create(IDatabaseConnectionInfo connectionInfoMs, string certificateIssuerFilePath, string publicKeyPath, string symmetricKey)
	{
		CreateMediDatabaseCertificate(connectionInfoMs, certificateIssuerFilePath, publicKeyPath, symmetricKey);
	}

	private bool CreateMediDatabaseCertificate(IDatabaseConnectionInfo connectionInfoMs, string certificateIssuerFilePath, string publicKeyPath, string symmetricKey)
	{
		string certificateFileName = Path.Combine(Environment.CurrentDirectory, "Certificate.xml");
		if (CreateCertificateFile(certificateIssuerFilePath, publicKeyPath, certificateFileName, ExecuteCertificateIssuer) != 0)
		{
			return false;
		}
		string text = LoadCertificateFile(certificateFileName);
		if (text != null)
		{
			InsertCertificateInDatabase(connectionInfoMs, text, symmetricKey);
		}
		return true;
	}

	private string LoadCertificateFile(string certificateFileName)
	{
		if (File.Exists(certificateFileName))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(certificateFileName);
			string outerXml = xmlDocument.OuterXml;
			File.Delete(Path.Combine(Environment.CurrentDirectory, "Certificate.xml"));
			return outerXml.Replace("'", "''");
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, CreateMediaDatabaseCertificateTask_Resources.Unable_to_find_the_certificate_file_File, certificateFileName), Array.Empty<object>());
		return null;
	}

	private void InsertCertificateInDatabase(IDatabaseConnectionInfo connectionInfoMs, string xmlCertificate, string symmetricKey)
	{
		new SqlConnectionManager(connectionInfoMs, LogComponent).ExecuteRetryable(delegate(DatabaseConnection sqlConnection)
		{
			bool flag = false;
			bool flag2 = false;
			using (DatabaseCommand databaseCommand = sqlConnection.CreateCommand("SELECT [setting] FROM [SystemSettings] WHERE [Name] = 'MediaDBCertificate'"))
			{
				string text = (string)databaseCommand.ExecuteScalar();
				if (!string.IsNullOrEmpty(text))
				{
					flag = true;
					flag2 = text == xmlCertificate;
					LoggerExtensions.LogDebug(LogComponent, CreateMediaDatabaseCertificateTask_Resources.Media_database_certificate_allready_exists_in_database, Array.Empty<object>());
				}
			}
			if (!flag || flag2)
			{
				using (DatabaseCommand databaseCommand2 = sqlConnection.CreateCommand())
				{
					databaseCommand2.CommandType = CommandType.Text;
					databaseCommand2.CommandText = "INSERT INTO SystemSettings([LastModified], [Name], [Setting]) VALUES (GETUTCDATE(), 'MediaDBCertificate', @Setting)";
					databaseCommand2.AddParameter("@Setting", SqlDbType.NVarChar, 0, EncryptionFactory.CreateSymmetric(symmetricKey).EncryptToBase64String(xmlCertificate));
					databaseCommand2.ExecuteNonQuery();
					LoggerExtensions.LogDebug(LogComponent, CreateMediaDatabaseCertificateTask_Resources.Inserted_new_certificate_into_database, Array.Empty<object>());
				}
			}
		});
	}

	private int ExecuteCertificateIssuer(ProcessStartInfo processStartInfo)
	{
		int num = 0;
		using Process process = Process.Start(processStartInfo);
		certificateIssuerOutput = process.StandardOutput.ReadToEnd();
		process.WaitForExit();
		return process.ExitCode;
	}

	private int CreateCertificateFile(string certificateIssuerFilePath, string publicKeyPath, string certificateFileName, Func<ProcessStartInfo, int> processExecutor)
	{
		string arguments = string.Format(CultureInfo.InvariantCulture, CreateMediaDatabaseCertificateTask_Resources.privatekey_publickey, certificateFileName, publicKeyPath);
		ProcessStartInfo processStartInfo = new ProcessStartInfo();
		processStartInfo.FileName = certificateIssuerFilePath;
		processStartInfo.Arguments = arguments;
		processStartInfo.UseShellExecute = false;
		processStartInfo.RedirectStandardOutput = true;
		processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		processStartInfo.CreateNoWindow = true;
		int num = 0;
		try
		{
			num = processExecutor(processStartInfo);
		}
		catch (Exception arg)
		{
			LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, CreateMediaDatabaseCertificateTask_Resources.Exception_caught_in_CertificateIssuer, arg), Array.Empty<object>());
			throw;
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, CreateMediaDatabaseCertificateTask_Resources.CertificateIssuer_output, num, certificateIssuerOutput), Array.Empty<object>());
		return num;
	}
}
