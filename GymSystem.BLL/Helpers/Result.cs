using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Helpers
{
    public sealed record Result(bool success, string? errorMessage = null, ResultType rType = ResultType.Ok )
    {
        public static Result OK()=> new Result(true);
        public static Result NotFound(string error = "Not Found" , ResultType type = ResultType.NotFound) => new Result(false,error,type );
        public static Result Fail(string error , ResultType type = ResultType.Conflict) => new Result(false,error,type );
        public static Result Validation(string error , ResultType type = ResultType.Validation) => new Result(false,error,type );
    }

    public sealed record Result<T>(bool success,T? Value , string? errorMessage = null, ResultType rType = ResultType.Ok)
    {
        public static Result<T> OK(T value) => new Result<T>(true , value);
        public static Result<T> NotFound(string error = "Not Found", ResultType type = ResultType.NotFound) 
        { 
            return new Result<T>(false, default ,error, type);
        }
        public static Result<T> Fail(string error, ResultType type = ResultType.Conflict) => new Result<T>(false,default ,error, type);
    }

    public enum ResultType
    {
        Ok,
        NotFound,
        Conflict,
        Validation,
        Forbidden
    }
}
