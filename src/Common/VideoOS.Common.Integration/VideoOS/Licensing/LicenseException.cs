using System;
using System.Runtime.Serialization;

namespace VideoOS.Licensing;

[Serializable]
public class LicenseException : Exception, ISerializable
{
	private readonly LicenseErrorNumber errorNumber;

	private FeatureError featureErrorNumber;

	public int ErrorNumber => (int)errorNumber;

	public int FeatureErrorNumber
	{
		get
		{
			return (int)featureErrorNumber;
		}
		set
		{
			featureErrorNumber = (FeatureError)value;
		}
	}

	public LicenseException()
	{
	}

	public LicenseException(string message)
		: base(message)
	{
	}

	public LicenseException(string message, Exception inner)
		: base(message, inner)
	{
	}

	public LicenseException(string message, FeatureError errorNumber)
		: base(message)
	{
		featureErrorNumber = errorNumber;
	}

	public LicenseException(string message, FeatureError errorNumber, Exception inner)
		: base(message, inner)
	{
		featureErrorNumber = errorNumber;
	}

	public LicenseException(string message, LicenseErrorNumber errorNumber)
		: base(message)
	{
		this.errorNumber = errorNumber;
	}

	public LicenseException(string message, LicenseErrorNumber errorNumber, Exception inner)
		: base(message, inner)
	{
		this.errorNumber = errorNumber;
	}

	protected LicenseException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		errorNumber = (LicenseErrorNumber)info.GetInt32("ErrorNumber");
		featureErrorNumber = (FeatureError)info.GetInt32("FeatureErrorNumber");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("ErrorNumber", ErrorNumber);
		info.AddValue("FeatureErrorNumber", FeatureErrorNumber);
		base.GetObjectData(info, context);
	}
}
