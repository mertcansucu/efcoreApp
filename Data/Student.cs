using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace efcoreApp.Data
{
    public class Student
    {
        //Veritanında ki kolan başlıkları

        //id => primery key
        [Key]//primary olanları böyle işaretlemek gerekir
        public int StudentId { get; set; }
        public string? StudentAd { get; set; }
        public string? StudentSoyad { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}