namespace Web.EF.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<AlwaysEncryptedContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"EF\Migrations";
        }

        protected override void Seed(AlwaysEncryptedContext context)
        {

        }
    }
}
