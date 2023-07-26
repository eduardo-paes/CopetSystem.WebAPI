using Domain.Entities;
using Domain.Interfaces.Repositories.Bases;

namespace Domain.Interfaces.Repositories;
public interface IProjectActivityRepository : IGenericCRUDRepository<ProjectActivity>
{
    Task<IList<ProjectActivity>> GetByProjectId(Guid? projectId);
}