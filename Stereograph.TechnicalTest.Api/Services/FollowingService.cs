using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Stereograph.TechnicalTest.Api.Services;

using Models;

public class FollowingService
{
    private DbSet<Following> Followings { get; init; }
    private Func<int> SaveChanges { get; init; }

    public FollowingService(ApplicationDbContext dbContext)
    {
        this.Followings = dbContext.Followings;
        this.SaveChanges = dbContext.SaveChanges;
    }

    public IEnumerable<Following> GetAll() => this.Followings;

    public Following Add(FollowingDTO toAddDTO)
    {
        Following toAdd = new(toAddDTO);
        this.Followings.Add(toAdd);
        this.SaveChanges();
        return toAdd;
    }

    public Following GetByIds(int followerId, int followedPersonId) => this
        .Followings
        .FirstOrDefault(following => following.FollowerId == followerId && following.FollowedPersonId == followedPersonId);

    public bool Remove(int followerId, int followedPersonId)
    {
        Following toRemove = this.GetByIds(followerId, followedPersonId);
        if (toRemove is null)
        {
            return false;
        }

        this.Followings.Remove(toRemove);
        this.SaveChanges();
        return true;
    }
}
