using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace VideoOS.Installer.Core.Xml;

public static class XmlValidator
{
	[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "We kjust want to know if the stream is valid or not, We don't care what is wrong")]
	private static bool Validate(Stream xmlStream, Stream xsdStream, bool handleErrors)
	{
		bool result = true;
		if (xmlStream == null)
		{
			throw new ArgumentNullException("xmlStream");
		}
		if (xsdStream == null)
		{
			throw new ArgumentNullException("xsdStream");
		}
		xmlStream.Seek(0L, SeekOrigin.Begin);
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
		{
			ValidationType = ValidationType.Schema,
			IgnoreWhitespace = true
		};
		XmlSchema schema = XmlSchema.Read(xsdStream, null);
		xmlReaderSettings.Schemas.Add(schema);
		if (handleErrors)
		{
			xmlReaderSettings.ValidationEventHandler += delegate(object obj, ValidationEventArgs args)
			{
				if (args.Severity == XmlSeverityType.Error)
				{
					result = false;
				}
			};
		}
		using (XmlReader xmlReader = XmlReader.Create(xmlStream, xmlReaderSettings))
		{
			while (xmlReader.Read())
			{
			}
		}
		xmlStream.Position = 0L;
		xsdStream.Position = 0L;
		return result;
	}

	public static void Validate(Stream xmlStream, Stream xsdStream)
	{
		Validate(xmlStream, xsdStream, handleErrors: false);
	}

	public static bool TryValidate(Stream xmlStream, Stream xsdStream)
	{
		return Validate(xmlStream, xsdStream, handleErrors: true);
	}
}
