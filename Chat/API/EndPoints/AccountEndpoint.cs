using API.Common;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace API.EndPoints
{
    public static class AccountEndpoint
    {
        public static RouteGroupBuilder MapAccountEndpoint(this WebApplication app)
        {
            var group = app.MapGroup("/api/account").WithTags("account");

            group.MapPost("/register", async (HttpContext context, UserManager<AppUser> userManager, [FromForm] string fullName, [FromForm] string email, [FromForm] string password, [FromForm] IFormFile? profileImage) =>
            {
                var userFromDb = await userManager.FindByEmailAsync(email);

                if (userFromDb is not null)
                {
                    return Results.BadRequest(Response<string>.Failure("User are already exists"));
                }

                var user = new AppUser
                {
                    FullName = fullName,
                    Email = email,
                    UserName = email
                };

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    return Results.BadRequest(Response<string>.Failure("Failed to create user"));
                }

                return Results.Ok(Response<string>.Success("User registered successfully"));
            });

            return group;
        }
    }
}
