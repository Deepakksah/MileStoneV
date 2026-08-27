using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace VideoOS.IdentityServer.Data;

public class DbContextBuilder<TContext>(IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptionsBuilder) where TContext : DbContext
{
	private readonly List<Type> _interceptorTypes = new List<Type>();

	private readonly List<Action> _postActions = new List<Action>();

	public IServiceCollection Services { get; } = services;

	public DbContextBuilder<TContext> AddInterceptor<TInterceptor>() where TInterceptor : class, IInterceptor
	{
		_interceptorTypes.Add(typeof(TInterceptor));
		Services.AddScoped<TInterceptor>();
		return this;
	}

	public void AddPostAction(Action action)
	{
		_postActions.Add(action);
	}

	public IServiceCollection Register()
	{
		Services.AddDbContext<TContext>(delegate(IServiceProvider serviceProvider, DbContextOptionsBuilder optionsBuilder)
		{
			dbContextOptionsBuilder(optionsBuilder);
			IInterceptor[] interceptors = _interceptorTypes.Select((Type t) => (IInterceptor)serviceProvider.GetRequiredService(t)).ToArray();
			optionsBuilder.AddInterceptors(interceptors);
		});
		foreach (Action postAction in _postActions)
		{
			postAction();
		}
		return Services;
	}
}
