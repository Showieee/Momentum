using Microsoft.EntityFrameworkCore;
using Momentum.Domain.Entities;
using Momentum.Persistance;
using Momentum.Services.Interfaces;
using Momentum.Services.Models;

namespace Momentum.Services;

public class EventOrderService : IEventOrderService
{
    private readonly MomentumDbContext _dbContext;

    public EventOrderService(MomentumDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<EventOrderModel>> Get()
    {
        var orders = await _dbContext.Set<EventOrderEntity>()
            .AsNoTracking()
            .Include(eo => eo.Person)
            .Include(eo => eo.Event)
            .Include(eo => eo.Products)
            .ThenInclude(eop => eop.Product)
            .ToListAsync();

        return orders.Select(ToModel).ToList();
    }

    public async Task<EventOrderModel?> GetById(Guid id)
    {
        var order = await _dbContext.Set<EventOrderEntity>()
            .AsNoTracking()
            .Include(eo => eo.Person)
            .Include(eo => eo.Event)
            .Include(eo => eo.Products)
            .ThenInclude(eop => eop.Product)
            .FirstOrDefaultAsync(x => x.Id == id);

        return order == null ? null : ToModel(order);
    }

    public async Task<EventOrderModel> Create(CreateEventOrderRequest request)
    {
        var person = await _dbContext.Set<PersonEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.PersonId);
        
        if (person == null)
            throw new InvalidOperationException("Person not found");

        var eventEntity = await _dbContext.Set<EventEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.EventId);
        
        if (eventEntity == null)
            throw new InvalidOperationException("Event not found");

        var order = new EventOrderEntity
        {
            Id = Guid.NewGuid(),
            PersonId = request.PersonId,
            EventId = request.EventId,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Add(order);

        foreach (var productRequest in request.Products)
        {
            var product = await _dbContext.Set<ProductEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productRequest.ProductId);
            
            if (product == null)
                throw new InvalidOperationException($"Product {productRequest.ProductId} not found");

            var orderProduct = new EventOrderProductEntity
            {
                Id = Guid.NewGuid(),
                EventOrderId = order.Id,
                ProductId = productRequest.ProductId,
                Quantity = productRequest.Quantity,
                UnitPrice = product.Price,
                NumberOfPeople = product.IsPerPerson ? Math.Max(1, productRequest.NumberOfPeople) : 1,
                NumberOfHours = product.IsHourly ? Math.Max(1, productRequest.NumberOfHours) : 1
            };

            _dbContext.Add(orderProduct);
        }

        await _dbContext.SaveChangesAsync();

        return await GetById(order.Id) ?? throw new InvalidOperationException("Failed to create order");
    }

    public async Task Update(Guid id, CreateEventOrderRequest request)
    {
        var order = await _dbContext.Set<EventOrderEntity>()
            .Include(eo => eo.Products)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (order == null)
            throw new InvalidOperationException("Order not found");

        var person = await _dbContext.Set<PersonEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.PersonId);
        
        if (person == null)
            throw new InvalidOperationException("Person not found");

        var eventEntity = await _dbContext.Set<EventEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.EventId);
        
        if (eventEntity == null)
            throw new InvalidOperationException("Event not found");

        order.PersonId = request.PersonId;
        order.EventId = request.EventId;
        order.UpdatedAt = DateTime.UtcNow;

        // Remove existing products
        _dbContext.RemoveRange(order.Products);

        // Add new products
        foreach (var productRequest in request.Products)
        {
            var product = await _dbContext.Set<ProductEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productRequest.ProductId);
            
            if (product == null)
                throw new InvalidOperationException($"Product {productRequest.ProductId} not found");

            var orderProduct = new EventOrderProductEntity
            {
                Id = Guid.NewGuid(),
                EventOrderId = order.Id,
                ProductId = productRequest.ProductId,
                Quantity = productRequest.Quantity,
                UnitPrice = product.Price,
                NumberOfPeople = product.IsPerPerson ? Math.Max(1, productRequest.NumberOfPeople) : 1,
                NumberOfHours = product.IsHourly ? Math.Max(1, productRequest.NumberOfHours) : 1
            };

            order.Products.Add(orderProduct);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task MarkAsPaid(Guid id)
    {
        var order = await _dbContext.Set<EventOrderEntity>()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (order == null)
            throw new InvalidOperationException("Order not found");

        if (order.IsPaid)
            return;

        order.IsPaid = true;
        order.PaidAt = DateTime.UtcNow;
        order.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var order = await _dbContext.Set<EventOrderEntity>()
            .Include(eo => eo.Products)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (order != null)
        {
            _dbContext.RemoveRange(order.Products);
            _dbContext.Remove(order);
            await _dbContext.SaveChangesAsync();
        }
    }

    private static EventOrderModel ToModel(EventOrderEntity entity)
    {
        return new EventOrderModel
        {
            Id = entity.Id,
            PersonId = entity.PersonId,
            Person = entity.Person == null ? null : new PersonOrderModel
            {
                Id = entity.Person.Id,
                FirstName = entity.Person.FirstName,
                LastName = entity.Person.LastName,
                CNP = entity.Person.CNP
            },
            EventId = entity.EventId,
            Event = entity.Event == null ? null : new EventModel
            {
                Id = entity.Event.Id,
                Type = entity.Event.Type,
                Name = entity.Event.Name,
                Date = entity.Event.Date
            },
            Products = entity.Products.Select(eop => new EventOrderProductModel
            {
                Id = eop.Id,
                ProductId = eop.ProductId,
                Product = eop.Product == null ? null : new ProductModel
                {
                    Id = eop.Product.Id,
                    Type = eop.Product.Type,
                    Name = eop.Product.Name,
                    Description = eop.Product.Description,
                    Price = eop.Product.Price,
                    IsPerPerson = eop.Product.IsPerPerson,
                    IsHourly = eop.Product.IsHourly
                },
                Quantity = eop.Quantity,
                UnitPrice = eop.UnitPrice,
                NumberOfPeople = eop.NumberOfPeople,
                NumberOfHours = eop.NumberOfHours
            }).ToList(),
            IsPaid = entity.IsPaid,
            PaidAt = entity.PaidAt,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
