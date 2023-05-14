using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

namespace ToDo;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Todo").WithTags(nameof(Todo));

        group.MapGet("/", async (ToDoContext db) =>
        {
            return await db.Todo.ToListAsync();
        })
        .WithName("GetAllTodos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Todo>, NotFound>> (int id, ToDoContext db) =>
        {
            return await db.Todo.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Todo model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetTodoById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Todo todo, ToDoContext db) =>
        {
            var affected = await db.Todo
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, todo.Id)
                  .SetProperty(m => m.Title, todo.Title)
                  .SetProperty(m => m.Description, todo.Description)
                  .SetProperty(m => m.Done, todo.Done)
                  .SetProperty(m => m.Archived, todo.Archived)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateTodo")
        .WithOpenApi();

        group.MapPost("/", async (Todo todo, ToDoContext db) =>
        {
            db.Todo.Add(todo);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Todo/{todo.Id}",todo);
        })
        .WithName("CreateTodo")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ToDoContext db) =>
        {
            var affected = await db.Todo
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteTodo")
        .WithOpenApi();
    }
}
