using DataAccess.ServiceRepository;
using HalloDoc.DataContext;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.Implementation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAdminLog, AdminLog>();
builder.Services.AddScoped<IAdminDashboard, AdminDashboard>();
builder.Services.AddScoped<IAdminDashboardDataTable, AdminDashboardDataTable>();
builder.Services.AddScoped<IViewCaseRepo, ViewCaseRepo>();
builder.Services.AddScoped<IBlockCaseRepository, BlockCaseRepository>();
builder.Services.AddScoped<IAddOrUpdateRequestNotes, AddOrUpdateRequestNotes>();
builder.Services.AddScoped<IAddOrUpdateRequestStatusLog, AddOrUpdateRequestStatusLog>();
builder.Services.AddScoped<IAuthorizatoinRepository, AuthorizationRepository>();
builder.Services.AddScoped<IJwtRepository, JwtRepository>();
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
app.UseSession();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
pattern: "{controller=Admin}/{action=AdminDashboard}/{id?}");
//pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
