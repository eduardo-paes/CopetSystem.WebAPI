using Domain.Contracts.SubArea;
using Domain.Interfaces.UseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;
using System.Threading.Tasks;
using System;

namespace Domain.UseCases
{
    public class CreateSubArea : ICreateSubArea
    {
        #region Global Scope
        private readonly ISubAreaRepository _subAreaRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly IMapper _mapper;
        public CreateSubArea(ISubAreaRepository subAreaRepository, IAreaRepository areaRepository, IMapper mapper)
        {
            _subAreaRepository = subAreaRepository;
            _areaRepository = areaRepository;
            _mapper = mapper;
        }
        #endregion

        public async Task<DetailedReadSubAreaOutput> Execute(CreateSubAreaInput input)
        {
            var entity = await _subAreaRepository.GetByCode(input.Code);
            if (entity != null)
                throw new Exception($"Já existe uma Subárea para o código {input.Code}");

            // Verifica id da área
            if (input.AreaId == null)
                throw new Exception("O Id da Área não pode ser vazio.");

            // Valida se existe área
            var area = await _areaRepository.GetById(input.AreaId);

            // Verifica se Área existe
            if (area == null)
                throw new Exception("A Área informada não existe.");

            if (area.DeletedAt != null)
                throw new Exception("A Área informada está inativa.");

            // Cria nova área
            entity = await _subAreaRepository.Create(_mapper.Map<Domain.Entities.SubArea>(input));
            return _mapper.Map<DetailedReadSubAreaOutput>(entity);
        }
    }
}