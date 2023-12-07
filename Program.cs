using efcoreApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//DataContext benim sqlite veritabanı ile bağlantı kurup kullanağım yer olduğu için bağlantıyı sağladım
//aspsettings.Development.json=>programı geliştirdiğimiz yer
//aspsettings.json programı yayınladığımız yer
//programı geliştirdiğimiz için sqlite özel bir kodu aspsettings.Development.json içine ekledim ve onu burda çağırdım
/*
,
    "ConnectionStrings": {
      "database": "Data Source=database.db"
    }
*/
builder.Services.AddDbContext<DataContext>(options =>{
    var config = builder.Configuration;
    var connectionString= config.GetConnectionString("database");
    options.UseSqlite(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
