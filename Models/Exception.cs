
using Market.Constants;

namespace Market.Models
{
    public class ServiceException : Exception
    {
        public ResultCode ResultCode { get; }

        public ServiceException(ResultCode resultCode) : base(resultCode.Name)
        {
            ResultCode = resultCode;
        }

        public ServiceException(ResultCode resultCode, string message) : base(message)
        {
            resultCode.Name = message;
            ResultCode = resultCode;
        }
    }
}