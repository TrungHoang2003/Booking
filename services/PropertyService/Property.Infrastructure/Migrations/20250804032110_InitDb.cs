using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Property.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AmenityGroupId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IconUrl = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AmenityGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IconUrl = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenityGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsRoomBased = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PropertyTypeId = table.Column<int>(type: "integer", nullable: false),
                    HostId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    FloorNumber = table.Column<int>(type: "integer", nullable: true),
                    NeighborhoodDescription = table.Column<string>(type: "text", nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "text", nullable: true),
                    CheckInTimeFrom = table.Column<TimeSpan>(type: "interval", nullable: true),
                    CheckInTimeUntil = table.Column<TimeSpan>(type: "interval", nullable: true),
                    CheckOutTimeUntil = table.Column<TimeSpan>(type: "interval", nullable: true),
                    PetAllowed = table.Column<bool>(type: "boolean", nullable: true),
                    SmokingAllowed = table.Column<bool>(type: "boolean", nullable: true),
                    PartyAllowed = table.Column<bool>(type: "boolean", nullable: true),
                    AgeRestriction = table.Column<int>(type: "integer", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    PostCode = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_PropertyTypes_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "PropertyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyAmenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PropertyId = table.Column<int>(type: "integer", nullable: false),
                    AmenityId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyAmenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyAmenities_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PropertyId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyLanguages_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RentalUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PropertyId = table.Column<int>(type: "integer", nullable: false),
                    MaxAdults = table.Column<int>(type: "integer", nullable: false),
                    MaxChildren = table.Column<int>(type: "integer", nullable: false),
                    BasePricePerNight = table.Column<decimal>(type: "numeric", nullable: false),
                    PriceCurrency = table.Column<string>(type: "text", nullable: false),
                    RentalType = table.Column<string>(type: "text", nullable: false),
                    BathroomsCount = table.Column<int>(type: "integer", nullable: false),
                    BedroomsCount = table.Column<int>(type: "integer", nullable: false),
                    RentalUnit_RentalType = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    SharedBathroom = table.Column<bool>(type: "boolean", nullable: false),
                    Size = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalUnits_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bedrooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RentalUnitId = table.Column<int>(type: "integer", nullable: false),
                    DoubleBeds = table.Column<int>(type: "integer", nullable: false),
                    SingleBeds = table.Column<int>(type: "integer", nullable: false),
                    KingBeds = table.Column<int>(type: "integer", nullable: false),
                    SofaBeds = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bedrooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bedrooms_RentalUnits_RentalUnitId",
                        column: x => x.RentalUnitId,
                        principalTable: "RentalUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntityId = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    EntityType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Properties_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_RentalUnits_EntityId",
                        column: x => x.EntityId,
                        principalTable: "RentalUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RentalUnitAmenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RentalUnitId = table.Column<int>(type: "integer", nullable: false),
                    AmenityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalUnitAmenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalUnitAmenities_RentalUnits_RentalUnitId",
                        column: x => x.RentalUnitId,
                        principalTable: "RentalUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "Id", "AmenityGroupId", "Description", "IconUrl", "IsPaid", "Name" },
                values: new object[,]
                {
                    { 1, null, null, null, false, "Air conditioning" },
                    { 2, null, null, null, false, "Heating" },
                    { 3, null, null, null, false, "Free Wifi" },
                    { 4, null, null, null, false, "Electric vehicle charing station" },
                    { 5, null, null, null, false, "Free parking on premises" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "English" },
                    { 2, "French" },
                    { 3, "German" },
                    { 4, "Italian" },
                    { 5, "Spanish" },
                    { 6, "Japanese" },
                    { 7, "Chinese" },
                    { 8, "Korean" },
                    { 9, "Russian" },
                    { 10, "Arabic" }
                });

            migrationBuilder.InsertData(
                table: "PropertyTypes",
                columns: new[] { "Id", "Description", "IsRoomBased", "Name" },
                values: new object[,]
                {
                    { 1, "Furnished and self-catering accommodation available for short- and long-term rental", false, "Apartment" },
                    { 2, "Free-standing home with private, external entrance and rented specifically for holidays", false, "Holiday home" },
                    { 4, "Private self-standing and self-catering home with luxury feel", false, "Villa" },
                    { 5, "Free-standing home characterised by sloped roof and rented specifically for holidays", false, "Chalet" },
                    { 6, "Private self-catering residences located on a shared grounds with shared facilities or recreational activities", false, "Holiday park" },
                    { 7, "A self-catering apartment with some hotel facilities like a reception desk", false, "Aparthotel" },
                    { 8, "Accommodation for travellers often offering restaurants, meeting rooms and other guest services", true, "Hotel" },
                    { 9, "Private home with separate living facilities for host and guest", true, "Guest house" },
                    { 10, "Private home offering overnight stays and breakfast", true, "Bed and breakfast" },
                    { 11, "Private home with shared living facilities for host and guest", true, "Homestay" },
                    { 12, "Budget accommodation with mostly dorm-style bedding and a social atmosphere", true, "Hostel" },
                    { 13, "A self-catering apartment with some hotel facilities like a reception desk", true, "Aparthotel" },
                    { 14, "Extremely small units or capsules offering cheap and basic overnight accommodation", true, "Capsule hotel" },
                    { 15, "Private home with simple accommodation in the countryside", true, "Country house" },
                    { 16, "Private farm with simple accommodation", true, "Farm stay" },
                    { 17, "Small and basic accommodation with a rustic feel", true, "Inn" },
                    { 18, "Adult-only accommodation rented per hour or night", true, "Love hotel" },
                    { 19, "Roadside hotel usually for motorists, with direct access to parking and little to no amenities", true, "Motel" },
                    { 20, "Traditional Moroccan accommodation with a courtyard and luxury feel", true, "Riad" },
                    { 21, "A place for relaxation with onsite restaurants, activities and often with a luxury feel", true, "Resort" },
                    { 22, "Traditional Japanese-style accommodation with meal options", true, "Ryokan" },
                    { 23, "Private home with accommodation surrounded by nature, such as mountains or forest", true, "Lodge" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bedrooms_RentalUnitId",
                table: "Bedrooms",
                column: "RentalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_EntityId",
                table: "Images",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyTypeId",
                table: "Properties",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyAmenities_PropertyId",
                table: "PropertyAmenities",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyLanguages_PropertyId",
                table: "PropertyLanguages",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalUnitAmenities_RentalUnitId",
                table: "RentalUnitAmenities",
                column: "RentalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalUnits_PropertyId",
                table: "RentalUnits",
                column: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.DropTable(
                name: "AmenityGroups");

            migrationBuilder.DropTable(
                name: "Bedrooms");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "PropertyAmenities");

            migrationBuilder.DropTable(
                name: "PropertyLanguages");

            migrationBuilder.DropTable(
                name: "RentalUnitAmenities");

            migrationBuilder.DropTable(
                name: "RentalUnits");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "PropertyTypes");
        }
    }
}
