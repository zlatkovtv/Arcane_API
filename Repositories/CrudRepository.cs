public abstract class CrudRepository<T>: ICrudRepository<T>
{
    protected readonly IDatabaseRepository dbContext;

    protected CrudRepository(IDatabaseRepository dbContext) {
        this.dbContext = dbContext;
    }

    public abstract int Add(T item);
    public abstract int Delete(int id);
    public abstract T Get(int id);
    public abstract int Update(T item);
}