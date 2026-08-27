using System;

namespace VideoOS.GisMapSharedData.DataContracts.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public sealed class DatafieldAttribute : Attribute
{
}
