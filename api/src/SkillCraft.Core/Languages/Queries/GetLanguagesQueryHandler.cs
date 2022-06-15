using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Languages.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Languages.Queries
{
  internal class GetLanguagesQueryHandler : IRequestHandler<GetLanguagesQuery, ListModel<LanguageModel>>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetLanguagesQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<ListModel<LanguageModel>> Handle(GetLanguagesQuery request, CancellationToken cancellationToken)
    {
      IQueryable<Language> query = _dbContext.Languages
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
          LanguageSort.Name => request.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          LanguageSort.UpdatedAt => request.Desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
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

      Language[] languages = await query.ToArrayAsync(cancellationToken);

      return new ListModel<LanguageModel>(
        _mapper.Map<IEnumerable<LanguageModel>>(languages),
        total
      );
    }
  }
}
