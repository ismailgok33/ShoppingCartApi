using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Utility
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ApiName { get; set; }
        public string ApiVersion { get; set; }
        public string ApiBaseUrl { get; set; }
        public string OidcSwaggerUIClientId { get; set; }
        public bool RequireHttpsMetadata { get; set; }
        public string OidcApiName { get; set; }
        public string AdministrationRole { get; set; }
        public bool CorsAllowAnyOrigin { get; set; }
        public string[] CorsAllowOrigins { get; set; }
        public string RedisServer { get; set; }
        public string RedisPrefix { get; set; }
        public string AuthBaseUrl { get; set; }
    }
}
