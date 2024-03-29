namespace CodeBase.Domain.Models
{
    public class UserData
    {
        public UserData(string name) => Name = name;

        public string Name { get; }
    }
}