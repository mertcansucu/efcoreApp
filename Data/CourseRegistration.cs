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
        public Student Student { get; set; } = null!;//veritabında o bilginin hangi kayıttan aldığımı bulmak ve birbirine bağlamam lazım bunu da joinler ile yapablirim .net te bu işlemi bu şekilde yapıcam
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;//veritabında o bilginin hangi kayıttan aldığımı bulmak ve birbirine bağlamam lazım bunu da joinler ile yapablirim .net te bu işlemi bu şekilde yapıcam
        public DateTime RegistrationDate { get; set; }
    }
}