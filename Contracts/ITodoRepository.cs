using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITodoRepository: ICrudRepository<Todo>
{
    Task<IEnumerable<Todo>> GetAll(int? id);
}