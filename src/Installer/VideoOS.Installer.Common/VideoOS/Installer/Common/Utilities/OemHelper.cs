using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;
using VideoOS.Installer.Common.Properties;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Installer.Common.Utilities;

public class OemHelper
{
	private OemResourcePackage _oem;

	private string _companyName;

	private string _productNameWithoutCompany;

	private string _upgradeLicenseUrl;

	private static Icon _installerIcon;

	private static Image _backgroundImage;

	private string _certificateGuideUri;

	public string CompanyName => _companyName;

	public string ProductNameWithoutCompany => _productNameWithoutCompany;

	public Icon InstallerIcon => _installerIcon;

	public Image BackgroundImage => _backgroundImage;

	public string UpgradeLicenseUrl => _upgradeLicenseUrl;

	public string CertificateGuideUri => _certificateGuideUri;

	public OemHelper()
	{
		LoadResourceFile();
	}

	private void LoadResourceFile()
	{
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resource.xml"));
			_oem = new OemResourcePackage(xmlDocument);
			if (_oem.ValidateSignature())
			{
				_companyName = ((OemResourceItemString)_oem.GetResourceItem("CompanyName")).ResourceValue;
				_productNameWithoutCompany = ((OemResourceItemString)_oem.GetResourceItem("ProductNameWithoutCompany")).ResourceValue;
				_installerIcon = ((OemResourceItemIcon)_oem.GetResourceItem("Icon")).ResourceValue;
				_backgroundImage = Image.FromHbitmap(((Bitmap)((OemResourceItemImage)_oem.GetResourceItem("BackgroundImage")).ResourceValue).GetHbitmap());
				_upgradeLicenseUrl = ((OemResourceItemString)_oem.GetResourceItem("UpgradeLicenseUrl"))?.ResourceValue;
				_certificateGuideUri = ((OemResourceItemString)_oem.GetResourceItem("CertificateGuideUri"))?.ResourceValue;
			}
			else
			{
				MessageHelper.ShowInstallerMessageBox(Resources.MessageBox_InvalidResourceSignatureCaption, Resources.MessageBox_InvalidResourceSignatureText, InstallerMessageBoxIcon.Error, InstallerMessageBoxButtons.OK);
			}
		}
		catch (Exception ex)
		{
			MessageHelper.ShowInstallerMessageBox(Resources.MessageBox_InvalidResourceSignatureCaption, ex.Message, InstallerMessageBoxIcon.Error, InstallerMessageBoxButtons.OK);
		}
	}
}
