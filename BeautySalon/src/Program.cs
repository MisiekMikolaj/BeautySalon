using BeautySalon.ApplicationServices.Components;
using BeautySalon.DataAcces.Data;
using BeautySalon.DataAcces.Data.Entities.Stuff;
using BeautySalon.DataAcces.Data.Entities.Users;
using BeautySalon.DataAcces.Data.Repositories;
using BeautySalon.UI;
using BeautySalon.UI.UserInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


var services = new ServiceCollection();
services.AddSingleton<IApp, App>();

services.AddSingleton<IRepository<Employee>, SqlRepository<Employee>> ();
services.AddSingleton<IRepository<Boss>, SqlRepository<Boss>>();
services.AddSingleton<IRepository<Client>, SqlRepository<Client>>();
services.AddSingleton<IRepository<Service>, SqlRepository<Service>>();
services.AddSingleton<IRepository<WorkSchedule>, SqlRepository<WorkSchedule>>();
services.AddSingleton<IRepository<Day>, SqlRepository<Day>>();
services.AddSingleton<IRepository<Houer>, SqlRepository<Houer>>();
services.AddSingleton<IUserComunication, UserComunication>();
services.AddSingleton<ITxtReader, TxtReader>();
services.AddSingleton<ITxtWritter, TxtWritter>();

services.AddDbContext<BeautySalonDbContext>(
        options => options.UseSqlServer(@"Data Source=DESKTOP-24JQ58J\SQLEXPRESS;Initial Catalog=lol;Integrated Security=True"));

var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetRequiredService<IApp>();

app.Run();
