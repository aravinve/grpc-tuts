using Grpc.Core;
using System;

using TodoApp.Models;

namespace TodoApp.Services;

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

    public override Task<DeleteTodoResponse> DeleteTodo(DeleteTodoRequest request, ServerCallContext context)
    {
        if (_dictionary.ContainsKey(request.Id))
        {
            _dictionary.Remove(request.Id);
            PrintTodoDict("Deleted TODO");
            return Task.FromResult(new DeleteTodoResponse());

        }
        else
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "TodoId not found in the list."));
        }
    }

    public override Task<GetTodoResponse> GetTodo(GetTodoRequest request, ServerCallContext context)
    {
        if (_dictionary.TryGetValue(request.Id, out var todoItem))
        {
            PrintTodoDict("Get TODO");
            return Task.FromResult(new GetTodoResponse
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

    public override Task<ListTodoResponse> ListTodo(ListTodoRequest request, ServerCallContext context)
    {
        PrintTodoDict("List TODO");
        List<TodoDto> todos;
        lock (_dictionary)
        {
            todos = new List<TodoDto>(_dictionary.Values);
        }
        return Task.FromResult(new ListTodoResponse
        {
            Todos = { todos }
        });
    }


    #region Helper Methods

    private void PrintTodoDict(string action)
    {
        _logger.LogInformation(action);
        if (_dictionary.Count <= 0)
        {
            _logger.LogInformation("Your Dict is Empty!!");
        }
        else
        {
            _logger.LogInformation($"Your Dict Count = {_dictionary.Count}");
        }
        foreach (var kvp in _dictionary)
        {
            _logger.LogInformation($"{kvp.Key}:{kvp.Value}");
        }
    }

    #endregion

}