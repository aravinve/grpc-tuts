using Grpc.Core;
using System;

namespace grpc_tuts.Services;

public class TodoService : Todo.TodoBase {

    private readonly ILogger<TodoService> _logger;
    public TodoService(ILogger<TodoService> logger)
    {
        _logger = logger;
    }

    public override Task<CreateTodoResponse> CreateTodo(CreateTodoRequest request, ServerCallContext context)
    {
        var guid = Guid.NewGuid();
        return Task.FromResult(new CreateTodoResponse {
            Id = guid.ToString(),
            Title = request.Title,
            IsCompleted = request.IsCompleted
        });
    }

}