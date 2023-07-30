namespace Stereograph.TechnicalTest.Api.Models;

public class FollowingDTO
{
    public int FollowerId { get; set; }
    public int FollowedPersonId { get; set; }

    public FollowingDTO()
    { }

    public FollowingDTO(Following following)
    {
        this.FollowerId = following.FollowerId;
        this.FollowedPersonId = following.FollowedPersonId;
    }
}
