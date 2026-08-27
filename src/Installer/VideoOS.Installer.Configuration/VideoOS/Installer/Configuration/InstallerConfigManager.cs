using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Installer.ProductConfig;
using VideoOS.Installer.Configuration.Description;
using VideoOS.Installer.Configuration.Package;
using VideoOS.Installer.Configuration.TreckDescriptions;
using VideoOS.Installer.Core.Arguments;
using VideoOS.Installer.Core.Arguments.Interfaces;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Configuration;

public class InstallerConfigManager
{
	private const string _treckDescFilename = "TrackDescriptions.xml";

	public static PackageType LoadPackageFromFile(string filename, string curDir)
	{
		PackageType packageType = null;
		filename = Path.Combine(curDir, filename);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(PackageType));
		try
		{
			using TextReader textReader = new StreamReader(filename);
			try
			{
				return (PackageType)xmlSerializer.Deserialize(textReader);
			}
			catch (InvalidOperationException ex)
			{
				LogManager.Write($"Load of XML file failed: {ex.TypeAndMessage()}.", TraceEventType.Critical);
				throw;
			}
			catch (Exception ex2)
			{
				LogManager.Write($"Load of XML file failed: {ex2.TypeAndMessage()}.", TraceEventType.Critical);
				throw;
			}
		}
		catch (FileNotFoundException ex3)
		{
			LogManager.Write(ex3.TypeAndMessage(), TraceEventType.Critical);
			throw new ArgumentException("File not found.", filename);
		}
	}

	public static Product LoadProduct(string filename)
	{
		Product product = null;
		LogManager.Write($"Loading {filename}...", TraceEventType.Information);
		try
		{
			using TextReader stream = new StreamReader(filename);
			return LoadProduct(stream);
		}
		catch (FileNotFoundException ex)
		{
			LogManager.Write(ex.TypeAndMessage(), TraceEventType.Critical);
			throw new ArgumentException("File not found.", filename);
		}
	}

	public static Product LoadProduct(TextReader stream)
	{
		Product product = null;
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(Product));
		try
		{
			return (Product)xmlSerializer.Deserialize(stream);
		}
		catch (InvalidOperationException ex)
		{
			LogManager.Write($"Load of XML file failed: {ex.TypeAndMessage()}.", TraceEventType.Critical);
			throw;
		}
		catch (Exception ex2)
		{
			LogManager.Write($"Load of XML file failed: {ex2.TypeAndMessage()}.", TraceEventType.Critical);
			throw;
		}
	}

	public static InstallerConfig LoadInstallerConfig(string filename, bool is64BitOS)
	{
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		return LoadInstallerConfigFromFile(filename, directoryName, is64BitOS);
	}

	public static InstallerConfig LoadInstallerConfigFromFile(string filename, string curDir, bool is64bitOS)
	{
		XmlSerializer xmlSerializer = null;
		InstallerConfig installerConfig = null;
		filename = Path.Combine(curDir, filename);
		LogManager.Write($"Loading {filename}...", TraceEventType.Information);
		try
		{
			xmlSerializer = new XmlSerializer(typeof(InstallerConfig));
		}
		catch (Exception ex)
		{
			LogManager.Write($"Load of XML file failed: {ex.TypeAndMessage()}.", TraceEventType.Critical);
			throw;
		}
		using TextReader textReader = new StringReader(ApplyGlobalPrerequisites(filename).OuterXml);
		try
		{
			installerConfig = (InstallerConfig)xmlSerializer.Deserialize(textReader);
			List<int> list = new List<int>();
			int num = 0;
			InstallerPackageType[] installerPackages = installerConfig.InstallerPackages;
			foreach (InstallerPackageType installerPackageType in installerPackages)
			{
				if (installerPackageType.ProductManifest.Format == FormatType.Milestone)
				{
					string filename2 = installerPackageType.ProductManifest.Filename;
					if (installerPackageType.ProductManifest64 != null && is64bitOS)
					{
						if (File.Exists(Path.Combine(curDir, installerPackageType.ProductManifest64.Filename)))
						{
							filename2 = installerPackageType.ProductManifest64.Filename;
							installerPackageType.ProductManifest.Platform = "x64";
							installerPackageType.ProductManifest64.Platform = "x86";
							LogManager.Write($"Loading 64 bit version of the package {filename2}");
						}
						else
						{
							LogManager.Write($"64 bit file '{installerPackageType.ProductManifest64.Filename}' was not found so attempt to read 32 bit version of this file '{filename2}'", TraceEventType.Warning);
						}
					}
					try
					{
						installerPackageType.ProductManifest.ManifestData = LoadPackageFromFile(filename2, curDir);
					}
					catch (Exception)
					{
						LogManager.Write($"Could not load package {installerPackageType.ProductManifest.Filename}.", TraceEventType.Critical);
						list.Add(num);
					}
					num++;
				}
				if (installerPackageType.ProductManifest.Format == FormatType.Microsoft)
				{
					installerPackageType.ProductManifest.ManifestData = LoadProduct(installerPackageType.ProductManifest.Filename);
				}
			}
			int num2 = 0;
			foreach (int item in list)
			{
				int num3 = item - num2;
				string iD = installerConfig.InstallerPackages[num3].ID;
				InstallerPackageType[] array = new InstallerPackageType[installerConfig.InstallerPackages.Length - 1];
				if (num3 > 0)
				{
					Array.Copy(installerConfig.InstallerPackages, 0, array, 0, num3);
				}
				if (num3 < installerConfig.InstallerPackages.Length - 1)
				{
					Array.Copy(installerConfig.InstallerPackages, num3 + 1, array, num3, installerConfig.InstallerPackages.Length - num3 - 1);
				}
				installerConfig.InstallerPackages = array;
				int num4 = 0;
				bool flag = false;
				PackageRefType[] installExecuteSequence = installerConfig.InstallExecuteSequence;
				for (int i = 0; i < installExecuteSequence.Length; i++)
				{
					if (installExecuteSequence[i].ID == iD)
					{
						num3 = num4;
						flag = true;
						break;
					}
					num4++;
				}
				if (flag)
				{
					PackageRefType[] array2 = new PackageRefType[installerConfig.InstallExecuteSequence.Length - 1];
					if (num3 > 0)
					{
						Array.Copy(installerConfig.InstallExecuteSequence, 0, array2, 0, num3);
					}
					if (num3 < installerConfig.InstallExecuteSequence.Length - 1)
					{
						Array.Copy(installerConfig.InstallExecuteSequence, num3 + 1, array2, num3, installerConfig.InstallExecuteSequence.Length - num3 - 1);
					}
					installerConfig.InstallExecuteSequence = array2;
					LogManager.Write($"Package with ID {iD} was removed from the installer sequencer because it was not loaded");
				}
				num2++;
			}
			LoadInstallerPrerequisiteXml(installerConfig, curDir);
			LogManager.Write("XML file loaded.");
			return installerConfig;
		}
		catch (InvalidOperationException ex3)
		{
			LogManager.Write($"Load of XML file failed: {ex3.TypeAndMessage()}.", TraceEventType.Critical);
			throw;
		}
		catch (Exception ex4)
		{
			LogManager.Write($"Load of XML file failed: {ex4.TypeAndMessage()}.", TraceEventType.Critical);
			throw;
		}
	}

	public static XmlDocument ApplyGlobalPrerequisites(string filename)
	{
		XmlDocument globalPrerequisitesdoc = LoadGlobalPrerequisites();
		XmlDocument xmlDocument = LoadInstallerConfig(filename);
		ImportGlobalPrerequisitesIntoInstallerConfig(globalPrerequisitesdoc, xmlDocument);
		return xmlDocument;
	}

	private static void ImportGlobalPrerequisitesIntoInstallerConfig(XmlDocument globalPrerequisitesdoc, XmlDocument installerConfigdoc)
	{
		try
		{
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(globalPrerequisitesdoc.NameTable);
			xmlNamespaceManager.AddNamespace("ab", "urn:milestone-systems");
			foreach (XmlNode item in globalPrerequisitesdoc.SelectNodes("//ab:InstallerPrerequisite", xmlNamespaceManager))
			{
				XmlNode newChild = installerConfigdoc.ImportNode(item, deep: true);
				if (installerConfigdoc.DocumentElement["InstallerPrerequisites"] == null)
				{
					XmlElement newChild2 = installerConfigdoc.CreateElement("InstallerPrerequisites");
					installerConfigdoc.DocumentElement.AppendChild(newChild2);
				}
				installerConfigdoc.DocumentElement["InstallerPrerequisites"].AppendChild(newChild);
			}
		}
		catch (Exception ex)
		{
			LogManager.Write($"Import of global prerequisites failed {ex.TypeAndMessage()}.", TraceEventType.Critical);
			throw;
		}
	}

	private static XmlDocument LoadInstallerConfig(string filename)
	{
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			return xmlDocument;
		}
		catch (Exception ex)
		{
			LogManager.Write($"Load of XML file failed: {ex.TypeAndMessage()}.", TraceEventType.Critical);
			throw;
		}
	}

	private static XmlDocument LoadGlobalPrerequisites()
	{
		try
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			XmlDocument xmlDocument = new XmlDocument();
			using (Stream inStream = executingAssembly.GetManifestResourceStream("VideoOS.Installer.Configuration.GlobalPrerequisites.GlobalPrerequisites.xml"))
			{
				xmlDocument.Load(inStream);
			}
			return xmlDocument;
		}
		catch (Exception ex)
		{
			LogManager.Write($"Load of global prerequisites failed {ex.TypeAndMessage()}.", TraceEventType.Critical);
			throw;
		}
	}

	public static Descriptions LoadTreckDescriptions(string dirName)
	{
		XmlSerializer xmlSerializer = null;
		Descriptions descriptions = null;
		string text = Path.Combine(dirName, "TrackDescriptions.xml");
		LogManager.Write($"Loading {text}...", TraceEventType.Information);
		try
		{
			xmlSerializer = new XmlSerializer(typeof(Descriptions));
		}
		catch (Exception ex)
		{
			LogManager.Write($"Load of XML file failed: {ex.TypeAndMessage()}.", TraceEventType.Critical);
			throw;
		}
		using TextReader textReader = new StreamReader(text);
		try
		{
			return (Descriptions)xmlSerializer.Deserialize(textReader);
		}
		catch (InvalidOperationException ex2)
		{
			LogManager.Write($"Load of XML file failed: {ex2.TypeAndMessage()}.", TraceEventType.Critical);
			throw;
		}
		catch (Exception ex3)
		{
			LogManager.Write($"Load of XML file failed: {ex3.TypeAndMessage()}.", TraceEventType.Critical);
			throw;
		}
	}

	public static PackageType LoadPackage(string fileName)
	{
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		return LoadPackageFromFile(fileName, directoryName);
	}

	private static void LoadInstallerPrerequisiteXml(InstallerConfig installerConfig, string curDir)
	{
		if (installerConfig.InstallerPrerequisites == null || installerConfig.InstallerPrerequisites.InstallerPrerequisite == null)
		{
			return;
		}
		InstallerPrerequisiteCollectionType[] installerPrerequisite = installerConfig.InstallerPrerequisites.InstallerPrerequisite;
		foreach (InstallerPrerequisiteCollectionType installerPrerequisiteCollectionType in installerPrerequisite)
		{
			if (installerPrerequisiteCollectionType.PrerequisiteInfo != null && installerPrerequisiteCollectionType.PrerequisiteInfo.ProductManifest != null && installerPrerequisiteCollectionType.PrerequisiteInfo.ProductManifest.Format == FormatType.Microsoft)
			{
				try
				{
					installerPrerequisiteCollectionType.PrerequisiteInfo.ProductManifest.PrerequisitData = LoadPrerequisiteData(curDir, installerPrerequisiteCollectionType);
				}
				catch (Exception exception)
				{
					LogManager.Write(exception);
				}
			}
		}
	}

	private static Product LoadPrerequisiteData(string curDir, InstallerPrerequisiteCollectionType prerequisite)
	{
		if (!string.IsNullOrEmpty(prerequisite.PrerequisiteInfo.InstallationFolder))
		{
			return LoadProduct(Path.Combine(curDir, prerequisite.PrerequisiteInfo.InstallationFolder, prerequisite.PrerequisiteInfo.ProductManifest.Filename));
		}
		string name = $"VideoOS.Installer.Configuration.GlobalPrerequisites.{prerequisite.PrerequisiteInfo.ManifestResourceFolderName}.{prerequisite.PrerequisiteInfo.ProductManifest.Filename}";
		using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
		using TextReader stream2 = new StreamReader(stream);
		return LoadProduct(stream2);
	}

	public static PackageConfig LoadDescription(string filename)
	{
		PackageConfig packageConfig = null;
		filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), filename);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(PackageConfig));
		LogManager.Write($"Loading {filename}...", TraceEventType.Information);
		using TextReader textReader = new StreamReader(filename);
		try
		{
			return (PackageConfig)xmlSerializer.Deserialize(textReader);
		}
		catch (InvalidOperationException ex)
		{
			LogManager.Write($"Load of XML file failed: {ex.TypeAndMessage()}.", TraceEventType.Critical);
			throw;
		}
		catch (Exception ex2)
		{
			LogManager.Write($"Load of XML file failed: {ex2.TypeAndMessage()}.", TraceEventType.Critical);
			throw;
		}
	}

	public bool CheckConfiguration(InstallerConfig installerConfig)
	{
		return false;
	}

	public static InstallInfo LoadInstallInfo(string filename)
	{
		XmlSerializer xmlSerializer = null;
		InstallInfo result = null;
		LogManager.Write($"Loading {filename}...", TraceEventType.Information);
		try
		{
			xmlSerializer = new XmlSerializer(typeof(InstallInfo));
		}
		catch (Exception ex)
		{
			LogManager.Write($"Load of XML file failed: {ex.TypeAndMessage()}.", TraceEventType.Critical);
			throw;
		}
		if (File.Exists(filename))
		{
			using TextReader textReader = new StreamReader(filename);
			try
			{
				result = (InstallInfo)xmlSerializer.Deserialize(textReader);
				LogManager.Write("InstallInfo.xml was loaded");
			}
			catch (InvalidOperationException ex2)
			{
				LogManager.Write($"Load of XML file failed: {ex2.TypeAndMessage()}.", TraceEventType.Critical);
				throw;
			}
			catch (Exception ex3)
			{
				LogManager.Write($"Load of XML file failed: {ex3.TypeAndMessage()}.", TraceEventType.Critical);
				throw;
			}
		}
		return result;
	}

	public static IArgumentCollection LoadCommandLineArguments(string filename)
	{
		IArgumentCollection result = null;
		filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), filename);
		if (File.Exists(filename))
		{
			LogManager.Write($"Loading {filename}...", TraceEventType.Information);
			using Stream xmlStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
			try
			{
				result = ArgumentsInterpreter.CreateArgumentsFromXmlStream(xmlStream);
			}
			catch (InvalidOperationException ex)
			{
				LogManager.Write($"Load of XML file failed: {ex.TypeAndMessage()}.", TraceEventType.Critical);
				throw;
			}
			catch (Exception ex2)
			{
				LogManager.Write($"Load of XML file failed: {ex2.TypeAndMessage()}.", TraceEventType.Critical);
				throw;
			}
		}
		return result;
	}
}
