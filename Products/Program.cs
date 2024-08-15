using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Onyx.Product.Infrastructure.Repositories;
using Onyx.Product.Infrastructure.Repositories.Interfaces;
using Onyx.ProductsApi;
using Onyx.ProductsApi.Profiles;
using Onyx.ProductsService;
using ProductsApi.Infrastructure.Data;
using System.Text;


public partial class Program
{

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        builder.Services.AddControllers();

        var jwtSettings = builder.Configuration.GetSection("Jwt");

        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);  //Encoding.UTF8.GetBytes("OnyxJwtSecretKey-eff03fda-5685-412f-8395-ba91aebea873");

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 ValidIssuer = "OnyxProductIssuer",
                 ValidAudience = "OnyxProductAudience",
                 IssuerSigningKey = new SymmetricSecurityKey(key)
             };
             options.Events = new JwtBearerEvents
             {
                 OnAuthenticationFailed = context =>
                 {
                     Console.WriteLine("Token failed validation: " + context.Exception.Message);
                     return Task.CompletedTask;
                 },
                 OnChallenge = context =>
                 {
                     Console.WriteLine("Token validation challenge: " + context.ErrorDescription);
                     return Task.CompletedTask;
                 }
             };
         });


        builder.Services.AddScoped<IProductService, ProductService>();
        
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddDbContext<ProductsDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAutoMapper(config =>
        {
            config.AddProfile<ProductProfile>();
        });

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        builder.Services.AddSwaggerGen();
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.MapControllers();
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        app.UseCors("AllowAllOrigins");
        app.Run();
    }
}

public partial class Program { }