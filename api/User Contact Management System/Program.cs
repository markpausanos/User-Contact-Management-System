using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using User_Contact_Management_System.Configurations;
using User_Contact_Management_System.Data;
using User_Contact_Management_System.Repositories.Users;
using User_Contact_Management_System.Services.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<APIDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("APIDbContext")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
ConfigureServices(builder.Services, builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
    services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<JwtConfig>>().Value);
    var jwtSettings = configuration.GetSection("JwtConfig").Get<JwtConfig>();
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key!))
            };
        });

    services.AddTransient<APIDbContext>();

    services.AddScoped<IUserService, UserService>();

    services.AddScoped<IUserRepository, UserRepository>();
}