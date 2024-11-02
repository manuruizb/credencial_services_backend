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

var credenciales = new List<Credencial>();
const string correctKey = "123456789";

app.MapPost("/api/authenticate", ([FromBodyAttribute] string key) =>
{
    Console.WriteLine(key);
    if (key == correctKey)
    {
        return Results.Ok(true);
    }

    return Results.Ok(false);
});

app.MapPost("/api/credenciales", (Credencial credencial) =>
{
    foreach (var item in credenciales)
    {
        if(item.usuario.ToLower() == credencial.usuario.ToLower())
        {
            return Results.BadRequest("El nombre de usuario ya existe.");
        }
    }
    credenciales.Add(credencial);

    return Results.Ok();
});

app.MapGet("/api/credenciales", () =>
{

    return Results.Ok(credenciales);

});

app.MapGet("/api/search", (string search) =>
{
    foreach (var item in credenciales)
    {
        if (item.servicio.ToLower() == search.ToLower() || item.usuario.ToLower() == search.ToLower())
        {
            return Results.Ok(item);
        }
    }
    return Results.BadRequest("No se encontró.");
});

app.MapDelete("/api/delete", (string usuario) =>
{
    foreach (var item in credenciales)
    {
        if(item.usuario.ToLower() == usuario.ToLower())
        {
            credenciales.Remove(item);
            return Results.Ok();
        }
    }
    return Results.BadRequest("No se pudo eliminar. ");
});


app.Run();

public class Credencial
{
    public string servicio { get; set; }
    public string usuario { get; set; }
    public string password { get; set; }
}