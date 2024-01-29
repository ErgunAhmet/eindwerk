using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Tools
{
    internal class ExpressionToStringVisitor : ExpressionVisitor
    {
        private List<KeyValuePair<string, string>> _values = new List<KeyValuePair<string, string>>();

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is MemberExpression me)
                HandleMemberExpression(node, me);
            else if (node.Expression is ConstantExpression ce)
                HandleConstantExpression(node, ce);
            else if (node.Expression == null && node.Member != null)
                HandleStaticMember(node.Member);

            return base.VisitMember(node);
        }

        private void HandleStaticMember(MemberInfo memberInfo)
        {
            object value = null;

            var name = memberInfo.DeclaringType.Name + "." + memberInfo.Name;

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    value = ((FieldInfo)memberInfo).GetValue(null);
                    break;
                case MemberTypes.Property:
                    value = ((PropertyInfo)memberInfo).GetValue(null);
                    break;
            }

            TryAddValue(name, value);
        }

        private void HandleConstantExpression(MemberExpression node, ConstantExpression ce)
        {
            var type = ce.Value.GetType();
            var field = type.GetField(node.Member.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var value = field.GetValue(ce.Value);

            TryAddValue(node.ToString(), value);
        }

        private void HandleMemberExpression(MemberExpression node, MemberExpression me)
        {
            var objectMember = Expression.Convert(me, typeof(object));

            var getterLambda = Expression.Lambda<Func<object>>(objectMember);

            object obj = null;

            try
            {
                obj = getterLambda.Compile().Invoke();
            }
            catch (InvalidOperationException)
            {
                return;
            }

            var property = obj.GetType().GetProperty(node.Member.Name);

            if (property == null)
                return;

            var member = property.GetValue(obj);

            TryAddValue(node.ToString(), member);
        }

        private void TryAddValue(string key, object value)
        {
            if (IsAllowedToStringType(value?.GetType()))
                _values.Add(KeyValuePair.Create(key.ToString(), value?.ToString() ?? "null"));
            else if (value is IEnumerable values)
            {
                var stringValue = ConvertIEnumerableToString(values);
                _values.Add(KeyValuePair.Create(key.ToString(), stringValue ?? "null"));
            }
        }

        private string ConvertIEnumerableToString(IEnumerable values)
        {
            var stringBuilder = new StringBuilder();

            var type = values.GetType();
            stringBuilder.Append($"{type.Name}<{type.GenericTypeArguments[0].FullName}>(");

            foreach (var v in values)
            {
                stringBuilder.Append(v?.ToString());
                stringBuilder.Append(", ");
            }

            //Manually remove last ", " from value list string because check for last index not possible on non-generic IEnumerable
            stringBuilder.Remove(stringBuilder.Length - 2, 2);

            stringBuilder.Append(")");

            return stringBuilder.ToString();
        }

        private bool IsAllowedToStringType(Type type)
        {
            if (type == null)
                return true;

            if (type.IsPrimitive)
                return true;

            if (type.IsEnum)
                return true;

            if (type == typeof(string))
                return true;

            if (type == typeof(Guid))
                return true;

            if (type == typeof(DateTime))
                return true;

            return false;
        }

        public string ToString(Expression node)
        {
            var expressionString = node.ToString();

            Visit(node);

            if (_values.Any())
            {
                foreach (var value in _values)
                {
                    int pos = expressionString.IndexOf(value.Key);

                    if (pos < 0)
                        continue;

                    expressionString = expressionString.Remove(pos, value.Key.Length).Insert(pos, value.Value);
                }

                _values.Clear();
            }

            return expressionString;
        }
    }
}
