using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace Project.Core.Models.Dto.User
{
    public class DtoEditUser
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "newPassword")]
        public string NewPassword { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string Firstname { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string Lastmame { get; set; }

        [JsonProperty(PropertyName = "mail")]
        public string Mail { get; set; }

        [JsonProperty(PropertyName = "aboume")]
        public string Aboutme { get; set; }
    }
}
