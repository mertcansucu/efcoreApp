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
            return View();
        }

        public async Task<IActionResult> Create(){
            ViewBag.Students = new SelectList(await _context.Students.ToListAsync(), "StudentId","StudentAd");
            //SelectList içine alıp yapmamın nedeni tasarımda selecklist açıp öğrencileri göstermek için
            ViewBag.Courses = new SelectList(await _context.Courses.ToListAsync(), "CourseId","Title");

            return View();
        }

    }
}