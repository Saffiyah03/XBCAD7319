﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace MVCAPP.Models
{
    public class UserLogin{
        [Key]
        public string Email {get; set;}
        [Required]
        public string Password {get; set;}
    }
}