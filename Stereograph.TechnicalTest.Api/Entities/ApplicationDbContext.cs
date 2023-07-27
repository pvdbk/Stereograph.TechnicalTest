using Microsoft.EntityFrameworkCore;

namespace Stereograph.TechnicalTest.Api.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
