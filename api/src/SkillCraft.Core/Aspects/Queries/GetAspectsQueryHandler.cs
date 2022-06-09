using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Aspects.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Aspects.Queries
{
  internal class GetAspectsQueryHandler : IRequestHandler<GetAspectsQuery, ListModel<AspectModel>>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAspectsQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<ListModel<AspectModel>> Handle(GetAspectsQuery request, CancellationToken cancellationToken)
    {
      IQueryable<Aspect> query = _dbContext.Aspects
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

      long total = await query.LongCountAsync(cancellationToken);

      if (request.Sort.HasValue)
      {
        query = request.Sort.Value switch
        {
          AspectSort.Name => request.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          AspectSort.UpdatedAt => request.Desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The aspect sort \"{request.Sort}\" is not valid.", nameof(request)),
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

      Aspect[] aspects = await query.ToArrayAsync(cancellationToken);

      return new ListModel<AspectModel>(
        _mapper.Map<IEnumerable<AspectModel>>(aspects),
        total
      );
    }
  }
}
