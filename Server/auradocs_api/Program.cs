using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using auradocs_api.Contexts;
using auradocs_api.Middleware;
using auradocs_api.Services;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using auradocs_api;
using DinkToPdf.Contracts;
using DinkToPdf;
using auradocs_api.Data;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
//register DBcontext
builder.Services.AddDbContext<AuradocsContext>(options =>{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//JWT service registration
var jwtConfig = builder.Configuration.GetSection("JwtSettings");
var Key = Encoding.UTF8.GetBytes(jwtConfig["key"]);
builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options=>{
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuer  = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer  = jwtConfig["Issuer"],
        ValidAudience  = jwtConfig["Audience"],
        IssuerSigningKey  = new SymmetricSecurityKey(Key)
    };
});

builder.Services.AddAuthorization();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<JWTService>();
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);
builder.Services.Configure<LLMSettings>(
    builder.Configuration.GetSection("LLMSettings")
);

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<IConverter>(
    new SynchronizedConverter(new PdfTools())
);
builder.Services.AddScoped<IConvertFileService, ConvertHtmlToPdf>();
builder.Services.AddScoped<IAIService,AIService>();
builder.Services.AddScoped<ILLMClients, OpenAiClient>();
builder.Services.AddScoped<UserInformationService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Auradocs Api",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});



string[] allowedOrigins = builder.Configuration
    .GetSection("AllowedOrigins")
    .Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // only if using cookies
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("FrontendPolicy"); 

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<JwtRefreshMiddleware>();
app.MapControllers();

app.Run();
