using AutoMapper;
using Domain.Interfaces.Repositories;
using Application.Interfaces.UseCases.AssistanceType;
using Application.Ports.AssistanceType;

namespace Application.UseCases.AssistanceType
{
    public class GetAssistanceTypes : IGetAssistanceTypes
    {
        #region Global Scope
        private readonly IAssistanceTypeRepository _repository;
        private readonly IMapper _mapper;
        public GetAssistanceTypes(IAssistanceTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        #endregion Global Scope

        public async Task<IQueryable<ResumedReadAssistanceTypeOutput>> ExecuteAsync(int skip, int take)
        {
            // Valida valores de skip e take
            if (skip < 0 || take < 1)
                throw new ArgumentException("Parâmetros inválidos.");

            var entities = await _repository.GetAllAsync(skip, take);
            return _mapper.Map<IEnumerable<ResumedReadAssistanceTypeOutput>>(entities).AsQueryable();
        }
    }
}