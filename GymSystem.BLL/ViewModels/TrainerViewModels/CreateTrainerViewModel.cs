using GymSystem.DAL.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymSystem.BLL.ViewModels.TrainerViewModels
{
    public class CreateTrainerViewModel
    {
        [Required(ErrorMessage = "Profile Photo Is Required")]
        [Display(Name = "Profile Photo")]
        public IFormFile PhotoFile { get; set; } = default!;

        [Required(ErrorMessage ="Name Is Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage ="Name Can Only Contain Letters And Spaces")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage ="Email Is Required")]
        [EmailAddress(ErrorMessage ="Invalid Email Format")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must be Valid Egyptian number.")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = default!;
        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }
        [Required(ErrorMessage = " Building Number Is Required.")]
        [Range(1, 90000, ErrorMessage = "Building Number must be a positive Number.")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = " Street Name Is Required.")]
        [StringLength(150, ErrorMessage = "Street Name cannot exceed 150 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Street Name can only contain letters and spaces.")]
        public string Street { get; set; } = default!;

        [Required(ErrorMessage = " City Name Is Required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City Name cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Name can only contain letters and spaces.")]
        public string City { get; set; } = default!;

        public TrainingType Specialities { get; set; }
    }
}
