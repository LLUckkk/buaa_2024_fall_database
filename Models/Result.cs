using Market.Constants;

namespace Market.Models
{
    public class Empty { }
    public class Result : Result<Empty>
    {
        public static Result Ok() => new Result();
        public static Result Fail(ResultCode resultCode) => new Result { Code = resultCode.Code, Message = resultCode.Name };
        public static Result Fail(ResultCode resultCode, string message) => new Result { Code = resultCode.Code, Message = message };
    }

    public class Result<T>
    {
        public int Code { get; protected set; }
        public string Message { get; protected set; }
        public T? Data { get; protected set; }

        public Result()
        {
        }

        private Result(ResultCode resultCode)
        {
            Code = resultCode.Code;
            Message = resultCode.Name;
        }

        private Result(int code, string message)
        {
            Code = code;
            Message = message;
        }

        private Result(T result, ResultCode resultCode)
        {
            Data = result;
            Message = resultCode.Name;
            Code = resultCode.Code;
        }

        private Result(T result, AuthCode authCode)
        {
            Data = result;
            Message = authCode.Name;
            Code = authCode.Code;
        }

        public static Result<T> Ok(T data)
        {
            return new Result<T>(data, ResultCode.Success);
        }

        public static Result<T> Fail(ResultCode resultCode)
        {
            return new Result<T>(resultCode);
        }

        public static Result<T> Fail(ResultCode resultCode, string message)
        {
            return new Result<T>(resultCode.Code, message);
        }


        public static Result<T> Fail(AuthCode authCode, string message)
        {
            return new Result<T>(authCode.Code, message);
        }
    }
}