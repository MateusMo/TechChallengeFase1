using Consumer;
using ContactZone.Application.Repositories;
using ContactZone.Application.Services;
using ContactZone.Infrastructure.Data;
using ContactZone.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Registrar o DbContext
builder.Services.AddDbContext<ContactZoneDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ContactZone"),
        b => b.MigrationsAssembly("ContactZone.Infrastructure"))
    , ServiceLifetime.Scoped);

// Registrar os repositórios e serviços
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();

// Configurar o MassTransit com RabbitMQ
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:HostName"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:UserName"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });

        // Configurar os consumidores
        cfg.ReceiveEndpoint("CreateQueue", e =>
        {
            e.Consumer<CreateContactConsumer>(context);
        });

        cfg.ReceiveEndpoint("DeleteQueue", e =>
        {
            e.Consumer<DeleteContactConsumer>(context);
        });

        cfg.ReceiveEndpoint("PutQueue", e =>
        {
            e.Consumer<UpdateContactConsumer>(context);
        });
    });
});

// Registrar o Worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();