﻿using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ISubAreaRepository : IGenericCRUDRepository<SubArea>
    {
        Task<SubArea?> GetByCode(string? code);
        Task<IEnumerable<SubArea>> GetSubAreasByArea(Guid? areaId, int skip, int take);
    }
}