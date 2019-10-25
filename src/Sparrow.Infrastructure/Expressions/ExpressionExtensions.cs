using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sparrow.Infrastructure.Expressions
{
    public static class ExpressionExtensions
    {
        public static Expression<T> Compose<T>(this Expression<T> left, Expression<T> right,
            Func<Expression, Expression, Expression> merge)
        {
            var map = left.Parameters
                .Select((f, i) => new { f, s = right.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            var rightBody = ParameterRebinder.ReplaceParameters(map, right.Body);
            return Expression.Lambda<T>(merge(left.Body, rightBody), left.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            return left.Compose(right, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            return left.Compose(right, Expression.OrElse);
        }

        private class ParameterRebinder : ExpressionVisitor
        {
            private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

            private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
                Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (_map.TryGetValue(node, out ParameterExpression replacement))
                    node = replacement;

                return base.VisitParameter(node);
            }
        }
    }
}
