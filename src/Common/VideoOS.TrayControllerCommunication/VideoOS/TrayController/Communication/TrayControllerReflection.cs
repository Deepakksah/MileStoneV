using System;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;

namespace VideoOS.TrayController.Communication;

public static class TrayControllerReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static TrayControllerReflection()
	{
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Expected O, but got Unknown
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Expected O, but got Unknown
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Expected O, but got Unknown
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChRUcmF5Q29udHJvbGxlci5wcm90bxofZ29vZ2xlL3Byb3RvYnVmL3RpbWVz" + "dGFtcC5wcm90bxobZ29vZ2xlL3Byb3RvYnVmL2VtcHR5LnByb3RvIiwKE0dl" + "dE1lc3NhZ2VzX1JlcXVlc3QSFQoNaGlnaHdhdGVyTWFyaxgBIAEoAyJcChBN" + "ZXNzYWdlX1Jlc3BvbnNlEhAKCHNlcXVlbmNlGAEgASgDEigKBHdoZW4YAiAB" + "KAsyGi5nb29nbGUucHJvdG9idWYuVGltZXN0YW1wEgwKBHdoYXQYAyABKAki" + "NgoPQXJyYXlPZk1lc3NhZ2VzEiMKCE1lc3NhZ2VzGAEgAygLMhEuTWVzc2Fn" + "ZV9SZXNwb25zZSLBAQoVRmFpbG92ZXJSZWNvcmRlclN0YXRlEiQKHEhhc1Nz" + "bEVycm9yV2l0aExvY2FsUmVjb3JkZXIYASABKAgSLgomSGFzQXV0aG9yaXph" + "dGlvbkVycm9yV2l0aExvY2FsUmVjb3JkZXIYAiABKAgSIQoZTG9jYWxSZWNv" + "cmRlclN0YXJ0dXBFcnJvchgDIAEoCBIvCg5XYXRjaEl0ZW1FcnJvchgEIAEo" + "DjIXLlJlY29yZGVyV2F0Y2hJdGVtRXJyb3IifwoVUmVjb3JkZXJTdGF0ZVJl" + "c3BvbnNlEjEKDHNlcnZpY2VTdGF0ZRgBIAEoDjIbLlNlcnZpY2VTdGF0ZV9D" + "b21tdW5pY2F0aW9uEjMKDWNvbm5lY3RTdGF0dXMYAiABKA4yHC5Db25uZWN0" + "U3RhdHVzX0NvbW11bmljYXRpb24iSAoTU2VydmVyU3RhdGVSZXNwb25zZRIx" + "CgxzZXJ2aWNlU3RhdGUYASABKA4yGy5TZXJ2aWNlU3RhdGVfQ29tbXVuaWNh" + "dGlvbiK2AQoVRmFpbG92ZXJTdGF0ZVJlc3BvbnNlEjEKDHNlcnZpY2VTdGF0" + "ZRgBIAEoDjIbLlNlcnZpY2VTdGF0ZV9Db21tdW5pY2F0aW9uEjMKDWNvbm5l" + "Y3RTdGF0dXMYAiABKA4yHC5Db25uZWN0U3RhdHVzX0NvbW11bmljYXRpb24S" + "NQoVZmFpbG92ZXJSZWNvcmRlclN0YXRlGAMgASgLMhYuRmFpbG92ZXJSZWNv" + "cmRlclN0YXRlKp8BChpTZXJ2aWNlU3RhdGVfQ29tbXVuaWNhdGlvbhIRCg1J" + "bmRldGVybWluYXRlEAASEwoPQ29udGludWVQZW5kaW5nEAESCgoGUGF1c2Vk" + "EAISEAoMUGF1c2VQZW5kaW5nEAMSCwoHUnVubmluZxAEEhAKDFN0YXJ0UGVu" + "ZGluZxAFEgsKB1N0b3BwZWQQBhIPCgtTdG9wUGVuZGluZxAHKpcBChtDb25u" + "ZWN0U3RhdHVzX0NvbW11bmljYXRpb24SEAoMRGlzY29ubmVjdGVkEAASEAoM" + "UmVjb25uZWN0aW5nEAESDgoKQ29ubmVjdGluZxACEg0KCUNvbm5lY3RlZBAD" + "EhoKFkNvbm5lY3RlZEF0dGFjaFBlbmRpbmcQBBIZChVTc2xDb21tdW5pY2F0" + "aW9uRXJyb3IQBSpLChZSZWNvcmRlcldhdGNoSXRlbUVycm9yEggKBE5vbmUQ" + "ABIUChBTc2xDb21tdW5pY2F0aW9uEAESEQoNQXV0aG9yaXphdGlvbhACMpkB" + "Ch1SZWNvcmRlclRyYXlDb250cm9sbGVyU2VydmljZRI1CgtHZXRNZXNzYWdl" + "cxIULkdldE1lc3NhZ2VzX1JlcXVlc3QaEC5BcnJheU9mTWVzc2FnZXMSQQoP" + "R2V0U2VydmljZVN0YXRlEhYuZ29vZ2xlLnByb3RvYnVmLkVtcHR5GhYuUmVj" + "b3JkZXJTdGF0ZVJlc3BvbnNlMpUBChtTZXJ2ZXJUcmF5Q29udHJvbGxlclNl" + "cnZpY2USNQoLR2V0TWVzc2FnZXMSFC5HZXRNZXNzYWdlc19SZXF1ZXN0GhAu" + "QXJyYXlPZk1lc3NhZ2VzEj8KD0dldFNlcnZpY2VTdGF0ZRIWLmdvb2dsZS5w" + "cm90b2J1Zi5FbXB0eRoULlNlcnZlclN0YXRlUmVzcG9uc2UymQEKHUZhaWxv" + "dmVyVHJheUNvbnRyb2xsZXJTZXJ2aWNlEjUKC0dldE1lc3NhZ2VzEhQuR2V0" + "TWVzc2FnZXNfUmVxdWVzdBoQLkFycmF5T2ZNZXNzYWdlcxJBCg9HZXRTZXJ2" + "aWNlU3RhdGUSFi5nb29nbGUucHJvdG9idWYuRW1wdHkaFi5GYWlsb3ZlclN0" + "YXRlUmVzcG9uc2VCJ6oCJFZpZGVvT1MuVHJheUNvbnRyb2xsZXIuQ29tbXVu" + "aWNhdGlvbmIGcHJvdG8z"), (FileDescriptor[])(object)new FileDescriptor[2]
		{
			TimestampReflection.Descriptor,
			EmptyReflection.Descriptor
		}, new GeneratedClrTypeInfo(new Type[3]
		{
			typeof(ServiceState_Communication),
			typeof(ConnectStatus_Communication),
			typeof(RecorderWatchItemError)
		}, (Extension[])null, (GeneratedClrTypeInfo[])(object)new GeneratedClrTypeInfo[7]
		{
			new GeneratedClrTypeInfo(typeof(GetMessages_Request), (MessageParser)(object)GetMessages_Request.Parser, new string[1] { "HighwaterMark" }, (string[])null, (Type[])null, (Extension[])null, (GeneratedClrTypeInfo[])null),
			new GeneratedClrTypeInfo(typeof(Message_Response), (MessageParser)(object)Message_Response.Parser, new string[3] { "Sequence", "When", "What" }, (string[])null, (Type[])null, (Extension[])null, (GeneratedClrTypeInfo[])null),
			new GeneratedClrTypeInfo(typeof(ArrayOfMessages), (MessageParser)(object)ArrayOfMessages.Parser, new string[1] { "Messages" }, (string[])null, (Type[])null, (Extension[])null, (GeneratedClrTypeInfo[])null),
			new GeneratedClrTypeInfo(typeof(FailoverRecorderState), (MessageParser)(object)FailoverRecorderState.Parser, new string[4] { "HasSslErrorWithLocalRecorder", "HasAuthorizationErrorWithLocalRecorder", "LocalRecorderStartupError", "WatchItemError" }, (string[])null, (Type[])null, (Extension[])null, (GeneratedClrTypeInfo[])null),
			new GeneratedClrTypeInfo(typeof(RecorderStateResponse), (MessageParser)(object)RecorderStateResponse.Parser, new string[2] { "ServiceState", "ConnectStatus" }, (string[])null, (Type[])null, (Extension[])null, (GeneratedClrTypeInfo[])null),
			new GeneratedClrTypeInfo(typeof(ServerStateResponse), (MessageParser)(object)ServerStateResponse.Parser, new string[1] { "ServiceState" }, (string[])null, (Type[])null, (Extension[])null, (GeneratedClrTypeInfo[])null),
			new GeneratedClrTypeInfo(typeof(FailoverStateResponse), (MessageParser)(object)FailoverStateResponse.Parser, new string[3] { "ServiceState", "ConnectStatus", "FailoverRecorderState" }, (string[])null, (Type[])null, (Extension[])null, (GeneratedClrTypeInfo[])null)
		}));
	}
}
