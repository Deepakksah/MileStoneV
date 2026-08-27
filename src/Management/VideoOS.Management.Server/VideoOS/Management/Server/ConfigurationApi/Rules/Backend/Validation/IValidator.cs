namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public interface IValidator<T>
{
	ValidationError[] Validate(IGeminiRequestContext requestContext, T model);
}
