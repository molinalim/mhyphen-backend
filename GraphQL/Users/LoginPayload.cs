using mhyphen.Models;

namespace mhyphen.GraphQL.Users
{
    public record LoginPayload(
        User user,
        string jwt);
}
