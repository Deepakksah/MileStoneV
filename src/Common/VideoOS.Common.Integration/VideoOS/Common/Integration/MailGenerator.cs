using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using VideoOS.Common.Integration.Properties;

namespace VideoOS.Common.Integration;

public class MailGenerator
{
	public class EmbeddedImage
	{
		public string TimeStamp;

		public LinkedResource LinkedImage;

		public EmbeddedImage(string timeStamp, LinkedResource linkedImage)
		{
			TimeStamp = timeStamp;
			LinkedImage = linkedImage;
		}
	}

	private SmtpClient _smtpClient = new SmtpClient();

	private MailMessage _emailMessage;

	private List<EmbeddedImage> _embeddedImageList;

	private bool _imageError;

	private bool _videoError;

	private string _errorMessage = string.Empty;

	private string _triggeringDeviceName;

	private string _mediaDeviceName;

	private string _customMessage;

	private bool _embedImages;

	public string MailServer
	{
		get
		{
			return _smtpClient.Host;
		}
		set
		{
			_smtpClient.Host = value;
		}
	}

	public bool UseSsl
	{
		get
		{
			return _smtpClient.EnableSsl;
		}
		set
		{
			_smtpClient.EnableSsl = value;
		}
	}

	public int Port
	{
		get
		{
			return _smtpClient.Port;
		}
		set
		{
			_smtpClient.Port = value;
		}
	}

	public string Recipients
	{
		get
		{
			string text = string.Empty;
			foreach (MailAddress item in _emailMessage.To)
			{
				text += string.Format(CultureInfo.InvariantCulture, "{0};", item.Address);
			}
			if (text[text.Length - 1] == ';')
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			_emailMessage.To.Add(value.Replace(';', ','));
		}
	}

	public string Sender => _emailMessage.Sender.Address;

	public string Subject
	{
		get
		{
			return _emailMessage.Subject;
		}
		set
		{
			_emailMessage.Subject = value;
		}
	}

	public string TriggeringDeviceName
	{
		get
		{
			string undefined = MailGeneratorResources.Undefined;
			if (_triggeringDeviceName != null && _triggeringDeviceName.Length != 0)
			{
				return _triggeringDeviceName;
			}
			return undefined;
		}
		set
		{
			_triggeringDeviceName = value;
		}
	}

	public string MediaDeviceName
	{
		get
		{
			string undefined = MailGeneratorResources.Undefined;
			if (_mediaDeviceName != null && _mediaDeviceName.Length != 0)
			{
				return _mediaDeviceName;
			}
			return undefined;
		}
		set
		{
			_mediaDeviceName = value;
		}
	}

	public string CustomMessage
	{
		get
		{
			return _customMessage;
		}
		set
		{
			_customMessage = value;
		}
	}

	public bool EmbedImages
	{
		get
		{
			return _embedImages;
		}
		set
		{
			_embedImages = value;
		}
	}

	public MailGenerator(string mailServer, string sender, string recipients, bool useSsl, int port)
	{
		if (recipients == null)
		{
			throw new ArgumentNullException("recipients");
		}
		MailServer = mailServer;
		UseSsl = useSsl;
		Port = port;
		_emailMessage = new MailMessage(sender, recipients.Replace(';', ','));
		_emailMessage.SubjectEncoding = Encoding.UTF8;
		_embeddedImageList = new List<EmbeddedImage>();
	}

	public MailGenerator(MailGeneratorData mailGeneratorData)
		: this(mailGeneratorData.MailServer, mailGeneratorData.Sender, mailGeneratorData.Recipients, mailGeneratorData.UseSsl, mailGeneratorData.Port)
	{
		Subject = mailGeneratorData.Subject;
		TriggeringDeviceName = mailGeneratorData.TriggeringDeviceName;
		MediaDeviceName = mailGeneratorData.MediaDeviceName;
		CustomMessage = mailGeneratorData.CustomMessage;
		if (mailGeneratorData.LoginMode)
		{
			SetLoginMode(mailGeneratorData.Username, mailGeneratorData.Password);
		}
	}

	public void SetLoginMode(string username, string password)
	{
		_smtpClient.Credentials = new NetworkCredential(username, password);
	}

	public void AddImage(byte[] imageData, DateTime timestamp)
	{
		string text = string.Format(CultureInfo.InvariantCulture, "{0:dd.MM.yyyy - HH.mm.ss.fff}", timestamp);
		MemoryStream contentStream = new MemoryStream(imageData);
		if (_embedImages)
		{
			string contentId = Guid.NewGuid().ToString().ToUpperInvariant();
			LinkedResource linkedResource = new LinkedResource(contentStream, "image/jpeg");
			linkedResource.ContentId = contentId;
			_embeddedImageList.Add(new EmbeddedImage(text, linkedResource));
			return;
		}
		string name = string.Format(CultureInfo.InvariantCulture, "{0}.jpg", text);
		Attachment attachment = new Attachment(contentStream, name, "image/jpeg");
		ContentDisposition contentDisposition = attachment.ContentDisposition;
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		contentDisposition.CreationDate = timestamp;
		Thread.CurrentThread.CurrentCulture = currentCulture;
		_emailMessage.Attachments.Add(attachment);
	}

	public void AddImage(string fileName)
	{
		if (_embedImages)
		{
			string contentId = Guid.NewGuid().ToString().ToUpperInvariant();
			LinkedResource linkedResource = new LinkedResource(fileName, "image/jpeg");
			linkedResource.ContentId = contentId;
			_embeddedImageList.Add(new EmbeddedImage(Path.GetFileNameWithoutExtension(fileName), linkedResource));
		}
		else
		{
			Attachment attachment = new Attachment(fileName, "image/jpeg");
			ContentDisposition contentDisposition = attachment.ContentDisposition;
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			contentDisposition.CreationDate = File.GetCreationTime(fileName);
			Thread.CurrentThread.CurrentCulture = currentCulture;
			_emailMessage.Attachments.Add(attachment);
		}
	}

	public void AddVideo(string fileName)
	{
		Attachment attachment = new Attachment(fileName);
		ContentDisposition contentDisposition = attachment.ContentDisposition;
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		contentDisposition.CreationDate = File.GetCreationTime(fileName);
		Thread.CurrentThread.CurrentCulture = currentCulture;
		_emailMessage.Attachments.Add(attachment);
	}

	public void SetError(MailMediaError mailGeneratorError)
	{
		switch (mailGeneratorError)
		{
		case MailMediaError.MailMediaNoConfiguration:
			_videoError = true;
			_errorMessage += MailGeneratorResources.MailMediaErrorNoConfiguration;
			break;
		case MailMediaError.MailMediaErrorInConfiguration:
			_videoError = true;
			_errorMessage += MailGeneratorResources.MailMediaErrorInConfiguration;
			break;
		case MailMediaError.MailMediaErrorCodecDoesNotSupportThisMedia:
			_videoError = true;
			_errorMessage += MailGeneratorResources.MailMediaErrorCodecDoesNotSupportThisMedia;
			break;
		case MailMediaError.MailMediaErrorNoImagesAvailable:
			_imageError = true;
			_errorMessage += MailGeneratorResources.MailMediaErrorNoImagesAvailable;
			break;
		case MailMediaError.MailMediaErrorUnknownError:
			_videoError = true;
			_errorMessage += MailGeneratorResources.MailMediaErrorUnknownError;
			break;
		case MailMediaError.MailMediaErrorNoCodecSelected:
			_videoError = true;
			_errorMessage += MailGeneratorResources.MailMediaErrorNoCodecSelected;
			break;
		case MailMediaError.MailMediaErrorConnectionBroken:
			_videoError = true;
			_errorMessage += MailGeneratorResources.MailMediaErrorConnectionBroken;
			break;
		}
	}

	public void GenerateMail()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat(CultureInfo.CurrentCulture, MailGeneratorResources.Mail1, TriggeringDeviceName);
		stringBuilder.AppendFormat(CultureInfo.CurrentCulture, MailGeneratorResources.Mail2, MediaDeviceName);
		if (!string.IsNullOrEmpty(_customMessage))
		{
			stringBuilder.AppendFormat(CultureInfo.CurrentCulture, MailGeneratorResources.Mail3, _customMessage);
		}
		if (_imageError || _videoError)
		{
			stringBuilder.Append(_errorMessage);
		}
		if (!_imageError)
		{
			foreach (EmbeddedImage embeddedImage in _embeddedImageList)
			{
				DateTime result;
				string value = (DateTime.TryParseExact(embeddedImage.TimeStamp, "dd.MM.yyyy - HH.mm.ss.fff", null, DateTimeStyles.None, out result) ? string.Format(CultureInfo.InvariantCulture, "{0}.{1}", result, result.ToString("fff", CultureInfo.InvariantCulture)) : embeddedImage.TimeStamp);
				stringBuilder.Append("<P>");
				stringBuilder.Append(value);
				stringBuilder.Append("<br>");
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "<IMG src=\"cid:{0}\">", embeddedImage.LinkedImage.ContentId);
				stringBuilder.Append("</P>");
			}
		}
		AlternateView alternateView = AlternateView.CreateAlternateViewFromString(stringBuilder.ToString(), null, "text/html");
		foreach (EmbeddedImage embeddedImage2 in _embeddedImageList)
		{
			alternateView.LinkedResources.Add(embeddedImage2.LinkedImage);
		}
		_emailMessage.AlternateViews.Add(alternateView);
		_emailMessage.IsBodyHtml = true;
		_emailMessage.BodyEncoding = Encoding.UTF8;
	}

	public void SendMail()
	{
		GenerateMail();
		_smtpClient.Send(_emailMessage);
		_emailMessage.Dispose();
	}
}
