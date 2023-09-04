namespace Core.Entity.Entities
{
    public class EmailRecipient
    {
        public EmailRecipient(int id, string email, string passwordHash)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;

        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }



    }
}
