using System;

namespace VideoOS.Management.Server;

public struct TraceLocation : IEquatable<TraceLocation>
{
	private readonly string _area;

	private readonly string _layer;

	private readonly string _methodName;

	public bool IsInitialized
	{
		get
		{
			if (_area != null && _layer != null)
			{
				return _methodName != null;
			}
			return false;
		}
	}

	public TraceLocation(string area, string layer, string methodName)
	{
		_area = area;
		_layer = layer;
		_methodName = methodName;
	}

	public bool Equals(TraceLocation other)
	{
		if (string.Compare(_area, other._area, StringComparison.CurrentCultureIgnoreCase) == 0 && string.Compare(_layer, other._layer, StringComparison.CurrentCultureIgnoreCase) == 0)
		{
			return string.Compare(_methodName, other._methodName, StringComparison.CurrentCultureIgnoreCase) == 0;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is TraceLocation)
		{
			return Equals((TraceLocation)obj);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (_area ?? string.Empty).GetHashCode() ^ (_layer ?? string.Empty).GetHashCode() ^ (_methodName ?? string.Empty).GetHashCode();
	}

	public override string ToString()
	{
		if (!IsInitialized)
		{
			return null;
		}
		return _area + "." + _layer + "." + _methodName;
	}
}
