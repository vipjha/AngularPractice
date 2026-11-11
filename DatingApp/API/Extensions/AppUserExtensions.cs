using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Extensions
{
    public static class AppUserExtensions
    {
        public static UserDto ToDto(this AppUser appUser, ITokenService tokenService)
        {
            var token = tokenService.CreateToken(appUser);
        {
            return new UserDto
            {
                Id = appUser.Id,
                Email = appUser.Email,
                DisplayName = appUser.DisplayName,
                Token = tokenService.CreateToken(appUser)
            };
        }
      }
}
}