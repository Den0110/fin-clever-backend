using System.ComponentModel.DataAnnotations;


namespace FinClever
{
    public class User
    {
        [Key]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }

        public User() { }

        public User(string? id, string? name, string? email, string? imageUrl)
        {
            Id = id;
            Name = name;
            Email = email;
            ImageUrl = imageUrl;
        }
    }
}
