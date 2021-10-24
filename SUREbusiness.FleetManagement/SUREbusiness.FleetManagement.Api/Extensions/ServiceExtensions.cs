using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SUREbusiness.FleetManagement.BLL.Commands;
using SUREbusiness.FleetManagement.BLL.Models;
using SUREbusiness.FleetManagement.BLL.Services;
using SUREbusiness.FleetManagement.BLL.Validators;
using SUREbusiness.FleetManagement.DAL.Entities;
using SUREbusiness.FleetManagement.DAL.Repositories;
using System;

namespace SUREbusiness.FleetManagement.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            return services.AddScoped<IVehicleLicensePlateExistsService, VehicleLicensePlateExistsService>();
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IVehicleRepository, VehicleRepository>();
        }

        public static IServiceCollection ConfigureMappers(this IServiceCollection services)
        {
            return services.AddAutoMapper(config =>
            {
                config.CreateMap<Vehicle, VehicleEntity>().ReverseMap();
                config.CreateMap<CreateVehicleCommand, Vehicle>();
                config.CreateMap<VehicleEntity, VehiclePatchModel>();
            });
        }

        public static IServiceCollection ConfigureValidators(this IServiceCollection services)
        {
            return services
                .AddScoped<IValidator<Vehicle>, VehicleValidator>();
        }

        public static IServiceCollection ConfigureClients(this IServiceCollection services, IConfiguration config)
        {
            var rdwOpenDataBaseUrl = config.GetValue<string>("RDWOpenDataBaseUrl");

            services.AddHttpClient("rdw", httpClient =>
            {
                httpClient.BaseAddress = new Uri(rdwOpenDataBaseUrl);
            });

            return services;
        }
    }
}
