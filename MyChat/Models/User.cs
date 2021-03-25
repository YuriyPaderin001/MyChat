namespace MyChat.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public User(int id, string firstName, string middleName, string lastName, string login, string password)
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Login = login;
            Password = password;
        }

        public override string ToString()
        {
            return $"{Id} {FirstName} {MiddleName} {LastName} {Login} {Password}";
        }
    }
}
