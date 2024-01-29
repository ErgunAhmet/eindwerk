using Shared.Extensions;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.MongoDB
{
    public static class MongoDBFilter
    {
        public static Expression<Func<TSource, bool>> CheckIfDatePersiodIsActive<TSource>(Expression<Func<TSource, DateTime>> selectorStart, Expression<Func<TSource, DateTime?>> selectorEnd, DateTime? checkDate = null)
        {
            var argument = Expression.Parameter(typeof(TSource));

            var startName = GetPropertyName(selectorStart);
            var startDateProperty = Expression.Property(argument, startName);

            var endName = GetPropertyName(selectorEnd);
            var endDateProperty = Expression.Property(argument, endName);

            var date = checkDate ?? DateTime.UtcNow;
            var dateConst = Expression.Constant(date);
            var dateNullConst = Expression.Constant(null, typeof(DateTime?));
            var dateNullableConst = Expression.Constant((DateTime?)date, typeof(DateTime?));

            var startDateExpression = Expression.Lambda<Func<TSource, bool>>(Expression.LessThanOrEqual(startDateProperty, dateConst), new[] { argument });

            var endDateNullExpression = Expression.Lambda<Func<TSource, bool>>(Expression.Equal(endDateProperty, dateNullConst), new[] { argument });

            var endDateValueExpression = Expression.Lambda<Func<TSource, bool>>(Expression.GreaterThanOrEqual(endDateProperty, dateNullableConst), new[] { argument });

            var endDateExpression = endDateNullExpression.Or(endDateValueExpression);

            var totalExpression = startDateExpression.And(endDateExpression);

            return RenameParameters(totalExpression);
        }

        public static Expression<Func<TSource, bool>> CheckIfDatePersiodIsInActive<TSource>(Expression<Func<TSource, DateTime>> selectorStart, Expression<Func<TSource, DateTime?>> selectorEnd, DateTime? checkDate = null)
        {
            var argument = Expression.Parameter(typeof(TSource));

            var startName = GetPropertyName(selectorStart);
            var startDateProperty = Expression.Property(argument, startName);

            var endName = GetPropertyName(selectorEnd);
            var endDateProperty = Expression.Property(argument, endName);

            var date = checkDate ?? DateTime.UtcNow;
            var dateConst = Expression.Constant(date);
            var dateNullConst = Expression.Constant(null, typeof(DateTime?));
            var dateNullableConst = Expression.Constant((DateTime?)date, typeof(DateTime?));

            var startDateExpression = Expression.Lambda<Func<TSource, bool>>(Expression.GreaterThan(startDateProperty, dateConst), new[] { argument });

            var endDateNullExpression = Expression.Lambda<Func<TSource, bool>>(Expression.NotEqual(endDateProperty, dateNullConst), new[] { argument });

            var endDateValueExpression = Expression.Lambda<Func<TSource, bool>>(Expression.LessThan(endDateProperty, dateNullableConst), new[] { argument });

            var endDateExpression = endDateNullExpression.And(endDateValueExpression);

            var totalExpression = startDateExpression.Or(endDateExpression);

            return RenameParameters(totalExpression);
        }

        private static string GetPropertyName<TSource, TDestination>(Expression<Func<TSource, TDestination>> selector)
        {
            MemberExpression foundMemberExpression = null;

            if (selector.Body is MemberExpression memberExpression)
                foundMemberExpression = memberExpression;

            if (selector.Body is UnaryExpression unaryExpression)
                foundMemberExpression = (MemberExpression)unaryExpression.Operand;

            if (foundMemberExpression == null)
                throw new NotSupportedException();

            var propertyinfo = (PropertyInfo)foundMemberExpression.Member;
            return propertyinfo.Name;
        }

        private static Expression<Func<TSource, bool>> RenameParameters<TSource>(Expression<Func<TSource, bool>> originalExpression)
        {
            Expression<Func<TSource, bool>> renameParameterExpression = x => true;

            var map = renameParameterExpression.Parameters
               .Select((f, i) => new { f, s = originalExpression.Parameters[i] })
               .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first
            var secondBody = ParameterRebinderVisitor.ReplaceParameters(map, originalExpression.Body);

            return Expression.Lambda<Func<TSource, bool>>(secondBody, renameParameterExpression.Parameters);
        }
    }
}
