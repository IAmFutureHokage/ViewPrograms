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
        public string Password { get; set; }
        public string Role { get; set; }
        public string PasswordHash { get; set; }
        public Boolean Deleted { get; set; }

    }
}
