using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momentum.Services.Models;
public class AddressModel
{
	public Guid Id { get; set; }
	public required string StreetName { get; set; }
	public required string StreetNumber { get; set; }
	public required string City { get; set; }
	public required string State { get; set; }
	public required string Country { get; set; }
	public string? PhoneNumber { get; set; }
	public required string Email { get; set; }
}
