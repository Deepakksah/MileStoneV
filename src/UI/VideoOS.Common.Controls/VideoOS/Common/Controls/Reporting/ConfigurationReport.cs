using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Tables;

namespace VideoOS.Common.Controls.Reporting;

public class ConfigurationReport : IDisposable, IConfigurationReport
{
	private const string UnicodeCheckmark = "✓";

	private const string UnicodeCheckmarkBold = "✔";

	private const string UnicodeBallot = "☐";

	private const string UnicodeBallotCheck = "☑";

	private readonly PdfFont _defaultFont;

	private readonly PdfFont _tableOfContentFontBold;

	private readonly PdfFont _sectionFont;

	private readonly PdfFont _subSectionFont;

	private readonly PdfFont _footerFont;

	private readonly PdfFont _heading1Font;

	private readonly PdfFont _heading2Font;

	private PdfFont _tableDefaultFont;

	private PdfFont _tableHeaderFont;

	private PdfFont _tableSectionFont;

	private PdfFont _tableKeyFont;

	private readonly PdfFont _symbolFont = (PdfFont)new PdfStandardFont((PdfFontFamily)4, 11f);

	private readonly PdfPen _tableBorderPen = PdfPens.Black;

	private readonly PdfBrush _defaultBrush = PdfBrushes.Black;

	private readonly PdfBrush _footerBrush = PdfBrushes.Gray;

	private readonly float _defaultFontSize = 9f;

	private readonly float _footerFontSize = 8f;

	private readonly float _sectionFontSize = 14f;

	private readonly float _subsectionFontSize = 10f;

	private readonly PdfStringFormat _sectionStringFormat = new PdfStringFormat();

	private readonly PdfStringFormat _tableStringFormat = new PdfStringFormat((PdfTextAlignment)0, (PdfVerticalAlignment)0);

	private readonly PdfLayoutFormat _tableLayoutFormat = new PdfLayoutFormat
	{
		Break = (PdfLayoutBreakType)1,
		Layout = (PdfLayoutType)0
	};

	private readonly float _defaultVerticalSpacing = 6f;

	private readonly float _tableCellPadding = 2f;

	private readonly float _indentSpacing = 20f;

	private readonly float _tableOfContentsMargin = 70f;

	private readonly PdfPen _tableGridPen = new PdfPen(new PdfColor((byte)160, (byte)160, (byte)160));

	private readonly PdfBrush _tableHeaderFontBrush = PdfBrushes.White;

	private readonly PdfBrush _tableHeaderBackgroundBrush = (PdfBrush)new PdfSolidBrush(new PdfColor((byte)22, (byte)140, (byte)210));

	private readonly PdfBrush _tableBackgroundBrush;

	private readonly PdfBrush _tableAltBackgroundBrush = (PdfBrush)new PdfSolidBrush(new PdfColor((byte)224, (byte)224, (byte)224));

	private readonly PdfDocument _document;

	private PdfPage _currentPage;

	private PointF _currentPos;

	private PdfBookmarkBase _sectionBookmark;

	private PdfBookmarkBase _subsectionBookmark;

	private Collection<Tuple<string, int, PdfDestination>> _tableOfContentsEntries;

	private PdfPage _tableOfContentsPage;

	private string _tableOfContentsTitle;

	private Image _tableOfContentsImage;

	private int _tableSectionCount;

	private float _tableImageTop;

	private Image _tableImage;

	private bool _suppressTableStyle;

	private float _headerTextAngle;

	private string[] _rotatedColumnHeaders;

	private SizeF _clientSize;

	private bool _sectionAdded;

	private static Regex regexRtl = new Regex("^.{0,5}(?:\\p{IsArabic}|\\p{IsHebrew})");

	public bool ShowTableAlternateRowBackground { get; set; }

	public bool ShowTableHeaderBackground { get; set; }

	public bool ShowTableGrid { get; set; }

	public int TableSectionCount => _tableSectionCount;

	public bool ExcludePersonalData { get; set; }

	public ConfigurationReport(PageSettings pageSettings)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Expected O, but got Unknown
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Expected O, but got Unknown
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Expected O, but got Unknown
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Expected O, but got Unknown
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Expected O, but got Unknown
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Expected O, but got Unknown
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Expected O, but got Unknown
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Expected O, but got Unknown
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Expected O, but got Unknown
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Expected O, but got Unknown
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Expected O, but got Unknown
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Expected O, but got Unknown
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Expected O, but got Unknown
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Expected O, but got Unknown
		if (pageSettings == null)
		{
			throw new ArgumentNullException("pageSettings");
		}
		_document = new PdfDocument();
		_document.PageSettings.Orientation = (PdfPageOrientation)0;
		switch (pageSettings.PageSize)
		{
		case PageSize.Legal:
			_document.PageSettings.Size = PdfPageSize.Legal;
			break;
		case PageSize.Letter:
			_document.PageSettings.Size = PdfPageSize.Letter;
			break;
		default:
			_document.PageSettings.Size = PdfPageSize.A4;
			break;
		}
		switch (pageSettings.MarginSize)
		{
		case MarginSize.Narrow:
			_document.PageSettings.Margins = new PdfMargins
			{
				Left = 36f,
				Right = 36f,
				Top = 18f,
				Bottom = 18f
			};
			break;
		case MarginSize.Wide:
			_document.PageSettings.Margins = new PdfMargins
			{
				Left = 72f,
				Right = 72f,
				Top = 36f,
				Bottom = 36f
			};
			break;
		default:
			_document.PageSettings.Margins = new PdfMargins
			{
				Left = 54f,
				Right = 54f,
				Top = 24f,
				Bottom = 24f
			};
			break;
		}
		switch (pageSettings.FontType)
		{
		case FontType.SystemFont:
		{
			string fontFamily = pageSettings.FontFamily;
			_defaultFont = (PdfFont)new PdfTrueTypeFont(new Font(fontFamily, _defaultFontSize, FontStyle.Regular), true);
			_tableOfContentFontBold = (PdfFont)new PdfTrueTypeFont(new Font(fontFamily, _defaultFontSize, FontStyle.Bold), true);
			_sectionFont = (PdfFont)new PdfTrueTypeFont(new Font(fontFamily, _sectionFontSize, FontStyle.Bold), true);
			_subSectionFont = (PdfFont)new PdfTrueTypeFont(new Font(fontFamily, _subsectionFontSize, FontStyle.Bold), true);
			_footerFont = (PdfFont)new PdfTrueTypeFont(new Font(fontFamily, _footerFontSize, FontStyle.Regular), true);
			_heading1Font = (PdfFont)new PdfTrueTypeFont(new Font(fontFamily, _defaultFontSize, FontStyle.Bold | FontStyle.Underline), true);
			_heading2Font = (PdfFont)new PdfTrueTypeFont(new Font(fontFamily, _defaultFontSize, FontStyle.Underline), true);
			_tableDefaultFont = (PdfFont)new PdfTrueTypeFont(new Font(fontFamily, _defaultFontSize, FontStyle.Regular), true);
			_tableHeaderFont = (PdfFont)new PdfTrueTypeFont(new Font(fontFamily, _defaultFontSize, FontStyle.Bold), true);
			_tableSectionFont = (PdfFont)new PdfTrueTypeFont(new Font(fontFamily, _defaultFontSize, FontStyle.Regular), true);
			_tableKeyFont = (PdfFont)new PdfTrueTypeFont(new Font(fontFamily, _defaultFontSize, FontStyle.Italic), true);
			break;
		}
		case FontType.CjkFont:
		{
			if (Enum.TryParse<CjkFontFamily>(pageSettings.FontFamily, out var result2))
			{
				_defaultFont = (PdfFont)new PdfCjkStandardFont((PdfCjkFontFamily)result2, _defaultFontSize, (PdfFontStyle)0);
				_tableOfContentFontBold = (PdfFont)new PdfCjkStandardFont((PdfCjkFontFamily)result2, _defaultFontSize, (PdfFontStyle)1);
				_sectionFont = (PdfFont)new PdfCjkStandardFont((PdfCjkFontFamily)result2, _sectionFontSize, (PdfFontStyle)1);
				_subSectionFont = (PdfFont)new PdfCjkStandardFont((PdfCjkFontFamily)result2, _subsectionFontSize, (PdfFontStyle)1);
				_footerFont = (PdfFont)new PdfCjkStandardFont((PdfCjkFontFamily)result2, _footerFontSize, (PdfFontStyle)0);
				_heading1Font = (PdfFont)new PdfCjkStandardFont((PdfCjkFontFamily)result2, _defaultFontSize, (PdfFontStyle)5);
				_heading2Font = (PdfFont)new PdfCjkStandardFont((PdfCjkFontFamily)result2, _defaultFontSize, (PdfFontStyle)4);
				_tableDefaultFont = (PdfFont)new PdfCjkStandardFont((PdfCjkFontFamily)result2, _defaultFontSize, (PdfFontStyle)0);
				_tableHeaderFont = (PdfFont)new PdfCjkStandardFont((PdfCjkFontFamily)result2, _defaultFontSize, (PdfFontStyle)1);
				_tableSectionFont = (PdfFont)new PdfCjkStandardFont((PdfCjkFontFamily)result2, _defaultFontSize, (PdfFontStyle)0);
				_tableKeyFont = (PdfFont)new PdfCjkStandardFont((PdfCjkFontFamily)result2, _defaultFontSize, (PdfFontStyle)2);
			}
			break;
		}
		default:
		{
			if (Enum.TryParse<StandardFontFamily>(pageSettings.FontFamily, out var result))
			{
				_defaultFont = (PdfFont)new PdfStandardFont((PdfFontFamily)result, _defaultFontSize, (PdfFontStyle)0);
				_tableOfContentFontBold = (PdfFont)new PdfStandardFont((PdfFontFamily)result, _defaultFontSize, (PdfFontStyle)1);
				_sectionFont = (PdfFont)new PdfStandardFont((PdfFontFamily)result, _sectionFontSize, (PdfFontStyle)1);
				_subSectionFont = (PdfFont)new PdfStandardFont((PdfFontFamily)result, _subsectionFontSize, (PdfFontStyle)1);
				_footerFont = (PdfFont)new PdfStandardFont((PdfFontFamily)result, _footerFontSize, (PdfFontStyle)0);
				_heading1Font = (PdfFont)new PdfStandardFont((PdfFontFamily)result, _defaultFontSize, (PdfFontStyle)5);
				_heading2Font = (PdfFont)new PdfStandardFont((PdfFontFamily)result, _defaultFontSize, (PdfFontStyle)4);
				_tableDefaultFont = (PdfFont)new PdfStandardFont((PdfFontFamily)result, _defaultFontSize, (PdfFontStyle)0);
				_tableHeaderFont = (PdfFont)new PdfStandardFont((PdfFontFamily)result, _defaultFontSize, (PdfFontStyle)1);
				_tableSectionFont = (PdfFont)new PdfStandardFont((PdfFontFamily)result, _defaultFontSize, (PdfFontStyle)0);
				_tableKeyFont = (PdfFont)new PdfStandardFont((PdfFontFamily)result, _defaultFontSize, (PdfFontStyle)2);
			}
			break;
		}
		}
		_tableOfContentsEntries = new Collection<Tuple<string, int, PdfDestination>>();
	}

	public void Save(string path)
	{
		UpdateTableOfContents();
		((PdfDocumentBase)_document).Save(path);
	}

	public void AddDefaultHeader(Image leftImage, Image centerImage, Image rightImage)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		if (leftImage == null)
		{
			throw new ArgumentNullException("leftImage");
		}
		if (rightImage == null)
		{
			throw new ArgumentNullException("rightImage");
		}
		float num = 50f;
		float width = _document.PageSettings.Width - _document.PageSettings.Margins.Left - _document.PageSettings.Margins.Right;
		RectangleF rectangleF = new RectangleF(0f, 0f, width, num + 2f * _defaultVerticalSpacing);
		PdfPageTemplateElement val = new PdfPageTemplateElement(rectangleF);
		float num2 = (float)leftImage.Width * num / (float)leftImage.Height;
		float num3 = (float)rightImage.Width * num / (float)rightImage.Height;
		RectangleF rectangleF2 = new RectangleF(0f, 0f, num2, num);
		RectangleF rectangleF3 = new RectangleF(rectangleF.Width - num3, 0f, num3, num);
		RectangleF rectangleF4 = new RectangleF(num2, 0f, rectangleF3.X - num2, num);
		PdfImage val2 = (PdfImage)new PdfBitmap(leftImage);
		PdfImage val3 = (PdfImage)new PdfBitmap(rightImage);
		PdfImage val4 = (PdfImage)new PdfBitmap(centerImage);
		val.Graphics.DrawImage(val2, rectangleF2);
		val.Graphics.DrawImage(val3, rectangleF3);
		val.Graphics.DrawImage(val4, rectangleF4);
		_document.Template.Top = val;
	}

	public void AddDefaultHeader(Image headerImage)
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		if (headerImage == null)
		{
			throw new ArgumentNullException("headerImage");
		}
		float num = _document.PageSettings.Width - _document.PageSettings.Margins.Left - _document.PageSettings.Margins.Right;
		float num2 = num * (float)headerImage.Height / (float)headerImage.Width;
		PdfPageTemplateElement val = new PdfPageTemplateElement(new RectangleF(0f, 0f, num, num2 + 2f * _defaultVerticalSpacing));
		RectangleF rectangleF = new RectangleF(0f, 0f, num, num2);
		PdfImage val2 = (PdfImage)new PdfBitmap(headerImage);
		val.Graphics.DrawImage(val2, rectangleF);
		_document.Template.Top = val;
	}

	public void AddDefaultFooter(string footerTextLeft, string footerTextRight, string pageFormat)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		float height = 50f;
		float width = _document.PageSettings.Width - _document.PageSettings.Margins.Left - _document.PageSettings.Margins.Right;
		RectangleF rectangleF = new RectangleF(0f, 0f, width, height);
		PdfPageTemplateElement val = new PdfPageTemplateElement(rectangleF);
		PdfStringFormat val2 = new PdfStringFormat((PdfTextAlignment)0, (PdfVerticalAlignment)1);
		PdfStringFormat val3 = new PdfStringFormat((PdfTextAlignment)2, (PdfVerticalAlignment)1);
		val.Graphics.DrawString(footerTextLeft, _footerFont, _footerBrush, rectangleF, val2);
		val.Graphics.DrawString(footerTextRight, _footerFont, _footerBrush, rectangleF, val3);
		PdfPageNumberField val4 = new PdfPageNumberField(_footerFont, _footerBrush);
		PdfPageCountField val5 = new PdfPageCountField(_footerFont, _footerBrush);
		((PdfGraphicsElement)new PdfCompositeField(_footerFont, _footerBrush, pageFormat, (PdfAutomaticField[])(object)new PdfAutomaticField[2]
		{
			(PdfAutomaticField)val4,
			(PdfAutomaticField)val5
		})
		{
			Bounds = rectangleF,
			StringFormat = new PdfStringFormat((PdfTextAlignment)1, (PdfVerticalAlignment)1)
		}).Draw(val.Graphics);
		_document.Template.Bottom = val;
	}

	public void AddTableOfContents(string name, Image headerImage)
	{
		_tableOfContentsTitle = name;
		_tableOfContentsImage = headerImage;
	}

	public void AddFrontPage(string name, Image headerImage)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		AddReportSection();
		SetHeaderImage(headerImage);
		((PdfDocumentBase)_document).Bookmarks.Add(name).Destination = new PdfDestination((PdfPageBase)(object)_currentPage);
		((PdfPageBase)_currentPage).Graphics.DrawString(name, _sectionFont, _defaultBrush, _currentPos);
		UpdatePosition(_sectionFont.Height + _defaultVerticalSpacing);
	}

	public void AddSection(string name)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		if (_tableOfContentsPage == null && !string.IsNullOrEmpty(_tableOfContentsTitle))
		{
			AddReportSection();
			_tableOfContentsPage = _currentPage;
			SetHeaderImage(_tableOfContentsImage);
			((PdfDocumentBase)_document).Bookmarks.Add(_tableOfContentsTitle).Destination = new PdfDestination((PdfPageBase)(object)_currentPage);
			AddReportSection();
		}
		if (_currentPage == null)
		{
			AddReportSection();
		}
		if (_currentPos.Y != 0f)
		{
			AddSpace(_sectionAdded ? 2 : 4);
		}
		float height = _sectionFont.MeasureString(name, _clientSize.Width).Height;
		AddPageIfNeeded(height + _subSectionFont.Height + _heading1Font.Height + _defaultFont.Height + 5f * _defaultVerticalSpacing);
		((PdfPageBase)_currentPage).Graphics.DrawRectangle(PdfBrushes.LightGray, new RectangleF(_currentPos, new SizeF(_clientSize.Width, height)));
		_sectionStringFormat.TextDirection = (PdfTextDirection)(IsRightToLeft(name) ? 2 : 0);
		new PdfTextElement(name, _sectionFont, (PdfPen)null, _defaultBrush, _sectionStringFormat).Draw(_currentPage, _currentPos, _clientSize.Width, new PdfLayoutFormat());
		PdfBookmark val = ((PdfDocumentBase)_document).Bookmarks.Add(name);
		val.Destination = new PdfDestination((PdfPageBase)(object)_currentPage, _currentPos);
		_sectionBookmark = (PdfBookmarkBase)(object)val;
		_subsectionBookmark = null;
		_tableOfContentsEntries.Add(new Tuple<string, int, PdfDestination>(name, 1, val.Destination));
		UpdatePosition(height + 2f * _defaultVerticalSpacing);
		_sectionAdded = true;
	}

	public void AddSubsection(string name)
	{
		AddSubsection(name, tocEntry: false);
	}

	public void AddSubsection(string name, bool tocEntry)
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		if (_currentPos.Y != 0f && !_sectionAdded)
		{
			AddSpace(2);
		}
		float height = _subSectionFont.MeasureString(name, _clientSize.Width).Height;
		AddPageIfNeeded(height + _heading1Font.Height + _defaultFont.Height + 3f * _defaultVerticalSpacing);
		((PdfPageBase)_currentPage).Graphics.DrawRectangle(PdfBrushes.LightGray, new RectangleF(_currentPos, new SizeF(_clientSize.Width, height)));
		_sectionStringFormat.TextDirection = (PdfTextDirection)(IsRightToLeft(name) ? 2 : 0);
		new PdfTextElement(name, _subSectionFont, (PdfPen)null, _defaultBrush, _sectionStringFormat).Draw(_currentPage, _currentPos, _clientSize.Width, new PdfLayoutFormat());
		PdfBookmark val = _sectionBookmark.Add(name);
		val.Destination = new PdfDestination((PdfPageBase)(object)_currentPage, _currentPos);
		_subsectionBookmark = (PdfBookmarkBase)(object)val;
		if (tocEntry)
		{
			_tableOfContentsEntries.Add(new Tuple<string, int, PdfDestination>(name, 2, val.Destination));
		}
		UpdatePosition(height + 2f * _defaultVerticalSpacing);
		_sectionAdded = true;
	}

	public void AddHeading1(string name)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		if (_currentPos.Y != 0f && !_sectionAdded)
		{
			AddSpace(2);
		}
		float height = _heading1Font.MeasureString(name, _clientSize.Width).Height;
		AddPageIfNeeded(height + _defaultFont.Height + _defaultVerticalSpacing);
		_sectionStringFormat.TextDirection = (PdfTextDirection)(IsRightToLeft(name) ? 2 : 0);
		new PdfTextElement(name, _heading1Font, (PdfPen)null, _defaultBrush, _sectionStringFormat).Draw(_currentPage, _currentPos, _clientSize.Width, new PdfLayoutFormat());
		PdfBookmarkBase val = _subsectionBookmark ?? _sectionBookmark;
		if (val != null)
		{
			val.Add(name).Destination = new PdfDestination((PdfPageBase)(object)_currentPage, _currentPos);
		}
		UpdatePosition(height + _defaultVerticalSpacing);
		_sectionAdded = false;
	}

	public void AddHeading2(string name)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		if (_currentPos.Y != 0f && !_sectionAdded)
		{
			AddSpace(1);
		}
		float height = _heading2Font.MeasureString(name, _clientSize.Width).Height;
		AddPageIfNeeded(height + _defaultFont.Height + _defaultVerticalSpacing);
		_sectionStringFormat.TextDirection = (PdfTextDirection)(IsRightToLeft(name) ? 2 : 0);
		new PdfTextElement(name, _heading2Font, (PdfPen)null, _defaultBrush, _sectionStringFormat).Draw(_currentPage, _currentPos, _clientSize.Width, new PdfLayoutFormat());
		UpdatePosition(height + _defaultVerticalSpacing);
		_sectionAdded = false;
	}

	public void AddSpace(int number)
	{
		UpdatePosition((float)number * _defaultVerticalSpacing);
	}

	public void AddText(string text)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		AddPageIfNeeded(_defaultFont.Height);
		_sectionStringFormat.TextDirection = (PdfTextDirection)(IsRightToLeft(text) ? 2 : 0);
		PdfTextElement val = new PdfTextElement(text, _defaultFont, (PdfPen)null, _defaultBrush, _sectionStringFormat);
		PdfLayoutFormat val2 = new PdfLayoutFormat();
		val2.Break = (PdfLayoutBreakType)0;
		val2.Layout = (PdfLayoutType)0;
		PdfLayoutResult val3 = (PdfLayoutResult)(object)val.Draw(_currentPage, _currentPos, _clientSize.Width, val2);
		UpdatePosition(val3.Bounds.Height + _defaultVerticalSpacing);
		_sectionAdded = false;
	}

	public void AddBulletCollection(BulletCollection bullets)
	{
		DrawBulletCollection(bullets, 0);
		AddSpace(1);
		_sectionAdded = false;
	}

	public void AddTable(ReportTableBase table)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		if (table == null)
		{
			throw new ArgumentNullException("table");
		}
		float height;
		float minHeight;
		PdfLightTable val = CreateDefaultTable(table, out height, out minHeight);
		val.BeginCellLayout += new BeginCellLayoutEventHandler(TextDirection_BeginCellLayout);
		val.BeginCellLayout += new BeginCellLayoutEventHandler(ReplaceUnicodeCheckmarks_BeginCellLayout);
		if (table.HeaderAngle != 0f)
		{
			_headerTextAngle = table.HeaderAngle;
			val.BeginCellLayout += new BeginCellLayoutEventHandler(RotateTableHeaders_BeginCellLayout);
			val.EndCellLayout += new EndCellLayoutEventHandler(RotateTableHeaders_EndCellLayout);
		}
		if (table is KeyValueImageTable keyValueImageTable)
		{
			AddPageIfNeeded(height);
			_tableImage = keyValueImageTable.Image;
			val.BeginCellLayout += new BeginCellLayoutEventHandler(KeyValueTable_BeginCellLayout);
			val.EndCellLayout += new EndCellLayoutEventHandler(KeyValueImageTable_EndCellLayout);
		}
		else
		{
			AddPageIfNeeded(minHeight);
			if (table is KeyValueTable)
			{
				val.BeginCellLayout += new BeginCellLayoutEventHandler(KeyValueTable_BeginCellLayout);
			}
			else if (table is KeyValueSectionsTable)
			{
				val.BeginCellLayout += new BeginCellLayoutEventHandler(KeyValueSectionsTable_BeginCellLayout);
			}
		}
		DrawTable(val);
		_sectionAdded = false;
	}

	public void AddImage(Image image)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		if (image == null)
		{
			throw new ArgumentNullException("image");
		}
		PdfUnitConvertor val = new PdfUnitConvertor();
		SizeF sizeF = new SizeF(_clientSize.Width, 300f);
		SizeF size = val.ConvertToPixels((SizeF)image.Size, (PdfGraphicsUnit)3);
		if (size.Width > sizeF.Width)
		{
			size.Width = sizeF.Width;
			size.Height *= sizeF.Width / size.Width;
		}
		if (size.Width > sizeF.Width)
		{
			size.Width *= sizeF.Height / size.Height;
			size.Height = sizeF.Height;
		}
		AddPageIfNeeded(size.Height);
		PdfLayoutResult val2 = ((PdfLayoutElement)new PdfBitmap(image)).Draw(_currentPage, new RectangleF(_currentPos, size), new PdfLayoutFormat());
		UpdatePosition(val2.Bounds.Height + _defaultVerticalSpacing);
		_sectionAdded = false;
	}

	public void AddSignatureFooter(string dateDisplayValue, string signature1, string signature2, Image logo)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		float num = 250f;
		float num2 = 200f;
		float num3 = _clientSize.Width - num - 20f;
		AddPageIfNeeded(num2);
		_currentPos.Y = _clientSize.Height - num2;
		if (logo != null)
		{
			SizeF sizeF = logo.Size;
			float num4 = num3 / sizeF.Width;
			float num5 = num2 / sizeF.Height;
			if (num4 > num5)
			{
				sizeF.Width *= num5;
				sizeF.Height = num2;
			}
			else
			{
				sizeF.Height *= num4;
				sizeF.Width = num3;
			}
			PointF pointF = new PointF(_clientSize.Width - sizeF.Width, _currentPos.Y);
			((PdfPageBase)_currentPage).Graphics.DrawImage((PdfImage)new PdfBitmap(logo), pointF, sizeF);
		}
		AddText(dateDisplayValue);
		_currentPos.Y += 50f;
		((PdfPageBase)_currentPage).Graphics.DrawLine(PdfPens.Black, _currentPos, new PointF(num, _currentPos.Y));
		_currentPos.Y += 5f;
		AddText(signature1);
		_currentPos.Y += 50f;
		((PdfPageBase)_currentPage).Graphics.DrawLine(PdfPens.Black, _currentPos, new PointF(num, _currentPos.Y));
		_currentPos.Y += 5f;
		AddText(signature2);
	}

	private void SetHeaderImage(Image headerImage)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		if (headerImage != null)
		{
			float num = _clientSize.Width * (float)headerImage.Height / (float)headerImage.Width;
			PdfPageTemplateElement val = new PdfPageTemplateElement(new RectangleF(0f, 0f, _clientSize.Width, num + 2f * _defaultVerticalSpacing));
			RectangleF rectangleF = new RectangleF(0f, 0f, _clientSize.Width, num);
			PdfImage val2 = (PdfImage)new PdfBitmap(headerImage);
			val.Graphics.DrawImage(val2, rectangleF);
			((PdfDocumentTemplate)_currentPage.Section.Template).Top = val;
			_clientSize.Height -= val.Height;
		}
	}

	private bool ReduceTableFontSize()
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected I4, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected I4, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected I4, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected I4, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Expected O, but got Unknown
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Expected O, but got Unknown
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Expected O, but got Unknown
		float num = Math.Max(_tableDefaultFont.Size - 1f, 2f);
		float num2 = Math.Max(_tableHeaderFont.Size - 1f, 2f);
		float num3 = Math.Max(_tableSectionFont.Size - 1f, 2f);
		float num4 = Math.Max(_tableKeyFont.Size - 1f, 2f);
		if (num == _tableDefaultFont.Size && num2 == _tableHeaderFont.Size && num3 == _tableSectionFont.Size && num4 == _tableKeyFont.Size)
		{
			return false;
		}
		PdfFont tableDefaultFont = _tableDefaultFont;
		PdfTrueTypeFont val = (PdfTrueTypeFont)(object)((tableDefaultFont is PdfTrueTypeFont) ? tableDefaultFont : null);
		PdfFont tableDefaultFont2 = _tableDefaultFont;
		PdfCjkStandardFont val2 = (PdfCjkStandardFont)(object)((tableDefaultFont2 is PdfCjkStandardFont) ? tableDefaultFont2 : null);
		PdfFont tableDefaultFont3 = _tableDefaultFont;
		PdfStandardFont val3 = (PdfStandardFont)(object)((tableDefaultFont3 is PdfStandardFont) ? tableDefaultFont3 : null);
		if (val != null)
		{
			string name = ((PdfFont)val).Name;
			FontStyle[] array = new FontStyle[4]
			{
				(FontStyle)_tableDefaultFont.Style,
				(FontStyle)_tableHeaderFont.Style,
				(FontStyle)_tableSectionFont.Style,
				(FontStyle)_tableKeyFont.Style
			};
			val.Dispose();
			((PdfTrueTypeFont)_tableHeaderFont).Dispose();
			((PdfTrueTypeFont)_tableSectionFont).Dispose();
			((PdfTrueTypeFont)_tableKeyFont).Dispose();
			_tableDefaultFont = (PdfFont)new PdfTrueTypeFont(new Font(name, num, array[0]), true);
			_tableHeaderFont = (PdfFont)new PdfTrueTypeFont(new Font(name, num2, array[1]), true);
			_tableSectionFont = (PdfFont)new PdfTrueTypeFont(new Font(name, num3, array[2]), true);
			_tableKeyFont = (PdfFont)new PdfTrueTypeFont(new Font(name, num4, array[4]), true);
		}
		else if (val2 != null)
		{
			_tableDefaultFont = (PdfFont)new PdfCjkStandardFont(val2.FontFamily, num, _tableDefaultFont.Style);
			_tableHeaderFont = (PdfFont)new PdfCjkStandardFont(val2.FontFamily, num2, _tableHeaderFont.Style);
			_tableSectionFont = (PdfFont)new PdfCjkStandardFont(val2.FontFamily, num3, _tableSectionFont.Style);
			_tableKeyFont = (PdfFont)new PdfCjkStandardFont(val2.FontFamily, num4, _tableKeyFont.Style);
		}
		else if (val3 != null)
		{
			_tableDefaultFont = (PdfFont)new PdfStandardFont(val3.FontFamily, num, _tableDefaultFont.Style);
			_tableHeaderFont = (PdfFont)new PdfStandardFont(val3.FontFamily, num2, _tableHeaderFont.Style);
			_tableSectionFont = (PdfFont)new PdfStandardFont(val3.FontFamily, num3, _tableSectionFont.Style);
			_tableKeyFont = (PdfFont)new PdfStandardFont(val3.FontFamily, num4, _tableKeyFont.Style);
		}
		return true;
	}

	private void AddReportSection()
	{
		PdfSection val = _document.Sections.Add();
		_currentPage = val.Pages.Add();
		_currentPos = new PointF(0f, 0f);
		SizeF size = _document.PageSettings.Size;
		PdfMargins margins = _document.PageSettings.Margins;
		_clientSize = new SizeF(size.Width - margins.Left - margins.Right, size.Height - margins.Top - margins.Bottom);
		PdfPageTemplateElement top = _document.Template.Top;
		if (top != null)
		{
			_clientSize.Height -= top.Height;
		}
		PdfPageTemplateElement bottom = _document.Template.Bottom;
		if (bottom != null)
		{
			_clientSize.Height -= bottom.Height;
		}
	}

	private void AddPageIfNeeded(float minHeight)
	{
		if (_currentPage == null || _currentPos.Y + minHeight > _clientSize.Height)
		{
			int count = _document.Sections.Count;
			if (count == 0)
			{
				throw new InvalidOperationException("No section to add page to.");
			}
			_currentPage = _document.Sections[count - 1].Pages.Add();
			_currentPos = new PointF(0f, 0f);
		}
	}

	private void UpdatePosition(float y)
	{
		PdfPage val = _document.Pages[_document.Pages.Count - 1];
		if (_currentPage == val)
		{
			_currentPos.Y += y;
			return;
		}
		_currentPage = val;
		_currentPos.Y = y;
	}

	private void DrawBulletCollection(BulletCollection bulletCollection, int indentLevel)
	{
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		string text = bulletCollection.Marker + " ";
		float num = _defaultFont.MeasureString(bulletCollection.Marker).Width + _defaultFont.MeasureString(" ").Width;
		float num2 = _currentPos.X + (float)indentLevel * _indentSpacing;
		float num3 = num2 + num;
		float num4 = _clientSize.Width - num3;
		foreach (Bullet item in bulletCollection)
		{
			float height = _defaultFont.MeasureString(item.Value, num4).Height;
			AddPageIfNeeded(height);
			((PdfPageBase)_currentPage).Graphics.DrawString(text, _defaultFont, _defaultBrush, num2, _currentPos.Y);
			_sectionStringFormat.TextDirection = (PdfTextDirection)(IsRightToLeft(item.Value) ? 2 : 0);
			new PdfTextElement(item.Value, _defaultFont, (PdfPen)null, _defaultBrush, _sectionStringFormat).Draw(_currentPage, new PointF(num3, _currentPos.Y), num4, new PdfLayoutFormat());
			UpdatePosition(height);
			if (item.Bullets.Count > 0)
			{
				DrawBulletCollection(item.Bullets, indentLevel + 1);
			}
		}
	}

	private PdfLightTable CreateDefaultTable(ReportTableBase table, out float height, out float minHeight)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		PdfLightTable val = new PdfLightTable();
		val.ColumnProportionalSizing = true;
		DataTable dataTable = table.DataTable;
		if (table.HeaderAngle != 0f)
		{
			string text = "";
			int num = 0;
			_rotatedColumnHeaders = new string[dataTable.Columns.Count];
			foreach (DataColumn column in dataTable.Columns)
			{
				_rotatedColumnHeaders[num] = column.ColumnName;
				SizeF sizeF = RotatedRectSize(_defaultFont.MeasureString(column.ColumnName), table.HeaderAngle);
				while (_defaultFont.MeasureString(text).Height < sizeF.Height)
				{
					text += "\n";
				}
				column.ColumnName = num.ToString(NumberFormatInfo.InvariantInfo);
				num++;
			}
			dataTable.Columns[0].ColumnName = text;
		}
		val.DataSource = dataTable;
		_suppressTableStyle = table.SuppressTableStyle;
		PdfPen val2 = ((ShowTableGrid && !_suppressTableStyle) ? _tableGridPen : PdfPens.Transparent);
		val.Style.DefaultStyle = new PdfCellStyle(_tableDefaultFont, _defaultBrush, val2);
		val.Style.DefaultStyle.BackgroundBrush = _tableBackgroundBrush;
		val.Style.DefaultStyle.StringFormat = _tableStringFormat;
		if (ShowTableAlternateRowBackground && !_suppressTableStyle)
		{
			val.Style.AlternateStyle = new PdfCellStyle(_tableDefaultFont, _defaultBrush, val2);
			val.Style.AlternateStyle.BackgroundBrush = _tableAltBackgroundBrush;
			val.Style.AlternateStyle.StringFormat = _tableStringFormat;
		}
		if (ShowTableHeaderBackground)
		{
			val.Style.HeaderStyle = new PdfCellStyle(_tableHeaderFont, _tableHeaderFontBrush, val2);
			val.Style.HeaderStyle.BackgroundBrush = (ShowTableHeaderBackground ? _tableHeaderBackgroundBrush : _tableBackgroundBrush);
		}
		else
		{
			val.Style.HeaderStyle = new PdfCellStyle(_tableHeaderFont, PdfBrushes.Black, val2);
			val.Style.HeaderStyle.BackgroundBrush = _tableBackgroundBrush;
		}
		val.Style.HeaderStyle.StringFormat = _tableStringFormat;
		val.Style.ShowHeader = table.GetHeaders() != null;
		val.Style.RepeatHeader = table.RepeatHeader;
		val.Style.HeaderSource = (PdfHeaderSource)0;
		if (!_suppressTableStyle)
		{
			val.Style.BorderPen = _tableBorderPen;
		}
		val.Style.CellPadding = _tableCellPadding;
		int[] columnWidths = table.GetColumnWidths();
		if (columnWidths != null && ((PdfCollection)val.Columns).Count == columnWidths.Length)
		{
			for (int i = 0; i < columnWidths.Length; i++)
			{
				val.Columns[i].Width = columnWidths[i];
			}
		}
		do
		{
			val.Style.DefaultStyle.Font = _tableDefaultFont;
			if (val.Style.AlternateStyle != null)
			{
				val.Style.AlternateStyle.Font = _tableDefaultFont;
			}
			val.Style.HeaderStyle.Font = _tableHeaderFont;
			height = CalculateTableHeight(val, _clientSize.Width, out minHeight);
		}
		while (height == 0f && ReduceTableFontSize());
		return val;
	}

	private float CalculateCellHeight(string text, PdfFont font, float cellWidth)
	{
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		float num = cellWidth - 2f * _tableCellPadding - 1f;
		SizeF sizeF = font.MeasureString(text, num);
		if (sizeF.Height == 0f && sizeF.Width == 0f)
		{
			return 0f;
		}
		return sizeF.Height + 2f * _tableCellPadding + _tableBorderPen.Width;
	}

	private float CalculateTableHeight(PdfLightTable pdfTable, float tableWidth, out float minHeight)
	{
		float num = 0f;
		for (int i = 0; i < ((PdfCollection)pdfTable.Columns).Count; i++)
		{
			num += pdfTable.Columns[i].Width;
		}
		float num2 = ((pdfTable.Style.BorderPen != null) ? pdfTable.Style.BorderPen.Width : 0f);
		float num3 = (tableWidth - num2) / num;
		float num4 = (minHeight = num2 + 1f);
		if (pdfTable.DataSource is DataTable dataTable)
		{
			if (pdfTable.Style.ShowHeader)
			{
				int num5 = 0;
				float num6 = 0f;
				foreach (DataColumn column in dataTable.Columns)
				{
					float cellWidth = pdfTable.Columns[num5++].Width * num3;
					float num7 = CalculateCellHeight(column.ColumnName, pdfTable.Style.HeaderStyle.Font, cellWidth);
					if (num7 == 0f)
					{
						return 0f;
					}
					num6 = Math.Max(num6, num7);
				}
				num4 += num6;
				minHeight += num6;
			}
			bool flag = true;
			foreach (DataRow row in dataTable.Rows)
			{
				int num8 = 0;
				float num9 = 0f;
				object[] itemArray = row.ItemArray;
				foreach (object obj2 in itemArray)
				{
					float cellWidth2 = pdfTable.Columns[num8++].Width * num3;
					float num10 = CalculateCellHeight(obj2 as string, pdfTable.Style.DefaultStyle.Font, cellWidth2);
					if (num10 == 0f)
					{
						return 0f;
					}
					num9 = Math.Max(num9, num10);
				}
				num4 += num9;
				if (flag)
				{
					minHeight += num9;
					flag = false;
				}
			}
		}
		return num4;
	}

	private void DrawTable(PdfLightTable table)
	{
		//IL_0020: Expected O, but got Unknown
		PdfLayoutResult val;
		try
		{
			val = ((PdfLayoutElement)table).Draw(_currentPage, _currentPos, _tableLayoutFormat);
		}
		catch (PdfLightTableException ex)
		{
			if (!(((Exception)ex).Message == "Can't draw table, because there is not enough space for it.") || _currentPos.Y == 0f)
			{
				throw;
			}
			AddPageIfNeeded(10000f);
			val = ((PdfLayoutElement)table).Draw(_currentPage, _currentPos, _tableLayoutFormat);
		}
		UpdatePosition(val.Bounds.Height + _defaultVerticalSpacing);
	}

	private void DrawCellText(string text, PdfGraphics graphics, RectangleF bounds, PdfFont font, PdfCellStyle style, bool drawBackground, bool drawGrid)
	{
		RectangleF rectangleF = new RectangleF(bounds.X + _tableCellPadding, bounds.Y + _tableCellPadding, bounds.Width - 2f * _tableCellPadding, bounds.Height - _tableCellPadding);
		if (drawBackground && !_suppressTableStyle)
		{
			graphics.DrawRectangle(style.BackgroundBrush, bounds);
		}
		if (drawGrid && !_suppressTableStyle)
		{
			graphics.DrawRectangle(style.BorderPen, bounds);
		}
		if (text != null)
		{
			graphics.DrawString(text, font, style.TextBrush, rectangleF, style.StringFormat);
		}
	}

	private void UpdateTableOfContents()
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		if (_tableOfContentsPage == null)
		{
			return;
		}
		_currentPage = _tableOfContentsPage;
		_currentPos = new PointF(_tableOfContentsMargin, 0f);
		_clientSize = _currentPage.GetClientSize();
		((PdfPageBase)_currentPage).Graphics.DrawString(_tableOfContentsTitle, _sectionFont, _defaultBrush, _currentPos);
		_currentPos.Y += _sectionFont.Height + 2f * _defaultVerticalSpacing;
		int num = _document.Pages.IndexOf(_currentPage);
		Collection<Tuple<string, PdfFont, PdfPage, RectangleF, PdfDestination>> collection = new Collection<Tuple<string, PdfFont, PdfPage, RectangleF, PdfDestination>>();
		foreach (Tuple<string, int, PdfDestination> tableOfContentsEntry in _tableOfContentsEntries)
		{
			if (_document.Pages.IndexOf((PdfPage)tableOfContentsEntry.Item3.Page) > num)
			{
				collection.Add(CalculateTocEntry(tableOfContentsEntry.Item1, tableOfContentsEntry.Item2, tableOfContentsEntry.Item3));
			}
		}
		foreach (Tuple<string, PdfFont, PdfPage, RectangleF, PdfDestination> item in collection)
		{
			DrawTocEntry(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5);
		}
	}

	private Tuple<string, PdfFont, PdfPage, RectangleF, PdfDestination> CalculateTocEntry(string title, int level, PdfDestination destination)
	{
		PdfFont val = ((level == 1) ? _tableOfContentFontBold : _defaultFont);
		_currentPos.X = _tableOfContentsMargin + 25f * (float)(level - 1);
		float num = _clientSize.Width - _currentPos.X - _tableOfContentsMargin - 25f;
		SizeF size = val.MeasureString(title, num);
		if (_currentPos.Y + size.Height > _clientSize.Height)
		{
			_currentPage = _currentPage.Section.Pages.Add();
			_currentPos.Y = 0f;
		}
		RectangleF item = new RectangleF(_currentPos, size);
		_currentPos.Y += size.Height;
		return new Tuple<string, PdfFont, PdfPage, RectangleF, PdfDestination>(title, val, _currentPage, item, destination);
	}

	private void DrawTocEntry(string title, PdfFont font, PdfPage page, RectangleF titleBounds, PdfDestination destination)
	{
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		string text = " ";
		PdfPageBase page2 = destination.Page;
		PdfPage val = (PdfPage)(object)((page2 is PdfPage) ? page2 : null);
		if (destination != null)
		{
			text = " " + (_document.Pages.IndexOf(val) + 1).ToString(NumberFormatInfo.InvariantInfo);
		}
		PointF pointF = new PointF(_clientSize.Width - _tableOfContentsMargin, titleBounds.Y + titleBounds.Height - font.Height);
		float num = pointF.X - titleBounds.X - titleBounds.Width - 2f;
		if (titleBounds.Height > font.Height)
		{
			num += 4f;
		}
		while (font.MeasureString("." + text).Width <= num)
		{
			text = "." + text;
		}
		float num2 = Math.Max(titleBounds.Width, font.MeasureString(".").Width);
		while (font.MeasureString(title + ".", num2) == titleBounds.Size)
		{
			title += ".";
		}
		PdfLayoutFormat val2 = new PdfLayoutFormat
		{
			Break = (PdfLayoutBreakType)1,
			Layout = (PdfLayoutType)0
		};
		_sectionStringFormat.TextDirection = (PdfTextDirection)(IsRightToLeft(title) ? 2 : 0);
		new PdfTextElement(title, font, (PdfPen)null, _defaultBrush, _sectionStringFormat).Draw(page, titleBounds, val2);
		PdfStringFormat val3 = new PdfStringFormat((PdfTextAlignment)2, (PdfVerticalAlignment)0);
		((PdfLayoutElement)new PdfTextElement(text, font, new PdfPen(_defaultBrush, 0f), _defaultBrush, val3)).Draw(page, pointF);
		RectangleF rectangleF = new RectangleF(titleBounds.X, titleBounds.Y, pointF.X - titleBounds.X, titleBounds.Height);
		PdfGoToAction val4 = new PdfGoToAction(destination);
		PdfActionAnnotation val5 = new PdfActionAnnotation(rectangleF, (PdfAction)(object)val4);
		((PdfAnnotation)val5).Border = new PdfAnnotationBorder(0f);
		((PdfLinkAnnotation)val5).HighlightMode = (PdfHighlightMode)0;
		page.Annotations.Add((PdfAnnotation)(object)val5);
	}

	private static PointF Rotate(PointF point, float angle)
	{
		double num = (double)angle * Math.PI / 180.0;
		double num2 = Math.Sin(num);
		double num3 = Math.Cos(num);
		return new PointF((float)((double)point.X * num3 - (double)point.Y * num2), (float)((double)point.X * num2 + (double)point.Y * num3));
	}

	private static PointF RotateLowerLeftRectPoint(RectangleF bounds, SizeF textSize, float angle)
	{
		double num = (double)angle * Math.PI / 180.0;
		double num2 = Math.Sin(num);
		double num3 = Math.Cos(num);
		PointF result = Rotate(new PointF(bounds.X, bounds.Y + bounds.Height), 0f - angle);
		if (num3 >= 0.0)
		{
			if (num2 > 0.0)
			{
				float x = (float)((double)textSize.Height * num2);
				float num4 = (float)((double)textSize.Width * num2 + (double)textSize.Height * num3);
				PointF pointF = Rotate(new PointF(x, 0f - num4), 0f - angle);
				result = new PointF(result.X + pointF.X, result.Y + pointF.Y);
			}
			else if (num2 < 0.0)
			{
				float num5 = (float)((double)textSize.Height * num3);
				PointF pointF2 = Rotate(new PointF(0f, 0f - num5), 0f - angle);
				result = new PointF(result.X + pointF2.X, result.Y + pointF2.Y);
			}
		}
		return result;
	}

	private static SizeF RotatedRectSize(SizeF size, float angle)
	{
		double num = (double)Math.Abs(angle) * Math.PI / 180.0;
		double num2 = Math.Sin(num);
		double num3 = Math.Cos(num);
		return new SizeF((float)((double)size.Width * num3 + (double)size.Height * num2), (float)((double)size.Width * num2 + (double)size.Height * num3));
	}

	private void DrawRotatedText(PdfGraphics graphics, RectangleF bounds, string text, PdfCellStyle style, float angle)
	{
		if (!_suppressTableStyle)
		{
			if (ShowTableHeaderBackground)
			{
				graphics.DrawRectangle(style.BackgroundBrush, bounds);
			}
			if (ShowTableGrid)
			{
				graphics.DrawRectangle(style.BorderPen, bounds);
			}
		}
		RectangleF bounds2 = new RectangleF(bounds.X + _tableCellPadding, bounds.Y + _tableCellPadding, bounds.Width - 2f * _tableCellPadding, bounds.Height - 2f * _tableCellPadding);
		SizeF textSize = style.Font.MeasureString(text);
		PointF pointF = RotateLowerLeftRectPoint(bounds2, textSize, angle);
		graphics.RotateTransform(angle);
		graphics.DrawString(text, style.Font, style.TextBrush, pointF);
		graphics.RotateTransform(0f - angle);
	}

	private void TextDirection_BeginCellLayout(object sender, BeginCellLayoutEventArgs args)
	{
		if (IsRightToLeft(((CellLayoutEventArgs)args).Value))
		{
			_tableStringFormat.TextDirection = (PdfTextDirection)2;
		}
		else
		{
			_tableStringFormat.TextDirection = (PdfTextDirection)1;
		}
	}

	private static bool IsRightToLeft(string strToTest)
	{
		if (string.IsNullOrEmpty(strToTest))
		{
			return false;
		}
		return regexRtl.IsMatch(strToTest);
	}

	private void RotateTableHeaders_BeginCellLayout(object sender, BeginCellLayoutEventArgs args)
	{
		if (((CellLayoutEventArgs)args).RowIndex == -1 && ((CellLayoutEventArgs)args).CellIndex > 0 && _rotatedColumnHeaders != null && _rotatedColumnHeaders.Length > ((CellLayoutEventArgs)args).CellIndex)
		{
			PdfLightTable val = (PdfLightTable)((sender is PdfLightTable) ? sender : null);
			DrawRotatedText(((CellLayoutEventArgs)args).Graphics, ((CellLayoutEventArgs)args).Bounds, _rotatedColumnHeaders[((CellLayoutEventArgs)args).CellIndex], val.Style.HeaderStyle, _headerTextAngle);
			args.Skip = true;
		}
	}

	private void RotateTableHeaders_EndCellLayout(object sender, EndCellLayoutEventArgs args)
	{
		if (((CellLayoutEventArgs)args).RowIndex == -1 && ((CellLayoutEventArgs)args).CellIndex == 0 && _rotatedColumnHeaders != null && _rotatedColumnHeaders.Length != 0)
		{
			PdfLightTable val = (PdfLightTable)((sender is PdfLightTable) ? sender : null);
			DrawRotatedText(((CellLayoutEventArgs)args).Graphics, ((CellLayoutEventArgs)args).Bounds, _rotatedColumnHeaders[0], val.Style.HeaderStyle, _headerTextAngle);
		}
	}

	private void ReplaceUnicodeCheckmarks_BeginCellLayout(object sender, BeginCellLayoutEventArgs args)
	{
		PdfLightTable val = (PdfLightTable)((sender is PdfLightTable) ? sender : null);
		if (((CellLayoutEventArgs)args).Value == "✓")
		{
			PdfCellStyle style = ((((CellLayoutEventArgs)args).RowIndex % 2 == 1 && val.Style.AlternateStyle != null) ? val.Style.AlternateStyle : (style = val.Style.DefaultStyle));
			DrawCellText("3", ((CellLayoutEventArgs)args).Graphics, ((CellLayoutEventArgs)args).Bounds, _symbolFont, style, ShowTableAlternateRowBackground, ShowTableGrid);
			args.Skip = true;
		}
		if (((CellLayoutEventArgs)args).Value == "✔")
		{
			PdfCellStyle style2 = ((((CellLayoutEventArgs)args).RowIndex % 2 == 1 && val.Style.AlternateStyle != null) ? val.Style.AlternateStyle : (style2 = val.Style.DefaultStyle));
			DrawCellText("4", ((CellLayoutEventArgs)args).Graphics, ((CellLayoutEventArgs)args).Bounds, _symbolFont, style2, ShowTableAlternateRowBackground, ShowTableGrid);
			args.Skip = true;
		}
		if (((CellLayoutEventArgs)args).Value == "☐")
		{
			PdfCellStyle style3 = ((((CellLayoutEventArgs)args).RowIndex % 2 == 1 && val.Style.AlternateStyle != null) ? val.Style.AlternateStyle : (style3 = val.Style.DefaultStyle));
			DrawCellText("r", ((CellLayoutEventArgs)args).Graphics, ((CellLayoutEventArgs)args).Bounds, _symbolFont, style3, ShowTableAlternateRowBackground, ShowTableGrid);
			args.Skip = true;
		}
		else if (((CellLayoutEventArgs)args).Value == "☑")
		{
			PdfCellStyle style4 = ((((CellLayoutEventArgs)args).RowIndex % 2 == 1 && val.Style.AlternateStyle != null) ? val.Style.AlternateStyle : (style4 = val.Style.DefaultStyle));
			DrawCellText("r", ((CellLayoutEventArgs)args).Graphics, ((CellLayoutEventArgs)args).Bounds, _symbolFont, style4, ShowTableAlternateRowBackground, ShowTableGrid);
			RectangleF bounds = ((CellLayoutEventArgs)args).Bounds;
			bounds.X += 1.5f;
			bounds.Y -= 2f;
			DrawCellText("3", ((CellLayoutEventArgs)args).Graphics, bounds, _symbolFont, style4, drawBackground: false, drawGrid: false);
			args.Skip = true;
		}
	}

	private void KeyValueTable_BeginCellLayout(object sender, BeginCellLayoutEventArgs args)
	{
		if (((CellLayoutEventArgs)args).RowIndex >= 0 && ((CellLayoutEventArgs)args).CellIndex == 0)
		{
			PdfLightTable val = (PdfLightTable)((sender is PdfLightTable) ? sender : null);
			PdfCellStyle style = ((((CellLayoutEventArgs)args).RowIndex % 2 == 1 && val.Style.AlternateStyle != null) ? val.Style.AlternateStyle : (style = val.Style.DefaultStyle));
			DrawCellText(((CellLayoutEventArgs)args).Value, ((CellLayoutEventArgs)args).Graphics, ((CellLayoutEventArgs)args).Bounds, _tableKeyFont, style, ShowTableAlternateRowBackground, ShowTableGrid);
			args.Skip = true;
		}
	}

	private void KeyValueSectionsTable_BeginCellLayout(object sender, BeginCellLayoutEventArgs args)
	{
		if (((CellLayoutEventArgs)args).RowIndex < 0 || ((CellLayoutEventArgs)args).CellIndex >= 2)
		{
			return;
		}
		PdfLightTable val = (PdfLightTable)((sender is PdfLightTable) ? sender : null);
		if (((CellLayoutEventArgs)args).CellIndex == 0 && !string.IsNullOrEmpty(((CellLayoutEventArgs)args).Value))
		{
			if (((CellLayoutEventArgs)args).RowIndex == 0)
			{
				_tableSectionCount = 0;
			}
			else
			{
				_tableSectionCount = TableSectionCount + 1;
			}
		}
		PdfCellStyle style = ((((((CellLayoutEventArgs)args).CellIndex == 0) ? ((TableSectionCount & 1) == 1) : ((((CellLayoutEventArgs)args).RowIndex & 1) == 1)) && val.Style.AlternateStyle != null) ? val.Style.AlternateStyle : val.Style.DefaultStyle);
		PdfFont font = ((((CellLayoutEventArgs)args).CellIndex == 0) ? _tableSectionFont : _tableKeyFont);
		bool drawGrid = ((CellLayoutEventArgs)args).CellIndex != 0 && ShowTableGrid;
		DrawCellText(((CellLayoutEventArgs)args).Value, ((CellLayoutEventArgs)args).Graphics, ((CellLayoutEventArgs)args).Bounds, font, style, ShowTableAlternateRowBackground, drawGrid);
		if (ShowTableGrid && ((CellLayoutEventArgs)args).CellIndex == 0 && !string.IsNullOrEmpty(((CellLayoutEventArgs)args).Value))
		{
			RectangleF bounds = ((CellLayoutEventArgs)args).Bounds;
			((CellLayoutEventArgs)args).Graphics.DrawLine(val.Style.DefaultStyle.BorderPen, bounds.X, bounds.Y, bounds.X + bounds.Width, bounds.Y);
		}
		args.Skip = true;
	}

	private void KeyValueImageTable_EndCellLayout(object sender, EndCellLayoutEventArgs args)
	{
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		PdfLightTable val = (PdfLightTable)((sender is PdfLightTable) ? sender : null);
		if (((CellLayoutEventArgs)args).CellIndex != ((PdfCollection)val.Columns).Count - 1)
		{
			return;
		}
		if (ShowTableAlternateRowBackground || ShowTableGrid)
		{
			RectangleF bounds = ((CellLayoutEventArgs)args).Bounds;
			((CellLayoutEventArgs)args).Graphics.DrawRectangle(PdfPens.White, PdfBrushes.White, bounds);
			((CellLayoutEventArgs)args).Graphics.DrawLine(val.Style.DefaultStyle.BorderPen, bounds.X, bounds.Y, bounds.X, bounds.Y + bounds.Height);
		}
		if (((CellLayoutEventArgs)args).RowIndex == 0)
		{
			_tableImageTop = ((CellLayoutEventArgs)args).Bounds.Y + _tableCellPadding;
		}
		else
		{
			if (((CellLayoutEventArgs)args).RowIndex != ((PdfCollection)val.Rows).Count - 1)
			{
				return;
			}
			if (_tableImage != null)
			{
				float num = ((CellLayoutEventArgs)args).Bounds.Y + ((CellLayoutEventArgs)args).Bounds.Height - _tableCellPadding - _tableImageTop;
				float num2 = num * (float)_tableImage.Width / (float)_tableImage.Height;
				float num3 = ((CellLayoutEventArgs)args).Bounds.Width - 2f * _tableCellPadding;
				if (num2 > num3)
				{
					num2 = num3;
					num = (float)_tableImage.Height * num2 / (float)_tableImage.Width;
				}
				float num4 = ((CellLayoutEventArgs)args).Bounds.X + (((CellLayoutEventArgs)args).Bounds.Width - num2) / 2f;
				((CellLayoutEventArgs)args).Graphics.DrawImage((PdfImage)new PdfBitmap(_tableImage), num4, _tableImageTop, num2, num);
				return;
			}
			float num5 = ((CellLayoutEventArgs)args).Bounds.Y + ((CellLayoutEventArgs)args).Bounds.Height - _tableCellPadding - _tableImageTop;
			float num6 = num5 * 4f / 3f;
			float num7 = ((CellLayoutEventArgs)args).Bounds.Width - 2f * _tableCellPadding;
			if (num6 > num7)
			{
				num6 = num7;
				num5 = num6 * 3f / 4f;
			}
			float num8 = ((CellLayoutEventArgs)args).Bounds.X + (((CellLayoutEventArgs)args).Bounds.Width - num6) / 2f;
			((CellLayoutEventArgs)args).Graphics.DrawRectangle(PdfPens.Black, num8, _tableImageTop, num6, num5);
			((CellLayoutEventArgs)args).Graphics.DrawLine(PdfPens.Black, num8, _tableImageTop, num8 + num6, _tableImageTop + num5);
			((CellLayoutEventArgs)args).Graphics.DrawLine(PdfPens.Black, num8, _tableImageTop + num5, num8 + num6, _tableImageTop);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		if (disposing)
		{
			if (_defaultFont is PdfTrueTypeFont)
			{
				((PdfTrueTypeFont)_defaultFont).Dispose();
				((PdfTrueTypeFont)_tableOfContentFontBold).Dispose();
				((PdfTrueTypeFont)_sectionFont).Dispose();
				((PdfTrueTypeFont)_subSectionFont).Dispose();
				((PdfTrueTypeFont)_footerFont).Dispose();
				((PdfTrueTypeFont)_heading1Font).Dispose();
				((PdfTrueTypeFont)_heading2Font).Dispose();
				((PdfTrueTypeFont)_tableDefaultFont).Dispose();
				((PdfTrueTypeFont)_tableHeaderFont).Dispose();
				((PdfTrueTypeFont)_tableSectionFont).Dispose();
				((PdfTrueTypeFont)_tableKeyFont).Dispose();
			}
			_document.Dispose();
		}
	}
}
