
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.OutputCaching;
global using MediatR;
global using SmartServiceBooking.Domain.Shared;
global using SmartServiceBookingPlatform_API.Extensions;
global using FluentValidation;
global using Microsoft.AspNetCore.Diagnostics;
global using System.Diagnostics;
global using Microsoft.AspNetCore.Authorization;
global using SmartServiceBooking.Application.Features.ServiceProvider.Commands.CreateServiceProvider;
global using SmartServiceBooking.Domain.Constants;
global using SmartServiceBooking.Application.Features.ServiceProvider.Commands.UpdateServiceProvider;
global using SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetProvidersByService;
global using SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetServiceProviderById;
global using SmartServiceBooking.Application.Features.ServiceProvider.Commands.DeleteServiceProvider;
global using SmartServiceBookingPlatform_API.Features.ServiceProviders.Requests;
