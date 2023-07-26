using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FoolProof.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

using User_Contact_Management_System.Configurations;
using User_Contact_Management_System.Data;
using User_Contact_Management_System.Models;
using User_Contact_Management_System.Repositories.Contacts;
using User_Contact_Management_System.Repositories.RefreshTokens;
using User_Contact_Management_System.Repositories.Users;
using User_Contact_Management_System.Services.Contacts;
using User_Contact_Management_System.Services.Users;
using User_Contact_Management_System.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

await ConfigureServices(builder.Services, builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();

async Task ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    var keyVaultEndpoint = builder.Configuration.GetSection("KeyVault:URL").Value!;
    
    var secretClient = new SecretClient(new Uri(keyVaultEndpoint!), new DefaultAzureCredential());

    KeyVaultSecret jwtSecret = await secretClient.GetSecretAsync("jwt-secret");
    KeyVaultSecret dbSecret = await secretClient.GetSecretAsync("azure-sqldb-connection");

    string? dbSecretValue = dbSecret.Value ?? configuration.GetConnectionString("APIDbContext");
    services.AddDbContext<APIDbContext>(options =>
        options.UseSqlServer(dbSecretValue));

    services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<APIDbContext>()
        .AddDefaultTokenProviders();

    services.AddFoolProof();

    services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement() 
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
    });

    services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
    var jwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfig>();
    jwtConfig.Secret = jwtSecret.Value ?? jwtConfig.Secret;
    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = false,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret!))
    };

    services.AddSingleton(jwtConfig);
    services.AddSingleton(tokenValidationParameters);

    services.AddCors(options => options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://linkup-ucms.azurewebsites.net")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    }));

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = tokenValidationParameters;
    });


    services.AddTransient<APIDbContext>();
    services.AddTransient<UserUtils>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IContactService, ContactService>();
    services.AddScoped<IContactRepository, ContactRepository>();
    services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
}
