using BasicJWTAuth.Models;

namespace BasicJWTAuth.Repository
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(Users user);
    }
}
