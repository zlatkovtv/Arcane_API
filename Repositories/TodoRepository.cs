using System.Data;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

public sealed class TodoRepository: CrudRepository<Todo>, ITodoRepository
{
    public TodoRepository(IDatabaseRepository dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Todo>> GetAll(int? userId)
    {
        using (IDbConnection connection = this.dbContext.GetConnection()) {
            connection.Open();
            return await connection.QueryAsync<Todo>("SELECT * FROM TODO WHERE USERID = @UserId", new { UserId = userId });
        }
    }
    
    public override Todo Get(int id)
    {
        throw new System.NotImplementedException();
    }

    public override int Add(Todo todo)
    {
        using (IDbConnection connection = this.dbContext.GetConnection()) {
            connection.Open();
            return connection.Execute("INSERT TODO([Description], [UserId]) values(@Description, @UserId)", new { Description = todo.Description, UserId = todo.UserId });
        }
    }

    public override int Delete(int id)
    {
        using (IDbConnection connection = this.dbContext.GetConnection()) {
            connection.Open();
            return connection.Execute("DELETE FROM TODO WHERE ID = @Id", new { Id = id});
        }
    }

    public int DeleteByDesc(Todo todo)
    {
        using (IDbConnection connection = this.dbContext.GetConnection()) {
            connection.Open();
            return connection.Execute("DELETE FROM TODO WHERE DESCRIPTION like @Desc AND USERID = @UserId", new { Desc = todo.Description, UserId = todo.UserId });
        }
    }

    public override int Update(Todo todo)
    {
        using (IDbConnection connection = this.dbContext.GetConnection()) {
            connection.Open();
            return connection.Execute("UPDATE TODO SET DESCRIPTION=@Description, USERID = @UserId WHERE ID = @Id", new { Id = todo.Id, Description = todo.Description, UserId = todo.UserId });
        }
    }
}