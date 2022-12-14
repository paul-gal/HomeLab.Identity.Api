// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Models;
using IdentityServer;
using Serilog;

namespace HomeLab.Identity.Api;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var scopes = new List<ApiScope>();
        var clients = new List<InMemoryClient>();
        builder.Configuration.AddEnvironmentVariables("homelab_");

        builder.Configuration.Bind("ApiScopes", scopes);
        builder.Configuration.Bind("Clients", clients);

        clients.ForEach(x =>
        {
            x.ClientSecrets = new List<Secret>(){
                new Secret(x.ClientSecret.Sha256(), "App Secret")
                    };
            x.ClientSecret = null;
            x.AccessTokenLifetime = (int)TimeSpan.FromHours(x.AccessTokenLifetimeInHours).TotalSeconds;
            x.AllowedGrantTypes = GrantTypes.ClientCredentials;
            x.Enabled = true;
        });

        builder.Services.AddIdentityServer()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(scopes)
            .AddInMemoryClients(clients);

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {

        app.UseSerilogRequestLogging();
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseIdentityServer();


        return app;
    }

    internal class InMemoryClient : Client
    {
        public string ClientSecret { get; set; }
        public int AccessTokenLifetimeInHours { get; set; }
    }
}