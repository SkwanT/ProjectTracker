namespace ProjectTracker.Migrations.ProjectTracker
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    FirstName = c.String(nullable: false, maxLength: 128),
                    LastName = c.String(nullable: false, maxLength: 128),
                    UserName = c.String(nullable: false, maxLength: 256),
                    PasswordHash = c.String(nullable: false),
                    SecurityStamp = c.String(nullable: false),
                    RoleID = c.Int(nullable: false),
                    Active = c.Boolean(nullable: false),
                    DateAdded = c.DateTime(nullable: false),
                    InsertUserID = c.String(nullable: false, maxLength: 128),
                    Deleted = c.Boolean(nullable: false),
                    UpdateUserID = c.String(nullable: false, maxLength: 128),
                    UpdateDate = c.DateTime(nullable: false),
                    AdrianaID = c.Long(),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.RoleID);

            CreateTable(
                "dbo.Roles",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    RoleName = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Scripts",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    EntryDate = c.DateTime(nullable: false),
                    ScriptName = c.String(),
                    ScriptTypeID = c.Int(nullable: false),
                    ProjectName = c.String(nullable: false, maxLength: 500),
                    ProjectLink = c.String(maxLength: 500),
                    ProjectStatus = c.Boolean(nullable: false),
                    ProjectLocation = c.String(nullable: false, maxLength: 500),
                    Comments = c.String(),
                    AuthorID = c.Int(nullable: false),
                    CoAuthor1ID = c.Int(),
                    CoAuthor2ID = c.Int(),
                    Deleted = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Authors", t => t.AuthorID, cascadeDelete: true)
                .ForeignKey("dbo.ScriptTypes", t => t.ScriptTypeID, cascadeDelete: true)
                .Index(t => t.ScriptTypeID)
                .Index(t => t.AuthorID);

            CreateTable(
                "dbo.Reports",
                c => new
                {
                    ID = c.Int(nullable: false),
                    ComplexityID = c.Int(nullable: false),
                    CountryID = c.Int(nullable: false),
                    Points = c.Int(),
                    TaskSentDate = c.DateTime(),
                    ScriptEntryDate = c.DateTime(),
                    ScriptDoneDate = c.DateTime(),
                    ScriptStatus = c.Boolean(nullable: false),
                    EstimatedScriptingHours = c.Decimal(precision: 18, scale: 2),
                    ActualScriptingHours = c.Decimal(precision: 18, scale: 2),
                    ActualTestingHours = c.Decimal(precision: 18, scale: 2),
                    ScriptInTestErrors = c.Boolean(nullable: false),
                    ScriptInFieldErrors = c.Boolean(nullable: false),
                    ScriptComments = c.String(),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Complexities", t => t.ComplexityID, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.CountryID, cascadeDelete: true)
                .ForeignKey("dbo.Scripts", t => t.ID)
                .Index(t => t.ID)
                .Index(t => t.ComplexityID)
                .Index(t => t.CountryID);

            CreateTable(
                "dbo.Complexities",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Type = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Countries",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 8),
                    Name = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.ScriptTypes",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Type = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.ID);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Scripts", "ScriptTypeID", "dbo.ScriptTypes");
            DropForeignKey("dbo.Reports", "ID", "dbo.Scripts");
            DropForeignKey("dbo.Reports", "CountryID", "dbo.Countries");
            DropForeignKey("dbo.Reports", "ComplexityID", "dbo.Complexities");
            DropForeignKey("dbo.Scripts", "AuthorID", "dbo.Authors");
            DropForeignKey("dbo.Authors", "RoleID", "dbo.Roles");
            DropIndex("dbo.Reports", new[] { "CountryID" });
            DropIndex("dbo.Reports", new[] { "ComplexityID" });
            DropIndex("dbo.Reports", new[] { "ID" });
            DropIndex("dbo.Scripts", new[] { "AuthorID" });
            DropIndex("dbo.Scripts", new[] { "ScriptTypeID" });
            DropIndex("dbo.Authors", new[] { "RoleID" });
            DropTable("dbo.ScriptTypes");
            DropTable("dbo.Countries");
            DropTable("dbo.Complexities");
            DropTable("dbo.Reports");
            DropTable("dbo.Scripts");
            DropTable("dbo.Roles");
            DropTable("dbo.Authors");
        }
    }
}
