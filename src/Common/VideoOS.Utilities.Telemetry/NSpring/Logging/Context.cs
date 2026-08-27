using System;
using System.Collections;
using System.Globalization;
using System.Threading;

namespace NSpring.Logging;

public abstract class Context
{
	private sealed class MutableContext : Context
	{
		private Hashtable properties;

		private string[] propertyNames;

		private int propertyCount;

		private bool arePropertyNamesChanged;

		public override int PropertyCount => propertyCount;

		public override string[] PropertyNames
		{
			get
			{
				if (arePropertyNamesChanged)
				{
					if (propertyCount >= propertyNames.Length)
					{
						propertyNames = new string[propertyCount * 2];
					}
					else
					{
						Array.Clear(propertyNames, 0, propertyNames.Length);
					}
					if (propertyCount != 0)
					{
						IEnumerator enumerator = properties.Keys.GetEnumerator();
						int num = 0;
						while (enumerator.MoveNext())
						{
							propertyNames[num++] = (string)enumerator.Current;
						}
						Array.Sort(propertyNames, 0, propertyCount);
					}
					arePropertyNamesChanged = false;
				}
				if (propertyCount == 0)
				{
					return EMPTY_STRING_ARRAY;
				}
				string[] array = new string[propertyCount];
				Array.Copy(propertyNames, 0, array, 0, propertyCount);
				return array;
			}
		}

		public override bool IsMutable => true;

		public MutableContext()
		{
			properties = new Hashtable(50, 0.5f);
			propertyNames = new string[50];
			propertyCount = 0;
			arePropertyNamesChanged = false;
			Thread currentThread = Thread.CurrentThread;
			threadName = currentThread.Name;
			threadId = Convert.ToString(currentThread.ManagedThreadId);
			culture = currentThread.CurrentCulture;
			uiCulture = currentThread.CurrentUICulture;
			threadPriority = currentThread.Priority;
			systemName = defaultSystemName;
			userName = Environment.UserName;
			applicationName = AppDomain.CurrentDomain.SetupInformation.ApplicationName;
			if (applicationName != null)
			{
				return;
			}
			applicationName = Thread.GetDomain().FriendlyName;
			string text = applicationName.ToLower();
			for (int i = 0; i < FILE_EXTENSIONS.Length; i++)
			{
				if (text.EndsWith(FILE_EXTENSIONS[i]))
				{
					applicationName = applicationName.Substring(0, applicationName.Length - FILE_EXTENSIONS[i].Length);
					break;
				}
			}
		}

		public override string GetPropertyName(int index)
		{
			if (index < 0 || index >= propertyCount)
			{
				throw new IndexOutOfRangeException();
			}
			if (arePropertyNamesChanged)
			{
				if (propertyCount >= propertyNames.Length)
				{
					propertyNames = new string[propertyCount * 2];
				}
				else
				{
					Array.Clear(propertyNames, 0, propertyNames.Length);
				}
				if (propertyCount != 0)
				{
					IEnumerator enumerator = properties.Keys.GetEnumerator();
					int num = 0;
					while (enumerator.MoveNext())
					{
						propertyNames[num++] = (string)enumerator.Current;
					}
					Array.Sort(propertyNames, 0, propertyCount);
				}
				arePropertyNamesChanged = false;
			}
			return propertyNames[index];
		}

		public override string GetProperty(int index)
		{
			if (index >= propertyCount)
			{
				throw new IndexOutOfRangeException();
			}
			if (arePropertyNamesChanged)
			{
				if (propertyCount >= propertyNames.Length)
				{
					propertyNames = new string[propertyCount * 2];
				}
				else
				{
					Array.Clear(propertyNames, 0, propertyNames.Length);
				}
				if (propertyCount != 0)
				{
					IEnumerator enumerator = properties.Keys.GetEnumerator();
					int num = 0;
					while (enumerator.MoveNext())
					{
						propertyNames[num++] = (string)enumerator.Current;
					}
					Array.Sort(propertyNames, 0, propertyCount);
				}
				arePropertyNamesChanged = false;
			}
			return (string)properties[propertyNames[index]];
		}

		public override string GetProperty(string propertyName)
		{
			if (propertyName == null)
			{
				return null;
			}
			return (string)properties[propertyName];
		}

		public override void Refresh()
		{
			Thread currentThread = Thread.CurrentThread;
			threadName = currentThread.Name;
			threadId = Convert.ToString(currentThread.ManagedThreadId);
			culture = currentThread.CurrentCulture;
			uiCulture = currentThread.CurrentUICulture;
			threadPriority = currentThread.Priority;
			if (applicationName == null)
			{
				applicationName = Thread.GetDomain().FriendlyName;
				if (applicationName == null)
				{
					applicationName = Thread.GetDomain().FriendlyName;
					string text = applicationName.ToLower();
					for (int i = 0; i < FILE_EXTENSIONS.Length; i++)
					{
						if (text.EndsWith(FILE_EXTENSIONS[i]))
						{
							applicationName = applicationName.Substring(0, applicationName.Length - FILE_EXTENSIONS[i].Length);
							break;
						}
					}
				}
			}
			if (systemName == null)
			{
				systemName = Environment.MachineName;
			}
			if (userName == null)
			{
				userName = Environment.UserName;
			}
		}

		public override void ClearProperties()
		{
			properties.Clear();
			Array.Clear(propertyNames, 0, propertyCount);
			propertyCount = 0;
			arePropertyNamesChanged = false;
		}

		public override void RemoveProperty(string propertyName)
		{
			if (propertyName != null && properties.ContainsKey(propertyName))
			{
				properties.Remove(propertyName);
				propertyCount--;
				arePropertyNamesChanged = true;
			}
		}

		public override void SetProperty(string propertyName, string propertyValue)
		{
			if (propertyName == null)
			{
				throw new ArgumentNullException();
			}
			if (propertyValue == null)
			{
				RemoveProperty(propertyName);
				return;
			}
			if (!properties.ContainsKey(propertyName))
			{
				arePropertyNamesChanged = true;
				propertyCount++;
			}
			properties[propertyName] = propertyValue;
		}

		public override Context Copy()
		{
			return new ImmutableContext(this);
		}
	}

	private sealed class ImmutableContext : Context
	{
		private string[] propertyNames;

		private string[] propertyValues;

		public override int PropertyCount => propertyNames.Length;

		public override string[] PropertyNames
		{
			get
			{
				int num = propertyNames.Length;
				if (num == 0)
				{
					return EMPTY_STRING_ARRAY;
				}
				string[] array = new string[num];
				Array.Copy(propertyNames, 0, array, 0, num);
				return array;
			}
		}

		public override bool IsMutable => false;

		public ImmutableContext(MutableContext context)
		{
			threadName = context.threadName;
			threadId = context.threadId;
			culture = context.culture;
			uiCulture = context.uiCulture;
			threadPriority = context.threadPriority;
			applicationName = context.applicationName;
			systemName = context.systemName;
			propertyNames = context.PropertyNames;
			if (propertyNames.Length == 0)
			{
				propertyValues = EMPTY_STRING_ARRAY;
				return;
			}
			propertyValues = new string[propertyNames.Length];
			for (int i = 0; i < propertyValues.Length; i++)
			{
				propertyValues[i] = context.GetProperty(propertyNames[i]);
			}
		}

		public override string GetPropertyName(int index)
		{
			return propertyNames[index];
		}

		public override string GetProperty(int index)
		{
			return propertyValues[index];
		}

		public override string GetProperty(string propertyName)
		{
			for (int i = 0; i < propertyNames.Length; i++)
			{
				if (propertyNames[i].Equals(propertyName))
				{
					return propertyValues[i];
				}
			}
			return null;
		}

		public override void Refresh()
		{
		}

		public override void ClearProperties()
		{
		}

		public override void RemoveProperty(string propertyName)
		{
		}

		public override void SetProperty(string propertyName, string propertyValue)
		{
		}

		public override Context Copy()
		{
			return this;
		}
	}

	protected static string[] EMPTY_STRING_ARRAY = new string[0];

	private static string[] FILE_EXTENSIONS = new string[4] { ".asp", ".aspx", ".dll", ".exe" };

	private static readonly string GUID = Guid.NewGuid().ToString().ToUpper()
		.Substring(0, 16);

	protected string threadName;

	protected string threadId;

	protected ThreadPriority threadPriority;

	protected CultureInfo culture;

	protected CultureInfo uiCulture;

	protected static string defaultSystemName = Environment.MachineName;

	protected string systemName;

	protected string applicationName;

	protected string userName;

	public static Context CurrentContext
	{
		get
		{
			LocalDataStoreSlot namedDataSlot = Thread.GetNamedDataSlot(GUID);
			Context context = (Context)Thread.GetData(namedDataSlot);
			if (context == null)
			{
				context = new MutableContext();
				Thread.SetData(namedDataSlot, context);
			}
			return context;
		}
	}

	public abstract bool IsMutable { get; }

	public string ApplicationName
	{
		get
		{
			return applicationName;
		}
		set
		{
			applicationName = value;
		}
	}

	public string SystemName
	{
		get
		{
			return systemName;
		}
		set
		{
			systemName = value;
		}
	}

	public string UserName
	{
		get
		{
			return userName;
		}
		set
		{
			userName = value;
		}
	}

	public string ThreadName => threadName;

	public string ThreadId => threadId;

	public CultureInfo Culture => culture;

	public CultureInfo UICulture => uiCulture;

	public ThreadPriority ThreadPriority => threadPriority;

	public abstract string[] PropertyNames { get; }

	public abstract int PropertyCount { get; }

	public abstract string GetPropertyName(int index);

	public abstract string GetProperty(int index);

	public abstract string GetProperty(string propertyName);

	public abstract void Refresh();

	public abstract void ClearProperties();

	public abstract void RemoveProperty(string propertyName);

	public abstract void SetProperty(string propertyName, string propertyValue);

	public abstract Context Copy();
}
