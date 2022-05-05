using System.ComponentModel.DataAnnotations;

namespace Adventure.Dto;

public record UserAdventure([Required] User User, [Required] Adventure Adventure, [Required] ICollection<SelectedRoute> SelectedRoutes);