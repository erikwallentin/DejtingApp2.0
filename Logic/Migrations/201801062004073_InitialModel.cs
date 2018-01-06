namespace Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "User_Id", "dbo.AspNetUsers");
            CreateTable(
                "dbo.FriendRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FriendReceiver_Id = c.String(maxLength: 128),
                        FriendSender_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FriendReceiver_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FriendSender_Id)
                .Index(t => t.FriendReceiver_Id)
                .Index(t => t.FriendSender_Id);
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User1_Id = c.String(maxLength: 128),
                        User2_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User1_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User2_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.User1_Id)
                .Index(t => t.User2_Id)
                .Index(t => t.ApplicationUser_Id);
            
            AddColumn("dbo.Messages", "Receiver_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Messages", "Sender_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Messages", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Messages", "Receiver_Id");
            CreateIndex("dbo.Messages", "Sender_Id");
            CreateIndex("dbo.Messages", "ApplicationUser_Id");
            AddForeignKey("dbo.Messages", "Receiver_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Messages", "Sender_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Messages", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Messages", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendRequests", "FriendSender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendRequests", "FriendReceiver_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Sender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Receiver_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friends", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friends", "User2_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friends", "User1_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Messages", new[] { "Sender_Id" });
            DropIndex("dbo.Messages", new[] { "Receiver_Id" });
            DropIndex("dbo.Friends", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Friends", new[] { "User2_Id" });
            DropIndex("dbo.Friends", new[] { "User1_Id" });
            DropIndex("dbo.FriendRequests", new[] { "FriendSender_Id" });
            DropIndex("dbo.FriendRequests", new[] { "FriendReceiver_Id" });
            DropColumn("dbo.Messages", "ApplicationUser_Id");
            DropColumn("dbo.Messages", "Sender_Id");
            DropColumn("dbo.Messages", "Receiver_Id");
            DropTable("dbo.Friends");
            DropTable("dbo.FriendRequests");
            AddForeignKey("dbo.Messages", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
