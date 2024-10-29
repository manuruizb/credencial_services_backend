using System.Text;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("*") // Reemplaza con la URL de BlazorWasm
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
var app = builder.Build();

app.UseCors(); // Habilita CORS

app.MapPost("/api/authenticate", ([FromBodyAttribute]string key) => {
    Console.WriteLine(key);
    if (key == "123456789") {
        var token = Convert.ToBase64String(Encoding.UTF32.GetBytes("123456789"));
        return Results.Ok(token);
    }

    return Results.Unauthorized();
});

app.Run();
