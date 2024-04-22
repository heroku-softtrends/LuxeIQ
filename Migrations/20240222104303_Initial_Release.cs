using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LuxeIQ.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Release : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                schema: "public",
                columns: table => new
                {
                    manufacturerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    businessName = table.Column<string>(type: "text", nullable: false),
                    address1 = table.Column<string>(type: "text", nullable: true),
                    address2 = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    zipcode = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    contactName = table.Column<string>(type: "text", nullable: true),
                    contactEmail = table.Column<string>(type: "text", nullable: true),
                    corporateAdmin = table.Column<string>(type: "text", nullable: true),
                    corporateAdminEmail = table.Column<string>(type: "text", nullable: true),
                    salesAdmin = table.Column<string>(type: "text", nullable: true),
                    salesAdminEmail = table.Column<string>(type: "text", nullable: true),
                    otherAdmin = table.Column<string>(type: "text", nullable: true),
                    otherAdminEmail = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    product_attributes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.manufacturerId);
                });

            migrationBuilder.CreateTable(
                name: "SalesRepAgency",
                schema: "public",
                columns: table => new
                {
                    salesRepAgencyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    territoryNumber = table.Column<long>(type: "bigint", nullable: true),
                    salesRepAgencyName = table.Column<string>(type: "text", nullable: true),
                    address1 = table.Column<string>(type: "text", nullable: true),
                    address2 = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    zipcode = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    administrator = table.Column<string>(type: "text", nullable: true),
                    administratorMail = table.Column<string>(type: "text", nullable: true),
                    territoryName = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesRepAgency", x => x.salesRepAgencyId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "public",
                columns: table => new
                {
                    userId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userType = table.Column<string>(type: "text", nullable: true),
                    ManufacturerId = table.Column<long>(type: "bigint", nullable: true),
                    salesRepAgencyId = table.Column<long>(type: "bigint", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    whatsappMobile = table.Column<string>(type: "text", nullable: true),
                    activationStatus = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    zipCode = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "ManufacturerTerritories",
                schema: "public",
                columns: table => new
                {
                    territoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    manufacturerId = table.Column<long>(type: "bigint", nullable: false),
                    repCode = table.Column<long>(type: "bigint", nullable: true),
                    salesRegion = table.Column<string>(type: "text", nullable: true),
                    salesAgency = table.Column<string>(type: "text", nullable: true),
                    salesTerritory = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManufacturerTerritories", x => x.territoryId);
                    table.ForeignKey(
                        name: "FK_ManufacturerTerritories_Manufacturers_manufacturerId",
                        column: x => x.manufacturerId,
                        principalSchema: "public",
                        principalTable: "Manufacturers",
                        principalColumn: "manufacturerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "public",
                columns: table => new
                {
                    productId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    manufacturerId = table.Column<long>(type: "bigint", nullable: false),
                    tableName = table.Column<string>(type: "text", nullable: true),
                    productAttributes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.productId);
                    table.ForeignKey(
                        name: "FK_Products_Manufacturers_manufacturerId",
                        column: x => x.manufacturerId,
                        principalSchema: "public",
                        principalTable: "Manufacturers",
                        principalColumn: "manufacturerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wholesalers",
                schema: "public",
                columns: table => new
                {
                    wholesalerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    manufacturerId = table.Column<long>(type: "bigint", nullable: false),
                    businessName = table.Column<string>(type: "text", nullable: true),
                    address1 = table.Column<string>(type: "text", nullable: true),
                    address2 = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    zipcode = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    purchasingMultiplier = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wholesalers", x => x.wholesalerId);
                    table.ForeignKey(
                        name: "FK_Wholesalers_Manufacturers_manufacturerId",
                        column: x => x.manufacturerId,
                        principalSchema: "public",
                        principalTable: "Manufacturers",
                        principalColumn: "manufacturerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WholesalerHQ",
                schema: "public",
                columns: table => new
                {
                    wholesalerHQId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    wholesalerId = table.Column<long>(type: "bigint", nullable: false),
                    accountNo = table.Column<int>(type: "integer", nullable: true),
                    salesRegion = table.Column<int>(type: "integer", nullable: true),
                    salesTerritory = table.Column<int>(type: "integer", nullable: true),
                    customer = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    zipcode = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    fax = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WholesalerHQ", x => x.wholesalerHQId);
                    table.ForeignKey(
                        name: "FK_WholesalerHQ_Wholesalers_wholesalerId",
                        column: x => x.wholesalerId,
                        principalSchema: "public",
                        principalTable: "Wholesalers",
                        principalColumn: "wholesalerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WholesalerShowrooms",
                schema: "public",
                columns: table => new
                {
                    showroomId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    wholesalerId = table.Column<long>(type: "bigint", nullable: false),
                    wholesalerAccountNo = table.Column<string>(type: "text", nullable: true),
                    businessName = table.Column<string>(type: "text", nullable: true),
                    address1 = table.Column<string>(type: "text", nullable: true),
                    address2 = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    zipcode = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    phoneNumber = table.Column<string>(type: "text", nullable: true),
                    contactName = table.Column<string>(type: "text", nullable: true),
                    contactMail = table.Column<string>(type: "text", nullable: true),
                    branchNumber = table.Column<string>(type: "text", nullable: true),
                    manufacturerAccountNo = table.Column<string>(type: "text", nullable: true),
                    buyingMultiplier = table.Column<string>(type: "text", nullable: true),
                    territoryNumber = table.Column<long>(type: "bigint", nullable: true),
                    territoryName = table.Column<string>(type: "text", nullable: true),
                    salesAgency = table.Column<string>(type: "text", nullable: true),
                    salesRep = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WholesalerShowrooms", x => x.showroomId);
                    table.ForeignKey(
                        name: "FK_WholesalerShowrooms_Wholesalers_wholesalerId",
                        column: x => x.wholesalerId,
                        principalSchema: "public",
                        principalTable: "Wholesalers",
                        principalColumn: "wholesalerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturerTerritories_manufacturerId",
                schema: "public",
                table: "ManufacturerTerritories",
                column: "manufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_manufacturerId",
                schema: "public",
                table: "Products",
                column: "manufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_WholesalerHQ_wholesalerId",
                schema: "public",
                table: "WholesalerHQ",
                column: "wholesalerId");

            migrationBuilder.CreateIndex(
                name: "IX_Wholesalers_manufacturerId",
                schema: "public",
                table: "Wholesalers",
                column: "manufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_WholesalerShowrooms_wholesalerId",
                schema: "public",
                table: "WholesalerShowrooms",
                column: "wholesalerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManufacturerTerritories",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SalesRepAgency",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "public");

            migrationBuilder.DropTable(
                name: "WholesalerHQ",
                schema: "public");

            migrationBuilder.DropTable(
                name: "WholesalerShowrooms",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Wholesalers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Manufacturers",
                schema: "public");
        }
    }
}
