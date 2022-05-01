namespace Hyka.Areas.Identity.PoliciesDefinition
{
    public class Policy
    {
        public const string REQUIRE_BLOCKBUSTER = "RequireBlockbuster";
        public const string REQUIRE_ADMIN = "RequireAdmin";

        public static List<String> PoliciesList = new(){
            REQUIRE_ADMIN,
            REQUIRE_BLOCKBUSTER
        };
    }
}