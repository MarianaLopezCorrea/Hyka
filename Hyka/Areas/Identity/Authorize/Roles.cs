
namespace Hyka.Areas.Identity.RolesDefinition
{
    public class Roles
    {
        public const string ADMIN = "Admin";

        public const string BLOCKBUSTER = "Blockbuster";

        public static List<String> RolesList = new(){
            ADMIN,
            BLOCKBUSTER,
        };
    }
}