using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RefreshJWTToken.Data;
using RefreshJWTToken.Repository.JWT;
using RefreshJWTToken.Repository.UserService;
using System;
using System.Text;
using System.Threading.Tasks;

namespace RefreshJWTToken
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

			services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<IdentityUser, IdentityRole>(options => {
				options.Password.RequireUppercase = true; // on production add more secured options
				options.Password.RequireDigit = true;
				//options.SignIn.RequireConfirmedEmail = true;
				options.Password.RequireLowercase = true;
				options.Password.RequiredLength = 8;
				options.User.RequireUniqueEmail = true;
				
			}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

			services.AddAuthentication(x => {
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(o => {
				var Key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
				o.SaveToken = true;
				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false, // on production make it true
					ValidateAudience = false, // on production make it true
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = Configuration["JWT:Issuer"],
					ValidAudience = Configuration["JWT:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Key),
					ClockSkew = TimeSpan.Zero
				};
				o.Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = context => {
						if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
						{
							context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
						}
						return Task.CompletedTask;
					}
				};
			});
			services.AddControllers();
			services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", policy =>
				{
					policy.AllowAnyHeader();
					policy.AllowAnyMethod();
					policy.AllowAnyOrigin();
				});
			});
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "BasicJWTAuth", Version = "v1" });
			});
			services.AddSingleton<IJWTManagerRepository, JWTManagerRepository>();
			services.AddScoped<IUserServiceRepository, UserServiceRepository>();
			
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RefreshJWTToken v1"));
            }
			
			app.UseHttpsRedirection();

			app.UseRouting();
			app.UseCors("AllowAll");

			app.UseAuthentication(); // This need to be added before UseAuthorization()	
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
    }
}
