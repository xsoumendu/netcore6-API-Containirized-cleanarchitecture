using AutoMapper;
using ContainerApp.Contracts.Data;
using ContainerApp.Contracts.DTO;
using MediatR;

namespace ContainerApp.Core.Handlers.Queries
{
    public class GetAllItemsQuery : IRequest<IEnumerable<ItemDTO>>
    {
    }

    public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, IEnumerable<ItemDTO>>
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;
        /*
         * We can try to Inject Distributed Cache here to Improve e-Tag Performance
         * 
         * 
         */
        public GetAllItemsQueryHandler(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemDTO>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        {

            var entities = await Task.FromResult(_repository.Items.GetAll());
            var result = _mapper.Map<IEnumerable<ItemDTO>>(entities);


            return result;
        }
    }
}