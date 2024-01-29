using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    public class AppException : ApplicationException
    {
        public string ErrorCode { get; private set; }
        public string ErrorField { get; private set; }
        public IReadOnlyDictionary<string, string> MetaData { get; private set; }

        public AppException(string errorField, string errorCode, Dictionary<string, string> metaData = null, Exception innerException = null)
            : base(GetErrorMessage(errorField, errorCode), innerException)
        {
            ErrorCode = errorCode;
            ErrorField = errorField;
            MetaData = metaData;
        }

        public AppException(string errorCode, Dictionary<string, string> metaData = null, Exception innerException = null)
            : base(GetErrorMessage(null, errorCode), innerException)
        {
            ErrorCode = errorCode;
            MetaData = metaData;
        }

        public override string ToString()
        {
            var message = GetErrorMessage(ErrorField, ErrorCode);

            if (MetaData?.Any() == true)
            {
                foreach (var m in MetaData)
                {
                    message += $"{Environment.NewLine}{m.Key}: {m.Value}";
                }
            }

            return message;
        }

        private static string GetErrorMessage(string errorField, string errorCode)
        {
            var message = ResourceKeyResolver.Resolve(errorCode);

            if (string.IsNullOrEmpty(message))
                message = errorCode;

            if (string.IsNullOrEmpty(errorField))
                return message;

            return string.Format(message, errorField);
        }
    }
}
