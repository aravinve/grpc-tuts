using Grpc.Core;
using System;

using grpc_tuts.Models;

namespace grpc_tuts.Services;

public class TodoService : Todo.TodoBase
{
    private readonly ILogger<TodoService> _logger;
    private static readonly Dictionary<string, TodoDto> _dictionary = new Dictionary<string, TodoDto>();
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
        PrintTodoDict("Created TODO");
        return Task.FromResult(new CreateTodoResponse
        {
            Id = todo.Id,
            Title = todo.Title,
            IsCompleted = todo.IsCompleted
        });
    }

    public override Task<UpdateTodoResponse> UpdateTodo(UpdateTodoRequest request, ServerCallContext context)
    {
        if (_dictionary.TryGetValue(request.Id, out var todoItem))
        {
            todoItem.Title = request.Title;
            todoItem.IsCompleted = request.IsCompleted;
            _dictionary.Remove(request.Id);
            _dictionary.Add(todoItem.Id, todoItem);
            PrintTodoDict("Updated TODO");
            return Task.FromResult(new UpdateTodoResponse
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                IsCompleted = todoItem.IsCompleted
            });

        }
        else
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "TodoId not found in the list."));
        }
    }


    #region Helper Methods

    private void PrintTodoDict(string action)
    {
        _logger.LogInformation(action);
        if (_dictionary.Count <= 0)
        {
            _logger.LogInformation("Your Dict is Empty!!");
        }
        foreach (var kvp in _dictionary)
        {
            _logger.LogInformation($"{kvp.Key}:{kvp.Value}");
        }
    }

    #endregion

}