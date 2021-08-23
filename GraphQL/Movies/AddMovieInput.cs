using mhyphen.Models;

namespace mhyphen.GraphQL.Movies
{
    public record AddMovieInput(
        string Title,
        string Plot,
        string ImageURL,
        double Rating,
        string Genre,
        int Runtime);
}
