using GymSystem.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.BLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Required(ErrorMessage = "Height is required.")]
        [Range(0.1 , 300, ErrorMessage = "Height must be a positive number between 0.1 and 300 cm.")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "Weight is required.")]
        [Range(0.1 , 500, ErrorMessage = "Weight must be a positive number between 0.1 and 500 kg.")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Blood Type is required.")]
        public BloodType BloodType { get; set; }

        public string? Note { get; set; }

    }
}