using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OneBlog.Application.Modules.Client;
using OneBlog.Domain.Entities;
using OneBlog.Persistence.DataContext;
using Riode.Domain.Models.DataContext;
using System;
using System.Linq;

namespace OneBlog.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(cfg =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

                cfg.Filters.Add(new AuthorizeFilter(policy));
            });
            /*  services.AddDbContext<OneBlogDbContext>(cfg =>
              {
                  cfg.UseSqlServer(Configuration.GetConnectionString("cString"));
              });*/
            services.AddDbContext<OneBlogDbContext>(options =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                string connStr;
                if (env == "Development")
                {
                    connStr = Configuration.GetConnectionString("cString");
                    options.UseSqlServer(connStr);
                }
                else
                {
                    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                    connUrl = connUrl.Replace("postgres://", string.Empty);
                    var pgUserPass = connUrl.Split("@")[0];
                    var pgHostPortDb = connUrl.Split("@")[1];
                    var pgHostPort = pgHostPortDb.Split("/")[0];
                    var pgDb = pgHostPortDb.Split("/")[1];
                    var pgUser = pgUserPass.Split(":")[0];
                    var pgPass = pgUserPass.Split(":")[1];
                    var pgHost = pgHostPort.Split(":")[0];
                    var pgPort = pgHostPort.Split(":")[1];
                    connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}; SSL Mode=Require; Trust Server Certificate=true";
                    options.UseNpgsql(connStr);
                }
               
            });
            var asmbls = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("OneBlog")).ToArray();
            services.AddMediatR(asmbls);
            services.AddMediatR(typeof(BlogListCommand));
            services.AddMediatR(typeof(Blog));

            //Identity
            services.AddIdentity<OneBlogUser, OneBlogRole>()
              .AddEntityFrameworkStores<OneBlogDbContext>();

            services.AddScoped<UserManager<OneBlogUser>>();
            services.AddScoped<SignInManager<OneBlogUser>>();
            services.AddScoped<RoleManager<OneBlogRole>>();

            /*  services.AddScoped<IClaimsTransformation, AppClaimProvider>();*/

            services.Configure<IdentityOptions>(cfg =>
            {
                cfg.Password.RequireDigit = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequiredUniqueChars = 1;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredLength = 3;

                cfg.User.RequireUniqueEmail = true;
                cfg.Lockout.MaxFailedAccessAttempts = 3;
                cfg.Lockout.DefaultLockoutTimeSpan = new System.TimeSpan(0, 3, 0);

            });
            services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/signin.html";
                cfg.AccessDeniedPath = "/accessdenied.html";

                cfg.ExpireTimeSpan = new TimeSpan(0, 5, 0);
                cfg.Cookie.Name = "OneBlog";
            });
            services.AddAuthentication();
            services.AddAuthorization();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            /*  services.AddAuthorization(cfg =>
              {
                  foreach (var item in Extension.principlies)
                  {
                      cfg.AddPolicy(item, p =>
                      {
                          p.RequireAssertion(assertion =>
                          {
                              return
                              assertion.User.IsInRole("SuperAdmin") ||
                              assertion.User.HasClaim(c => c.Type.Equals(item) && c.Value.Equals("1"));

                          });
                      });
                  }
              });*/
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.SeedMembership();
            app.UseStaticFiles();

            app.UseRouting();
            app.Use(async (context, next) =>
            {
                if (!context.Request.Cookies.ContainsKey("OneBlog") && context.Request.Path.Equals("/admin")
                )
                {
                    context.Response.Redirect("/admin/signin.html");
                    await context.Response.CompleteAsync();
                }
                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                   );

                endpoints.MapControllerRoute("admin-signIn", "admin/signin.html",
              defaults: new
              {
                  controller = "Account",
                  area = "Admin",
                  action = "Login"
              }
              );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
