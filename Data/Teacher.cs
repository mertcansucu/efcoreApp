using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace efcoreApp.Data
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }
        public string? TeacherAd { get; set; }
        public string? TeacherSoyad { get; set; }
        public string TeachertAdSoyad {//selectde seçme yaparken ad soyad tek bir satırda görünsün diye üste aldığım veri tabanı bilgilerini alıp direk birleştirip gösterdim
            get{
                return this.TeacherAd + " " + this.TeacherSoyad;
            }
        }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        [DataType(DataType.Date)]//sadece tarih gelsin dedim saati çıkardım
        [DisplayFormat(DataFormatString ="{0:dd-MM-yyyy}",ApplyFormatInEditMode =false)]//tarih bilgisi textbox a yüklendiğinde istediğim gibi editleyebilirim
        public DateTime StartingDate { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();//bir öğretmen birden fazla kursun öğretmeni olabilir ondan bu şekilde yaptım ama course.cs de normal şekilde idsini alıp eşleştirdim çünkü bir öğretmen bir kursu bir kere verebilir
    }
}