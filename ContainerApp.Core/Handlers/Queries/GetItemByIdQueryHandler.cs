using AutoMapper;
using ContainerApp.Contracts.Data;
using ContainerApp.Contracts.DTO;
using ContainerApp.Contracts.Services;
using ContainerApp.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ContainerApp.Core.Handlers.Queries
{
    public class GetItemByIdQuery : IRequest<ItemDTO>
    {
        public int ItemId { get; }
        public GetItemByIdQuery(int id)
        {
            ItemId = id;
        }
    }

    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemDTO>
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;
        private readonly ICachingService _cache;
        private readonly ILogger<GetItemByIdQueryHandler> _logger;

        public GetItemByIdQueryHandler(ILogger<GetItemByIdQueryHandler> logger, IUnitOfWork repository, IMapper mapper, ICachingService cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        public async Task<ItemDTO> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var cachedItem = _cache.GetItem<ItemDTO>($"item_{request.ItemId}");
            if (cachedItem != null)
            {
                _logger.LogInformation($"Item Exists in Cache. Return CachedItem.");
                return cachedItem;
            }

            _logger.LogInformation($"Item doesn't exist in Cache.");

            var item = await Task.FromResult(_repository.Items.Get(request.ItemId));
            if (item == null)
            {
                throw new EntityNotFoundException($"No Item found for Id {request.ItemId}");
            }

            var result = _mapper.Map<ItemDTO>(item);

            _logger.LogInformation($"Add Item to Cache and return.");
            var _ = _cache.SetItem($"item_{request.ItemId}", result);
            return result;
        }
    }
}