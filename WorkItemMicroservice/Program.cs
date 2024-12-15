using Microsoft.EntityFrameworkCore;
using WorkItemMicroservice.Data;
using WorkItemMicroservice.Repositories;
using WorkItemMicroservice.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<WorkItemDbContext>(options =>
    options.UseInMemoryDatabase("WorkItemDb"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddScoped<IWorkItemRepository, WorkItemRepository>();
builder.Services.AddScoped<IWorkItemService, WorkItemService>();
builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//DB initialization
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WorkItemDbContext>();
    context.Database.EnsureCreated(); // Create in-memory database

}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
