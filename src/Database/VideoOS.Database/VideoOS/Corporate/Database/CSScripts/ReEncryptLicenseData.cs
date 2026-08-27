using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Crypto;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Database.KeyManagement;
using VideoOS.Utilities.Validation;

namespace VideoOS.Corporate.Database.CSScripts;

public static class ReEncryptLicenseData
{
	public static void Update(DatabaseConnection connection, ILogger logger, IEncryptionKeyReader keyReader)
	{
		ISymmetricEncryption symmetricEncryption = EncryptionFactory.CreateSymmetric(keyReader.GetUnencryptedKey());
		List<(Guid, string)> list = new List<(Guid, string)>();
		using (DatabaseCommand databaseCommand = connection.CreateCommand())
		{
			databaseCommand.CommandType = CommandType.Text;
			databaseCommand.CommandText = "SELECT [IDLicense], [Data] FROM [License]";
			using (DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader())
			{
				while (databaseDataReader.Read())
				{
					Guid guid = databaseDataReader.GetGuid(0);
					LoggerExtensions.LogInformation(logger, "Processing license with ID: {LicenseId}", new object[1] { guid });
					byte[] array = ReadBytes(databaseDataReader, 1);
					if (array.Length == 0)
					{
						continue;
					}
					byte[] bytes = symmetricEncryption.DecryptSequence(array);
					if (!IsValidUTF8XmlByteArray(bytes))
					{
						string text = Encoding.Unicode.GetString(bytes);
						string text2 = "";
						if (Base64StringValidator.IsValidBase64String(text))
						{
							byte[] bytes2 = Convert.FromBase64String(text);
							text2 = Encoding.Unicode.GetString(bytes2);
						}
						else
						{
							text2 = text;
						}
						list.Add((guid, text2));
					}
				}
			}
			foreach (var item in list)
			{
				byte[] bytes3 = Encoding.UTF8.GetBytes(item.Item2);
				byte[] value = symmetricEncryption.EncryptSequence(bytes3);
				using DatabaseCommand databaseCommand2 = connection.CreateCommand();
				LoggerExtensions.LogInformation(logger, "Updating license with ID: {LicenseId}", new object[1] { item.Item1 });
				databaseCommand2.CommandType = CommandType.Text;
				databaseCommand2.CommandText = "UPDATE [License] Set [Data] = @Data WHERE [IDLicense] = @IDLicense";
				databaseCommand2.AddParameter("@Data", SqlDbType.VarBinary, 0, value);
				databaseCommand2.AddParameter("@IDLicense", SqlDbType.UniqueIdentifier, item.Item1);
				databaseCommand2.ExecuteNonQuery();
			}
		}
		static bool IsValidUTF8XmlByteArray(byte[] bytes4)
		{
			UTF8Encoding uTF8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
			string xml;
			try
			{
				xml = uTF8Encoding.GetString(bytes4);
			}
			catch (ArgumentException)
			{
				return false;
			}
			try
			{
				new XmlDocument().LoadXml(xml);
				return true;
			}
			catch (XmlException)
			{
				return false;
			}
		}
		static byte[] ReadBytes(DatabaseDataReader reader, int ordinal)
		{
			using MemoryStream memoryStream = new MemoryStream();
			using Stream stream = reader.GetStream(ordinal);
			stream.CopyTo(memoryStream);
			return memoryStream.ToArray();
		}
	}
}
