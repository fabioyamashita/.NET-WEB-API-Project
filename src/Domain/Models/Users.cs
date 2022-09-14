namespace SPX_WEBAPI.Domain.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public Users(string name, string username, string password, string role)
        {
            Name = name;
            Username = username;
            Password = password;
            Role = role;
        }
    }
}
