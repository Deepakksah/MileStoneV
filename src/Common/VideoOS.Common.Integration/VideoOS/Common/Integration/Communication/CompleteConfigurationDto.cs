using System;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public sealed class CompleteConfigurationDto
{
	private ServerResponseStatusDto serverResponseStatus;

	private SystemConfigurationDto systemConfigurationDto;

	private SecurityConfigurationDto securityConfigurationDto;

	private SecurityTokenCollectionDto securityTokensDto;

	public ServerResponseStatusDto ServerResponseStatus
	{
		[DebuggerStepThrough]
		get
		{
			return serverResponseStatus;
		}
	}

	public SystemConfigurationDto SystemConfigurationDto
	{
		[DebuggerStepThrough]
		get
		{
			return systemConfigurationDto;
		}
	}

	public SecurityConfigurationDto SecurityConfigurationDto
	{
		[DebuggerStepThrough]
		get
		{
			return securityConfigurationDto;
		}
	}

	public SecurityTokenCollectionDto SecurityTokensDto
	{
		[DebuggerStepThrough]
		get
		{
			return securityTokensDto;
		}
	}

	public CompleteConfigurationDto(ServerResponseStatusDto serverResponseStatus, SystemConfigurationDto systemConfigurationDto, SecurityConfigurationDto securityConfigurationDto, SecurityTokenCollectionDto securityTokensDto)
	{
		this.serverResponseStatus = serverResponseStatus;
		this.systemConfigurationDto = systemConfigurationDto;
		this.securityConfigurationDto = securityConfigurationDto;
		this.securityTokensDto = securityTokensDto;
	}
}
