using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;


namespace API.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
      /*public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
      {
        
      } */  

      public DbSet<AppUser> Users { get; set; }
    }
}