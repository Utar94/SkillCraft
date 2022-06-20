using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Conditions.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Conditions.Queries
{
  internal class GetConditionsQueryHandler : IRequestHandler<GetConditionsQuery, ListModel<ConditionModel>>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetConditionsQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<ListModel<ConditionModel>> Handle(GetConditionsQuery request, CancellationToken cancellationToken)
    {
      IQueryable<Condition> query = _dbContext.Conditions
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
          ConditionSort.MaxLevel => request.Desc ? query.OrderByDescending(x => x.MaxLevel) : query.OrderBy(x => x.MaxLevel),
          ConditionSort.Name => request.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          ConditionSort.UpdatedAt => request.Desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The condition sort \"{request.Sort}\" is not valid.", nameof(request)),
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

      Condition[] conditions = await query.ToArrayAsync(cancellationToken);

      return new ListModel<ConditionModel>(
        _mapper.Map<IEnumerable<ConditionModel>>(conditions),
        total
      );
    }
  }
}
