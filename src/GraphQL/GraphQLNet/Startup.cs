﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GraphQL.Server;
using GraphQLNet.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using GraphQL.Types;
using GraphQL.Server.Ui.GraphiQL;
using GraphQLNet.Query;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace GraphQLNet
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            Environment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true;
                options.ExposeExceptions = Environment.IsDevelopment();
            })
            .AddGraphQLAuthorization(options =>
            {
                options.AddPolicy("Authorized", p => p.RequireAuthenticatedUser());
                //var policy = new AuthorizationPolicyBuilder()
                //                    .
                //options.AddPolicy("Authorized", p => p.RequireClaim(ClaimTypes.Name, "Tom"));
            });
            //.AddUserContextBuilder(context => new GraphQLUserContext { User = context.User });

            services.AddSingleton<MessageSchema>();
            services.AddSingleton<MessageQuery>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseGraphQL<MessageSchema>("/graphql");
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions()
            {
                Path = "/ui/playground"
            });
            app.UseGraphiQLServer(new GraphiQLOptions
            {
                GraphiQLPath = "/ui/graphiql",
                GraphQLEndPoint = "/graphql"
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
