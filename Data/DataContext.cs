using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Data
{
    public class DataContext: DbContext
    {
        //options bilgisini dışardan almasını sağladım(Program.cs içinde oluşturduğum options)
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            
        }

        //DataContext üzerinden ilgili tipler uygulama tarafından erişebilir ve veritabı tarafından senkorize edilebilir bir veri tabanı oluşturdum
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<CourseRegistration> CourseRegistrations => Set<CourseRegistration>();
    }
    //database erişimin iki yolu var biri code ile diğeri elle hazırlanan database entegre etmek proje ilde ben burda coe ile yapıcam (code-first)

    //code-first => entity,dbcontext => database(sqlite)
    // databasee-first => sql server
}