using Keeper_ContentService.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Repositories.Implementations;
using Keeper_ContentService.Services.Interfaces;
using Keeper_ContentService.Services.Implementations;
using Keeper_ContentService.Middlewares;

namespace Keeper_ContentService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Auth
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration.GetSection("JwtSettings:ValidIssuer").Value,
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration.GetSection("JwtSettings:ValidAudience").Value,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                            builder.Configuration.GetSection("JwtSettings:IssuerSigningKey").Value
                            )),
                        ValidAlgorithms = new string[] { SecurityAlgorithms.HmacSha256 },
                    };
                });

            // Db

            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection));

            // Repositories

            builder.Services.AddScoped<IArticlesRepository, ArticlesRepository>();
            builder.Services.AddScoped<IArticleStatusesRepository, ArticleStatusesRepository>();
            builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
            builder.Services.AddScoped<ILikedArticlesRepository, LikedArticlesRepository>();
            builder.Services.AddScoped<ISavedArticlesRepository, SavedArticlesRepository>();

            // Services

            builder.Services.AddScoped<IDTOMapperService, DTOMapperService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<IArticlesStatusesService, ArticleStatusesService>();
            builder.Services.AddScoped<ICategoriesService, CategoriesService>();
            builder.Services.AddScoped<ICommentsService, CommentsService>();
            builder.Services.AddScoped<ILikedArticlesService, LikedArticlesService>();
            builder.Services.AddScoped<ISavedArticlesService, SavedArticlesService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Middlewares
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
