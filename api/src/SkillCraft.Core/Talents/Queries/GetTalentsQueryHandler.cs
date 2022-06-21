using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Models;
using SkillCraft.Core.Talents.Models;

namespace SkillCraft.Core.Talents.Queries
{
  internal class GetTalentsQueryHandler : IRequestHandler<GetTalentsQuery, ListModel<TalentModel>>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetTalentsQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<ListModel<TalentModel>> Handle(GetTalentsQuery request, CancellationToken cancellationToken)
    {
      IQueryable<Talent> query = _dbContext.Talents
        .AsNoTracking()
        .Include(x => x.RequiredTalent)
        .Where(x => x.WorldId == _appContext.World.Id);

      if (request.Deleted.HasValue)
      {
        query = query.Where(x => x.Deleted == request.Deleted.Value);
      }
      if (request.MultipleAcquisition.HasValue)
      {
        query = query.Where(x => x.MultipleAcquisition == request.MultipleAcquisition.Value);
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
          TalentSort.Name => request.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          TalentSort.UpdatedAt => request.Desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The talent sort \"{request.Sort}\" is not valid.", nameof(request)),
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

      Talent[] talents = await query.ToArrayAsync(cancellationToken);

      return new ListModel<TalentModel>(
        _mapper.Map<IEnumerable<TalentModel>>(talents),
        total
      );
    }
  }
}
