﻿using MediatR;

namespace Onyx.ProductsApi.Commands
{
    public class DeleteProductCommand : IRequest
    {
        public DeleteProductCommand(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
