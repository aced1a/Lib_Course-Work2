namespace Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.author",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        first_name = c.String(maxLength: 50, unicode: false),
                        middle_name = c.String(maxLength: 50, unicode: false),
                        last_name = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.book_author",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        book_id = c.Int(nullable: false),
                        author_id = c.Int(nullable: false),
                        from_story = c.Boolean(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.book", t => t.book_id, cascadeDelete: true)
                .ForeignKey("dbo.author", t => t.author_id, cascadeDelete: true)
                .Index(t => t.book_id)
                .Index(t => t.author_id);
            
            CreateTable(
                "dbo.book",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        title = c.String(nullable: false, maxLength: 50, unicode: false),
                        year = c.Int(),
                        isbn = c.String(maxLength: 13, unicode: false),
                        description = c.String(unicode: false, storeType: "text"),
                        note = c.String(unicode: false, storeType: "text"),
                        location_id = c.Int(),
                        cover_id = c.Int(),
                        binding_id = c.Int(),
                        image = c.Binary(storeType: "image"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.binding_type", t => t.binding_id)
                .ForeignKey("dbo.cover_type", t => t.cover_id)
                .ForeignKey("dbo.location", t => t.location_id)
                .Index(t => t.location_id)
                .Index(t => t.cover_id)
                .Index(t => t.binding_id);
            
            CreateTable(
                "dbo.binding_type",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.book_genre",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        book_id = c.Int(nullable: false),
                        genre_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.genre", t => t.genre_id, cascadeDelete: true)
                .ForeignKey("dbo.book", t => t.book_id, cascadeDelete: true)
                .Index(t => t.book_id)
                .Index(t => t.genre_id);
            
            CreateTable(
                "dbo.genre",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.book_publisher",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        book_id = c.Int(nullable: false),
                        publisher_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.publisher", t => t.publisher_id, cascadeDelete: true)
                .ForeignKey("dbo.book", t => t.book_id, cascadeDelete: true)
                .Index(t => t.book_id)
                .Index(t => t.publisher_id);
            
            CreateTable(
                "dbo.publisher",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.book_story",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        book_id = c.Int(nullable: false),
                        story_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.story", t => t.story_id, cascadeDelete: true)
                .ForeignKey("dbo.book", t => t.book_id, cascadeDelete: true)
                .Index(t => t.book_id)
                .Index(t => t.story_id);
            
            CreateTable(
                "dbo.story",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        title = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.story_author",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        story_id = c.Int(nullable: false),
                        author_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.story", t => t.story_id, cascadeDelete: true)
                .ForeignKey("dbo.author", t => t.author_id, cascadeDelete: true)
                .Index(t => t.story_id)
                .Index(t => t.author_id);
            
            CreateTable(
                "dbo.cover_type",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.location",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        rack = c.String(maxLength: 50, unicode: false),
                        shelf = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.story_author", "author_id", "dbo.author");
            DropForeignKey("dbo.book_author", "author_id", "dbo.author");
            DropForeignKey("dbo.book", "location_id", "dbo.location");
            DropForeignKey("dbo.book", "cover_id", "dbo.cover_type");
            DropForeignKey("dbo.book_story", "book_id", "dbo.book");
            DropForeignKey("dbo.story_author", "story_id", "dbo.story");
            DropForeignKey("dbo.book_story", "story_id", "dbo.story");
            DropForeignKey("dbo.book_publisher", "book_id", "dbo.book");
            DropForeignKey("dbo.book_publisher", "publisher_id", "dbo.publisher");
            DropForeignKey("dbo.book_genre", "book_id", "dbo.book");
            DropForeignKey("dbo.book_genre", "genre_id", "dbo.genre");
            DropForeignKey("dbo.book_author", "book_id", "dbo.book");
            DropForeignKey("dbo.book", "binding_id", "dbo.binding_type");
            DropIndex("dbo.story_author", new[] { "author_id" });
            DropIndex("dbo.story_author", new[] { "story_id" });
            DropIndex("dbo.book_story", new[] { "story_id" });
            DropIndex("dbo.book_story", new[] { "book_id" });
            DropIndex("dbo.book_publisher", new[] { "publisher_id" });
            DropIndex("dbo.book_publisher", new[] { "book_id" });
            DropIndex("dbo.book_genre", new[] { "genre_id" });
            DropIndex("dbo.book_genre", new[] { "book_id" });
            DropIndex("dbo.book", new[] { "binding_id" });
            DropIndex("dbo.book", new[] { "cover_id" });
            DropIndex("dbo.book", new[] { "location_id" });
            DropIndex("dbo.book_author", new[] { "author_id" });
            DropIndex("dbo.book_author", new[] { "book_id" });
            DropTable("dbo.location");
            DropTable("dbo.cover_type");
            DropTable("dbo.story_author");
            DropTable("dbo.story");
            DropTable("dbo.book_story");
            DropTable("dbo.publisher");
            DropTable("dbo.book_publisher");
            DropTable("dbo.genre");
            DropTable("dbo.book_genre");
            DropTable("dbo.binding_type");
            DropTable("dbo.book");
            DropTable("dbo.book_author");
            DropTable("dbo.author");
        }
    }
}
