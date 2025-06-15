using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NeuroTumAI.APIs.Errors;
using NeuroTumAI.APIs.Middlewares;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Authorization;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Repository;
using NeuroTumAI.Repository.Data;
using NeuroTumAI.Service.Jobs;
using NeuroTumAI.Service.Mappings;
using NeuroTumAI.Service.Providers.Identity;
using NeuroTumAI.Service.Services.AccountService;
using NeuroTumAI.Service.Services.AdminService;
using NeuroTumAI.Service.Services.AppointmentService;
using NeuroTumAI.Service.Services.AuthService;
using NeuroTumAI.Service.Services.BlobStorageService;
using NeuroTumAI.Service.Services.CancerDetectionService;
using NeuroTumAI.Service.Services.ChatService;
using NeuroTumAI.Service.Services.ClinicService;
using NeuroTumAI.Service.Services.ContactUsService;
using NeuroTumAI.Service.Services.DashboardService;
using NeuroTumAI.Service.Services.DoctorService;
using NeuroTumAI.Service.Services.EmailService;
using NeuroTumAI.Service.Services.FireBaseNotificationService;
using NeuroTumAI.Service.Services.LocalizationService;
using NeuroTumAI.Service.Services.MriScanService;
using NeuroTumAI.Service.Services.NotificationService;
using NeuroTumAI.Service.Services.PostService;
using NeuroTumAI.Service.Services.ReviewService;
using NeuroTumAI.Service.Services.UserDeviceTokenService;
using Quartz;
using System.Globalization;
using System.Text;

namespace NeuroTumAI.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{

			services.AddAutoMapper(typeof(MappingProfile));
			services.AddSignalR();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<ILocalizationService, LocalizationService>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IPostService, PostService>();
			services.AddScoped<IBlobStorageService, BlobStorageService>();
			services.AddScoped<IClinicService, ClinicService>();
			services.AddScoped<IAppointmentService, AppointmentService>();
			services.AddScoped<IReviewService, ReviewService>();
			services.AddScoped<IDoctorService, DoctorService>();
			services.AddScoped<IChatService, ChatService>();
			services.AddScoped<IAdminService, AdminService>();
			services.AddScoped<IDashboardService, DashboardService>();
			services.AddScoped<IContactUsService, ContactUsService>();
			services.AddScoped<ICancerDetectionService, CancerDetectionService>();
			services.AddScoped<IMriScanService, MriScanService>();
			services.AddScoped<IFireBaseNotificationService, FireBaseNotificationService>();
			services.AddScoped<INotificationService, NotificationService>();
			services.AddScoped<IUserDeviceTokenService, UserDeviceTokenService>();
			services.AddHttpClient<ICancerDetectionService, CancerDetectionService>();
			services.AddTransient<IEmailService, EmailService>();
			services.AddScoped<ExceptionMiddleware>();

			services.AddQuartz(q =>
			{
				q.UseMicrosoftDependencyInjectionJobFactory();

				var jobKey = new JobKey("MriReviewCleanupJob");

				q.AddJob<MriReviewCleanupJob>(opts => opts.WithIdentity(jobKey));
				q.AddTrigger(opts => opts
					.ForJob(jobKey)
					.WithIdentity("MriReviewCleanupTrigger")
					.WithCronSchedule("0 0,30 * * * ?"));
			});

			services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);


			services.AddLocalization();

			services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new[]
				{
					new CultureInfo("en"),
					new CultureInfo("ar")
				};

				options.DefaultRequestCulture = new RequestCulture("en");
				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;
				options.ApplyCurrentCultureToResponseHeaders = true;
			});


			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{

					var errors = actionContext.ModelState
												   .Where(P => P.Value.Errors.Count > 0)
												   .SelectMany(P => P.Value.Errors)
												   .Select(E => E.ErrorMessage)
												   .ToList();

					var response = new ApiValidationErrorResponse() { Errors = errors };

					return new UnprocessableEntityObjectResult(response);
				};
			});

			return services;
		}

		public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddIdentity<ApplicationUser, IdentityRole>()
					.AddEntityFrameworkStores<StoreContext>()
					.AddDefaultTokenProviders()
					.AddTokenProvider<CustomEmailTokenProvider<ApplicationUser>>("EmailConfirmation");


			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidIssuer = configuration["JWT:ValidIssuer"],
					ValidateAudience = true,
					ValidAudience = configuration["JWT:ValidAudience"],
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"] ?? string.Empty)),
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};

				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var accessToken = context.Request.Query["access_token"];

						// If the request is for the hub...
						var path = context.HttpContext.Request.Path;
						if (!string.IsNullOrEmpty(accessToken) &&
							(path.StartsWithSegments("/posthub") || path.StartsWithSegments("/chatHub")))
						{
							// Read the token from the query string
							context.Token = accessToken;
						}
						return Task.CompletedTask;
					}
				};
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("ActiveUserOnly", policy =>
					policy.Requirements.Add(new ActiveUserRequirement()));
			});

			services.AddScoped<IAuthorizationHandler, ActiveUserHandler>();
			//services.AddScoped(typeof(IAuthService), typeof(AuthService));

			return services;

		}
	}
}
