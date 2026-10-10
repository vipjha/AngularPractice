using API.Data;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer(); // Replace AddOpenApi with AddEndpointsApiExplorer
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register your DbContext with SQL Server
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<JWTService>();

//Identity
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric =false;

    options.SignIn.RequireConfirmedEmail = true;
})
    .AddRoles<IdentityRole>() // Add role support
    .AddRoleManager<RoleManager<IdentityRole>>() // Add RoleManager
    .AddEntityFrameworkStores<Context>()
    .AddSignInManager<SignInManager<User>>()
    .AddUserManager<UserManager<User>>()
    .AddDefaultTokenProviders();

//be able to authenticate user using JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            //ValidAudience = builder.Configuration["JWT:Audience"], // <-- add this
            ValidateIssuer = true,
            ValidateAudience = false
        };
    });



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // Serves the generated OpenAPI/Swagger specification as a JSON endpoint
    app.UseSwaggerUI(); // Serves the interactive web UI
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Remove app.MapOpenApi(); as it is not standard for Swashbuckle
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
