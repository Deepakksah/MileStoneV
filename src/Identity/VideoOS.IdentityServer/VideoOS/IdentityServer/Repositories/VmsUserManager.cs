using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Extensions;
using VideoOS.IdentityServer.Helper;
using VideoOS.IdentityServer.Model.Requests;
using VideoOS.IdentityServer.Repositories.Context;
using VideoOS.IdentityServer.ResourceParameters;

namespace VideoOS.IdentityServer.Repositories;

public class VmsUserManager : UserManager<VmsUser>
{
	private readonly IDbContextProvider<IdentityDbContextWrapper> _provider;

	private readonly IDateTimeHelper _dateTimeHelper;

	private IQueryable<VmsUser> VmsUserCollection => _provider.Context.Users;

	public VmsUserManager(IUserStore<VmsUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<VmsUser> passwordHasher, IEnumerable<IUserValidator<VmsUser>> userValidators, IEnumerable<IPasswordValidator<VmsUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<VmsUser>> logger, IDbContextProvider<IdentityDbContextWrapper> provider, IDateTimeHelper dateTimeHelper)
		: base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
	{
		_provider = provider;
		_dateTimeHelper = dateTimeHelper;
	}

	public virtual async Task<VmsUser> FindByIdAsync(string id, params string[] includes)
	{
		return await VmsUserCollection.IncludeMultiple(includes).SingleOrDefaultAsync((VmsUser u) => u.Id == id);
	}

	public virtual async Task<IEnumerable<VmsUser>> FindByIdsAsync(IEnumerable<string> ids, params string[] includes)
	{
		return await VmsUserCollection.Where((VmsUser u) => ids.Contains(u.Id)).IncludeMultiple(includes).ToListAsync();
	}

	public virtual async Task<VmsUser> FindByLoginAsync(string loginProvider, string providerKey, params string[] includes)
	{
		IdentityUserLogin<string> login = await _provider.Context.UserLogins.SingleOrDefaultAsync((IdentityUserLogin<string> userLogin) => userLogin.LoginProvider.ToLower() == loginProvider.ToLower() && userLogin.ProviderKey == providerKey);
		if (login == null)
		{
			return null;
		}
		return await VmsUserCollection.IncludeMultiple(includes).SingleOrDefaultAsync((VmsUser u) => u.Id == login.UserId);
	}

	public virtual async Task<IEnumerable<VmsUser>> FindByNamesAsync(IEnumerable<string> names, params string[] includes)
	{
		return await VmsUserCollection.Where((VmsUser u) => names.Contains(u.UserName)).IncludeMultiple(includes).ToListAsync();
	}

	public virtual async Task<VmsUser> FindByNameAsync(string name, params string[] includes)
	{
		UsersResourceParameters resourceParameters = new UsersResourceParameters
		{
			Username = name
		};
		return (await FindByFilterAsync(resourceParameters, includes)).SingleOrDefault();
	}

	public virtual async Task<IEnumerable<VmsUser>> FindByFilterAsync(Expression<Func<VmsUser, bool>> filter, params string[] includes)
	{
		if (filter == null)
		{
			throw new ArgumentNullException("filter");
		}
		return await VmsUserCollection.Where(filter).IncludeMultiple(includes).ToListAsync();
	}

	public virtual async Task<IEnumerable<VmsUser>> FindByFilterAsync(UsersResourceParameters resourceParameters, params string[] includes)
	{
		if (resourceParameters == null)
		{
			throw new ArgumentNullException("resourceParameters");
		}
		IQueryable<VmsUser> source = VmsUserCollection.IncludeMultiple(includes);
		if (string.IsNullOrWhiteSpace(resourceParameters.Username) && resourceParameters.ClaimType == null && resourceParameters.ClaimValue == null)
		{
			return await source.AsNoTracking().ToListAsync();
		}
		if (!string.IsNullOrWhiteSpace(resourceParameters.Username))
		{
			string username = resourceParameters.Username.Trim();
			source = source.Where((VmsUser a) => a.NormalizedUserName == NormalizeName(username)).IncludeMultiple(includes);
		}
		if (resourceParameters.ClaimType != null)
		{
			string claimType = resourceParameters.ClaimType.Trim();
			source = source.Where((VmsUser a) => a.Claims.SingleOrDefault((VmsUserClaim s) => s.ClaimType == claimType) != null);
		}
		if (resourceParameters.ClaimValue != null)
		{
			string claimValue = resourceParameters.ClaimValue.Trim();
			source = source.Where((VmsUser a) => a.Claims.SingleOrDefault((VmsUserClaim s) => s.ClaimValue == claimValue) != null);
		}
		return await source.ToListAsync();
	}

	public async Task<IdentityResult> ChangePasswordAndResetForcePasswordChangeAsync(VmsUser user, string currentPassword, string newPassword)
	{
		IdentityResult identityResult = await ChangePasswordAsync(user, currentPassword, newPassword);
		if (!identityResult.Succeeded)
		{
			return identityResult;
		}
		user.ForcePasswordChange = false;
		return await UpdateUserAsync(user);
	}

	public virtual async Task<IEnumerable<IdentityUserLogin<string>>> GetLoginsAsync(LoginsResourceParameters resourceParameters)
	{
		IQueryable<IdentityUserLogin<string>> source = _provider.Context.UserLogins;
		if (!string.IsNullOrWhiteSpace(resourceParameters.LoginProviderId))
		{
			string loginProvider = resourceParameters.LoginProviderId.Trim();
			source = source.Where((IdentityUserLogin<string> a) => a.LoginProvider.ToLower() == loginProvider.ToLower());
		}
		if (!string.IsNullOrWhiteSpace(resourceParameters.ProviderKey))
		{
			string providerKey = resourceParameters.ProviderKey.Trim();
			source = source.Where((IdentityUserLogin<string> a) => a.ProviderKey == providerKey);
		}
		return await source.ToListAsync();
	}

	public virtual async Task<VmsUser> UpdateUserClaims(VmsUser user, List<Claim> claims, UserLoginInfo info)
	{
		List<VmsUserClaim> userExternalClaims = user.Claims.Where((VmsUserClaim c) => c.LoginProvider.Equals(info.LoginProvider, StringComparison.InvariantCultureIgnoreCase)).ToList();
		var (added, removed) = ClaimChanges(userExternalClaims, claims);
		if (added.Count == 0 && removed.Count == 0)
		{
			return user;
		}
		IExecutionStrategy strategy = _provider.CreateExecutionStrategy();
		try
		{
			await strategy.ExecuteInTransactionAsync(async delegate
			{
				_provider.Context.UserClaims.RemoveRange(removed);
				foreach (Claim item in added)
				{
					_provider.Context.UserClaims.Add(CreateVmsUserClaim(user, info, item));
				}
				await UpdateUserAsync(user);
			}, () => Task.FromResult(result: false));
		}
		catch (InvalidOperationException)
		{
		}
		return await FindByIdAsync(user.Id);
	}

	public (List<Claim> added, List<VmsUserClaim> removed) ClaimChanges(List<VmsUserClaim> userExternalClaims, List<Claim> newClaims)
	{
		Dictionary<string, VmsUserClaim> dictionary = new Dictionary<string, VmsUserClaim>();
		foreach (VmsUserClaim userExternalClaim in userExternalClaims)
		{
			string key = userExternalClaim.ClaimType + userExternalClaim.ClaimValue;
			if (!dictionary.ContainsKey(key))
			{
				dictionary.Add(key, userExternalClaim);
			}
		}
		List<Claim> list = new List<Claim>();
		HashSet<VmsUserClaim> unChanged = new HashSet<VmsUserClaim>();
		foreach (Claim newClaim in newClaims)
		{
			if (dictionary.ContainsKey(newClaim.Type + newClaim.Value))
			{
				unChanged.Add(dictionary[newClaim.Type + newClaim.Value]);
			}
			else
			{
				list.Add(newClaim);
			}
		}
		List<VmsUserClaim> item = (from c in dictionary
			where !unChanged.Contains(c.Value)
			select c.Value).ToList();
		return (added: list, removed: item);
	}

	public virtual async Task<IdentityResult> CreateUsersAsync(IEnumerable<UserForCreationModel> userModels)
	{
		IdentityResult result = IdentityResult.Success;
		IExecutionStrategy strategy = _provider.CreateExecutionStrategy();
		try
		{
			await strategy.ExecuteInTransactionAsync(async delegate
			{
				foreach (UserForCreationModel userModel in userModels)
				{
					VmsUser vmsUser = VmsUser.Create(userModel.UserName, userModel.Description, userModel.CanChangePassword, userModel.ForcePasswordChange, userModel.LockoutEnd, userModel.Active);
					if (!string.IsNullOrEmpty(userModel.Id))
					{
						vmsUser.Id = userModel.Id;
					}
					result = await CreateUserInternalAsync(vmsUser, userModel.Password, null, null);
					if (!result.Succeeded)
					{
						throw new InvalidOperationException("Abort");
					}
				}
			}, () => Task.FromResult(result: false));
		}
		catch (InvalidOperationException)
		{
		}
		return result;
	}

	public virtual async Task<IdentityResult> CreateUserAsync(VmsUser userEntity, string password, UserLoginInfo userLoginInfo, List<Claim> claims)
	{
		IdentityResult result = null;
		IExecutionStrategy strategy = _provider.CreateExecutionStrategy();
		try
		{
			await strategy.ExecuteInTransactionAsync(async delegate
			{
				result = await CreateUserInternalAsync(userEntity, password, userLoginInfo, claims);
				if (!result.Succeeded)
				{
					throw new InvalidOperationException("Abort");
				}
			}, () => Task.FromResult(result: false));
		}
		catch (InvalidOperationException)
		{
		}
		return result;
	}

	public virtual bool IsUserLockedOut(VmsUser user)
	{
		if (user == null)
		{
			throw new ArgumentNullException("user");
		}
		if (user.LockoutEnabled && user.LockoutEnd.HasValue)
		{
			return user.LockoutEnd.Value.Subtract(_dateTimeHelper.UtcNow) > TimeSpan.Zero;
		}
		return false;
	}

	public virtual async Task<IdentityResult> AddClaimsAsync(VmsUser user, UserLoginInfo loginInfo, IEnumerable<Claim> claims)
	{
		if (user == null)
		{
			throw new ArgumentNullException("user");
		}
		if (claims == null)
		{
			throw new ArgumentNullException("claims");
		}
		foreach (Claim claim in claims)
		{
			_provider.Context.UserClaims.Add(CreateVmsUserClaim(user, loginInfo, claim));
		}
		return await UpdateUserAsync(user);
	}

	public async Task<IdentityResult> CreateUserInternalAsync(VmsUser userEntity, string password, UserLoginInfo userLoginInfo, IReadOnlyCollection<Claim> claims)
	{
		IdentityResult identityResult = ((!string.IsNullOrEmpty(password)) ? (await CreateAsync(userEntity, password)) : (await CreateAsync(userEntity)));
		if (!identityResult.Succeeded)
		{
			return identityResult;
		}
		VmsUser user = await FindByNameAsync(userEntity.UserName);
		if (userLoginInfo != null)
		{
			identityResult = await AddLoginAsync(user, userLoginInfo);
			if (!identityResult.Succeeded)
			{
				return identityResult;
			}
		}
		if (claims != null && claims.Any())
		{
			identityResult = await AddClaimsAsync(user, userLoginInfo, claims);
		}
		return identityResult;
	}

	private static VmsUserClaim CreateVmsUserClaim(VmsUser user, UserLoginInfo loginInfo, Claim claim)
	{
		return new VmsUserClaim
		{
			UserId = user.Id,
			ClaimType = claim.Type,
			ClaimValue = claim.Value,
			LoginProvider = loginInfo?.LoginProvider
		};
	}
}
