using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using myTestWork.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<myTestWorkContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("myTestWorkContext"));
});

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddEndpointsApiExplorer();

// Create file path with annotations for Swagger UI
var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var filePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(filePath);
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PersonApi", Version = "v1" });
});
builder.Services.AddApiVersioning(p =>
{
    p.AssumeDefaultVersionWhenUnspecified = true;
    p.DefaultApiVersion = ApiVersion.Default;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
