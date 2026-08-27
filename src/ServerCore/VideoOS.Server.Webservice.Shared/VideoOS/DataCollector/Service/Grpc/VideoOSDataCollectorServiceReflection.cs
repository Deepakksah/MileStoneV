using System;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;

namespace VideoOS.DataCollector.Service.Grpc;

public static class VideoOSDataCollectorServiceReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static VideoOSDataCollectorServiceReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("CiNWaWRlb09TLkRhdGFDb2xsZWN0b3IuU2VydmljZS5wcm90bxobZ29vZ2xl" + "L3Byb3RvYnVmL2VtcHR5LnByb3RvGh9nb29nbGUvcHJvdG9idWYvdGltZXN0" + "YW1wLnByb3RvIj4KJUdldFBlcmZvcm1hbmNlQ291bnRlckluc3RhbmNlc1Jl" + "cXVlc3QSFQoNY2F0ZWdvcnlfbmFtZRgBIAEoCSKCAQoiR2V0UGVyZm9ybWFu" + "Y2VDb3VudGVyVmFsdWVzUmVxdWVzdBIVCg1jYXRlZ29yeV9uYW1lGAEgASgJ" + "EhQKDGNvdW50ZXJfbmFtZRgCIAEoCRIVCg1pbnN0YW5jZV9uYW1lGAMgASgJ" + "EhgKEHNhbXBsZV9mcmVxdWVuY3kYBCABKAUidgowUG9zdFVwZGF0ZWRDb3Vu" + "dGVyU3RhdGVSdWxlQ29uZmlndXJhdGlvbnNSZXF1ZXN0EkIKE3J1bGVfY29u" + "ZmlnX3VwZGF0ZXMYASADKAsyJS5Db3VudGVyU3RhdGVSdWxlc0NvbmZpZ3Vy" + "YXRpb25VcGRhdGUiMgogR2V0UGVyZm9ybWFuY2VDYXRlZ29yaWVzUmVzcG9u" + "c2USDgoGdmFsdWVzGAEgAygJIlUKJkdldFBlcmZvcm1hbmNlQ291bnRlcklu" + "c3RhbmNlc1Jlc3BvbnNlEisKBnZhbHVlcxgBIAMoCzIbLlBlcmZvcm1hbmNl" + "Q291bnRlckluc3RhbmNlIk8KI0dldFBlcmZvcm1hbmNlQ291bnRlclZhbHVl" + "c1Jlc3BvbnNlEigKBnZhbHVlcxgBIAMoCzIYLlBlcmZvcm1hbmNlQ291bnRl" + "clZhbHVlIisKGkdldERhdGFDb2xsZWN0b3JJZFJlc3BvbnNlEg0KBXZhbHVl" + "GAEgASgFIiAKD0lzQWxpdmVSZXNwb25zZRINCgV2YWx1ZRgBIAEoCCJsCiRD" + "b3VudGVyU3RhdGVSdWxlc0NvbmZpZ3VyYXRpb25VcGRhdGUSDwoHZGVsZXRl" + "ZBgBIAEoCBIzCgtydWxlX2NvbmZpZxgCIAEoCzIeLkNvdW50ZXJTdGF0ZVJ1" + "bGVDb25maWd1cmF0aW9uIsMCChpQZXJmb3JtYW5jZUNvdW50ZXJJbnN0YW5j" + "ZRIVCg1jYXRlZ29yeV9uYW1lGAEgASgJEhQKDGNvdW50ZXJfbmFtZRgCIAEo" + "CRIVCg1pbnN0YW5jZV9uYW1lGAMgASgJEi0KDGNvdW50ZXJfdHlwZRgEIAEo" + "DjIXLlBlcmZvcm1hbmNlQ291bnRlclR5cGUSGQoRY291bnRlcl9kaXJlY3Rp" + "b24YBSABKAgSFgoOZW5hYmxlX2hpc3RvcnkYBiABKAgSHwoXdHJpZ2dlcl9z" + "dGF0ZV9wZXJfdmFsdWUYByABKAgSNgoNY2F0ZWdvcnlfdHlwZRgIIAEoDjIf" + "LlBlcmZvcm1hbmNlQ291bnRlckNhdGVnb3J5VHlwZRIYChBzYW1wbGVfZnJl" + "cXVlbmN5GAkgASgFEgwKBHBhdGgYCiABKAki2QEKF1BlcmZvcm1hbmNlQ291" + "bnRlclZhbHVlEhUKDWNhdGVnb3J5X25hbWUYASABKAkSFAoMY291bnRlcl9u" + "YW1lGAIgASgJEhUKDWluc3RhbmNlX25hbWUYAyABKAkSMwoPY29sbGVjdGlv" + "bl90aW1lGAQgASgLMhouZ29vZ2xlLnByb3RvYnVmLlRpbWVzdGFtcBIRCgly" + "YXdfdmFsdWUYBSABKAMSGAoQY2FsY3VsYXRlZF92YWx1ZRgGIAEoAhIYChBz" + "ZW5kX2ltbWVkaWF0ZWx5GAcgASgIItUBCh1Db3VudGVyU3RhdGVSdWxlQ29u" + "ZmlndXJhdGlvbhIUCgxjb3VudGVyX3BhdGgYASABKAkSGQoRd2FybmluZ190" + "aHJlc2hvbGQYAiABKAISGgoSY3JpdGljYWxfdGhyZXNob2xkGAMgASgCEhMK" + "C3RpbWVfd2luZG93GAQgASgFEhcKD2NvdW50ZXJfcnVsZV9pZBgFIAEoCRIM" + "CgRuYW1lGAYgASgJEhUKDWNhdGVnb3J5X25hbWUYByABKAkSFAoMY291bnRl" + "cl9uYW1lGAggASgJIhUKBFVVSUQSDQoFdmFsdWUYASABKAkqrAYKFlBlcmZv" + "cm1hbmNlQ291bnRlclR5cGUSGQoVTlVNQkVSX09GX0lURU1TX0hFWDMyEAAS" + "GgoVTlVNQkVSX09GX0lURU1TX0hFWDY0EIACEhgKEk5VTUJFUl9PRl9JVEVN" + "U18zMhCAgAQSGAoSTlVNQkVSX09GX0lURU1TXzY0EICCBBIXChBDT1VOVEVS" + "X0RFTFRBXzMyEICIgAISFwoQQ09VTlRFUl9ERUxUQV82NBCAioACEhUKDlNB" + "TVBMRV9DT1VOVEVSEICIhAISIQoaQ09VTlRfUEVSX1RJTUVfSU5URVJWQUxf" + "MzIQgIiUAhIhChpDT1VOVF9QRVJfVElNRV9JTlRFUlZBTF82NBCAipQCEiQK" + "HFJBVEVfT0ZfQ09VTlRTX1BFUl9TRUNPTkRfMzIQgIiEggESJAocUkFURV9P" + "Rl9DT1VOVFNfUEVSX1NFQ09ORF82NBCAioSCARIUCgxSQVdfRlJBQ1RJT04Q" + "gIiIgAISFQoNQ09VTlRFUl9USU1FUhCAioSCAhIUCgxUSU1FUl8xMDBfTlMQ" + "gIrEggISFwoPU0FNUExFX0ZSQUNUSU9OEICIiIYCEh0KFUNPVU5URVJfVElN" + "RVJfSU5WRVJTRRCAioSKAhIcChRUSU1FUl8xMDBfTlNfSU5WRVJTRRCAisSK" + "AhIbChNDT1VOVEVSX01VTFRJX1RJTUVSEICKhJICEiIKGkNPVU5URVJfTVVM" + "VElfVElNRVJfMTAwX05TEICKxJICEiMKG0NPVU5URVJfTVVMVElfVElNRVJf" + "SU5WRVJTRRCAioSaAhIqCiJDT1VOVEVSX01VTFRJX1RJTUVSXzEwMF9OU19J" + "TlZFUlNFEICKxJoCEhgKEEFWRVJBR0VfVElNRVJfMzIQgIiIgAMSFAoMRUxB" + "UFNFRF9USU1FEICKkIEDEhgKEEFWRVJBR0VfQ09VTlRfNjQQgIqIgAQSEwoL" + "U0FNUExFX0JBU0UQgYiMgAQSFAoMQVZFUkFHRV9CQVNFEIKIjIAEEhAKCFJB" + "V19CQVNFEIOIjIAEEhoKEkNPVU5URVJfTVVMVElfQkFTRRCAioyQBCpfCh5Q" + "ZXJmb3JtYW5jZUNvdW50ZXJDYXRlZ29yeVR5cGUSEwoPU0lOR0xFX0lOU1RB" + "TkNFEAASEgoOTVVMVElfSU5TVEFOQ0UQARIUCgdVTktOT1dOEP//////////" + "/wEyxgQKGExpdmVEYXRhQ29sbGVjdG9yU2VydmljZRJVChhHZXRQZXJmb3Jt" + "YW5jZUNhdGVnb3JpZXMSFi5nb29nbGUucHJvdG9idWYuRW1wdHkaIS5HZXRQ" + "ZXJmb3JtYW5jZUNhdGVnb3JpZXNSZXNwb25zZRJxCh5HZXRQZXJmb3JtYW5j" + "ZUNvdW50ZXJJbnN0YW5jZXMSJi5HZXRQZXJmb3JtYW5jZUNvdW50ZXJJbnN0" + "YW5jZXNSZXF1ZXN0GicuR2V0UGVyZm9ybWFuY2VDb3VudGVySW5zdGFuY2Vz" + "UmVzcG9uc2USaAobR2V0UGVyZm9ybWFuY2VDb3VudGVyVmFsdWVzEiMuR2V0" + "UGVyZm9ybWFuY2VDb3VudGVyVmFsdWVzUmVxdWVzdBokLkdldFBlcmZvcm1h" + "bmNlQ291bnRlclZhbHVlc1Jlc3BvbnNlEnYKKVBvc3RVcGRhdGVkQ291bnRl" + "clN0YXRlUnVsZUNvbmZpZ3VyYXRpb25zEjEuUG9zdFVwZGF0ZWRDb3VudGVy" + "U3RhdGVSdWxlQ29uZmlndXJhdGlvbnNSZXF1ZXN0GhYuZ29vZ2xlLnByb3Rv" + "YnVmLkVtcHR5EkkKEkdldERhdGFDb2xsZWN0b3JJZBIWLmdvb2dsZS5wcm90" + "b2J1Zi5FbXB0eRobLkdldERhdGFDb2xsZWN0b3JJZFJlc3BvbnNlEjMKB0lz" + "QWxpdmUSFi5nb29nbGUucHJvdG9idWYuRW1wdHkaEC5Jc0FsaXZlUmVzcG9u" + "c2VCJaoCIlZpZGVvT1MuRGF0YUNvbGxlY3Rvci5TZXJ2aWNlLkdycGNiBnBy" + "b3RvMw=="), new FileDescriptor[2]
		{
			EmptyReflection.Descriptor,
			TimestampReflection.Descriptor
		}, new GeneratedClrTypeInfo(new System.Type[2]
		{
			typeof(PerformanceCounterType),
			typeof(PerformanceCounterCategoryType)
		}, null, new GeneratedClrTypeInfo[13]
		{
			new GeneratedClrTypeInfo(typeof(GetPerformanceCounterInstancesRequest), GetPerformanceCounterInstancesRequest.Parser, new string[1] { "CategoryName" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(GetPerformanceCounterValuesRequest), GetPerformanceCounterValuesRequest.Parser, new string[4] { "CategoryName", "CounterName", "InstanceName", "SampleFrequency" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(PostUpdatedCounterStateRuleConfigurationsRequest), PostUpdatedCounterStateRuleConfigurationsRequest.Parser, new string[1] { "RuleConfigUpdates" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(GetPerformanceCategoriesResponse), GetPerformanceCategoriesResponse.Parser, new string[1] { "Values" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(GetPerformanceCounterInstancesResponse), GetPerformanceCounterInstancesResponse.Parser, new string[1] { "Values" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(GetPerformanceCounterValuesResponse), GetPerformanceCounterValuesResponse.Parser, new string[1] { "Values" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(GetDataCollectorIdResponse), GetDataCollectorIdResponse.Parser, new string[1] { "Value" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(IsAliveResponse), IsAliveResponse.Parser, new string[1] { "Value" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(CounterStateRulesConfigurationUpdate), CounterStateRulesConfigurationUpdate.Parser, new string[2] { "Deleted", "RuleConfig" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(PerformanceCounterInstance), PerformanceCounterInstance.Parser, new string[10] { "CategoryName", "CounterName", "InstanceName", "CounterType", "CounterDirection", "EnableHistory", "TriggerStatePerValue", "CategoryType", "SampleFrequency", "Path" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(PerformanceCounterValue), PerformanceCounterValue.Parser, new string[7] { "CategoryName", "CounterName", "InstanceName", "CollectionTime", "RawValue", "CalculatedValue", "SendImmediately" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(CounterStateRuleConfiguration), CounterStateRuleConfiguration.Parser, new string[8] { "CounterPath", "WarningThreshold", "CriticalThreshold", "TimeWindow", "CounterRuleId", "Name", "CategoryName", "CounterName" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(UUID), UUID.Parser, new string[1] { "Value" }, null, null, null, null)
		}));
	}
}
