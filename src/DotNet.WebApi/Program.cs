using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using DotNet.ApplicationCore;
using DotNet.Infrastructure;
using System.Text;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using DotNet.Services;
using DotNet.ApplicationCore.Middleware;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
//private readonly IConfiguration _configuration;
// Add services to the container.

builder.Services.AddApplicationCore();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
//builder.Services.AddControllers().AddJsonOptions(x =>
//                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
//    (options =>
//{
//    options.AddDefaultPolicy(
//                      policy =>
//                      {
//                          policy.WithOrigins("http://localhost:4200",
//                                              "http://localhost:4200");
//                      });
//});
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "You api title", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
    // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // c.IncludeXmlComments(xmlPath);
});
builder.Services.AddAuthorization();
//builder.Configuration.AddJsonFile("errorcodes.json", false, true);
//builder.Services.AddOptions<AppSettingsJson>().Bind(configuration)              
//               .ValidateDataAnnotations();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(opt =>
//{
//    opt.TokenValidationParameters = new()
//    {
//cc
//    };
//})




    .AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false
        //ValidateIssuer = true,
        //ValidateAudience = true,
        //ValidateLifetime = true,
        //ValidateIssuerSigningKey = true,
        //ValidIssuer = builder.Configuration["Jwt:Issuer"],
        //ValidAudience = builder.Configuration["Jwt:Issuer"],
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
await using var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

//app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseCors(c =>
     c.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

//app.Run(async context =>
//{

//   // else if (context.Request.Path.Value.Contains(Configuration[Utiltiy.StaticData.Document.FOLDER_ACCESS_FROM_FRONT_END]))
//    //{
//        //var directory = env.ContentRootPath;
//        //var path = context.Request.Path.Value;
//        //if (path.Contains(Utiltiy.StaticData.Extensions.Htm))
//        //{
//          //  context.Response.ContentType = Utiltiy.StaticData.MimeType.Application_Octet_Stream;
//        //}
//      //  await context.Response.SendFileAsync(directory + path);
//    //}
//    //else 
//     if (context.Request.Path.Value == "/")
//    {
//        string ver = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
//        //await context.Response.WriteAsync($"{{status :'API is Running.'}} Version: {ver} Environment: {env.EnvironmentName}");
//        await context.Response.WriteAsync($"{{status :'API is Running.'}} Version: {ver} Environment: Prod");
//    }
//    else
//    {
//        await context.Response.WriteAsync("{ status :'404 Method Not Found'}");
//    }
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}
else
{
    app.UseSwagger();
    // app.UseSwaggerUI("/swagger/index.html");

    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/index.html", "My Cool API V1");
    //   // c.RoutePrefix = "mycoolapi/swagger";
    //});
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseAuthorization();

app.AddGlobalErrorHandler();

app.MapControllers();

app.Run();
