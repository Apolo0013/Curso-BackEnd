using Microsoft.EntityFrameworkCore;
//Model
using BackEnd.Model.Auth;

namespace BackEnd.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserModel> Users => Set<UserModel>();
}