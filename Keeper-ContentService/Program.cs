using Keeper_ContentService.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Keeper_ContentService.Middlewares;
using Keeper_ContentService.Repositories.ArticleStatusRepository.Interfaces;
using Keeper_ContentService.Repositories.ArticleStatusRepository.Implementations;
using Keeper_ContentService.Repositories.CategoryRepository.Interfaces;
using Keeper_ContentService.Repositories.CategoryRepository.Implementations;
using Keeper_ContentService.Repositories.CommentRepository.Interfaces;
using Keeper_ContentService.Repositories.CommentRepository.Implementations;
using Keeper_ContentService.Repositories.UserArticleActionRepository.Implementations;
using Keeper_ContentService.Repositories.ArticleRepository.Interfaces;
using Keeper_ContentService.Repositories.ArticleRepository.Implementations;
using Keeper_ContentService.Repositories.UserArticleActionRepository.Interfaces;
using Keeper_ContentService.Services.ArticleService.Implementations;
using Keeper_ContentService.Services.ArticleStatusService.Interfaces;
using Keeper_ContentService.Services.ArticleStatusService.Implementations;
using Keeper_ContentService.Services.CategoryService.Interfaces;
using Keeper_ContentService.Services.CategoryService.Implementations;
using Keeper_ContentService.Services.CommentService.Interfaces;
using Keeper_ContentService.Services.CommentService.Implementations;
using Keeper_ContentService.Services.DTOMapperService.Interfaces;
using Keeper_ContentService.Services.DTOMapperService.Implementations;
using Keeper_ContentService.Services.UserArticleActionService.Implementations;
using Keeper_ContentService.Services.ArticleService.Interfaces;
using Keeper_ContentService.Services.UserArticleActionService.Interfaces;
using Keeper_ContentService.Services.HttpClientService.Interfaces;
using Keeper_ContentService.Services.HttpClientService.Implementations;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Services.ProfileService.Interfaces;
using Keeper_ContentService.Services.ProfileService.Implementations;
using DotNetEnv;

namespace Keeper_ContentService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            if (builder.Environment.IsDevelopment())
                builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            else
                Env.Load();

            builder.Configuration.AddEnvironmentVariables();

            // HttpClient
            builder.Services.AddHttpClient<IHttpClientService, HttpClientService>();

            // Configuration
            builder.Services.Configure<ServiceUrlsDTO>(builder.Configuration.GetSection("ServicUrls"));

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
                            builder.Configuration.GetSection("JwtSettings:IssuerSigningKey").Value!
                            )),
                        ValidAlgorithms = new string[] { SecurityAlgorithms.HmacSha256 },
                    };
                });

            // Db

            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection));

            // Repositories

            builder.Services.AddScoped<IArticlesRepository, ArticlesRepository>();
            builder.Services.AddScoped<IArticleStatusesRepository, ArticleStatusRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
            builder.Services.AddScoped<ILikedArticlesRepository, LikedArticlesRepository>();
            builder.Services.AddScoped<ISavedArticlesRepository, SavedArticlesRepository>();

            // Services

            builder.Services.AddScoped<IDTOMapperService, DTOMapperService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<IArticlesStatusesService, ArticleStatusService>();
            builder.Services.AddScoped<IArticleStatusNamedProvider, ArticleStatusService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ILikedArticleService, LikedArticleService>();
            builder.Services.AddScoped<ISavedArticleService, SavedArticleService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();

            // Strategies

            builder.Services.AddScoped<IStatusChangeStrategy, PublishStrategy>();
            builder.Services.AddScoped<IStatusChangeStrategy, DraftStrategy>();
            builder.Services.AddScoped<IStatusChangeStrategy, ReviewStrategy>();
            builder.Services.AddScoped<IStatusChangeStrategy, ReadyToPublishStrategy>();

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
