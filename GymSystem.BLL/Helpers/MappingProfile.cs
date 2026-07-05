using AutoMapper;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapMember();
            MapPlan();
            MapSession();
            MapTrainer();
            MapMembership();
        }

        private void MapMember()
        {
            CreateMap<CreateMemberViewModel, Member>()
                                .ForMember(des => des.Address, opt => opt.MapFrom(src => new Address
                                {
                                    BuildingNumber = src.BuildingNumber,
                                    Street = src.Street,
                                    City = src.City
                                }))
                                .ForMember(des => des.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));

            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

            CreateMap<Member, MemberViewModel>()
                .ForMember(des => des.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(des => des.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(des => des.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

            CreateMap<Member, UpdateMemberViewModel>()
                .ForMember(des => des.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(des => des.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(des => des.City, opt => opt.MapFrom(src => src.Address.City));

            CreateMap<UpdateMemberViewModel, Member>()
                .ForMember(des => des.Name, opt => opt.Ignore())
                .ForMember(des => des.Photo, opt => opt.Ignore())
                .AfterMap((src, des) =>
                {
                    des.Address = new Address();
                    des.Address.BuildingNumber = src.BuildingNumber;
                    des.Address.Street = src.Street;
                    des.Address.City = src.City;
                    des.UpdatedAt = DateTime.Now;
                });
        }

        private void MapPlan() 
        {
            CreateMap<Plan, PlanViewModel>().ReverseMap();

            CreateMap<Plan, UpdatePlanViewModel>().ForMember(des => des.PlanName, opt => opt.MapFrom(src => src.Name));
            CreateMap<UpdatePlanViewModel, Plan>().ForMember(des => des.Name, opt => opt.MapFrom(src => src.PlanName));
        }

        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(des => des.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(des => des.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Session, UpdateSessionViewModel>();
            CreateMap<UpdateSessionViewModel, Session>()
                .ForMember(des => des.Id, opt => opt.Ignore())
                .ForMember(des => des.Capacity, opt => opt.Ignore())
                .ForMember(des => des.CategoryId, opt => opt.Ignore())
                .ForMember(des => des.AvailableSlots, opt => opt.Ignore());

            CreateMap<CreateSessionViewModel, Session>()
                .ForMember(des => des.Id, opt => opt.Ignore())
                .ForMember(des => des.AvailableSlots, opt => opt.Ignore());

        }
        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(des => des.Address, opt => opt.MapFrom(src => new Address()
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));


            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(des => des.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
                .ForMember(des => des.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()));

            CreateMap<Trainer, UpdateTrainerViewModel>()
                .AfterMap((src, des) =>
                {
                    des.BuildingNumber = src.Address.BuildingNumber;
                    des.Street = src.Address.Street;
                    des.City = src.Address.City;
                });
        }

        private void MapMembership()
        {
            CreateMap<Membership, MembershipViewModel>()
                .ForMember(des => des.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(des => des.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
                .ForMember(des => des.StartDate, opt => opt.MapFrom(src => src.StartDate.ToShortDateString()));

            CreateMap<CreateMembershipViewModel, Membership>();
            CreateMap<Plan, PlansListViewModel>()
                .ForMember(des => des.PlanId , opt => opt.MapFrom(src => src.Id))
                .ForMember(des => des.PlanName , opt => opt.MapFrom(src => src.Name));
            CreateMap<Member, MembersListViewModel>()
                .ForMember(des => des.MemberId, opt => opt.MapFrom(src => src.Id))
                .ForMember(des => des.MemberName, opt => opt.MapFrom(src => src.Name));
        }



    }
}
