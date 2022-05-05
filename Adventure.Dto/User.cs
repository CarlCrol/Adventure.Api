using System.ComponentModel.DataAnnotations;

namespace Adventure.Dto;

public record User([Required] string Username);