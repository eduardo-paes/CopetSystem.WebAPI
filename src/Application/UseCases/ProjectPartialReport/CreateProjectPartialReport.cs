using AutoMapper;
using Domain.Interfaces.Repositories;
using Application.Ports.ProjectPartialReport;
using Application.Interfaces.UseCases.ProjectPartialReport;
using Application.Validation;
using Domain.Entities.Enums;
using Domain.Interfaces.Services;

namespace Application.UseCases.ProjectPartialReport
{
    public class CreateProjectPartialReport : ICreateProjectPartialReport
    {
        #region Global Scope
        private readonly IProjectPartialReportRepository _projectReportRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITokenAuthenticationService _tokenAuthenticationService;
        private readonly IMapper _mapper;
        public CreateProjectPartialReport(IProjectPartialReportRepository projectReportRepository,
            IProjectRepository projectRepository,
            ITokenAuthenticationService tokenAuthenticationService,
            IMapper mapper)
        {
            _projectReportRepository = projectReportRepository;
            _projectRepository = projectRepository;
            _tokenAuthenticationService = tokenAuthenticationService;
            _mapper = mapper;
        }
        #endregion Global Scope

        public async Task<DetailedReadProjectPartialReportOutput> ExecuteAsync(CreateProjectPartialReportInput input)
        {
            // Obtém usuário logado
            var user = _tokenAuthenticationService.GetUserAuthenticatedClaims();

            // Cria entidade a partir do modelo
            Domain.Entities.ProjectPartialReport report = new(
                input.ProjectId,
                input.CurrentDevelopmentStage,
                TryCastEnum<EScholarPerformance>(input.ScholarPerformance),
                input.AdditionalInfo,
                user.Id
            );

            // Verifica se o projeto existe
            Domain.Entities.Project project = await _projectRepository.GetByIdAsync(report.ProjectId)
                ?? throw UseCaseException.NotFoundEntityById(nameof(Domain.Entities.Project));

            // Verifica se o projeto foi excluído
            UseCaseException.BusinessRuleViolation(project.DeletedAt != null,
                "O projeto informado já foi removido.");

            // Verifica se o projeto está em andamento
            UseCaseException.BusinessRuleViolation(project.Status != EProjectStatus.Started,
                "O projeto informado não está em andamento.");

            // Somente aluno ou professor do projeto pode fazer inclusão de relatório
            UseCaseException.BusinessRuleViolation(user.Id != project.StudentId && user.Id != project.ProfessorId,
                "Somente o aluno ou o professor orientador do projeto pode fazer inclusão de relatório.");

            // Verifica se o relatório está sendo enviado dentro do prazo
            var isBeforeDeadline = project.Notice?.PartialReportDeadline < DateTime.UtcNow;

            // Lança exceção caso o relatório esteja sendo enviado fora do prazo
            UseCaseException.BusinessRuleViolation(isBeforeDeadline,
                "Relatório enviado fora do prazo estipulado no edital.");

            // Cria entidade
            report = await _projectReportRepository.CreateAsync(report);

            // Salva entidade no banco
            return _mapper.Map<DetailedReadProjectPartialReportOutput>(report);
        }

        /// <summary>
        /// Tenta converter um objeto para um tipo Enum.
        /// </summary>
        /// <param name="value">Valor a ser convertido.</param>
        /// <typeparam name="T">Tipo para o qual ser convertido.</typeparam>
        /// <returns>Objeto com tipo convertido.</returns>
        private static T TryCastEnum<T>(object? value)
        {
            try
            {
                UseCaseException.NotInformedParam(value is null, typeof(T).ToString());
                return (T)Enum.Parse(typeof(T), value?.ToString()!);
            }
            catch (Exception)
            {
                throw UseCaseException.BusinessRuleViolation($"Não foi possível converter o valor {value} para o tipo {typeof(T)}.");
            }
        }
    }
}