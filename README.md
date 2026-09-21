# Task Tracker

A simple command-line task tracker written in C# (.NET 10). It lets you add, update, delete and track tasks from an interactive console. Tasks are saved to a JSON file, so they persist between runs.
This project is based on the Task Tracker exercise from roadmap.sh : https://roadmap.sh/projects/task-tracker.

## Requirements

- .NET 10 SDK

## Running

From the project folder:

    dotnet run

You can also open the project in your IDE (Visual Studio, Rider, VS Code) and run it from there. Once started, type commands at the prompt.

## Commands

    help                          Show the list of commands
    add "description"             Add a new task
    update <id> "description"     Change a task's description
    delete <id>                   Delete a task
    mark-todo <id>                Set a task's status to Todo
    mark-in-progress <id>         Set a task's status to In Progress
    mark-done <id>                Set a task's status to Done
    list                          List all tasks
    list "status"                 List tasks by status (done, todo, in-progress)
    exit                          Quit the application

## Data storage

Tasks are stored as JSON in JsonData/tasks.json.
Each task has a unique ID that is never reused, even after the task is deleted.