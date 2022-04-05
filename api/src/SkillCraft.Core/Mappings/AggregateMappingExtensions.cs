using AutoMapper;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Mappings
{
  internal static class AggregateMappingExtensions
  {
    internal static IMappingExpression<TEntity, TModel> MapAggregate<TEntity, TModel>(
      this IMappingExpression<TEntity, TModel> expression
    )
      where TEntity : Aggregate
      where TModel : AggregateModel
    {
      ArgumentNullException.ThrowIfNull(expression);

      return expression.ForMember(x => x.Id, x => x.MapFrom(y => y.Key));
    }
  }
}
