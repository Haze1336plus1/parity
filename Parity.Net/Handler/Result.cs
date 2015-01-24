using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Net.Handler
{
    public class Result
    {

        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }

        public static readonly Result Default;
        public static readonly Result Success;
        static Result()
        {
            Result.Default = new Result();
            Result.Default.IsSuccess = false;
            Result.Success = new Result();
        }

        public Result()
        {
            this.IsSuccess = true;
            this.Message = string.Empty;
        }
        public Result(string message)
        {
            this.IsSuccess = false;
            this.Message = message;
        }

    }
}
