using BCrypt.Net;

namespace FacilaIT.Helper.Shared
{
    public class PasswordHasher
    {
        static public string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        static public bool VerifyPassword(string password, string hashedPassword)
        {
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return isValidPassword;
        }
    }
}