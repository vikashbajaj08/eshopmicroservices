
namespace Catalog.API.Products.GetProduct
{
    public record GetProductsResponse(IEnumerable<Product> Products);
    public class GetProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender) =>
            {
               var results = await sender.Send(new GetProductsQuery());

                var response = results.Adapt<GetProductsResponse>();

                return Results.Ok(response);
            }).WithName("GetProducts").
            WithDescription("Returns list of products").
            Produces<GetProductsResponse>(StatusCodes.Status200OK).
            ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
