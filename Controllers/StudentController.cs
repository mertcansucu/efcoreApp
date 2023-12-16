using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class StudentController: Controller
    {
        private readonly DataContext _context;//üretilen private bir değer

        public StudentController(DataContext context){//konstraktır olarak tanımlanan değere eşitlener controller kullanılır buna mvc injection denir
            _context = context;
        }

        public async Task<IActionResult> Index(){//index html sayfasında veritabındaki kayıtlı öğrenci bilgilerinin gösterilmesi
            return View(await _context.Students.ToListAsync());
        }

        public IActionResult Create(){//ekleme ekranı tasarım
            return View();
        }

        [HttpPost] //form action (yeni eklenen öğrenci bilgilerinin çelilmesi)
        public async Task<IActionResult> Create(Student model){
            //veri tabanı bağlantıları sekron ve asekron şeklinde oluyor,ama veri tabanında asekron(async) kullanıyoruz
            /*
            async => lokanta mantığı siparişleri alır ve sırası gelen siparişi alır
            senkron =>bunda ise ilk sipariş tamamlanmadan diğer sipariş alınmıyor
            */

            //üste controller ile bağlantı sağlayıp o bilgileri kullanarak ekleme yapıcam
            _context.Students.Add(model);
            await _context.SaveChangesAsync();//kaydetme olduğu için sıraya alınsın diye bekleme yaptırdım
            return RedirectToAction("Index");//yeni kayıt eklendiğinde student/index e gönderdim
        }

        //edit
        public async Task<IActionResult> Edit(int? id){//edit sayfasında sadece seçilen kişinin bilgileri gönderilsin diye id yi html sayfasına gönderme işlemi
            if(id == null){
                return NotFound();
            }

           var std = await _context.Students.FindAsync(id); //bu sadece id için yapar 

            // var std = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id);diğer değerler içinde kontrol edebiliriz kontrol eder

            if (std == null)
            {
                return NotFound();
            }

            return View(std);
        }
        
    }
}