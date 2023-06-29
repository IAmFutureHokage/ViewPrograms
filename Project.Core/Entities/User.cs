using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Core.Entities
{

    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(255)]
        public string Login { get; set; }
        [MaxLength(255)]
        public string Firstname { get; set; }
        [MaxLength(255)]
        public string Lastname { get; set; }
        [MaxLength(255)]
        public string Mail { get; set; }
        [MaxLength(50)]
        #nullable enable
        public string? Aboutme { get; set; }
        [MaxLength(400)]
        public string? Avatar { get; set; }
        [MaxLength(255)]
        #nullable disable
        public string Role { get; set; } = "user";
        public string PasswordHash { get; set; }
        public Boolean Deleted { get; set; }

    }

}
