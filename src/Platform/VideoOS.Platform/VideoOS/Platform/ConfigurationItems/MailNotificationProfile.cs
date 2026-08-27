using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MailNotificationProfile : IConfigurationItem
{
	private ObservableCollection<string> _Recipients;

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public string Subject
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Subject");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Subject", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string MessageText
	{
		get
		{
			return base.Wrapper.GetPropertyValue("MessageText");
		}
		set
		{
			base.Wrapper.SetPropertyValue("MessageText", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ObservableCollection<string> Recipients
	{
		get
		{
			if (_Recipients == null)
			{
				_Recipients = base.Wrapper.GetPropertyArrayString("Recipients");
				_Recipients.CollectionChanged += _Recipients_CollectionChanged;
			}
			return _Recipients;
		}
	}

	public int TimeBetweenMails
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TimeBetweenMails");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimeBetweenMails", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool IncludeImages
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("IncludeImages");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("IncludeImages", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public int NumberOfImages
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("NumberOfImages");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("NumberOfImages", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool EmbedImages
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("EmbedImages");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("EmbedImages", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public int TimeBetweenImages
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TimeBetweenImages");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimeBetweenImages", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool IncludeAVI
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("IncludeAVI");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("IncludeAVI", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public int TimeBeforeEvent
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TimeBeforeEvent");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimeBeforeEvent", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int TimeAfterEvent
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TimeAfterEvent");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimeAfterEvent", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int FrameRate
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FrameRate");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FrameRate", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public MailNotificationProfile(ServerId serverId, ConfigurationItem mailNotificationProfile)
		: base(serverId, mailNotificationProfile)
	{
	}

	public MailNotificationProfile(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public MailNotificationProfile(FQID fqid)
		: base(fqid)
	{
	}

	private void _Recipients_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		base.Wrapper.SetPropertyArrayString("Recipients", _Recipients);
	}
}
