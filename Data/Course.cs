using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace efcoreApp.Data
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string? Title { get; set; }
        public int TeacherId { get; set; }//öğretmen bir kursu bir kere verebileceğinden bu şekilde ekledim
        //***public int? TeacherId burda int? yaparak veri girilmeye bilir dedim çünkü öğretmenler tablosunu sonradan eklediğim için kurslarda eşleşen olmadığı için veritabanı güncellenemeyecekti bunun iki yolu var biri bunu böyle yaptıktan sonra :
        /*
            287  dotnet ef migrations add AddTableTeacher
            288  dotnet ef migrations remove
            289  dotnet ef migrations add AddTableTeacher
            290  dotnet ef database update

            diğer yolu ise veritabanını silip yeniden kurmak bu da diğer yol ama bunu yaparsam içerde kayıtlı olan verilerde gider buna dikkat et bununla ilgili anlatım 79. da


            ****şimdi ise o alanları doldurup zorunlu hale getirdim
            313  dotnet ef migrations add ColumTeacherIdRequired
            314  dotnet ef database update

        */
        public Teacher Teacher { get; set; } = null!;//öğretmen bir kursu bir kere verebileceğinden bu şekilde ekledim
        //burda önceden oluşturduğum kayıtlara öğretmen atamasına izin vermediği için model içinde kursviewmodule ile yeni oluşturup bu sorunu çözdüm
        public ICollection<CourseRegistration> CourseRegistrations { get; set; } = new List<CourseRegistration>();//bunu eklememin nedeni öğrenci tablosundan detaya(edite)bastığımda o öğrencinin aldığı kursları görmek
    }
}