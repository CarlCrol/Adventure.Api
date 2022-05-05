using System.ComponentModel.DataAnnotations;

namespace Adventure.Dto;

public record Adventure([Required] string Title, [Required] ICollection<Route> Routes);