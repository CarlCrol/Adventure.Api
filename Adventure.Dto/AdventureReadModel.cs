using System.ComponentModel.DataAnnotations;

namespace Adventure.Dto;

public record AdventureReadModel(int Id, [Required] string Title, [Required] ICollection<Route> Routes) : Adventure(Title, Routes);