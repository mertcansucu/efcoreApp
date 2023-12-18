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
        [HttpGet]
        public async Task<IActionResult> Edit(int? id){//edit sayfasında sadece seçilen kişinin bilgileri gönderilsin diye id yi html sayfasına gönderme işlemi
            if(id == null){
                return NotFound();
            }

           var std = await _context
                    .Students
                    .Include(x => x.CourseRegistrations)//editte kurs kayıtları göstermek için inner join yaptım
                    .ThenInclude(x => x.Course)//üste kurs kayıt tablosuna gittim ama ordan da kursa gidip o veriyi almam için bunun içinde öğrenci ile kurs arasında başka bir join yapmam lazım onun içinde theninclude kullandım
                    .FirstOrDefaultAsync(x => x.StudentId == id); 
           
           //FindAsync(id) sadece id için yapar 
            // var std = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id);diğer değerler içinde kontrol edebiliriz kontrol eder

            if (std == null)
            {
                return NotFound();
            }

            return View(std);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//burayı get eden ve post eden kişinin aynı kişi olup olamdığını kontrol et dedim çünkü başka biri girip bilgileri kullanıp veya kendi güncelleme yapabilir
        public async Task<IActionResult> Edit(int id, Student model){
            if (id != model.StudentId)
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
                catch (DbUpdateConcurrencyException)//eğer bir hata varsa bu bölüme geç dedim
                {
                    if(!_context.Students.Any(o => o.StudentId == model.StudentId))//id kontrolu yaptım eğer eşit değilse hata verdirdim
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

           var ogr = await _context.Students.FindAsync(id);

           if (ogr == null)
           {
                return NotFound();
           }

           return View(ogr);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id)//fromform dememin nedeni ordaki id bilgisini al dedim başka yerdeki bilgitide diyip alablirdim bunların detaylı bilbisi .net mdel binding dökümantasyonunda,bunun diğer yolu üsteki gibi edit işleminde yaptım buda diğer yol
        {
            var ogr = await _context.Students.FindAsync(id);
            if (ogr == null)
            {
                return NotFound();
            }

            _context.Students.Remove(ogr);//silmek istediğm bilgiyi aldım
            await _context.SaveChangesAsync();//veritabından sildim
            return RedirectToAction("Index");
        }
        
    }
}