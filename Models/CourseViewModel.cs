using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using efcoreApp.Data;

namespace efcoreApp.Models
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name ="Kurs Başlığı")]//görünen kısmı kendim düzeltebilirim
        public string? Title { get; set; }
        public int TeacherId { get; set; }
        public ICollection<CourseRegistration> CourseRegistrations { get; set; } = new List<CourseRegistration>();//bunu eklememin nedeni öğrenci tablosundan detaya(edite)bastığımda o öğrencinin aldığı kursları görmek
    }
}