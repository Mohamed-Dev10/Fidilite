using System;
using System.Collections.Generic;
using System.Text;

namespace BRICOMA.ECOMMERCE.Models.Helpers
{
    public class RESTServiceResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public RESTServiceResponse()
        {

        }

        public RESTServiceResponse(T data)
        {
            this.Data = data;
        }

        public RESTServiceResponse(bool success, T data)
        {
            this.Success = success;
            this.Data = data;
        }

        public RESTServiceResponse(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }

        public RESTServiceResponse(bool success, string message, T data)
        {
            this.Success = success;
            this.Message = message;
            this.Data = data;
        }
    }
}

