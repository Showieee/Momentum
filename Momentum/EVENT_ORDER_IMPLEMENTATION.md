# Event Order Creation System - Implementation Guide

## Overview
This document outlines the complete implementation of an event order creation system for the Momentum application. This system allows users to create events with a multi-step wizard interface, where they can:
1. Select a person (who the event is for)
2. Fill in event details (name, type, date)
3. Select and configure products for the event
4. Review and confirm the order

---

## Architecture Components

### 1. **Domain Layer** (`Momentum.Domain`)

#### New Entities:
- **EventOrderEntity** - Represents an order linking a Person to an Event
  - `Id` (GUID)
  - `PersonId` (GUID - FK to Person)
  - `EventId` (GUID - FK to Event)
  - `CreatedAt` (DateTime)
  - `UpdatedAt` (DateTime)
  - `Products` (Collection of EventOrderProductEntity)

- **EventOrderProductEntity** - Junction table linking products to event orders
  - `Id` (GUID)
  - `EventOrderId` (GUID - FK to EventOrder)
  - `ProductId` (GUID - FK to Product)
  - `Quantity` (decimal)
  - `UnitPrice` (decimal)

**Location:** `Momentum.Domain\Entities\EventOrderEntity.cs`

---

### 2. **Service Layer** (`Momentum.Services`)

#### Service Models:
- **EventOrderModel** - Service representation of an event order
- **EventOrderProductModel** - Service representation of order products
- **CreateEventOrderRequest** - Request model for creating orders
- **PersonOrderModel** - Person details in order context

**Location:** `Momentum.Services\Models\EventOrderModel.cs`

#### Service Interface & Implementation:
- **IEventOrderService** - Interface defining order operations
- **EventOrderService** - Implementation with CRUD operations

**Locations:** 
- Interface: `Momentum.Services\Interfaces\IEventOrderService.cs`
- Implementation: `Momentum.Services\EventOrderService.cs`

**Features:**
- `Get()` - Retrieve all event orders
- `GetById(id)` - Retrieve specific event order
- `Create(request)` - Create new event order
- `Update(id, request)` - Update existing order
- `Delete(id)` - Delete event order

---

### 3. **API Layer** (`Momentum.API`)

#### API Models:
- **EventOrderResponse** - Response model for API clients
- **EventOrderProductResponse** - Product response model
- **PersonResponse** - Person details in API response
- **EventResponse** - Event details in API response
- **ProductResponse** - Product details in API response

**Location:** `Momentum.API\Models\EventOrderModels.cs`

#### API Controller:
**EventOrderController** - Handles HTTP requests for event orders

**Location:** `Momentum.API\Controllers\EventOrderController.cs`

**Endpoints:**
```
GET    /eventorder           - Get all event orders
GET    /eventorder/{id}      - Get specific event order
POST   /eventorder           - Create new event order
PUT    /eventorder/{id}      - Update event order
DELETE /eventorder/{id}      - Delete event order
```

---

### 4. **Persistence Layer** (`Momentum.Persistance`)

#### Entity Configuration:
- **EventOrderConfiguration** - EF Core configuration for EventOrderEntity
- **EventOrderProductConfiguration** - EF Core configuration for EventOrderProductEntity

**Location:** `Momentum.Persistance\EntityConfiguration\EventOrderConfiguration.cs`

#### Database Migration:
- **20260307120000_AddEventOrderEntity** - Creates EventOrders and EventOrderProducts tables

**Locations:**
- Migration: `Momentum.Persistance\Migrations\20260307120000_AddEventOrderEntity.cs`
- Designer: `Momentum.Persistance\Migrations\20260307120000_AddEventOrderEntity.Designer.cs`

---

### 5. **BackOffice (Blazor) Layer** (`Momentum.BackOffice`)

#### Service Interface (Refit Client):
- **IEventOrderService** - Refit HTTP client for communicating with API

**Location:** `Momentum.BackOffice\Services\IEventOrderService.cs`

#### ViewModels:
- **CreateEventOrderViewModel** - Main wizard state model
- **PersonOptionViewModel** - Person selection options
- **ProductSelectionViewModel** - Available products
- **SelectedProductViewModel** - Selected products with quantities

**Location:** `Momentum.BackOffice\Features\EventOrders\CreateEventOrderViewModel.cs`

#### Blazor Pages:
- **CreateEventOrderPage.razor** - UI markup for the wizard
- **CreateEventOrderPage.razor.cs** - Code-behind logic

**Locations:**
- Razor: `Momentum.BackOffice\Features\EventOrders\CreateEventOrderPage.razor`
- Code-behind: `Momentum.BackOffice\Features\EventOrders\CreateEventOrderPage.razor.cs`

**Route:** `/create-event-order`

---

## Step-by-Step User Flow

### Step 1: Select Person
- User sees a dropdown with all available persons
- Can search by name or CNP
- Selection is required to proceed

### Step 2: Event Details
- User enters event name (required)
- Selects event type from dropdown:
  - Wedding
  - Birthday
  - Teambuilding
  - Festival
- Selects event date using date picker
- All fields are required

### Step 3: Select Products
- User views all available products in a card layout
- Each product shows:
  - Product name
  - Type (Location, Food, Photography, Music, Videography)
  - Description (if available)
  - Price
  - Add/Remove button
- After selection, user can adjust quantities in a table
- Minimum one product required

### Step 4: Review & Confirm
- Summary of selected person
- Summary of event details
- Table of selected products with quantities and total prices
- Total order price displayed
- Submit button to create the order

---

## Integration Points

### Routes Configuration
**File:** `Momentum.Shared\Routes.cs`

Added routes:
```csharp
public static class EventOrders
{
    public const string Get = "/eventorder";
    public const string GetById = "/eventorder/{id}";
    public const string Create = "/eventorder";
    public const string Update = "/eventorder/{id}";
    public const string Delete = "/eventorder/{id}";
}
```

### Navigation
**File:** `Momentum.BackOffice\PageRoutes.cs`

Added route:
```csharp
public const string CreateEventOrder = "create-event-order";
```

**File:** `Momentum.BackOffice\Layout\MainLayout.razor.cs`

Added navigation menu item:
```csharp
_navItems.Add(new NavItem { Id = "14", Href = PageRoutes.CreateEventOrder, IconName = IconName.PlusCircle, Text = "Create Event Order" });
```

### Dependency Injection
**File:** `Momentum.Services\Extensions\ServiceCollectionExtensions.cs`

Added service registration:
```csharp
services.AddScoped<IEventOrderService, EventOrderService>();
```

**File:** `Momentum.Persistance\MomentumDbContext.cs`

Added DbSet:
```csharp
public virtual DbSet<EventOrderEntity> EventOrders { get; set; } = null!;
```

---

## Key Features

1. **Multi-Step Wizard Interface**
   - Clean step-by-step user experience
   - Progress bar showing current step
   - Previous/Next navigation
   - Form validation at each step

2. **Product Management**
   - Interactive product selection with visual feedback
   - Quantity adjustment for each product
   - Real-time price calculation
   - Total order value display

3. **Data Relationships**
   - EventOrder links Person to Event
   - EventOrderProduct tracks individual product quantities and prices
   - Cascading deletes for data integrity

4. **API Integration**
   - Full REST API for event order management
   - Refit HTTP client for Blazor communication
   - Error handling and validation
   - Success/error notifications

5. **Database Persistence**
   - EF Core entity configuration
   - Migration for database schema
   - Foreign key constraints and cascading deletes
   - Timestamp tracking (CreatedAt, UpdatedAt)

---

## Testing the Implementation

### 1. Create a Person
Navigate to Persons page and create a test person

### 2. Create Products
Navigate to Products page and create test products

### 3. Create an Event Order
1. Navigate to "Create Event Order" in the sidebar
2. Step 1: Select a person
3. Step 2: Fill in event details
4. Step 3: Select products and adjust quantities
5. Step 4: Review and confirm
6. System will:
   - Create the event with specified details
   - Create the event order linking person to event
   - Add selected products to the order
   - Show success notification
   - Redirect to events page

### 4. Verify in Database
Query EventOrders and EventOrderProducts tables to verify data was saved correctly

---

## Files Created

### Domain
- `Momentum.Domain\Entities\EventOrderEntity.cs`

### Services
- `Momentum.Services\Models\EventOrderModel.cs`
- `Momentum.Services\Interfaces\IEventOrderService.cs`
- `Momentum.Services\EventOrderService.cs`

### API
- `Momentum.API\Models\EventOrderModels.cs`
- `Momentum.API\Controllers\EventOrderController.cs`

### Persistence
- `Momentum.Persistance\EntityConfiguration\EventOrderConfiguration.cs`
- `Momentum.Persistance\Migrations\20260307120000_AddEventOrderEntity.cs`
- `Momentum.Persistance\Migrations\20260307120000_AddEventOrderEntity.Designer.cs`

### BackOffice
- `Momentum.BackOffice\Services\IEventOrderService.cs`
- `Momentum.BackOffice\Features\EventOrders\CreateEventOrderViewModel.cs`
- `Momentum.BackOffice\Features\EventOrders\CreateEventOrderPage.razor`
- `Momentum.BackOffice\Features\EventOrders\CreateEventOrderPage.razor.cs`

### Files Modified
- `Momentum.Shared\Routes.cs` - Added EventOrders routes
- `Momentum.BackOffice\PageRoutes.cs` - Added CreateEventOrder route
- `Momentum.BackOffice\Layout\MainLayout.razor.cs` - Added navigation menu item
- `Momentum.Services\Extensions\ServiceCollectionExtensions.cs` - Registered IEventOrderService
- `Momentum.Persistance\MomentumDbContext.cs` - Added EventOrders DbSet
- `Momentum.API\Extensions\MappingExtensions.cs` - Added mapping extension

---

## Future Enhancements

1. **Event Selection Instead of Creation**
   - Allow users to select existing events instead of creating new ones
   - Add "Create new event" option in wizard

2. **Order Management**
   - List all event orders
   - Edit existing orders
   - Cancel orders

3. **Calculations**
   - Discounts and promotions
   - Tax calculations
   - Payment information

4. **Notifications**
   - Email confirmation
   - SMS notifications
   - Reminder emails

5. **Reporting**
   - Orders by person
   - Orders by event type
   - Revenue reports
   - Product popularity

---

## Troubleshooting

### Event Not Created
- Ensure EventService is properly injected
- Check API is responding to POST /event requests
- Verify event details are correctly formatted

### Products Not Loading
- Verify ProductService is injected
- Check API response from GET /product
- Ensure products exist in database

### Order Not Saved
- Check EventOrderService injection
- Verify all required fields are populated
- Check database connection
- Review migration applied successfully

---

## Summary

This implementation provides a complete, production-ready event order creation system with:
- ✅ Clean architecture across all layers
- ✅ Type-safe API communication
- ✅ User-friendly multi-step wizard
- ✅ Full CRUD operations
- ✅ Database persistence with migrations
- ✅ Error handling and validation
- ✅ Real-time calculations
- ✅ Navigation integration
