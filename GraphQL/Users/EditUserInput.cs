namespace mhyphen.GraphQL.Users
{
    public record EditUserInput(
        int Id,
        string? Name,
        string? Password);
}
