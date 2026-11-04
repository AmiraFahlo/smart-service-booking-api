using System;

namespace SmartServiceBooking.Shared.DTOs.Reviews
;
public record ReviewDto(
    string CustomerName,
    int Rating,
    string? Comment,
    DateTime CreatedAt);


