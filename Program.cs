using Microsoft.EntityFrameworkCore;
using FacilaIT.Models;
using FacilaIT.Helper.Shared;
using FacilaIT.Helper.Rabc;

//
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Text;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using Newtonsoft.Json;
// using Microsoft.Extensions.DependencyInjection;
//




var builder = WebApplication.CreateBuilder(args);


// Get databae online
bool dbOnline = Boolean.Parse(builder.Configuration["ReadOnlineDB"]);
string DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
if (dbOnline)
{
    var wc = new System.Net.WebClient();
    wc.DownloadFile(builder.Configuration.GetConnectionString("OnlineDB"), @"onlineDB.db");
    DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnectionOnline");
}

//

// Add services to the container.
builder.Services.AddSignalR();
//
// builder.Services.AddDbContext<DBBContext>(opt =>
//     opt.UseInMemoryDatabase("DBList")) // using local memory
builder.Services.AddDbContext<DBBContext>(options =>
        options.UseNpgsql((DefaultConnection)));

// For Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DBBContext>()
    .AddDefaultTokenProviders();
// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Swagger generation options
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

// Add Memory Cache
builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowAnyOrigin();
                    //.SetIsOriginAllowed((host) => true)
                    //.AllowCredentials();
                });
        });

// logger
builder.Services.AddLogging(loggingBuilder =>
{
    var loggingSection = builder.Configuration.GetSection("Logging");
    string? logFilePath = builder.Configuration.GetValue<string>("Logging:File:Path");
    string? MinLevel = builder.Configuration.GetValue<string>("Logging:File:MinLevel");

    loggingBuilder.AddFile(logFilePath, fileLoggerOpts =>
    {
        fileLoggerOpts.FormatLogFileName = fName =>
        {
            return String.Format(fName, DateTime.UtcNow);
        };
        fileLoggerOpts.MinLevel = Common.LogMinLevel(MinLevel);
    });
    // loggingBuilder.AddFile(loggingSection);
});

// // RABC
// builder.Services.AddScoped<RbacService>(); // Register the RbacService with Scoped lifetime



builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// Cors
app.UseCors("AllowAllOrigins");

// Custom wwww
app.UseDefaultFiles();
app.UseStaticFiles();

//app.MapHub<ChartDataHub>("/chartHub");



// Get an instance of IMemoryCache
var cache = app.Services.GetRequiredService<IMemoryCache>();


// Get setting config locally
// fill cache 
Startup startupSettings = new Startup(cache);
startupSettings.ReadSettings();
// End


// Apply migration
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DBBContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}
//

app.Run();
