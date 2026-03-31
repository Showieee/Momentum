using Microsoft.AspNetCore.Mvc;
using Momentum.API.Models;
using Momentum.Services.Interfaces;
using Momentum.Services.Models;

namespace Momentum.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EventOrderController : ControllerBase
{
    private readonly IEventOrderService _eventOrderService;
    private readonly ILogger<EventOrderController> _logger;

    public EventOrderController(IEventOrderService eventOrderService, ILogger<EventOrderController> logger)
    {
        _eventOrderService = eventOrderService;
        _logger = logger;
    }

    [HttpGet(Name = "GetEventOrders")]
    public async Task<ActionResult<List<EventOrderResponse>>> Get()
    {
        _logger.LogInformation("Getting all event orders");
        
        var orders = await _eventOrderService.Get();
        
        var response = orders.Select(o => ToResponse(o)).ToList();
        return Ok(response);
    }

    [HttpGet("{id}", Name = "GetEventOrderById")]
    public async Task<ActionResult<EventOrderResponse>> GetById([FromRoute] Guid id)
    {
        _logger.LogInformation("Getting event order with id: {Id}", id);
        
        var order = await _eventOrderService.GetById(id);
        
        if (order == null)
            return NotFound();
            
        return Ok(ToResponse(order));
    }

    [HttpPost(Name = "CreateEventOrder")]
    public async Task<ActionResult<EventOrderResponse>> Create([FromBody] Models.CreateEventOrderRequest request)
    {
        _logger.LogInformation("Creating new event order for person {PersonId} and event {EventId}", 
            request.PersonId, request.EventId);

        var serviceRequest = new Services.Models.CreateEventOrderRequest
        {
            PersonId = request.PersonId,
            EventId = request.EventId,
            Products = request.Products.Select(p => new Services.Models.EventOrderProductRequest
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity
            }).ToList()
        };

        var order = await _eventOrderService.Create(serviceRequest);

        _logger.LogInformation("Event order created with id: {Id}", order.Id);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, ToResponse(order));
    }

    [HttpPut("{id}", Name = "UpdateEventOrder")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] Models.CreateEventOrderRequest request)
    {
        _logger.LogInformation("Updating event order with id: {Id}", id);

        var serviceRequest = new Services.Models.CreateEventOrderRequest
        {
            PersonId = request.PersonId,
            EventId = request.EventId,
            Products = request.Products.Select(p => new Services.Models.EventOrderProductRequest
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity
            }).ToList()
        };

        await _eventOrderService.Update(id, serviceRequest);

        _logger.LogInformation("Event order updated");
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteEventOrder")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        _logger.LogInformation("Deleting event order with id: {Id}", id);
        
        await _eventOrderService.Delete(id);
        
        _logger.LogInformation("Event order deleted");
        return NoContent();
    }

    private static EventOrderResponse ToResponse(EventOrderModel model)
    {
        return new EventOrderResponse
        {
            Id = model.Id,
            PersonId = model.PersonId,
            Person = model.Person == null ? null : new PersonResponse
            {
                Id = model.Person.Id,
                FirstName = model.Person.FirstName,
                LastName = model.Person.LastName,
                CNP = model.Person.CNP
            },
            EventId = model.EventId,
            Event = model.Event == null ? null : new EventResponse
            {
                Id = model.Event.Id,
                Type = (int)model.Event.Type,
                Name = model.Event.Name,
                Date = model.Event.Date
            },
            Products = model.Products.Select(p => new EventOrderProductResponse
            {
                Id = p.Id,
                ProductId = p.ProductId,
                Product = p.Product == null ? null : new ProductResponse
                {
                    Id = p.Product.Id,
                    Type = (int)p.Product.Type,
                    Name = p.Product.Name,
                    Description = p.Product.Description,
                    Price = p.Product.Price
                },
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice,
                TotalPrice = p.TotalPrice
            }).ToList(),
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            TotalPrice = model.TotalPrice
        };
    }
}
