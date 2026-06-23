using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymSystem.BLL.ViewModels.MemberViewModels
{
    public class UpdateMemberViewModel
    {
        public string? Name { get; set; }
        public string? Photo { get; set; }

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Phone Number Is Required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone Number Must be a Valid Egyptian Number")]
        public string Phone { get; set; } = default!;

        [Required(ErrorMessage ="Building Number Is Required")]
        [Range(1, int.MaxValue, ErrorMessage ="Building Number Must Be Greater than 0")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage ="City Is Required")]
        [StringLength(100, MinimumLength = 2 , ErrorMessage ="City Must be between 2 and 100 Characters")]
        [RegularExpression(@"[a-zA-Z\s]+$", ErrorMessage= "City Can Only Contain Letters & Spaces")]
        public string City { get; set; } = default!;
        [Required(ErrorMessage ="Street Is Required")]
        [StringLength(150, MinimumLength = 2 , ErrorMessage ="Street Must be between 2 and 150 Characters")]
        [RegularExpression(@"[a-zA-Z0-9\s]+$", ErrorMessage= "Street Can Only Contain Letters, Numbers & Spaces")]
        public string Street { get; set; } = default!;


    }
}
