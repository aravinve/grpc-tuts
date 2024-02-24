using System;

namespace TodoApp.Models;

public class TodoDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public bool IsCompleted { get; set; }

    public override string ToString()
    {
        return $"Title={Title};IsCompleted={IsCompleted}";
    }
}