using DownloadOrganizer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(x =>
{
    x.ServiceName = "Download Organizer";
});
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
