using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using VideoOS.Common.Integration;

namespace VideoOS.Common.Controls.Reporting;

public class FrontPageSettings
{
	private Image _image;

	public Collection<FrontPageSection> Sections { get; set; }

	public string ImageLocation { get; set; }

	[XmlIgnore]
	public Image Image
	{
		get
		{
			if (_image == null)
			{
				if (!string.IsNullOrEmpty(ImageLocation))
				{
					try
					{
						_image = Image.FromFile(ImageLocation);
					}
					catch (OutOfMemoryException)
					{
					}
					catch (FileNotFoundException)
					{
					}
					catch (ArgumentException)
					{
					}
				}
				else
				{
					_image = OemResources.Instance.Manager.ConfigReportFrontPageLogo;
				}
			}
			return _image;
		}
	}
}
