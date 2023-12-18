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
        public string StudentAdSoyad {//selectde seçme yaparken ad soyad tek bir satırda görünsün diye üste aldığım veri tabanı bilgilerini alıp direk birleştirip gösterdim
            get{
                return this.StudentAd + " " + this.StudentSoyad;
            }
        }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public ICollection<CourseRegistration> CourseRegistrations { get; set; } = new List<CourseRegistration>();//bunu eklememin nedeni öğrenci tablosundan detaya(edite)bastığımda o öğrencinin aldığı kursları görmek
    }
}