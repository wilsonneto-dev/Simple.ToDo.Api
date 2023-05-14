﻿namespace ToDo;

public class Todo
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool Done { get; set; }
    public bool Archived { get; set; }
}
