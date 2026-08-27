using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using VideoOS.Utilities.Validation;

namespace VideoOS.Common.Integration.Configuration;

public class SettingsXmlTransformerImplementation : ISettingsXmlTransformer
{
	private enum RewriteKind
	{
		Encrypt,
		Decrypt,
		Transform
	}

	private class StringReaderWithPosition : StringReader
	{
		private int _readPosition;

		public int Position => _readPosition;

		public StringReaderWithPosition(string s)
			: base(s)
		{
		}

		public override int Read(char[] buffer, int index, int count)
		{
			int num = base.Read(buffer, index, count);
			_readPosition += num;
			return num;
		}

		public override int Read()
		{
			int num = base.Read();
			_readPosition += num;
			return num;
		}
	}

	private readonly HashSet<string> _encryptedSettingsIds;

	private readonly string _encryptionMarker;

	public SettingsXmlTransformerImplementation(HashSet<string> encryptedSettingsIds, string encryptionMarker)
	{
		_encryptedSettingsIds = encryptedSettingsIds;
		_encryptionMarker = encryptionMarker;
	}

	public string Encrypt(string settingsXml, Func<string, string> encryptFunc)
	{
		settingsXml.NullCheck("", "Encrypt", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\Configuration\\Encryption\\SettingsXmlTransformer.cs", 57);
		encryptFunc.NullCheck("", "Encrypt", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\Configuration\\Encryption\\SettingsXmlTransformer.cs", 58);
		return RewriteXml(settingsXml, encryptFunc, RewriteKind.Encrypt);
	}

	public string Decrypt(string settingsXml, Func<string, string> decryptFunc)
	{
		settingsXml.NullCheck("", "Decrypt", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\Configuration\\Encryption\\SettingsXmlTransformer.cs", 71);
		decryptFunc.NullCheck("", "Decrypt", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\Configuration\\Encryption\\SettingsXmlTransformer.cs", 72);
		return RewriteXml(settingsXml, decryptFunc, RewriteKind.Decrypt);
	}

	public string Transform(string settingsXml, Func<string, string> transformFunc)
	{
		settingsXml.NullCheck("", "Transform", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\Configuration\\Encryption\\SettingsXmlTransformer.cs", 85);
		transformFunc.NullCheck("", "Transform", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\Configuration\\Encryption\\SettingsXmlTransformer.cs", 86);
		return RewriteXml(settingsXml, transformFunc, RewriteKind.Transform);
	}

	private string RewriteXml(string source, Func<string, string> valueTransformFunc, RewriteKind kind)
	{
		if (string.IsNullOrEmpty(source))
		{
			return source;
		}
		if (!ContainsEncryptedSettings(source))
		{
			return source;
		}
		if (source.Contains(Environment.NewLine))
		{
			source = source.Replace(Environment.NewLine, "");
		}
		List<(int, string, string)> substitutions = FindSubstitutions(source, valueTransformFunc, kind);
		return PerformSubstitutions(source, substitutions);
	}

	private List<(int, string, string)> FindSubstitutions(string source, Func<string, string> valueTransformFunc, RewriteKind kind)
	{
		List<(int, string, string)> list = new List<(int, string, string)>();
		StringReader stringReader = null;
		try
		{
			stringReader = new StringReader(source);
			using XmlReader xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings
			{
				IgnoreWhitespace = true,
				CheckCharacters = false,
				ValidationType = ValidationType.None
			});
			stringReader = null;
			if (!(xmlReader is IXmlLineInfo xmlLineInfo) || !xmlLineInfo.HasLineInfo())
			{
				throw new NotSupportedException("The hardware settings XML reader does not implement line info.");
			}
			while (xmlReader.ReadToFollowing("setting"))
			{
				if (!xmlReader.MoveToAttribute("typeid") || !_encryptedSettingsIds.Contains(xmlReader.Value) || !xmlReader.MoveToElement() || !xmlReader.ReadToDescendant("value") || !xmlReader.Read())
				{
					continue;
				}
				int linePosition = xmlLineInfo.LinePosition;
				string text = (string)xmlReader.ReadContentAs(typeof(string), null);
				if (!string.IsNullOrEmpty(text) && (kind != RewriteKind.Decrypt || text.StartsWith(_encryptionMarker, StringComparison.Ordinal)))
				{
					list.Add((linePosition - 1, kind switch
					{
						RewriteKind.Encrypt => _encryptionMarker + valueTransformFunc(text), 
						RewriteKind.Decrypt => valueTransformFunc(text.Substring(_encryptionMarker.Length)), 
						_ => valueTransformFunc(text), 
					}, text));
					if (list.Count == _encryptedSettingsIds.Count)
					{
						break;
					}
				}
			}
		}
		finally
		{
			stringReader?.Dispose();
		}
		return list;
	}

	private static string PerformSubstitutions(string source, List<(int, string, string)> substitutions)
	{
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		using StringReaderWithPosition stringReaderWithPosition = new StringReaderWithPosition(source);
		foreach (var substitution in substitutions)
		{
			int item = substitution.Item1;
			string item2 = substitution.Item2;
			string item3 = substitution.Item3;
			int num = item - stringReaderWithPosition.Position;
			char[] buffer = new char[Math.Max(num, item3.Length)];
			stringReaderWithPosition.Read(buffer, 0, num);
			stringWriter.Write(buffer, 0, num);
			stringReaderWithPosition.Read(buffer, 0, item3.Length);
			stringWriter.Write(item2);
		}
		stringWriter.Write(stringReaderWithPosition.ReadToEnd());
		return stringWriter.ToString();
	}

	private bool ContainsEncryptedSettings(string unencryptedSettingsXml)
	{
		foreach (string encryptedSettingsId in _encryptedSettingsIds)
		{
			if (unencryptedSettingsXml.IndexOf(encryptedSettingsId, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return true;
			}
		}
		return false;
	}
}
