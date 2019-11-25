using System.Collections.Generic;

public interface ICrudRepository<T>
{
    int Add(T item);
    int Delete(int id);
    T Get(int id);
    int Update(T item);
}