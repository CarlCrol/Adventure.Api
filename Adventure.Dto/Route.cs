namespace Adventure.Dto;

public record Route(string Decision, string Comment, ICollection<Route> SubRoutes);
