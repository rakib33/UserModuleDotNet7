﻿using Newtonsoft.Json;

namespace UserManagementCore.Models
{
    public class ErrorResponseModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }

        // other fields

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
