namespace Contracts.DTOs;

public record HostProfileDto(
    string? PropertyDescription,
    string? NeighborhoodDescription,
    string? HostDescription,
    string? HostName
    );