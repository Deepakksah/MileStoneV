using System;

namespace VideoOS.Toolkit;

public interface IUTCTimeProperty : IProperty
{
	DateTime Value { get; }
}
