namespace mhyphen.GraphQL.Users
{
    public record AddUserInput(
        int Id,
        string Name,
        string Password);
}
