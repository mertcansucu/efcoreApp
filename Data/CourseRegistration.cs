using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace efcoreApp.Data
{
    public class CourseRegistration
    {
        //kurs kayıt
        [Key]
        public int RegistrationId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}