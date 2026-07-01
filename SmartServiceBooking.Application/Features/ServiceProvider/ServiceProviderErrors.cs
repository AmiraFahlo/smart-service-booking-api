namespace SmartServiceBooking.Application.Features.ServiceProvider;
public static class ServiceProviderErrors
{
    public static readonly Error AlreadyExists = new(
        "ServiceProvider.AlreadyExists", 
        "The user is already registered as a service provider.", 
        ErrorType.Conflict);
    
    public static readonly Error NotFound = new(
        "ServiceProvider.NotFound", 
        "The service provider was not found.", 
        ErrorType.NotFound);
}
