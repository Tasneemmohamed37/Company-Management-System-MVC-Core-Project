using Company.BLL.Interfaces;
using Company.BLL;
using Company.DAL.Context;
using Company.DAL.Models;
using Company.PL.Settings;
using Company.PL.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Google;
using Company.PL.Mappers;

namespace Company.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Confige services [add services to container]

            builder.Services.AddControllersWithViews();

            #region Dbcontext
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("defualtConnnection"));
            });
			#endregion

			#region DI
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // all Bl scoped

			#region Auto Mapper
			builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
			builder.Services.AddAutoMapper(M => M.AddProfile(new UserProfile()));
			builder.Services.AddAutoMapper(M => M.AddProfile(new RoleProfile()));

            #endregion


            #region Authentication Scheme config
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "Account/Login";
                    options.AccessDeniedPath = "Home/Error";
                });

            #endregion

            #region to configure generic interface which include crateAsynce  
                builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireNonAlphanumeric = true; // @#$%
                    options.Password.RequireUppercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;
                })
                   .AddEntityFrameworkStores<CompanyDbContext>()
                   .AddDefaultTokenProviders();
            #endregion


            #region mail
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddTransient<IEmailSettings, EmailSettings>();

            #endregion

            #region SMS

            builder.Services.Configure<TwillioSettings>(builder.Configuration.GetSection("Twillio"));
            builder.Services.AddTransient<ISmsService, SmsService>();

            #endregion

            #region Google External login
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddGoogle(o =>
            {
                IConfiguration GoogleAuthSection = builder.Configuration.GetSection("Authentcation:Google");
                o.ClientId = GoogleAuthSection["ClientId"];
                o.ClientSecret = GoogleAuthSection["ClientSecret"];
            });
            #endregion


            #endregion

            #endregion


            var app = builder.Build();

            #region Configure the HTTP request pipeline.

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            #endregion


            app.Run();
        }
    }
}