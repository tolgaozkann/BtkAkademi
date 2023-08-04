﻿using AutoMapper;
using BtkAkademi.Entities.Dtos;
using BtkAkademi.Entities.Models;

namespace BtkAkademi.WebAPI.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdateBookDto,Book>().ReverseMap();
            CreateMap<Book, BookDto>();
            CreateMap<InsertBookDto, Book>();
        }
    }
}
