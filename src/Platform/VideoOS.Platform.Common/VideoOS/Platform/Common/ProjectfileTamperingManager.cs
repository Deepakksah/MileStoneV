using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace VideoOS.Platform.Common;

public class ProjectfileTamperingManager
{
	private const string _attributeName = "TamperingKey";

	public TamperingValidation DetectTampering(XmlDocument doc)
	{
		string text = Extract(doc);
		if (string.IsNullOrWhiteSpace(text))
		{
			return TamperingValidation.NotPossible;
		}
		string text2 = Compute(doc);
		if (!(text != text2))
		{
			return TamperingValidation.Untampered;
		}
		return TamperingValidation.Tampered;
	}

	public void AssignTamperingKey(XmlDocument doc)
	{
		string hash = Compute(doc);
		SetHash(doc, hash);
	}

	private static string Compute(XmlDocument doc)
	{
		doc.DocumentElement.RemoveAttribute("TamperingKey");
		return ComputeSha256(doc);
	}

	private static string ComputeSha256(XmlDocument doc)
	{
		using MemoryStream memoryStream = new MemoryStream();
		XmlWriter w = XmlWriter.Create(memoryStream);
		doc.WriteTo(w);
		memoryStream.Position = 0L;
		return Convert.ToBase64String(SHA1.Create().ComputeHash(memoryStream));
	}

	private static string Extract(XmlDocument doc)
	{
		return doc.DocumentElement.GetAttribute("TamperingKey");
	}

	private static void SetHash(XmlDocument doc, string hash)
	{
		doc.DocumentElement.SetAttribute("TamperingKey", hash);
	}
}
