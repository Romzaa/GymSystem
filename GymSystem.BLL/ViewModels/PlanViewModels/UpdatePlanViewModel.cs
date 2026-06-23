using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymSystem.BLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
        public string PlanName { get; set; } = default!;
        [Required(ErrorMessage ="Description Is Required")]
        [StringLength(200, MinimumLength = 5 , ErrorMessage ="Description Must Be Between 5 & 200 Characters")]
        public string Description { get; set; } = default!;
        [Required(ErrorMessage ="Duration Is Required")]
        [Range(1,365, ErrorMessage ="Duration Must Be Between 1 & 365 days")]
        public int DurationDays { get; set; }
        [Required(ErrorMessage ="Price Is Required")]
        [Range(0.1,10000, ErrorMessage ="Price Must Be Greater Than 0")]
        public decimal Price { get; set; }
    }
}
