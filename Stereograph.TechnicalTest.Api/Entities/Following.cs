namespace Stereograph.TechnicalTest.Api.Models;

public class Following
{
    public int FollowerId { get; set; }
    public int FollowedPersonId { get; set; }
    public Person Follower { get; set; }
    public Person FollowedPerson { get; set; }

    public Following()
    { }

    public Following(FollowingDTO following)
    {
        this.FollowerId = following.FollowerId;
        this.FollowedPersonId = following.FollowedPersonId;
    }
}
