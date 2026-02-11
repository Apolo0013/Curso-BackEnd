using Microsoft.EntityFrameworkCore;
//Model
using BackEnd.Model.Auth;
using BackEnd.Model.Course;


namespace BackEnd.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //usuarios
    public DbSet<DbUser> Users => Set<DbUser>();
    //Cursos dos usuarios
    public DbSet<DbUserCourses> UserCourses => Set<DbUserCourses>();
    //Cursos
    public DbSet<DbCourses> Courses => Set<DbCourses>();
    //Modulos
    public DbSet<DbModules> Modules => Set<DbModules>();
    //Aulas / classes
    public DbSet<DbClasses> Classes => Set<DbClasses>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DbUserCourses>(builder =>
        {
            builder.ToTable("users_courses");
            builder.HasKey(x => new { x.IdUser, x.IdCourse });
            builder.Property(x => x.PurchasedAt)
                .HasColumnName("purchasedAt")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();
        });
    }
}