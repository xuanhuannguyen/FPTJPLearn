using Microsoft.EntityFrameworkCore;
using JPLearn.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile("appsettings.Development.json", optional: true)
    .Build();

var services = new ServiceCollection();
var connectionString = configuration.GetConnectionString("DefaultConnection") 
    ?? "Host=localhost;Database=jplearn;Username=postgres;Password=postgres";

services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var serviceProvider = services.BuildServiceProvider();
using var scope = serviceProvider.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

var counts = await db.ExamQuestions
    .GroupBy(q => q.CourseCode)
    .Select(g => new { Course = g.Key, Count = g.Count() })
    .ToListAsync();

foreach (var item in counts)
{
    Console.WriteLine($"Course: {item.Course}, Questions: {item.Count}");
}
