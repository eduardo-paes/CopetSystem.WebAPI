using AutoMapper;
using Domain.Interfaces.Repositories;
using Application.Interfaces.UseCases.ProjectPartialReport;
using Application.Ports.ProjectPartialReport;
using Application.Validation;

namespace Application.UseCases.ProjectPartialReport
{
    public class GetProjectPartialReportById : IGetProjectPartialReportById
    {
        #region Global Scope
        private readonly IProjectPartialReportRepository _repository;
        private readonly IMapper _mapper;
        public GetProjectPartialReportById(IProjectPartialReportRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        #endregion Global Scope

        public async Task<DetailedReadProjectPartialReportOutput> ExecuteAsync(Guid? id)
        {
            UseCaseException.NotInformedParam(id is null, nameof(id));
            Domain.Entities.ProjectPartialReport? entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<DetailedReadProjectPartialReportOutput>(entity);
        }
    }
}