using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDbExample.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<MongoDbExampleDbContext>(options =>
{
    options.UseMongoDB("mongodb://localhost:27017", "TodoListExampleDb");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/todos", async (MongoDbExampleDbContext context) =>
{
    var todos = await context.Todos.ToListAsync().ConfigureAwait(false);
    return Results.Ok(todos);
});

app.MapGet("/todos/{id}", async (MongoDbExampleDbContext context, string id) =>
{
    var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == ObjectId.Parse(id)).ConfigureAwait(false);

    if (todo is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(todo);
});

app.MapPost("/todos", async (MongoDbExampleDbContext context, Todo todo) =>
{
    await context.Todos.AddAsync(todo).ConfigureAwait(false);
    await context.SaveChangesAsync().ConfigureAwait(false);
    return Results.Created($"/todos/{todo.Id}", todo);
});

app.MapPut("/todos/{id}", async (MongoDbExampleDbContext context, string id, Todo todo) =>
{
    var currentTodo = await context.Todos.FirstOrDefaultAsync(x => x.Id == ObjectId.Parse(id)).ConfigureAwait(false);

    if (currentTodo is null)
    {
        return Results.NotFound();
    }

    currentTodo.Title = todo.Title;
    currentTodo.IsCompleted = todo.IsCompleted;
    currentTodo.CreatedDate = todo.CreatedDate;
    currentTodo.UpdatedDate = DateTime.Now;

    context.Todos.Update(currentTodo);
    await context.SaveChangesAsync().ConfigureAwait(false);
    return Results.Ok();
});

app.MapDelete("/todos/{id}", async (MongoDbExampleDbContext context, string id) =>
{
    var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == ObjectId.Parse(id)).ConfigureAwait(false);

    if (todo is null)
    {
        return Results.NotFound();
    }

    context.Todos.Remove(todo);
    await context.SaveChangesAsync().ConfigureAwait(false);
    return Results.Ok();
});

app.Run();