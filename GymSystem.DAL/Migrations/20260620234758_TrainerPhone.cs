using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TrainerPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Trainer_Phone",
                table: "Trainers");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Trainer_Phone",
                table: "Trainers",
                sql: "Phone Like '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Trainer_Phone",
                table: "Trainers");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Trainer_Phone",
                table: "Trainers",
                sql: "Phone Like '01[0125][0-9][0-9][0-9][0-9][0-9][0-9]'");
        }
    }
}
