using AutoMapper;
using Logitar.Identity.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Models;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Worlds.Queries
{
  internal class GetWorldsQueryHandler : IRequestHandler<GetWorldsQuery, ListModel<WorldModel>>
  {
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public GetWorldsQueryHandler(IDbContext dbContext, IMapper mapper, IUserContext userContext)
    {
      _dbContext = dbContext;
      _mapper = mapper;
      _userContext = userContext;
    }

    public async Task<ListModel<WorldModel>> Handle(GetWorldsQuery request, CancellationToken cancellationToken)
    {
      IQueryable<World> query = _dbContext.Worlds
        .AsNoTracking()
        .Where(x => x.CreatedById == _userContext.Id);

      if (request.Search != null)
      {
        throw new NotImplementedException(); // TODO(fpion): implement
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (request.Sort.HasValue)
      {
        query = request.Sort.Value switch
        {
          WorldSort.Alias => request.Desc ? query.OrderByDescending(x => x.Alias) : query.OrderBy(x => x.Alias),
          WorldSort.Name => request.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          WorldSort.UpdatedAt => request.Desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The world sort \"{request.Sort}\" is not valid.", nameof(request)),
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

      World[] worlds = await query.ToArrayAsync(cancellationToken);

      return new ListModel<WorldModel>(
        _mapper.Map<IEnumerable<WorldModel>>(worlds),
        total
      );
    }
  }
}
