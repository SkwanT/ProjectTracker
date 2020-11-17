namespace ProjectTracker.Migrations.Audit
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditTBs",
                c => new
                {
                    ID = c.Long(nullable: false, identity: true),
                    UserID = c.Int(),
                    SessionID = c.String(),
                    IPAddress = c.String(),
                    PageAccessed = c.String(),
                    LoggedInAt = c.DateTime(),
                    LoggedOutAt = c.DateTime(),
                    LoginStatus = c.String(),
                    ControllerName = c.String(),
                    ActionName = c.String(),
                })
                .PrimaryKey(t => t.ID);

        }

        public override void Down()
        {
            DropTable("dbo.AuditTBs");
        }
    }
}
