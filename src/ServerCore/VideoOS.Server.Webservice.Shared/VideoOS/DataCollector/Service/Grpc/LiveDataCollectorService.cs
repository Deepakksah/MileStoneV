using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Threading;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace VideoOS.DataCollector.Service.Grpc;

public static class LiveDataCollectorService
{
	[GeneratedCode("grpc_csharp_plugin", null)]
	private static class __Helper_MessageCache<T>
	{
		public static readonly bool IsBufferMessage = typeof(IBufferMessage).GetTypeInfo().IsAssignableFrom(typeof(T));
	}

	public class LiveDataCollectorServiceClient : ClientBase<LiveDataCollectorServiceClient>
	{
		[GeneratedCode("grpc_csharp_plugin", null)]
		public LiveDataCollectorServiceClient(ChannelBase channel)
			: base(channel)
		{
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public LiveDataCollectorServiceClient(CallInvoker callInvoker)
			: base(callInvoker)
		{
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		protected LiveDataCollectorServiceClient()
		{
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		protected LiveDataCollectorServiceClient(ClientBaseConfiguration configuration)
			: base(configuration)
		{
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual GetPerformanceCategoriesResponse GetPerformanceCategories(Empty request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return GetPerformanceCategories(request, new CallOptions(headers, deadline, cancellationToken));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual GetPerformanceCategoriesResponse GetPerformanceCategories(Empty request, CallOptions options)
		{
			return base.CallInvoker.BlockingUnaryCall(__Method_GetPerformanceCategories, null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<GetPerformanceCategoriesResponse> GetPerformanceCategoriesAsync(Empty request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return GetPerformanceCategoriesAsync(request, new CallOptions(headers, deadline, cancellationToken));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<GetPerformanceCategoriesResponse> GetPerformanceCategoriesAsync(Empty request, CallOptions options)
		{
			return base.CallInvoker.AsyncUnaryCall(__Method_GetPerformanceCategories, null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual GetPerformanceCounterInstancesResponse GetPerformanceCounterInstances(GetPerformanceCounterInstancesRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return GetPerformanceCounterInstances(request, new CallOptions(headers, deadline, cancellationToken));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual GetPerformanceCounterInstancesResponse GetPerformanceCounterInstances(GetPerformanceCounterInstancesRequest request, CallOptions options)
		{
			return base.CallInvoker.BlockingUnaryCall(__Method_GetPerformanceCounterInstances, null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<GetPerformanceCounterInstancesResponse> GetPerformanceCounterInstancesAsync(GetPerformanceCounterInstancesRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return GetPerformanceCounterInstancesAsync(request, new CallOptions(headers, deadline, cancellationToken));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<GetPerformanceCounterInstancesResponse> GetPerformanceCounterInstancesAsync(GetPerformanceCounterInstancesRequest request, CallOptions options)
		{
			return base.CallInvoker.AsyncUnaryCall(__Method_GetPerformanceCounterInstances, null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual GetPerformanceCounterValuesResponse GetPerformanceCounterValues(GetPerformanceCounterValuesRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return GetPerformanceCounterValues(request, new CallOptions(headers, deadline, cancellationToken));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual GetPerformanceCounterValuesResponse GetPerformanceCounterValues(GetPerformanceCounterValuesRequest request, CallOptions options)
		{
			return base.CallInvoker.BlockingUnaryCall(__Method_GetPerformanceCounterValues, null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<GetPerformanceCounterValuesResponse> GetPerformanceCounterValuesAsync(GetPerformanceCounterValuesRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return GetPerformanceCounterValuesAsync(request, new CallOptions(headers, deadline, cancellationToken));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<GetPerformanceCounterValuesResponse> GetPerformanceCounterValuesAsync(GetPerformanceCounterValuesRequest request, CallOptions options)
		{
			return base.CallInvoker.AsyncUnaryCall(__Method_GetPerformanceCounterValues, null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual Empty PostUpdatedCounterStateRuleConfigurations(PostUpdatedCounterStateRuleConfigurationsRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return PostUpdatedCounterStateRuleConfigurations(request, new CallOptions(headers, deadline, cancellationToken));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual Empty PostUpdatedCounterStateRuleConfigurations(PostUpdatedCounterStateRuleConfigurationsRequest request, CallOptions options)
		{
			return base.CallInvoker.BlockingUnaryCall(__Method_PostUpdatedCounterStateRuleConfigurations, null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<Empty> PostUpdatedCounterStateRuleConfigurationsAsync(PostUpdatedCounterStateRuleConfigurationsRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return PostUpdatedCounterStateRuleConfigurationsAsync(request, new CallOptions(headers, deadline, cancellationToken));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<Empty> PostUpdatedCounterStateRuleConfigurationsAsync(PostUpdatedCounterStateRuleConfigurationsRequest request, CallOptions options)
		{
			return base.CallInvoker.AsyncUnaryCall(__Method_PostUpdatedCounterStateRuleConfigurations, null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual GetDataCollectorIdResponse GetDataCollectorId(Empty request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return GetDataCollectorId(request, new CallOptions(headers, deadline, cancellationToken));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual GetDataCollectorIdResponse GetDataCollectorId(Empty request, CallOptions options)
		{
			return base.CallInvoker.BlockingUnaryCall(__Method_GetDataCollectorId, null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<GetDataCollectorIdResponse> GetDataCollectorIdAsync(Empty request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return GetDataCollectorIdAsync(request, new CallOptions(headers, deadline, cancellationToken));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<GetDataCollectorIdResponse> GetDataCollectorIdAsync(Empty request, CallOptions options)
		{
			return base.CallInvoker.AsyncUnaryCall(__Method_GetDataCollectorId, null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual IsAliveResponse IsAlive(Empty request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return IsAlive(request, new CallOptions(headers, deadline, cancellationToken));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual IsAliveResponse IsAlive(Empty request, CallOptions options)
		{
			return base.CallInvoker.BlockingUnaryCall(__Method_IsAlive, null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<IsAliveResponse> IsAliveAsync(Empty request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return IsAliveAsync(request, new CallOptions(headers, deadline, cancellationToken));
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		public virtual AsyncUnaryCall<IsAliveResponse> IsAliveAsync(Empty request, CallOptions options)
		{
			return base.CallInvoker.AsyncUnaryCall(__Method_IsAlive, null, options, request);
		}

		[GeneratedCode("grpc_csharp_plugin", null)]
		protected override LiveDataCollectorServiceClient NewInstance(ClientBaseConfiguration configuration)
		{
			return new LiveDataCollectorServiceClient(configuration);
		}
	}

	private static readonly string __ServiceName = "LiveDataCollectorService";

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Marshaller<Empty> __Marshaller_google_protobuf_Empty = Marshallers.Create(__Helper_SerializeMessage, (DeserializationContext context) => __Helper_DeserializeMessage(context, Empty.Parser));

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Marshaller<GetPerformanceCategoriesResponse> __Marshaller_GetPerformanceCategoriesResponse = Marshallers.Create(__Helper_SerializeMessage, (DeserializationContext context) => __Helper_DeserializeMessage(context, GetPerformanceCategoriesResponse.Parser));

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Marshaller<GetPerformanceCounterInstancesRequest> __Marshaller_GetPerformanceCounterInstancesRequest = Marshallers.Create(__Helper_SerializeMessage, (DeserializationContext context) => __Helper_DeserializeMessage(context, GetPerformanceCounterInstancesRequest.Parser));

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Marshaller<GetPerformanceCounterInstancesResponse> __Marshaller_GetPerformanceCounterInstancesResponse = Marshallers.Create(__Helper_SerializeMessage, (DeserializationContext context) => __Helper_DeserializeMessage(context, GetPerformanceCounterInstancesResponse.Parser));

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Marshaller<GetPerformanceCounterValuesRequest> __Marshaller_GetPerformanceCounterValuesRequest = Marshallers.Create(__Helper_SerializeMessage, (DeserializationContext context) => __Helper_DeserializeMessage(context, GetPerformanceCounterValuesRequest.Parser));

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Marshaller<GetPerformanceCounterValuesResponse> __Marshaller_GetPerformanceCounterValuesResponse = Marshallers.Create(__Helper_SerializeMessage, (DeserializationContext context) => __Helper_DeserializeMessage(context, GetPerformanceCounterValuesResponse.Parser));

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Marshaller<PostUpdatedCounterStateRuleConfigurationsRequest> __Marshaller_PostUpdatedCounterStateRuleConfigurationsRequest = Marshallers.Create(__Helper_SerializeMessage, (DeserializationContext context) => __Helper_DeserializeMessage(context, PostUpdatedCounterStateRuleConfigurationsRequest.Parser));

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Marshaller<GetDataCollectorIdResponse> __Marshaller_GetDataCollectorIdResponse = Marshallers.Create(__Helper_SerializeMessage, (DeserializationContext context) => __Helper_DeserializeMessage(context, GetDataCollectorIdResponse.Parser));

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Marshaller<IsAliveResponse> __Marshaller_IsAliveResponse = Marshallers.Create(__Helper_SerializeMessage, (DeserializationContext context) => __Helper_DeserializeMessage(context, IsAliveResponse.Parser));

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Method<Empty, GetPerformanceCategoriesResponse> __Method_GetPerformanceCategories = new Method<Empty, GetPerformanceCategoriesResponse>(MethodType.Unary, __ServiceName, "GetPerformanceCategories", __Marshaller_google_protobuf_Empty, __Marshaller_GetPerformanceCategoriesResponse);

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Method<GetPerformanceCounterInstancesRequest, GetPerformanceCounterInstancesResponse> __Method_GetPerformanceCounterInstances = new Method<GetPerformanceCounterInstancesRequest, GetPerformanceCounterInstancesResponse>(MethodType.Unary, __ServiceName, "GetPerformanceCounterInstances", __Marshaller_GetPerformanceCounterInstancesRequest, __Marshaller_GetPerformanceCounterInstancesResponse);

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Method<GetPerformanceCounterValuesRequest, GetPerformanceCounterValuesResponse> __Method_GetPerformanceCounterValues = new Method<GetPerformanceCounterValuesRequest, GetPerformanceCounterValuesResponse>(MethodType.Unary, __ServiceName, "GetPerformanceCounterValues", __Marshaller_GetPerformanceCounterValuesRequest, __Marshaller_GetPerformanceCounterValuesResponse);

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Method<PostUpdatedCounterStateRuleConfigurationsRequest, Empty> __Method_PostUpdatedCounterStateRuleConfigurations = new Method<PostUpdatedCounterStateRuleConfigurationsRequest, Empty>(MethodType.Unary, __ServiceName, "PostUpdatedCounterStateRuleConfigurations", __Marshaller_PostUpdatedCounterStateRuleConfigurationsRequest, __Marshaller_google_protobuf_Empty);

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Method<Empty, GetDataCollectorIdResponse> __Method_GetDataCollectorId = new Method<Empty, GetDataCollectorIdResponse>(MethodType.Unary, __ServiceName, "GetDataCollectorId", __Marshaller_google_protobuf_Empty, __Marshaller_GetDataCollectorIdResponse);

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static readonly Method<Empty, IsAliveResponse> __Method_IsAlive = new Method<Empty, IsAliveResponse>(MethodType.Unary, __ServiceName, "IsAlive", __Marshaller_google_protobuf_Empty, __Marshaller_IsAliveResponse);

	public static ServiceDescriptor Descriptor => VideoOSDataCollectorServiceReflection.Descriptor.Services[0];

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static void __Helper_SerializeMessage(IMessage message, SerializationContext context)
	{
		if (message is IBufferMessage)
		{
			context.SetPayloadLength(message.CalculateSize());
			message.WriteTo(context.GetBufferWriter());
			context.Complete();
		}
		else
		{
			context.Complete(message.ToByteArray());
		}
	}

	[GeneratedCode("grpc_csharp_plugin", null)]
	private static T __Helper_DeserializeMessage<T>(DeserializationContext context, MessageParser<T> parser) where T : IMessage<T>
	{
		if (__Helper_MessageCache<T>.IsBufferMessage)
		{
			return parser.ParseFrom(context.PayloadAsReadOnlySequence());
		}
		return parser.ParseFrom(context.PayloadAsNewBuffer());
	}
}
