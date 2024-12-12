
namespace Catalog.API.Products.GetProduct
{
    public record GetProductRequest(int? PageNumber=1, int? PageSize=10);
    public record GetProductsResponse(IEnumerable<Product> Products);
    public class GetProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductRequest request, 
                ISender sender) =>
            {
               var query = request.Adapt<GetProductsQuery>();
               var results = await sender.Send(query);

                var response = results.Adapt<GetProductsResponse>();

                return Results.Ok(response);
            }).WithName("GetProducts").
            WithDescription("Returns list of products").
            Produces<GetProductsResponse>(StatusCodes.Status200OK).
            ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
