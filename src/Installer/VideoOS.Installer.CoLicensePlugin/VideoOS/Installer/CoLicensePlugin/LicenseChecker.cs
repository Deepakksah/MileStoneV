using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Xml.Linq;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Integration.ServerAPI;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Installer.Common;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Licensing;

namespace VideoOS.Installer.CoLicensePlugin;

public class LicenseChecker
{
	private string DefaultInstallDirRelativeToProgramFiles = OemResources.Instance.Server.DefaultInstallDir;

	private string _licenseFile;

	private const string _licenseTempName = "license.tmp";

	private ILicenseDocument _licenseDocument;

	public string DisplayName
	{
		get
		{
			if (_licenseDocument == null)
			{
				throw new ArgumentNullException("_licenseDocument");
			}
			return _licenseDocument.ProductName;
		}
	}

	public string ProductKey
	{
		get
		{
			if (_licenseDocument == null)
			{
				throw new ArgumentNullException("_licenseDocument");
			}
			return _licenseDocument.ProductSku;
		}
	}

	public LicenseChecker(string licenseFile)
	{
		_licenseFile = licenseFile;
		_licenseDocument = LoadLICFile(_licenseFile);
	}

	public LicenseStatus Check(string productKey)
	{
		try
		{
			string softwareLicenseCode = _licenseDocument.SoftwareLicenseCode;
			if (_licenseDocument.IsLicenseExpired)
			{
				return LicenseStatus.Expired;
			}
			CheckSLC(softwareLicenseCode);
			if (!string.IsNullOrEmpty(productKey) && productKey != _licenseDocument.ProductSku)
			{
				LogManager.Write(_licenseDocument.SoftwareLicenseCode);
				return LicenseStatus.ProductInvalid;
			}
			XDocument xDocument = new XDocument(new XDeclaration("1.0", "utf-16", "yes"), new XElement("LicenseInfo", new XElement("Features", _licenseDocument.AllFeatureNames.Select((string f) => new XElement("Feature", new XAttribute("name", f))))));
			GlobalData.SetValue("LicenseInformation", xDocument.ToString(), "LicenseChecker.Check()");
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			folderPath = Path.Combine(folderPath, GlobalData.OemHelper.CompanyName);
			folderPath = Path.Combine(folderPath, DefaultInstallDirRelativeToProgramFiles);
			folderPath = Path.Combine(folderPath, "license.tmp");
			if (File.Exists(folderPath))
			{
				EnsureFileNotReadOnly(folderPath);
			}
			EnsureDirectoryExists(Path.GetDirectoryName(folderPath));
			File.Copy(_licenseFile, folderPath, overwrite: true);
			EnsureFileNotReadOnly(folderPath);
			EnsureEveryoneFullControl(folderPath);
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "License file '{0}' placed at {1}", _licenseFile, folderPath));
			InstallerManager.Instance.Parameters["LICENSEPATH"] = folderPath;
			GlobalData.SetValue("LICENSEPATH", folderPath, "LicenseChecker.Check()");
		}
		catch (FileNotFoundException)
		{
			return LicenseStatus.NotFound;
		}
		catch (DirectoryNotFoundException)
		{
			return LicenseStatus.NotFound;
		}
		catch (FormatInvalidException)
		{
			return LicenseStatus.Invalid;
		}
		catch (ProductCodeInvalidException)
		{
			return LicenseStatus.ProductInvalid;
		}
		catch (SLCInvalidException)
		{
			return LicenseStatus.Invalid;
		}
		catch (Exception)
		{
			return LicenseStatus.Invalid;
		}
		return LicenseStatus.Ok;
	}

	private void EnsureDirectoryExists(string directoryPath)
	{
		if (!Directory.Exists(directoryPath))
		{
			Directory.CreateDirectory(directoryPath);
		}
	}

	private static void EnsureEveryoneFullControl(string filePath)
	{
		FileSecurity accessControl = File.GetAccessControl(filePath, AccessControlSections.Access);
		FileSystemAccessRule rule = new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, AccessControlType.Allow);
		accessControl.AddAccessRule(rule);
		File.SetAccessControl(filePath, accessControl);
	}

	private void EnsureFileNotReadOnly(string filePath)
	{
		FileAttributes attributes = File.GetAttributes(filePath);
		if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
		{
			attributes &= ~FileAttributes.ReadOnly;
			File.SetAttributes(filePath, attributes);
		}
	}

	private void CheckSLC(string slc)
	{
		slc = slc.Trim().ToUpper();
		try
		{
			Version version = new Version(ServerVersion.GetServerVersion(Assembly.GetExecutingAssembly()));
			SLC.Validate(slc, OemResources.Instance.App.VendorCode, version.Major, version.Minor);
		}
		catch (FormatInvalidException)
		{
			throw;
		}
		catch (ProductCodeInvalidException)
		{
			throw;
		}
		catch (SLCInvalidException)
		{
			throw;
		}
	}

	private ILicenseDocument LoadLICFile(string filename)
	{
		if (!File.Exists(filename))
		{
			throw new FileNotFoundException();
		}
		return LicenseFactory.CreateLicenseLoader().ImportLicenseFile(filename);
	}
}
