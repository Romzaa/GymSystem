using GymSystem.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.ViewModels.TrainerViewModels
{
    public class TrainerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string? Photo { get; set; } = default!;
        public Gender Gender { get; set; }
        public string DateOfBirth { get; set; } = default!;
        public string Address { get; set; } = default!;
        public TrainingType Specialities { get; set; }
    }
}
