using Amazon;
using Amazon.S3;
using Anyhandy.Common;
using Anyhandy.Interface;
using Anyhandy.Interface.Packages;
using Anyhandy.Interface.User;
using Anyhandy.Services.Users;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
AppConfigrationManager.AppSettings = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var accessKeyId = AppConfigrationManager.AppSettings["AWS:AccessKeyId"];
var secretAccessKey = AppConfigrationManager.AppSettings["AWS:SecretAccessKey"];

builder.Services
    .AddSingleton<IAmazonS3>(p => {
        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USEast1,
        };
        return new AmazonS3Client(accessKeyId, secretAccessKey, config);
    });



builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IPackage, PackagesService>();
builder.Services.AddScoped<IJobPost, JobPostService>();
builder.Services.AddScoped<ICountry, CountryService>();
builder.Services.AddScoped<IFileStorage, S3FileStorageService>();
//S3FileStorageService : IFileStorage
builder.Services.AddAuthentication("Bearer")
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = AppConfigrationManager.AppSettings["Jwt:Issuer"],
           ValidAudience = AppConfigrationManager.AppSettings["Jwt:Issuer"],
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfigrationManager.AppSettings["Jwt:Key"]))
       };
   });

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowAnyOrigin");
app.Run();
