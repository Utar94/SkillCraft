using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Customizations.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Customizations.Queries
{
  internal class GetCustomizationsQueryHandler : IRequestHandler<GetCustomizationsQuery, ListModel<CustomizationModel>>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetCustomizationsQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<ListModel<CustomizationModel>> Handle(GetCustomizationsQuery request, CancellationToken cancellationToken)
    {
      IQueryable<Customization> query = _dbContext.Customizations
        .AsNoTracking()
        .Where(x => x.WorldId == _appContext.World.Id);

      if (request.Deleted.HasValue)
      {
        query = query.Where(x => x.Deleted == request.Deleted.Value);
      }
      if (request.Search != null)
      {
        throw new NotImplementedException(); // TODO(fpion): implement
      }
      if (request.Type.HasValue)
      {
        query = query.Where(x => x.Type == request.Type.Value);
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (request.Sort.HasValue)
      {
        query = request.Sort.Value switch
        {
          CustomizationSort.Name => request.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          CustomizationSort.UpdatedAt => request.Desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The customization sort \"{request.Sort}\" is not valid.", nameof(request)),
        };
      }

      if (request.Index.HasValue)
      {
        query = query.Skip(request.Index.Value);
      }
      if (request.Count.HasValue)
      {
        query = query.Take(request.Count.Value);
      }

      Customization[] customizations = await query.ToArrayAsync(cancellationToken);

      return new ListModel<CustomizationModel>(
        _mapper.Map<IEnumerable<CustomizationModel>>(customizations),
        total
      );
    }
  }
}
