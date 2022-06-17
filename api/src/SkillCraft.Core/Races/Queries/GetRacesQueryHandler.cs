using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Models;
using SkillCraft.Core.Races.Models;

namespace SkillCraft.Core.Races.Queries
{
  internal class GetRacesQueryHandler : IRequestHandler<GetRacesQuery, ListModel<RaceModel>>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetRacesQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<ListModel<RaceModel>> Handle(GetRacesQuery request, CancellationToken cancellationToken)
    {
      IQueryable<Race> query = _dbContext.Races
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
          RaceSort.Name => request.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          RaceSort.UpdatedAt => request.Desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The race sort \"{request.Sort}\" is not valid.", nameof(request)),
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

      Race[] races = await query.ToArrayAsync(cancellationToken);

      return new ListModel<RaceModel>(
        _mapper.Map<IEnumerable<RaceModel>>(races),
        total
      );
    }
  }
}
