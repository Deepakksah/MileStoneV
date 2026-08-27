using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;
using VideoOS.Server.Webservice.Shared;

namespace VideoOS.Management.Server;

public sealed class MatrixViewData
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Address { get; set; }

	public int Port { get; set; }

	public string Password { get; set; }

	public DateTime LastModified { get; set; }

	public static IEnumerable<MatrixViewData> QueryMatrixViews(IGeminiRequestContext requestContext)
	{
		using IMatrixViewDataComponent matrixViewDataComponent = requestContext.CreateMatrixViewDataComponent();
		Collection<MatrixViewData> collection = new Collection<MatrixViewData>();
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (MatrixViewData item in matrixViewDataComponent.QueryMatrixViews())
		{
			if (securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Matrix, "GENERIC_READ", item.Id.ToString()))
			{
				collection.Add(item);
			}
		}
		return collection;
	}

	public static MatrixViewData QueryMatrix(IGeminiRequestContext requestContext, string id)
	{
		Validation.CheckObjectId(ref id, "id", allowNullOrEmpty: false);
		try
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Matrix, "GENERIC_READ", id);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			return null;
		}
		using IMatrixViewDataComponent matrixViewDataComponent = requestContext.CreateMatrixViewDataComponent();
		return matrixViewDataComponent.QueryMatrix(id);
	}

	public static void UpdateMatrix(IGeminiRequestContext requestContext, MatrixViewData matrix)
	{
		Validation.CheckName(matrix.Name, "name");
		string description = matrix.Description;
		Validation.CheckDescription(ref description, "description");
		matrix.Description = description;
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Matrix, "GENERIC_WRITE", matrix.Id.ToString());
		using IMatrixViewDataComponent matrixViewDataComponent = requestContext.CreateMatrixViewDataComponent();
		matrixViewDataComponent.UpdateMatrix(matrix);
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogEntryAuditLogger.AuditMatrixUpdated(matrix.Id, matrix.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Matrix", matrix.Id.ToString(), EventAction.Updated, matrix.Name);
	}

	public static MatrixViewData CreateMatrix(IGeminiRequestContext requestContext, string id, string name, string description, string address, int port, string password)
	{
		Validation.CheckName(name, "name");
		Validation.CheckDescription(ref description, "description");
		requestContext.GetSecurityManagerService().CheckOverallPermission(requestContext, SecurityNamespaceIdentifier.Matrix, "GENERIC_WRITE");
		MatrixViewData matrixViewData = new MatrixViewData
		{
			Id = id,
			Name = name,
			Description = description,
			Address = address,
			Port = port,
			Password = password
		};
		using IMatrixViewDataComponent matrixViewDataComponent = requestContext.CreateMatrixViewDataComponent();
		matrixViewDataComponent.CreateMatrix(matrixViewData);
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogEntryAuditLogger.AuditMatrixCreated(matrixViewData.Id, matrixViewData.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Matrix", matrixViewData.Id.ToString(), EventAction.Added, matrixViewData.Name);
		return matrixViewData;
	}

	public static void DeleteMatrix(IGeminiRequestContext requestContext, string id, string name)
	{
		requestContext.GetSecurityManagerService().CheckOverallPermission(requestContext, SecurityNamespaceIdentifier.Matrix, "GENERIC_WRITE");
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Matrix, "GENERIC_WRITE", id.ToString());
		using IMatrixViewDataComponent matrixViewDataComponent = requestContext.CreateMatrixViewDataComponent();
		matrixViewDataComponent.DeleteMatrix(id);
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogEntryAuditLogger.AuditMatrixDelete(id, name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Matrix", id.ToString(), EventAction.Deleted, string.Empty);
	}

	public static void SetMatrixCameras(IGeminiRequestContext requestContext, MatrixViewData matrix, string cameraId)
	{
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.AuthenticatedUserName, requestContext.RemoteIPAddress);
		CallWithRetryExceptionHandling(requestContext, delegate(ServerWebServiceClient serverWebServiceClient)
		{
			serverWebServiceClient.MatrixMonitorActivateCamera(new Guid(matrix.Id), new Guid(cameraId), basicCallContextInfo);
		}, delegate
		{
			GeminiTrace.Error(TraceType.Configuration, "Management Server communication failed: MatrixMonitorActivateCamera('{0}','{1}')", matrix.Id, cameraId);
		});
	}

	private static void CallWithRetryExceptionHandling(IGeminiRequestContext requestContext, Action<ServerWebServiceClient> call, Action exceptionTraceCall)
	{
		ServerWebServiceManager serverWebServiceManager = null;
		try
		{
			serverWebServiceManager = requestContext.GetService<ServerClientService>().GetClient();
			serverWebServiceManager.CallWithRetry(call, exceptionTraceCall);
		}
		finally
		{
			requestContext.GetService<ServerClientService>().ReleaseClient(serverWebServiceManager);
		}
	}
}
