using Microsoft.EntityFrameworkCore;

namespace Stereograph.TechnicalTest.Api.Models;

public class ApplicationDbContext : DbContext
{
    public virtual DbSet<Person> Persons { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }

    public ApplicationDbContext() { }
}
