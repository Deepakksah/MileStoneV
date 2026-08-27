using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace VideoOS.TrayController.Communication;

public static class FailoverTrayControllerService
{
	[GeneratedCode("grpc_csharp_plugin", null)]
	private static class __Helper_MessageCache<T>
	{
		public static readonly bool IsBufferMessage = typeof(IBufferMessage).GetTypeInfo().IsAssignableFrom(typeof(T));
	}

	[BindServiceMethod(typeof(FailoverTrayControllerService), "BindService")]
	public abstract class FailoverTrayControllerServiceBase
	{
		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual Task<ArrayOfMessages> GetMessages(GetMessages_Request request, ServerCallContext context)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			throw new RpcException(new Status((StatusCode)12, ""));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual Task<FailoverStateResponse> GetServiceState(Empty request, ServerCallContext context)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			throw new RpcException(new Status((StatusCode)12, ""));
		}
	}

	public class FailoverTrayControllerServiceClient : ClientBase<FailoverTrayControllerServiceClient>
	{
		[GeneratedCode("grpc_csharp_plugin", null)]
		public FailoverTrayControllerServiceClient(ChannelBase channel)
			: base(channel)
		{
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public FailoverTrayControllerServiceClient(CallInvoker callInvoker)
			: base(callInvoker)
		{
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		protected FailoverTrayControllerServiceClient()
		{
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		protected FailoverTrayControllerServiceClient(ClientBaseConfiguration configuration)
			: base(configuration)
		{
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual ArrayOfMessages GetMessages(GetMessages_Request request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			return this.GetMessages(request, new CallOptions(headers, deadline, cancellationToken, (WriteOptions)null, (ContextPropagationToken)null, (CallCredentials)null));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual ArrayOfMessages GetMessages(GetMessages_Request request, CallOptions options)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return ((ClientBase)this).CallInvoker.BlockingUnaryCall<GetMessages_Request, ArrayOfMessages>(__Method_GetMessages, (string)null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<ArrayOfMessages> GetMessagesAsync(GetMessages_Request request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			return this.GetMessagesAsync(request, new CallOptions(headers, deadline, cancellationToken, (WriteOptions)null, (ContextPropagationToken)null, (CallCredentials)null));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<ArrayOfMessages> GetMessagesAsync(GetMessages_Request request, CallOptions options)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return ((ClientBase)this).CallInvoker.AsyncUnaryCall<GetMessages_Request, ArrayOfMessages>(__Method_GetMessages, (string)null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual FailoverStateResponse GetServiceState(Empty request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			return this.GetServiceState(request, new CallOptions(headers, deadline, cancellationToken, (WriteOptions)null, (ContextPropagationToken)null, (CallCredentials)null));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual FailoverStateResponse GetServiceState(Empty request, CallOptions options)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return ((ClientBase)this).CallInvoker.BlockingUnaryCall<Empty, FailoverStateResponse>(__Method_GetServiceState, (string)null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<FailoverStateResponse> GetServiceStateAsync(Empty request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			return this.GetServiceStateAsync(request, new CallOptions(headers, deadline, cancellationToken, (WriteOptions)null, (ContextPropagationToken)null, (CallCredentials)null));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<FailoverStateResponse> GetServiceStateAsync(Empty request, CallOptions options)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return ((ClientBase)this).CallInvoker.AsyncUnaryCall<Empty, FailoverStateResponse>(__Method_GetServiceState, (string)null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		protected override FailoverTrayControllerServiceClient NewInstance(ClientBaseConfiguration configuration)
		{
			return new FailoverTrayControllerServiceClient(configuration);
		}
	}

	private static readonly string __ServiceName = "FailoverTrayControllerService";

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Marshaller<GetMessages_Request> __Marshaller_GetMessages_Request = Marshallers.Create<GetMessages_Request>((Action<GetMessages_Request, SerializationContext>)__Helper_SerializeMessage, (Func<DeserializationContext, GetMessages_Request>)((DeserializationContext context) => __Helper_DeserializeMessage<GetMessages_Request>(context, GetMessages_Request.Parser)));

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Marshaller<ArrayOfMessages> __Marshaller_ArrayOfMessages = Marshallers.Create<ArrayOfMessages>((Action<ArrayOfMessages, SerializationContext>)__Helper_SerializeMessage, (Func<DeserializationContext, ArrayOfMessages>)((DeserializationContext context) => __Helper_DeserializeMessage<ArrayOfMessages>(context, ArrayOfMessages.Parser)));

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Marshaller<Empty> __Marshaller_google_protobuf_Empty = Marshallers.Create<Empty>((Action<Empty, SerializationContext>)__Helper_SerializeMessage, (Func<DeserializationContext, Empty>)((DeserializationContext context) => __Helper_DeserializeMessage<Empty>(context, Empty.Parser)));

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Marshaller<FailoverStateResponse> __Marshaller_FailoverStateResponse = Marshallers.Create<FailoverStateResponse>((Action<FailoverStateResponse, SerializationContext>)__Helper_SerializeMessage, (Func<DeserializationContext, FailoverStateResponse>)((DeserializationContext context) => __Helper_DeserializeMessage<FailoverStateResponse>(context, FailoverStateResponse.Parser)));

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Method<GetMessages_Request, ArrayOfMessages> __Method_GetMessages = new Method<GetMessages_Request, ArrayOfMessages>((MethodType)0, __ServiceName, "GetMessages", __Marshaller_GetMessages_Request, __Marshaller_ArrayOfMessages);

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Method<Empty, FailoverStateResponse> __Method_GetServiceState = new Method<Empty, FailoverStateResponse>((MethodType)0, __ServiceName, "GetServiceState", __Marshaller_google_protobuf_Empty, __Marshaller_FailoverStateResponse);

	public static ServiceDescriptor Descriptor => TrayControllerReflection.Descriptor.Services[2];

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static void __Helper_SerializeMessage(IMessage message, SerializationContext context)
	{
		if (message is IBufferMessage)
		{
			context.SetPayloadLength(message.CalculateSize());
			MessageExtensions.WriteTo(message, context.GetBufferWriter());
			context.Complete();
		}
		else
		{
			context.Complete(MessageExtensions.ToByteArray(message));
		}
	}

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static T __Helper_DeserializeMessage<T>(DeserializationContext context, MessageParser<T> parser) where T : IMessage<T>
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (__Helper_MessageCache<T>.IsBufferMessage)
		{
			return parser.ParseFrom(context.PayloadAsReadOnlySequence());
		}
		return parser.ParseFrom(context.PayloadAsNewBuffer());
	}

	[GeneratedCode("grpc_csharp_plugin", null)]
	public static ServerServiceDefinition BindService(FailoverTrayControllerServiceBase serviceImpl)
	{
		return ServerServiceDefinition.CreateBuilder().AddMethod<GetMessages_Request, ArrayOfMessages>(__Method_GetMessages, (UnaryServerMethod<GetMessages_Request, ArrayOfMessages>)serviceImpl.GetMessages).AddMethod<Empty, FailoverStateResponse>(__Method_GetServiceState, (UnaryServerMethod<Empty, FailoverStateResponse>)serviceImpl.GetServiceState)
			.Build();
	}

	[GeneratedCode("grpc_csharp_plugin", null)]
	public static void BindService(ServiceBinderBase serviceBinder, FailoverTrayControllerServiceBase serviceImpl)
	{
		Method<GetMessages_Request, ArrayOfMessages> _Method_GetMessages = __Method_GetMessages;
		object obj;
		if (serviceImpl != null)
		{
			obj = new UnaryServerMethod<GetMessages_Request, ArrayOfMessages>(serviceImpl.GetMessages);
		}
		else
		{
			obj = null;
		}
		serviceBinder.AddMethod<GetMessages_Request, ArrayOfMessages>(_Method_GetMessages, (UnaryServerMethod<GetMessages_Request, ArrayOfMessages>)obj);
		Method<Empty, FailoverStateResponse> _Method_GetServiceState = __Method_GetServiceState;
		object obj2;
		if (serviceImpl != null)
		{
			obj2 = new UnaryServerMethod<Empty, FailoverStateResponse>(serviceImpl.GetServiceState);
		}
		else
		{
			obj2 = null;
		}
		serviceBinder.AddMethod<Empty, FailoverStateResponse>(_Method_GetServiceState, (UnaryServerMethod<Empty, FailoverStateResponse>)obj2);
	}
}
