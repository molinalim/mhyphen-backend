using mhyphen.Models;

namespace mhyphen.GraphQL.Movies
{
    public record EditMovieInput(
        string MovieId,
        string? Title,
        string? Plot,
        string? Link,
        double? Rating,
        string? Genre,
        string? ImageURL,
        int? Runtime
        );
}
