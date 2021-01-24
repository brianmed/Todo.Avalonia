using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Todo.DbContexts;
using Todo.Entities;

namespace Todo.Repositories
{
    public interface ITodoRepository
    {
        Task<TodoEntity> CreateAsync(TodoEntity todo);
        Task<List<TodoEntity>> ReadAsync();
        Task<TodoEntity> UpdateAsync(TodoEntity todo);
        Task<bool> DeleteAsync(long id);
        Task<bool> DeleteAsync(TodoEntity todo);
    }

    public class TodoRepository : ITodoRepository
    {
        private TodoDbContext DbContext { get; init; }

        public TodoRepository(
            TodoDbContext dbContext
        )
        {
            DbContext = dbContext;
        }

        async public Task<TodoEntity> CreateAsync(TodoEntity todo)
        {
            await DbContext.Todos.AddAsync(todo);

            await DbContext.SaveChangesAsync();

            return todo;
        }

        public List<TodoEntity> Read()
        {
            return DbContext.Todos
                .OrderByDescending(v => v.TodoEntityId)
                .ToList();
        }

        async public Task<List<TodoEntity>> ReadAsync()
        {
            return await DbContext.Todos
                .OrderByDescending(v => v.TodoEntityId)
                .ToListAsync();
        }

        async public Task<TodoEntity> UpdateAsync(TodoEntity todo)
        {
            DbContext.Todos.Attach(todo);

            await DbContext.SaveChangesAsync();

            return todo;
        }

        async public Task<bool> DeleteAsync(long id)
        {
            TodoEntity todo = await DbContext.Todos
                .SingleAsync(t => t.TodoEntityId == id);

            if (todo != null)
            {
                DbContext.Remove(todo);
                await DbContext.SaveChangesAsync();

                return true;
            } else {
                return false;
            }
        }

        async public Task<bool> DeleteAsync(TodoEntity todo)
        {
            return await DeleteAsync(todo.TodoEntityId);
        }
    }
}