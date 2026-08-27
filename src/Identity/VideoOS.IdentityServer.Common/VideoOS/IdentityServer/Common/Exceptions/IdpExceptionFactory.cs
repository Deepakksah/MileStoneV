using System;
using System.Linq;
using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common.Exceptions;

public static class IdpExceptionFactory
{
	public static IdpException CreateException(int statusCode, ErrorResponse errorResponse)
	{
		switch (statusCode)
		{
		case 401:
			return new UnauthorizedException(statusCode, errorResponse);
		case 403:
			return new ForbiddenException(errorResponse);
		case 404:
			return CreateNotFoundException(errorResponse);
		case 500:
			return new InternalServerErrorException(errorResponse);
		default:
			switch (errorResponse?.Code)
			{
			case "LockedOut":
				return new LockedOutException(statusCode, errorResponse);
			case "ChangePassword":
				return new ChangePasswordException(statusCode, errorResponse);
			case "InvalidCredentials":
				return new UnauthorizedException(statusCode, errorResponse);
			default:
			{
				if (errorResponse?.Error == "invalid_client")
				{
					return new UnauthorizedException(statusCode, errorResponse);
				}
				if (errorResponse?.Error == "invalid_grant")
				{
					return new UnauthorizedException(statusCode, errorResponse);
				}
				if (statusCode == 400 && errorResponse?.Extensions != null && errorResponse.Extensions.TryGetValue("reason", out var value) && string.Equals(value.ToString(), "ModelValidationError", StringComparison.Ordinal))
				{
					return new InvalidArgumentsException(errorResponse);
				}
				switch (errorResponse?.Errors?.Keys.FirstOrDefault())
				{
				case "InvalidRoleName":
				case "InvalidUserName":
					return new InvalidNameException(statusCode, errorResponse);
				case "DuplicateUserName":
					return new AlreadyExistsException(statusCode, errorResponse);
				case "DuplicateEmail":
					return new EmailAlreadyExistsException(statusCode, errorResponse);
				case "ErrorTheSamePasswords":
					return new PasswordSameAsOldException(statusCode, errorResponse);
				case "PasswordRequiresLower":
				case "PasswordRequiresUpper":
				case "PasswordRequiresDigit":
				case "ErrorStrongPassword":
				case "PasswordRequiresUniqueChars":
				case "PasswordRequiresNonAlphanumeric":
				case "PasswordTooShort":
					return new PasswordNotComplexEnoughException(statusCode, errorResponse);
				case "LoginAlreadyAssociated":
					return new LoginAlreadyAssociatedException(statusCode, errorResponse);
				case "NotAllowed":
					return new NotAllowedException(statusCode, errorResponse);
				case "ArgumentsNotResolved":
					return new BadArgumentsException(errorResponse);
				case "DuplicateAdminIdentity":
					return new DuplicateAdminIdentityException(statusCode, errorResponse);
				case "AdminIdentityNotFound":
					return new AdminIdentityNotFoundException(statusCode, errorResponse);
				default:
					return new DefaultErrorException(statusCode, errorResponse);
				}
			}
			}
		}
	}

	private static IdpException CreateNotFoundException(ErrorResponse errorsResponse)
	{
		if (errorsResponse?.Errors?.Keys.FirstOrDefault() == "UserNotFound")
		{
			return new UserNotFoundException(errorsResponse);
		}
		return new NotFoundException();
	}
}
