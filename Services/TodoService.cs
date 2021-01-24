using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Todo.AppCtx;
using Todo.DbContexts;
using Todo.Entities;
using Todo.Repositories;

namespace Todo.Services
{
    public interface ITodoService
    {
        Task<TodoEntity> CreateAsync(string title);

        Task<List<TodoEntity>> ReadAsync();
    }

    public class TodoService : ITodoService
    {
        private TodoDbContext DbContext { get; init; }

        private ITodoRepository TodoRepository { get; init; }

        public TodoService(
            TodoDbContext dbContext,
            ITodoRepository todoRepository)
        {
            DbContext = dbContext;

            TodoRepository = todoRepository;
        }

        async public Task<TodoEntity> CreateAsync(string title)
        {
            TodoEntity todoEntity = new TodoEntity
            {
                Title = title,
                DayCreated = (int)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds,
            };

            await TodoRepository.CreateAsync(todoEntity);

            return todoEntity;
        }

        async public Task<List<TodoEntity>> ReadAsync()
        {
            return await TodoRepository.ReadAsync();
        }
    }
}