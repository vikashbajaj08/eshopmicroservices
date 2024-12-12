﻿namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

    public record GetProductByIdResult(Product Product);

    public class GetProductByIdValidator : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
        }
    }

    internal class GetProductByIdQueryHandler(IDocumentSession session) 
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {

           var product = await session.LoadAsync<Product>(query.Id,cancellationToken);

            if (product is null) {
                throw new ProductNotFoundException(query.Id);
            }
            return new GetProductByIdResult(product);
        }
    }
}
