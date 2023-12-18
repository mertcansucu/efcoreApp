using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoreApp.Data;
using efcoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var courses = await _context.Courses.Include(k=> k.Teacher).ToListAsync();
            return View(courses);
        }

        public async Task<IActionResult> Create(){//ekleme ekranı tasarım
            ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(),"TeacherId","TeachertAdSoyad");//öğretmen seçimi için öğretmenleride ekledim
            return View();
        }

        [HttpPost] //form action (yeni eklenen kurs bilgilerinin çelilmesi)
        [ValidateAntiForgeryToken]//burayı get eden ve post eden kişinin aynı kişi olup olamdığını kontrol et dedim çünkü başka biri girip bilgileri kullanıp veya kendi güncelleme yapabilir
        public async Task<IActionResult> Create(CourseViewModel model){
            //veri tabanı bağlantıları sekron ve asekron şeklinde oluyor,ama veri tabanında asekron(async) kullanıyoruz
            /*
            async => lokanta mantığı siparişleri alır ve sırası gelen siparişi alır
            senkron =>bunda ise ilk sipariş tamamlanmadan diğer sipariş alınmıyor
            */

            //üste controller ile bağlantı sağlayıp o bilgileri kullanarak ekleme yapıcam
            if(ModelState.IsValid){//bunu ekleme nedenim alan boş olursa çalışmasın yani hoca seçimi yapma zorunluluğunu yaptım yapmazsa hata verecek
                _context.Courses.Add(new Course() {CourseId = model.CourseId,Title = model.Title,TeacherId = model.TeacherId });
                await _context.SaveChangesAsync();//kaydetme olduğu için sıraya alınsın diye bekleme yaptırdım
                return RedirectToAction("Index");//yeni kayıt eklendiğinde student/index e gönderdim
            }
            ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(),"TeacherId","TeachertAdSoyad");//öğretmen seçimi için öğretmenleride ekledim
            return View(model);
        }

        //edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id){//edit sayfasında sadece seçilen kişinin bilgileri gönderilsin diye id yi html sayfasına gönderme işlemi
            if(id == null){
                return NotFound();
            }

           var course = await _context
           .Courses
           .Include(x => x.CourseRegistrations)//editte kurs kayıtları göstermek için inner join yaptım
           .ThenInclude(x => x.Student)//üste kurs kayıt tablosuna gittim ama ordan da kursa gidip o veriyi almam için bunun içinde öğrenci ile kurs arasında başka bir join yapmam lazım onun içinde theninclude kullandım
           .Select(k => new CourseViewModel{//burda yeni bir model içinde bir database oluşturup bilgileri ordan çektim çünkü kursa öğretmen atamasında hata almamak için
                CourseId = k.CourseId,
                Title = k.Title,
                TeacherId = k.TeacherId,
                CourseRegistrations = k.CourseRegistrations
           })
           .FirstOrDefaultAsync(x =>x.CourseId == id); 
           
           //FindAsync sadece id için yapar 

            // var std = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id);diğer değerler içinde kontrol edebiliriz kontrol eder

            if (course == null)
            {
                return NotFound();
            }

            ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(),"TeacherId","TeachertAdSoyad");//öğretmen seçimi için öğretmenleride ekledim

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//burayı get eden ve post eden kişinin aynı kişi olup olamdığını kontrol et dedim çünkü başka biri girip bilgileri kullanıp veya kendi güncelleme yapabilir
        public async Task<IActionResult> Edit(int id, CourseViewModel model){
            if (id != model.CourseId)
            {
                NotFound();
            }

            if (ModelState.IsValid)//modeldeki bilgiler istenilen formatta mı diye bak dedim öyleyse güncellemyei yapıcam
            {
                try
                {
                    _context.Update(new Course() {CourseId = model.CourseId,Title = model.Title,TeacherId = model.TeacherId });
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
            ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(),"TeacherId","TeachertAdSoyad");//öğretmen seçimi için öğretmenleride ekledim
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