using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;


using lms.Data;
using lms.Data.Services;
using lms.Data.Repositories;
//using lms.Data.Repositories.Abstract;
using lms.Models;
using lms.Data.Core;
using Microsoft.AspNetCore.DataProtection;
//using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;

namespace lms
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddControllersWithViews();

            services.AddHttpContextAccessor();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // JWT Authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Key);

            services.AddAuthentication(au => {
                au.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                au.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt => {
                jwt.RequireHttpsMetadata = false;
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


            // Repositories
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICourseOutlineRepository, CourseOutlineRepository>();
            services.AddScoped<ICourseOutcomeRepository, CourseOutcomeRepository>();
            services.AddScoped<ICourseAssessmentRepository, CourseAssessmentRepository>();
            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<ICompetenciesRepository, CompetenciesRepository>();
            services.AddScoped<ICourseEvaluationRepository, CourseEvaluationRepository>();
            services.AddScoped<ICourseSessionRepository, CourseSessionRepository>();
            services.AddScoped<ISessionTypeRepository, SessionTypeRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<IUserGroupRepository, UserGroupRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<ITagsRepository, TagsRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ILearnerRepository, LearnerRepository>();
            services.AddScoped<IAppraisalRepository, AppraisalRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<ILevelRepository, LevelRepository>();
            services.AddScoped<ITypesRepository, TypesRepository>();

            // Services
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IValidationService, ValidationService>();


            //services.AddDataProtection();

            //services.AddDataProtection().UseCryptographicAlgorithms(new AuthenticatedEncryptionSettings()
            //{
            //    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM,
            //    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            //});


            services.AddSingleton<IFileDirectory>(Configuration.GetSection("FileRepository").Get<FileDirectory>());

            //services.AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<lmsContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("lmsContext")));

            services.AddCors();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(
               options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            );

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
