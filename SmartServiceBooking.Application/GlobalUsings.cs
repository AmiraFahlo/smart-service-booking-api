global using MediatR;
global using Microsoft.EntityFrameworkCore;
global using SmartServiceBooking.Application.Common.Interfaces;
global using SmartServiceBooking.Domain.Shared;
global using SmartServiceBooking.Application.Common.Extensions;
global using SmartServiceBooking.Application.Features.Services.Extensions;
global using SmartServiceBooking.Application.Features.ServiceProvider.Extensions;
global using SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetProvidersByService;
global using SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetServiceProviderById;
global using SmartServiceBooking.Domain.Entities.Service;
global using FluentValidation;
global using SmartServiceBooking.Domain.Entities.Service.Specifications;
global using SmartServiceBooking.Domain.Constants;
global using SmartServiceBooking.Application.Features.Services.Queries.GetServiceById;
global using SmartServiceBooking.Application.Features.Services.Queries.GetServicesList;
global using SmartServiceBooking.Domain.Entities.Identity;
global using SmartServiceBooking.Application.Features.Auth;
global using SmartServiceBooking.Domain.Errors;
global using SmartServiceBooking.Shared.DTOs.Bookings;
global using SmartServiceBooking.Shared.DTOs.Categories;
global using SmartServiceBooking.Shared.DTOs.Reviews;
global using SmartServiceBooking.Shared.DTOs.ServiceProvider;
global using SmartServiceBooking.Shared.DTOs.Services;



