using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Stereograph.TechnicalTest.Api.Models;

public class ApplicationDbContext : DbContext
{
    public virtual DbSet<Person> Persons { get; set; }
    public virtual DbSet<Following> Followings { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }

    public ApplicationDbContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasMany(followedPerson => followedPerson.Followers)
            .WithMany(follower => follower.FollowedPersons)
            .UsingEntity<Following>(
                f => f
                    .HasOne(following => following.Follower)
                    .WithMany(follower => follower.Followings)
                    .HasForeignKey(following => following.FollowerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(),
                f => f
                    .HasOne(following => following.FollowedPerson)
                    .WithMany(follower => follower.FollowingsBy)
                    .HasForeignKey(following => following.FollowedPersonId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(),
                f => f
                    .HasIndex(following => new { following.FollowerId, following.FollowedPersonId })
                    .IsUnique()
            );
    }
}
