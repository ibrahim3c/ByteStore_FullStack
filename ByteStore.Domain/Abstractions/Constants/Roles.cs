namespace ByteStore.Domain.Abstractions.Constants
{
    public class Roles
    {
        public const string AdminRole = "Admin";
        public const string UserRole = "User";
        public static readonly string[] AllRoles = { AdminRole, UserRole };
    }
}
