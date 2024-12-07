using Market.Constants;

namespace Market.Models
{
    public class Response<T>
    {
        public int Code { get; private set; }
        public string Message { get; private set; }
        public T Result { get; private set; }

        private Response(T result, int code, string message)
        {
            Result = result;
            Code = code;
            Message = message;
        }

        private Response(T result, ResultCode resultCode)
        {
            Result = result;
            Message = resultCode.Name;
            Code = resultCode.Code;
        }

        private Response(T result, AuthCode authCode)
        {
            Result = result;
            Message = authCode.Name;
            Code = authCode.Code;
        }

        public static Response<string> Ok()
        {
            return new Response<string>(string.Empty, ResultCode.Success);
        }

        public static Response<T> Ok(T data)
        {
            return new Response<T>(data, ResultCode.Success);
        }

        public static Response<string> Fail(ResultCode resultCode)
        {
            return new Response<string>(string.Empty, resultCode);
        }

        public static Response<string> Fail(ResultCode resultCode, string message)
        {
            return new Response<string>(string.Empty, resultCode.Code, message);
        }

        public static Response<string> Fail(AuthCode authCode)
        {
            return new Response<string>(string.Empty, authCode);
        }

        public static Response<string> Fail(AuthCode authCode, string message)
        {
            return new Response<string>(string.Empty, authCode.Code, message);
        }
    }
}