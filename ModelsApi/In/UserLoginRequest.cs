using Domain;

namespace ModelsApi.In
{
    public class UserLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public User ToEntity()
        {
            return new User
            {
                Email = Email,
                Password = Password
            };
        }
    }
}