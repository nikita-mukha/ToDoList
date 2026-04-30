using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Services;
using ToDoList.Infrastructure.Identity;
using ToDoList.Infrastructure.Persistence;
using ToDoList.Web.Interfaces;
using ToDoList.Web.Services;
using EfToDoStorage = ToDoList.Infrastructure.Persistence.EfToDoStorage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IToDoStorage, EfToDoStorage>();
builder.Services.AddScoped<IEventStorage, EfEventStorage>();
builder.Services.AddScoped<IToDoManager, ToDoManager>();
builder.Services.AddScoped<IRecurringSeriesStorage, EfRecurringSeriesStorage>();
builder.Services.AddScoped<IRecurringOccurrenceGenerator, RecurringOccurrenceGenerator>();
builder.Services.AddScoped<IRecurringOccurrenceService, RecurringOccurrenceService>();
builder.Services.AddScoped<IRecurringToDoService, RecurringToDoService>();
builder.Services.AddScoped<IToDoIndexService, ToDoIndexService>();
builder.Services.AddScoped<IRecurringOccurrenceExceptionStorage, EfRecurringOccurrenceExceptionStorage>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=todos.db"));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 4;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStatusCodePagesWithReExecute("/Home/StatusCodePage", "?code={0}");
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=ToDo}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();