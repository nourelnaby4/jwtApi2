using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models;
using MoviesAPI.Service;

var builder = WebApplication.CreateBuilder(args);


var ConnectionString = builder.Configuration.GetConnectionString("DefualtConnection");
// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionString));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddScoped<IRepoService<Genre>, GenreService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()); 
app.UseAuthorization();

app.MapControllers();

app.Run();
