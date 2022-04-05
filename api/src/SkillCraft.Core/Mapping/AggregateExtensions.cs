using AutoMapper;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Mapping
{
  internal static class AggregateExtensions
  {
    internal static IMappingExpression<TEntity, TModel> MapAggregate<TEntity, TModel>(
      this IMappingExpression<TEntity, TModel> expression
    ) where TEntity : Aggregate where TModel : AggregateModel
    {
      return expression.ForMember(x => x.Id, x => x.MapFrom(y => y.Key));
    }
  }
}
