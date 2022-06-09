using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Customizations.Models;

namespace SkillCraft.Core.Customizations.Mutations
{
  internal class DeleteCustomizationMutationHandler : IRequestHandler<DeleteCustomizationMutation, CustomizationModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public DeleteCustomizationMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<CustomizationModel> Handle(DeleteCustomizationMutation request, CancellationToken cancellationToken)
    {
      Customization customization = await _dbContext.Customizations
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Customization>(request.Id);

      if (customization.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Customization>(customization, _appContext.UserId, _appContext.World);
      }

      customization.Delete(_appContext.UserId);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return _mapper.Map<CustomizationModel>(customization);
    }
  }
}
