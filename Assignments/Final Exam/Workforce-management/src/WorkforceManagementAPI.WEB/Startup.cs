using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WebApi.Middleware;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Contracts;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DAL.Repositories;
using WorkforceManagementAPI.DAL.Repositories.IdentityRepositories;
using WorkforceManagementAPI.DTO.Models;
using WorkforceManagementAPI.WEB.AuthorizationPolicies.Handlers;
using WorkforceManagementAPI.WEB.AuthorizationPolicies.Requirements;
using WorkforceManagementAPI.WEB.IdentityAuth;

namespace WorkforceManagementAPI.WEB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [System.Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WorkforceManagementAPI.WEB", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header (NO NEED for the word Bearer). Example: \"Authorization: {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http, //SecuritySchemeType.ApiKey
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {

                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },

                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },

                        new List<string>()
                    }
                });

                c.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date" });
            });

            // Register Automapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //EF
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:Default"]));
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IIdentityUserManager, IdentityUserManager>();
            services.AddTransient<IValidationService, ValidationService>();
            services.AddTransient<ITimeOffService, TimeOffService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<ITimeOffRepository, TimeOffRepository>();

            //Authorization handlers
            services.AddTransient<IAuthorizationHandler, TeamLeaderHandler>();
            services.AddTransient<IAuthorizationHandler, TeamMemberHandler>();
            services.AddTransient<IAuthorizationHandler, TimeOffCreatorHandler>();
            services.AddTransient<IAuthorizationHandler, AdminRoleHandler>();
            services.AddTransient<IAuthorizationHandler, AdminCantDeleteHimselfHandler>();

            //EF Identity
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })

            //Injecting the services and DB in the DI containter
                   .AddRoles<IdentityRole>()
                   .AddEntityFrameworkStores<DatabaseContext>();

            var builder = services.AddIdentityServer((options) =>
            {
                options.EmitStaticAudienceClaim = true;
            })

                //This is for dev only scenarios when you don’t have a certificate to use.
                .AddInMemoryApiScopes(IdentityConfig.ApiScopes)
                .AddInMemoryClients(IdentityConfig.Clients)
                .AddDeveloperSigningCredential()
                .AddResourceOwnerValidator<PasswordValidator>();

            // omitted for brevity
            // Authentication
            // Adds the asp.net auth services
            services
                .AddAuthorization(options =>
            {

                options.AddPolicy("Admin", policy =>
                policy.RequireRole("Admin"));

                options.AddPolicy("User", policy =>
                policy.RequireRole("User"));

                options.AddPolicy("TimeOffCreatorOrAdmin", policy =>
                policy.Requirements.Add(new TimeOffCreatorOrAdminRequirement()));

                options.AddPolicy("TeamMember", policy =>
                policy.Requirements.Add(new TeamMemberRequirement()));

                options.AddPolicy("TeamLeader", policy =>
                policy.Requirements.Add(new TeamLeaderRequirement()));

                options.AddPolicy("AdminCantDeleteHimself", policy =>
                policy.Requirements.Add(new AdminCantDeleteHimselfRequirement()));

            })
            
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                // Adds the JWT bearer token services that will authenticate each request based on the token in the Authorize header
                // and configures them to validate the token with the options

                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.Audience = "https://localhost:5001/resources";
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            DatabaseSeeder.Seed(app.ApplicationServices);

            app.UseIdentityServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WorkforceManagementAPI.WEB v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
