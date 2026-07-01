global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using SmartServiceBooking.Application.Common.Interfaces;
global using SmartServiceBooking.Infrastructure.Identity.Services;
global using SmartServiceBooking.Infrastructure.Persistence;
global using SmartServiceBooking.Infrastructure.Persistence.Interceptors;
global using SmartServiceBooking.Infrastructure.Persistence.Options;
global using SmartServiceBooking.Domain.Entities.Identity;
global using System.Text;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using SmartServiceBooking.Domain.Entities.Service;
global using DomainServiceProvider = SmartServiceBooking.Domain.Entities.Service.ServiceProvider;

