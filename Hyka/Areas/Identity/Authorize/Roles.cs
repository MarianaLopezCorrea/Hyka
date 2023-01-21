
namespace Hyka.Areas.Identity.RolesDefinition
{
    public static class Roles
    {
        public const string ADMIN = "Admin";

        public const string BLOCKBUSTER = "Blockbuster";

        public const string USER = "User";

        public static List<String> RolesList = new(){
            ADMIN,
            BLOCKBUSTER,
            USER
        };
    }
}