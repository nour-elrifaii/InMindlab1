using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApplication1.Middleware;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Validators;
using WebApplication2.Filters;
using WebApplication2.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", policy =>
    {
        policy.AllowAnyOrigin()  
            .AllowAnyHeader()  
            .AllowAnyMethod(); 
    });
});


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
}).AddFluentValidation();
builder.Services.AddValidatorsFromAssemblyContaining<StudentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddTransient<ObjectMapperService>();
//builder.Services.AddScoped<StudentService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.UseCors();

//app.Urls.Add("http://localhost:5263");  


app.Run();







