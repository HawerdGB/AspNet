using AppStore.Models.DataSeed;
using AppStore.Models.DBContext;
using AppStore.Models.Domain;
using AppStore.Repositories.Abstract;
using AppStore.Repositories.Implementation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ILibroService, LibroService>();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

builder.Services.AddDbContext<DataBaseContext>(opt =>{
         opt.LogTo(Console.WriteLine, new [] {
            DbLoggerCategory.Database.Command.Name
         },LogLevel.Information).EnableSensitiveDataLogging();
         opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteDatabase"));
         });

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<DataBaseContext>()
.AddDefaultTokenProviders();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using(var ambiente = app.Services.CreateScope()){
    var services = ambiente.ServiceProvider;

    try{
    var context = services.GetRequiredService<DataBaseContext>();

    var UserManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();


   await context.Database.MigrateAsync();
   await LoadDatabase.InsertarData(context, UserManager, roleManager);
    }
    catch(Exception e)
    {
        var logging = services.GetRequiredService<ILogger<Program>>();
        logging.LogError(e, "Ocurrio un error en la insercion de datos");
    }
}



app.Run();
