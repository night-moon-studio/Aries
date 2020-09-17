using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Mvc
{
    public struct ApiReturnResult
    {
        /// <summary>
        /// 消息
        /// </summary>
        [JsonPropertyName("msg")]
        public string Msg { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        //public string Description { get; set; }
        /// <summary>
        /// 返回对象
        /// </summary>
        [JsonPropertyName("data")]
        public object Data { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }
}
