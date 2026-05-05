using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfGuests = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CtetedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "RestaurantAdminInvitations",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        RestaurantId = table.Column<int>(type: "int", nullable: false),
            //        Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        IsUsed = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_RestaurantAdminInvitations", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Restaurants",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        OpeningHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        AverageRating = table.Column<double>(type: "float", nullable: false),
            //        ReviewsCount = table.Column<int>(type: "int", nullable: false),
            //        NeedsRatingUpdate = table.Column<bool>(type: "bit", nullable: false),
            //        OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        CtetedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Restaurants", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "categories",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        RestaurantId = table.Column<int>(type: "int", nullable: false),
            //        CtetedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_categories", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_categories_Restaurants_RestaurantId",
            //            column: x => x.RestaurantId,
            //            principalTable: "Restaurants",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Reviews",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Rating = table.Column<int>(type: "int", nullable: false),
            //        Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        RestaurantId = table.Column<int>(type: "int", nullable: false),
            //        CtetedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Reviews", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Reviews_Restaurants_RestaurantId",
            //            column: x => x.RestaurantId,
            //            principalTable: "Restaurants",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Tables",
            //    columns: table => new
            //    {
            //        TableNumber = table.Column<int>(type: "int", nullable: false),
            //        RestaurantId = table.Column<int>(type: "int", nullable: false),
            //        Capacity = table.Column<int>(type: "int", nullable: false),
            //        IsAvailable = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Tables", x => new { x.TableNumber, x.RestaurantId });
            //        table.ForeignKey(
            //            name: "FK_Tables_Restaurants_RestaurantId",
            //            column: x => x.RestaurantId,
            //            principalTable: "Restaurants",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "MenuItems",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        IsAvailable = table.Column<bool>(type: "bit", nullable: false),
            //        CategoryId = table.Column<int>(type: "int", nullable: false),
            //        CtetedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MenuItems", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_MenuItems_categories_CategoryId",
            //            column: x => x.CategoryId,
            //            principalTable: "categories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_categories_RestaurantId",
            //    table: "categories",
            //    column: "RestaurantId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MenuItems_CategoryId",
            //    table: "MenuItems",
            //    column: "CategoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Reviews_RestaurantId_UserId",
            //    table: "Reviews",
            //    columns: new[] { "RestaurantId", "UserId" },
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tables_RestaurantId",
            //    table: "Tables",
            //    column: "RestaurantId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tables_TableNumber_RestaurantId",
            //    table: "Tables",
            //    columns: new[] { "TableNumber", "RestaurantId" },
            //    unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "RestaurantAdminInvitations");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "Restaurants");
        }
    }
}
