﻿using Autofac;
using Autofac.Configuration;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Web.Http;
using System.Web.Mvc;
using Thinktecture.AuthorizationServer.EF;
using Thinktecture.AuthorizationServer.Interfaces;
using Thinktecture.AuthorizationServer.OAuth2;

namespace Thinktecture.AuthorizationServer.WebHost
{
    public static class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DummyTokenHandleManager>().As<ITokenHandleManager>();
            
            //builder.RegisterType<DummyAuthorizationServerConfiguration>().As<IAuthorizationServerConfiguration>();
            builder.RegisterType<EFAuthorizationServerConfiguration>().As<IAuthorizationServerConfiguration>();
            builder.RegisterType<AuthorizationServerContext>();
            
            builder.RegisterModule(new ConfigurationSettingsReader("autofac"));

            builder.RegisterControllers(typeof(AuthorizeController).Assembly);
            builder.RegisterControllers(typeof(AutofacConfig).Assembly);
            builder.RegisterApiControllers(typeof(TokenController).Assembly);

            var container = builder.Build(); 
            
            // MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Web API
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}