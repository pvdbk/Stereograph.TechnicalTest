using Microsoft.EntityFrameworkCore;

namespace Stereograph.TechnicalTest.Api.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
