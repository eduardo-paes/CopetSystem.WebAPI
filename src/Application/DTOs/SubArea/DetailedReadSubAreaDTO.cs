﻿using Application.DTOs.Area;
using Application.DTOs.Base;

namespace Application.DTOs.SubArea
{
    public class DetailedReadSubAreaDTO : ResponseDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public DateTime? DeletedAt { get; set; }
        public virtual DetailedReadAreaDTO? Area { get; set; }
    }
}