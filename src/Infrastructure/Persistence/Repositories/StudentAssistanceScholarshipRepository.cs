using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class StudentAssistanceScholarshipRepository : IStudentAssistanceScholarshipRepository
    {
        #region Global Scope
        private readonly ApplicationDbContext _context;
        public StudentAssistanceScholarshipRepository(ApplicationDbContext context) => _context = context;
        #endregion

        #region Public Methods
        public async Task<StudentAssistanceScholarship> Create(StudentAssistanceScholarship model)
        {
            _context.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<IEnumerable<StudentAssistanceScholarship>> GetAll(int skip, int take) => await _context.StudentAssistanceScholarships
            .Skip(skip)
            .Take(take)
            .AsAsyncEnumerable()
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<StudentAssistanceScholarship?> GetById(Guid? id) =>
            await _context.StudentAssistanceScholarships
                .IgnoreQueryFilters()
                .AsAsyncEnumerable()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<StudentAssistanceScholarship> Delete(Guid? id)
        {
            var model = await GetById(id)
                ?? throw new Exception($"Nenhum registro encontrado para o id ({id}) informado.");
            model.DeactivateEntity();
            return await Update(model);
        }

        public async Task<StudentAssistanceScholarship> Update(StudentAssistanceScholarship model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<StudentAssistanceScholarship?> GetStudentAssistanceScholarshipByName(string name)
        {
            string loweredName = name.ToLower();
            var entities = await _context.StudentAssistanceScholarships
                .Where(x => x.Name!.ToLower() == loweredName)
                .AsAsyncEnumerable()
                .ToListAsync();
            return entities.FirstOrDefault();
        }
        #endregion
    }
}