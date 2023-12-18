using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class TeacherController: Controller
    {
        private readonly DataContext _context;//üretilen private bir değer

        public TeacherController(DataContext context){//konstraktır olarak tanımlanan değere eşitlener controller kullanılır buna mvc injection denir
            _context = context;
        }
        public async Task<IActionResult> Index(){//index html sayfasında veritabındaki kayıtlı öğretmen bilgilerinin gösterilmesi
            return View(await _context.Teachers.ToListAsync());
        }

        public IActionResult Create(){//ekleme ekranı tasarım
            return View();
        }

        [HttpPost] //form action (yeni eklenen öğrenci bilgilerinin çelilmesi)
        public async Task<IActionResult> Create(Teacher model){
            //veri tabanı bağlantıları sekron ve asekron şeklinde oluyor,ama veri tabanında asekron(async) kullanıyoruz
            /*
            async => lokanta mantığı siparişleri alır ve sırası gelen siparişi alır
            senkron =>bunda ise ilk sipariş tamamlanmadan diğer sipariş alınmıyor
            */

            //üste controller ile bağlantı sağlayıp o bilgileri kullanarak ekleme yapıcam
            _context.Teachers.Add(model);
            await _context.SaveChangesAsync();//kaydetme olduğu için sıraya alınsın diye bekleme yaptırdım
            return RedirectToAction("Index");//yeni kayıt eklendiğinde student/index e gönderdim
        }

        
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var entity = await _context
                                .Teachers
                                .FirstOrDefaultAsync(o => o.TeacherId == id);

            if(entity == null) 
            {
                return NotFound();
            }

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Teacher model)
        {
            if(id != model.TeacherId)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!_context.Students.Any(o => o.StudentId == model.TeacherId))
                    {
                        return NotFound();
                    } 
                    else 
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}