using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Entities
{
    public class UserRelation : EntitySimple
    {
        public int UserId { get; set; }
        public int UserRelationId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("UserRelationId")]
        public User UserRelationUser { get; set; }
    }
}
