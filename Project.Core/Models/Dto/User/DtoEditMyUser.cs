using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace Project.Core.Models.Dto.User
{
    public class DtoEditMyUser
    {
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

        [JsonProperty(PropertyName = "avatarFile")]
        public IFormFile AvatarFile { get; set; }


    }
}
