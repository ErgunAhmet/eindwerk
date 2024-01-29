using Shared.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Validation
{
    public static class Guard
    {
        public static void ThatIsFalse(bool condition, string errorField, string errorCode, Dictionary<string, string> metaData = null)
        {
            ThatIsTrue(!condition, errorField, errorCode, metaData);
        }

        public static void ThatIsFalse(bool condition, string errorCode, Dictionary<string, string> metaData = null)
        {
            ThatIsTrue(!condition, errorCode, metaData);
        }

        public static void ThatIsTrue(bool condition, string errorField, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (!condition)
                throw new AppException(errorField, errorCode, metaData);
        }

        public static void ThatIsTrue(bool condition, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (!condition)
                throw new AppException(errorCode, metaData);
        }

        public static void AgainstNull<T>(T value, string errorField, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (value == null)
                throw new AppException(errorField, errorCode, metaData);
        }

        public static void AgainstNull<T>(T value, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (value == null)
                throw new AppException(errorCode, metaData);
        }

        public static void ThatIsNull<T>(T value, string errorField, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (value != null)
                throw new AppException(errorField, errorCode, metaData);
        }

        public static void ThatIsNull<T>(T value, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (value != null)
                throw new AppException(errorCode, metaData);
        }

        public static void AgainstNullOrEmpty(string value, string errorField, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (string.IsNullOrEmpty(value))
                throw new AppException(errorField, errorCode, metaData);
        }

        public static void AgainstNullOrEmpty(string value, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (string.IsNullOrEmpty(value))
                throw new AppException(errorCode, metaData);
        }

        public static void ThatIsNullOrEmpty(string value, string errorField, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (!string.IsNullOrEmpty(value))
                throw new AppException(errorField, errorCode, metaData);
        }

        public static void ThatIsNullOrEmpty(string value, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (!string.IsNullOrEmpty(value))
                throw new AppException(errorCode, metaData);
        }

        public static void IsEqual<T>(T value, T origin, string errorField, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (!EqualityComparer<T>.Default.Equals(value, origin))
                throw new AppException(errorField, errorCode, metaData);
        }

        public static void IsEqual<T>(T value, T origin, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (!EqualityComparer<T>.Default.Equals(value, origin))
                throw new AppException(errorCode, metaData);
        }

        public static void IsNotDefault<T>(T value, string errorField, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (EqualityComparer<T>.Default.Equals(value, default))
                throw new AppException(errorField, errorCode, metaData);
        }

        public static void IsNotDefault<T>(T value, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (EqualityComparer<T>.Default.Equals(value, default))
                throw new AppException(errorCode, metaData);
        }

        public static void HasItems(IList list, string errorField, string errorCode, Dictionary<string, string> metaData = null)
        {
            AgainstNull(list, errorField, errorCode, metaData);

            if (list.Count == 0)
                throw new AppException(errorField, errorCode, metaData);
        }

        public static void HasItems(IList list, string errorCode, Dictionary<string, string> metaData = null)
        {
            AgainstNull(list, errorCode, metaData);

            if (list.Count == 0)
                throw new AppException(errorCode, metaData);
        }

        public static void HasNoItems(IList list, string errorField, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (list?.Count > 0)
                throw new AppException(errorField, errorCode, metaData);
        }

        public static void HasNoItems(IList list, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (list?.Count > 0)
                throw new AppException(errorCode, metaData);
        }

        public static void ThatIsValidDateTime(DateTime value, string errorField, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (value == default || value.Kind != DateTimeKind.Utc)
                throw new AppException(errorField, errorCode, metaData);
        }

        public static void ThatIsValidDateTime(DateTime value, string errorCode, Dictionary<string, string> metaData = null)
        {
            if (value == default || value.Kind != DateTimeKind.Utc)
                throw new AppException(errorCode, metaData);
        }
    }
}
