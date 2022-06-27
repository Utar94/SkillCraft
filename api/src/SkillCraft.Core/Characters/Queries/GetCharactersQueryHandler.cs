using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Characters.Queries
{
  internal class GetCharactersQueryHandler : IRequestHandler<GetCharactersQuery, ListModel<CharacterModel>>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetCharactersQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<ListModel<CharacterModel>> Handle(GetCharactersQuery request, CancellationToken cancellationToken)
    {
      IQueryable<Character> query = _dbContext.Characters
        .AsNoTracking()
        .Include(x => x.Race)
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
          CharacterSort.Name => request.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          CharacterSort.Player => request.Desc ? query.OrderByDescending(x => x.Player) : query.OrderBy(x => x.Player),
          CharacterSort.UpdatedAt => request.Desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The character sort \"{request.Sort}\" is not valid.", nameof(request)),
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

      Character[] characters = await query.ToArrayAsync(cancellationToken);

      return new ListModel<CharacterModel>(
        _mapper.Map<IEnumerable<CharacterModel>>(characters),
        total
      );
    }
  }
}
