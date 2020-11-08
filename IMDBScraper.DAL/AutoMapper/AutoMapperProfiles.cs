using AutoMapper;
using IMDBScraper.DAL.DBEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMDBScraper.DAL.AutoMapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<IMDBScraper.DAL.DBEntity.Actor, IMDBScraper.Shared.DTO.Actor>()
                .ForMember(dest => dest.Name, opt => {
                    opt.MapFrom(src => src.Name);
                }).ForMember(dest => dest.ImageURL, opt => {
                    opt.MapFrom(src => src.ImageURL);
                }).ForMember(dest => dest.Roles, opt => {
                    opt.MapFrom(src => src.Roles);
                }).ForMember(dest => dest.Gender, opt => {
                    opt.MapFrom(src => src.Gender);
                }).ForMember(dest => dest.Id, opt => {
                    opt.MapFrom(src => src.InternalId);
                }).ForMember(dest => dest.IsHide, opt => {
                    opt.MapFrom(src => src.IsHide);
                });

            CreateMap<IMDBScraper.Shared.DTO.Actor,IMDBScraper.DAL.DBEntity.Actor>()
                   .ForMember(dest => dest.Name, opt => {
                       opt.MapFrom(src => src.Name);
                   }).ForMember(dest => dest.Roles, opt => {
                       opt.MapFrom(src => src.Roles);
                   }).ForMember(dest => dest.ImageURL, opt => {
                       opt.MapFrom(src => src.ImageURL);
                   }).ForMember(dest => dest.Gender, opt => {
                       opt.MapFrom(src => src.Gender);
                   }).ForMember(dest => dest.InternalId, opt => {
                       opt.MapFrom(src => src.Id);
                   }).ForMember(dest => dest.IsHide, opt => {
                       opt.MapFrom(src => src.IsHide);
                   });
        }

    }
}
