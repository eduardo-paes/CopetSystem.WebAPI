using Application.Interfaces.UseCases.ProjectFinalReport;
using Application.Ports.ProjectFinalReport;
using Application.Validation;
using AutoMapper;
using Domain.Interfaces.Repositories;

namespace Application.UseCases.ProjectFinalReport
{
    public class GetProjectFinalReportByProjectId : IGetProjectFinalReportsByProjectId
    {
        #region Global Scope
        private readonly IProjectFinalReportRepository _repository;
        private readonly IMapper _mapper;
        public GetProjectFinalReportByProjectId(IProjectFinalReportRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        #endregion Global Scope

        public async Task<IList<DetailedReadProjectFinalReportOutput>> ExecuteAsync(Guid? projectId)
        {
            UseCaseException.NotInformedParam(projectId is null, nameof(projectId));
            var entities = await _repository.GetByProjectIdAsync(projectId);
            return _mapper.Map<IList<DetailedReadProjectFinalReportOutput>>(entities);
        }
    }
}