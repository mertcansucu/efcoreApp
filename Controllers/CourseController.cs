using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class CourseController:Controller
    {
        private readonly DataContext _context;
        public CourseController(DataContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index(){
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

        public IActionResult Create(){//ekleme ekranı tasarım
            return View();
        }

        [HttpPost] //form action (yeni eklenen kurs bilgilerinin çelilmesi)
        [ValidateAntiForgeryToken]//burayı get eden ve post eden kişinin aynı kişi olup olamdığını kontrol et dedim çünkü başka biri girip bilgileri kullanıp veya kendi güncelleme yapabilir
        public async Task<IActionResult> Create(Course model){
            //veri tabanı bağlantıları sekron ve asekron şeklinde oluyor,ama veri tabanında asekron(async) kullanıyoruz
            /*
            async => lokanta mantığı siparişleri alır ve sırası gelen siparişi alır
            senkron =>bunda ise ilk sipariş tamamlanmadan diğer sipariş alınmıyor
            */

            //üste controller ile bağlantı sağlayıp o bilgileri kullanarak ekleme yapıcam
            _context.Courses.Add(model);
            await _context.SaveChangesAsync();//kaydetme olduğu için sıraya alınsın diye bekleme yaptırdım
            return RedirectToAction("Index");//yeni kayıt eklendiğinde student/index e gönderdim
        }

        //edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id){//edit sayfasında sadece seçilen kişinin bilgileri gönderilsin diye id yi html sayfasına gönderme işlemi
            if(id == null){
                return NotFound();
            }

           var course = await _context.Courses.FindAsync(id); //bu sadece id için yapar 

            // var std = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id);diğer değerler içinde kontrol edebiliriz kontrol eder

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//burayı get eden ve post eden kişinin aynı kişi olup olamdığını kontrol et dedim çünkü başka biri girip bilgileri kullanıp veya kendi güncelleme yapabilir
        public async Task<IActionResult> Edit(int id, Course model){
            if (id != model.CourseId)
            {
                NotFound();
            }

            if (ModelState.IsValid)//modeldeki bilgiler istenilen formatta mı diye bak dedim öyleyse güncellemyei yapıcam
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();//güncelleme bu bölümde oluyor üste bilgileri alıyorum ama veri tabanıunda güncelleme burdan oluyor
                }
                catch (DbUpdateException)//DbUpdateConcurrencyException yerine diğerini kullandım ikiside kullanıyor bu kullandığım daha genel bir şey,eğer bir hata varsa bu bölüme geç dedim
                {
                    if(!_context.Courses.Any(o => o.CourseId == model.CourseId))//id kontrolu yaptım eğer eşit değilse hata verdirdim
                    {
                        return NotFound();
                    }else
                    {
                        throw;//eğer if deki hata yoksa devam ettir dedim    
                    }
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

         //Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id){
            
            if(id == null){
                return NotFound();
            }

           var course = await _context.Courses.FindAsync(id);

           if (course == null)
           {
                return NotFound();
           }

           return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id)//fromform dememin nedeni ordaki id bilgisini al dedim başka yerdeki bilgitide diyip alablirdim bunların detaylı bilbisi .net mdel binding dökümantasyonunda,bunun diğer yolu üsteki gibi edit işleminde yaptım buda diğer yol
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);//silmek istediğm bilgiyi aldım
            await _context.SaveChangesAsync();//veritabından sildim
            return RedirectToAction("Index");
        }

    }
}