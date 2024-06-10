using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "lab");
        c.RoutePrefix = string.Empty; 
    });
}

List<Person> people = new List<Person>(); //test
Person GetPersonByEmail(string email) => people.SingleOrDefault(p => p.Email == email);

app.MapGet("/person/{email}", (string email) =>
{
    var person = GetPersonByEmail(email);
    return person is not null ? Results.Ok(person) : Results.NotFound();
});

app.MapPost("/person", (Person newPerson) =>
{
    if (people.Any(p => p.Email == newPerson.Email))
    {
        return Results.BadRequest("Person with this email already exists.");
    }
    people.Add(newPerson);
    return Results.Created($"/person/{newPerson.Email}", newPerson);
});

app.MapPut("/person/{email}", (string email, Person updatedPerson) =>
{
    var person = GetPersonByEmail(email);
    if (person is null)
    {
        return Results.NotFound();
    }
    person.Name = updatedPerson.Name;
    return Results.Ok(person);
});

app.MapPatch("/person/{email}", (string email, string name) =>
{
    var person = GetPersonByEmail(email);
    if (person is null)
    {
        return Results.NotFound();
    }
    person.Name = name;
    return Results.Ok(person);
});

app.MapDelete("/person/{email}", (string email) =>
{
    var person = GetPersonByEmail(email);
    if (person is null)
    {
        return Results.NotFound();
    }
    people.Remove(person);
    return Results.NoContent();
});

app.Run();

public class Person
{
    public string Email { get; set; }
    public string Name { get; set; }
}
