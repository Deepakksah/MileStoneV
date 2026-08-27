using System;
using System.Collections.Generic;
using VideoOS.Common.Integration.Properties;

namespace VideoOS.Common.Integration;

public class RetentionTimeOption
{
	private bool _default;

	private Guid _id;

	private string _name;

	private List<RetentionTime> _retentionTimeOptions;

	public bool Default
	{
		get
		{
			return _default;
		}
		set
		{
			_default = value;
		}
	}

	public Guid Id
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
		}
	}

	public string Name
	{
		get
		{
			if (Id.Equals(new Guid("b665dde9-1000-41fe-af15-ef5946314da7")))
			{
				return EvidenceLock.EvidenceLock_Default;
			}
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public List<RetentionTime> RetentionTimes
	{
		get
		{
			return _retentionTimeOptions;
		}
		set
		{
			_retentionTimeOptions = value;
		}
	}
}
