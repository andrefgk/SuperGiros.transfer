using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SuperGiros.Transfer.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SuperGiros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoPaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Celular = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoDocumento = table.Column<int>(type: "int", nullable: false),
                    NroDocumento = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifify = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModififyBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MontoDiario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NumeroClientes = table.Column<int>(type: "int", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifify = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModififyBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    TipoMovimiento = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Moneda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sede = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaRealizacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifify = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModififyBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifify = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModififyBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "ApellidoMaterno", "ApellidoPaterno", "Celular", "Created", "CreatedBy", "LastModifify", "LastModififyBy", "Nombre", "NroDocumento", "State", "TipoDocumento", "email" },
                values: new object[,]
                {
                    { 1, "Boza", "Mollo", "+51906081076", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Arely", 70537513, 1, 0, "arely@gmail.com" },
                    { 2, "Boza", "Mollo", "+51984452340", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Luis", 60528515, 1, 1, "luis@gmail.com" },
                    { 3, "Armejo", "Huachaca", "+51984173852", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Maria", 85214785, 1, 0, "maria@gmail.com" },
                    { 4, "Borda", "Rojas", "+51930161092", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Javier", 85214787, 1, 1, "javier@gmail.com" },
                    { 5, "Gallegos", "Flores", "+51901875421", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Andre", 78541587, 1, 0, "andre@gmail.com" },
                    { 6, "Bonifacio", "Quispe", "+51987951456", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Alejandro", 78541588, 1, 1, "alejandro@gmail.com" },
                    { 7, "Chumbe", "Rojas", "+51985741234", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Martin", 78965895, 1, 0, "martin@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "Id", "Created", "CreatedBy", "Estado", "LastModifify", "LastModififyBy", "MontoDiario", "Nombre", "NumeroClientes", "Saldo", "State", "Ubicacion" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5000m, "Oficina Central", 120, 20000m, 1, "Lima - Centro" },
                    { 2, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3000m, "Oficina Norte", 80, 15000m, 1, "Lima - Los Olivos" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "AccountId", "Created", "CreatedBy", "Descripcion", "FechaRealizacion", "LastModifify", "LastModififyBy", "Moneda", "Monto", "Sede", "State", "TipoMovimiento" },
                values: new object[,]
                {
                    { 1, 1001, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", "Depósito inicial de fondos", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sol", 1500.50m, "Cusco", 1, 1 },
                    { 2, 1001, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", "Retiro a cuenta bancaria", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sol", 200.00m, "Mazuko", 1, 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "CreatedBy", "LastModifify", "LastModififyBy", "PasswordHash", "Role", "State", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "$2a$11$iq30d.w1ZCDg2LU3VtpisuCG7kaLbFNE8nmiSLHZB5r3GSM3HpSBC", "Admin", 1, "admin" },
                    { 2, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "$2a$11$dfmFjF0uGcwaG5N/t9uzneSBeVA5cgCY6K1onyHqo5mHgUcXSrR2K", "Usuario", 1, "usuario" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Offices");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
