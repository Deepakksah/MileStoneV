using System;
using System.Text;

namespace NSpring.Logging;

public abstract class TypeFormatter
{
	private DataFormatter dataFormatter;

	protected Encoding encoding = Encoding.UTF8;

	public abstract Type SupportedType { get; }

	public abstract string SupportedTypeDescriptor { get; }

	public Encoding Encoding
	{
		get
		{
			return encoding;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			encoding = value;
		}
	}

	public abstract bool IsPassThroughRequested { get; }

	public DataFormatter DataFormatter
	{
		set
		{
			lock (this)
			{
				if (value != null)
				{
					if (dataFormatter != null)
					{
						throw new InvalidOperationException("A TypeFormatter instance can only be used with one data formatter");
					}
					dataFormatter = value;
				}
			}
		}
	}

	public abstract string Format(object o);

	public virtual void Format(object o, StringBuilder sb)
	{
		sb.Append(Format(o));
	}

	public abstract TypeFormatter Copy();
}
