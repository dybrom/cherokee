using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Cherokee.DAL.Repositories;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Cherokee.OAuth.Startup))]

namespace Cherokee.OAuth
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var unitOfWork = new UnitOfWork();
            var inMemo = new InMemoryManager();
            var factory = new IdentityServerServiceFactory();

            var entityFrameworkOptions = new EntityFrameworkServiceOptions
            {
                ConnectionString = ConfigurationManager
                .ConnectionStrings["TimeIdentity"].ConnectionString
            };

            var certificate = Convert.FromBase64String(ConfigurationManager.AppSettings["SigningCertificate"]);
            //var factory = new IdentityServerServiceFactory().UseInMemoryClients(inMemo.GetClients())
            //                                                .UseInMemoryScopes(inMemo.GetScopes())
            //                                                .UseInMemoryUsers(inMemo.GetUsers());
            
            factory.RegisterConfigurationServices(entityFrameworkOptions);
            factory.RegisterOperationalServices(entityFrameworkOptions);
            factory.UserService = new Registration<IUserService>(typeof(TimeUserService));
            factory.Register(new Registration<UnitOfWork>());
            factory.Register(new Registration<UnitOfWork>(unitOfWork));




            SetupClients(inMemo.GetClients(), entityFrameworkOptions);
            SetupScopes(inMemo.GetScopes(), entityFrameworkOptions);

            var options = new IdentityServerOptions
            {
                SigningCertificate = new X509Certificate2(certificate, "password"),
                RequireSsl = false,
                Factory = factory
            };
            app.UseIdentityServer(options);

          
        }

        public void SetupClients(IEnumerable<Client> clients, EntityFrameworkServiceOptions options)
        {
            using (var context = new ClientConfigurationDbContext
               (options.ConnectionString, options.Schema))
            {
                if (context.Clients.Any()) return;
                foreach (var client in clients) context.Clients.Add(client.ToEntity());
                context.SaveChanges();
            }
        }
        public void SetupScopes(IEnumerable<Scope> scopes, EntityFrameworkServiceOptions options)
        {
            using (var context = new ScopeConfigurationDbContext
               (options.ConnectionString, options.Schema))
            {
                if (context.Scopes.Any()) return;
                foreach (var scope in scopes) context.Scopes.Add(scope.ToEntity());
                context.SaveChanges();
            }
        }

    }
}
