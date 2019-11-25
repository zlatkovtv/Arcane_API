public interface IUserRepository: ICrudRepository<User>
{
    User Authenticate(User user);
}