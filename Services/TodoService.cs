using Grpc.Core;
using System;

using grpc_tuts.Models;

namespace grpc_tuts.Services;

public class TodoService : Todo.TodoBase
{

    private readonly ILogger<TodoService> _logger;
    private Dictionary<string, TodoDto> _dictionary = new Dictionary<string, TodoDto>();
    public TodoService(ILogger<TodoService> logger)
    {
        _logger = logger;
    }

    public override Task<CreateTodoResponse> CreateTodo(CreateTodoRequest request, ServerCallContext context)
    {
        var guid = Guid.NewGuid();
        var todo = new TodoDto()
        {
            Id = guid.ToString(),
            Title = request.Title,
            IsCompleted = request.IsCompleted
        };
        _dictionary.Add(todo.Id, todo);
        PrintTodoDict();
        return Task.FromResult(new CreateTodoResponse
        {
            Id = todo.Id,
            Title = todo.Title,
            IsCompleted = todo.IsCompleted
        });
    }


    #region Helper Methods

    private void PrintTodoDict() {
        foreach (var kvp in _dictionary)
        {
            Console.WriteLine($"{kvp.Key}:{kvp.Value}");
        }
    }

    #endregion

}