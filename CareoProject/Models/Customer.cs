using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CareoProject.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required,MaxLength(150)]
        public string Fullname { get; set; }
        [Required,MaxLength(300)]
        public string SpecialtyName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageUrl { get; set; }
    }
}
