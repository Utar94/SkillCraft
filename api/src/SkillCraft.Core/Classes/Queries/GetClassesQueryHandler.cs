using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Classes.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Classes.Queries
{
  internal class GetClassesQueryHandler : IRequestHandler<GetClassesQuery, ListModel<ClassModel>>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetClassesQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<ListModel<ClassModel>> Handle(GetClassesQuery request, CancellationToken cancellationToken)
    {
      IQueryable<Class> query = _dbContext.Classes
        .AsNoTracking()
        .Include(x => x.UniqueTalent)
        .Where(x => x.WorldId == _appContext.World.Id);

      if (request.Deleted.HasValue)
      {
        query = query.Where(x => x.Deleted == request.Deleted.Value);
      }
      if (request.Search != null)
      {
        throw new NotImplementedException(); // TODO(fpion): implement
      }
      if (request.Tiers != null)
      {
        query = query.Where(x => request.Tiers.Contains(x.Tier));
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (request.Sort.HasValue)
      {
        query = request.Sort.Value switch
        {
          ClassSort.Name => request.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          ClassSort.UpdatedAt => request.Desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The class sort \"{request.Sort}\" is not valid.", nameof(request)),
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

      Class[] classes = await query.ToArrayAsync(cancellationToken);

      return new ListModel<ClassModel>(
        _mapper.Map<IEnumerable<ClassModel>>(classes),
        total
      );
    }
  }
}
