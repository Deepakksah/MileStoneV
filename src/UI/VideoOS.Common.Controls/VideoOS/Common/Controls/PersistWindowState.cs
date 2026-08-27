using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace VideoOS.Common.Controls;

public class PersistWindowState : Component
{
	private Form m_parent;

	private bool m_allowSaveMinimized;

	private string _Filename;

	private Stream _stream;

	private bool _ResetPending;

	private string _localUserSettingsFolder;

	private IContainer components;

	public bool AllowSaveMinimized
	{
		get
		{
			return m_allowSaveMinimized;
		}
		set
		{
			m_allowSaveMinimized = value;
		}
	}

	public PersistWindowState(Form form, Stream stream, string localUserSettingsFolder)
	{
		_stream = stream;
		_localUserSettingsFolder = localUserSettingsFolder;
		PersistWindowStateInit(form);
	}

	public void ClearAll()
	{
		string[] files = Directory.GetFiles(_localUserSettingsFolder, "*_formpositioning.xml");
		for (int i = 0; i < files.Length; i++)
		{
			File.Delete(files[i]);
		}
		_ResetPending = true;
	}

	public PersistWindowState(Form form, string localUserSettingsFolder)
	{
		if (form == null)
		{
			throw new ArgumentNullException("form");
		}
		_localUserSettingsFolder = localUserSettingsFolder;
		_Filename = Path.Combine(_localUserSettingsFolder, CreateFileName(form.Name, "_formpositioning.xml"));
		PersistWindowStateInit(form);
	}

	private static string CreateFileName(string formName, string suffix)
	{
		Version version = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion);
		return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}", formName, version.Major, version.Minor, version.Revision, suffix);
	}

	public void Save(Stream stream)
	{
		if (!m_allowSaveMinimized && m_parent.WindowState == FormWindowState.Minimized)
		{
			m_parent.WindowState = FormWindowState.Normal;
		}
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, null);
		xmlTextWriter.Formatting = Formatting.Indented;
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("dialog");
		if (m_parent.WindowState != FormWindowState.Normal)
		{
			xmlTextWriter.WriteElementString("left", m_parent.RestoreBounds.Left.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteElementString("top", m_parent.RestoreBounds.Top.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteElementString("width", m_parent.RestoreBounds.Width.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteElementString("height", m_parent.RestoreBounds.Height.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteElementString("windowstate", m_parent.WindowState.ToString());
		}
		else
		{
			xmlTextWriter.WriteElementString("left", m_parent.Left.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteElementString("top", m_parent.Top.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteElementString("width", m_parent.Width.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteElementString("height", m_parent.Height.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteElementString("windowstate", m_parent.WindowState.ToString());
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		xmlTextWriter.Close();
	}

	public void Reset()
	{
		try
		{
			if (_Filename != null)
			{
				File.Delete(_Filename);
			}
			_ResetPending = true;
		}
		catch (Exception)
		{
		}
	}

	public void CloseWithoutSave()
	{
		try
		{
			_ResetPending = true;
		}
		catch (Exception)
		{
		}
	}

	public void RestoreWindowState()
	{
		try
		{
			if (_stream != null)
			{
				Load(_stream);
			}
			else if (_Filename != null && File.Exists(_Filename))
			{
				FileStream fileStream = new FileStream(_Filename, FileMode.Open, FileAccess.Read);
				Load(fileStream);
				fileStream.Close();
			}
			if (!m_parent.Visible)
			{
				m_parent.Visible = true;
			}
		}
		catch (Exception)
		{
		}
	}

	private void PersistWindowStateInit(Form form)
	{
		m_parent = form;
		m_parent.Closing += OnClosing;
		m_parent.Load += OnLoad;
	}

	private void Load(Stream stream)
	{
		bool flag = false;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(stream);
		int x = Convert.ToInt32(xmlDocument.SelectSingleNode("/dialog/left").InnerText, CultureInfo.InvariantCulture);
		int y = Convert.ToInt32(xmlDocument.SelectSingleNode("/dialog/top").InnerText, CultureInfo.InvariantCulture);
		int width = Convert.ToInt32(xmlDocument.SelectSingleNode("/dialog/width").InnerText, CultureInfo.InvariantCulture);
		int height = Convert.ToInt32(xmlDocument.SelectSingleNode("/dialog/height").InnerText, CultureInfo.InvariantCulture);
		FormWindowState windowState = (FormWindowState)Enum.Parse(m_parent.WindowState.GetType(), xmlDocument.SelectSingleNode("/dialog/windowstate").InnerText, ignoreCase: true);
		Screen[] allScreens = Screen.AllScreens;
		for (int i = 0; i < allScreens.Length; i++)
		{
			if (allScreens[i].Bounds.Contains(new Rectangle(new Point(x, y), new Size(width, height))))
			{
				flag = true;
			}
		}
		CenterToScreen(m_parent);
		if (flag)
		{
			m_parent.Size = new Size(width, height);
			m_parent.WindowState = windowState;
		}
		else
		{
			m_parent.WindowState = FormWindowState.Maximized;
		}
	}

	private void OnClosing(object sender, CancelEventArgs e)
	{
		try
		{
			if (_Filename != null && !_ResetPending)
			{
				FileStream fileStream = new FileStream(_Filename, FileMode.Create, FileAccess.ReadWrite);
				Save(fileStream);
				fileStream.Close();
			}
		}
		catch (Exception)
		{
		}
	}

	private void OnLoad(object sender, EventArgs e)
	{
		RestoreWindowState();
	}

	private static void CenterToScreen(Form form)
	{
		Point location = default(Point);
		Rectangle workingArea = Screen.FromPoint(new Point(Cursor.Position.X, Cursor.Position.Y)).WorkingArea;
		location.X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - form.Width) / 2);
		location.Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - form.Height) / 2);
		form.Location = location;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	[ExcludeFromCodeCoverage]
	private void InitializeComponent()
	{
		components = new Container();
	}
}
