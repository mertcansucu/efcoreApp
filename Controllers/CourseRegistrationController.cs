using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class CourseRegistrationController:Controller
    {
        private readonly DataContext _context;//üretilen private bir değer

        public CourseRegistrationController(DataContext context){//konstraktır olarak tanımlanan değere eşitlener controller kullanılır buna mvc injection denir
            _context = context;
        }

        public async Task<IActionResult> Index(){
            var courseRegistration = await _context
                                    .CourseRegistrations
                                    .Include(x => x.Student)//iclude ile o kişinin bilgilerini diğer tablodan isteyip index te gösterebilirim,yani join işlemi
                                    .Include(x => x.Course)
                                    .ToListAsync();
            /*navigation property include ile yaptığım join işleminin sql kodu
                SELECT "c"."RegistrationId", "c"."CourseId", "c"."RegistrationDate", "c"."StudentId", "s"."StudentId", "s"."Email", "s"."Phone", "s"."StudentAd", "s"."StudentSoyad", "c0"."CourseId", "c0"."Title"
                FROM "CourseRegistrations" AS "c"
                INNER JOIN "Students" AS "s" ON "c"."StudentId" = "s"."StudentId"
                INNER JOIN "Courses" AS "c0" ON "c"."CourseId" = "c0"."CourseId"

            */
            return View(courseRegistration);
        }

        public async Task<IActionResult> Create(){//select kutusu içine bilgiler burdan gider
            ViewBag.Students = new SelectList(await _context.Students.ToListAsync(), "StudentId","StudentAdSoyad");
            //SelectList içine alıp yapmamın nedeni tasarımda selecklist açıp öğrencileri göstermek için
            ViewBag.Courses = new SelectList(await _context.Courses.ToListAsync(), "CourseId","Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseRegistration model){
            model.RegistrationDate = DateTime.Now;
            _context.CourseRegistrations.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}