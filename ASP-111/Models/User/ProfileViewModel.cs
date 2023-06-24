namespace ASP_111.Models.User
{
    public class ProfileViewModel
    {
        public String Name { get; set; } = null!;
        public String Email { get; set; } = null!;
        public String Login { get; set; } = null!;
        public String? Avatar { get; set; }
        public DateTime CreatedDt { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}