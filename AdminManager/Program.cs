using AdminManager.Accessors;
using AdminManager.Service;
using AdminManager.Components;
using Quartz;
using Quartz.AspNetCore;
using Radzen;
using AdminManager.Model;
using AdminManager.Model.Shared;
using AdminManager.ModelView;
using Nelibur.ObjectMapper;
using AdminManager.ModelLog;

namespace AdminManager;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services);
        ConfigureMapper();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
           .AddInteractiveServerRenderMode();

        app.MapControllers();

        Startup(app.Services);

        app.Run();
    }

    private static void Startup(IServiceProvider appServices)
    { 
        var messageEventService = appServices.GetRequiredService<MessageService>();
        messageEventService.Load();

        var eventService = appServices.GetRequiredService<EventService>();
        eventService.Load();

        var adminMailService = appServices.GetRequiredService<AdminMailService>();
        adminMailService.Load();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddHttpClient();
        services.AddControllers();

        // Add Quartz services
        services.AddQuartz();
        services.AddQuartzServer(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });

        /*
        # Scoped, Singleton, 그리고 Transient는 .NET에서 주로 사용되는 Dependency Injection (DI) 컨테이너의 생명주기 관리 전략입니다. 이들은 서비스의 인스턴스를 어떻게 생성하고 재사용할지를 결정합니다.
        1.	Transient: Transient 생명주기를 가진 서비스는 요청할 때마다 새로운 인스턴스가 생성됩니다. 이는 서비스가 상태를 유지하지 않거나, 각 요청이 독립적이어야 하는 경우에 유용합니다.
        2.	Scoped: Scoped 생명주기를 가진 서비스는 요청을 처리하는 동안에만 인스턴스가 유지됩니다. 이는 요청 처리 중에 여러 서비스가 같은 인스턴스를 공유해야 하는 경우에 유용합니다.
        3.	Singleton: Singleton 생명주기를 가진 서비스는 애플리케이션 전체에서 하나의 인스턴스만 생성됩니다. 이는 애플리케이션 전체에서 공유해야 하는 상태를 유지하는 서비스에 유용합니다.
        */

        // Add Radzen services
        services.AddScoped<DialogService>();
        services.AddScoped<NotificationService>();
        services.AddScoped<TooltipService>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<IAdminMailService, AdminMailService>();

        services.AddSingleton<DapperProvider>();
        services.AddSingleton<GDTProvider>();
        services.AddSingleton<MongoProvider>();

        services.AddSingleton<IDBAccessor, DbAccessor>();
        services.AddSingleton<ILogAccessor, LogAccessor>();
        services.AddSingleton<IGDTAccessor, GDTAccessor>();
        services.AddSingleton<IRedisService, RedisService>();
        services.AddSingleton<IMapperService, MapperService>();

        services.AddSingleton<EventService>();
        services.AddSingleton<ContentService>();
        services.AddSingleton<MessageService>();
        services.AddSingleton<ScheduleService>();
        services.AddSingleton<AdminMailService>();
    }

    private static void ConfigureMapper()
    {
        TinyMapper.Bind<DateTimeModel, DateTimeModelView>();

        TinyMapper.Bind<AccountModel, AccountModelView>();
        TinyMapper.Bind<AccountTierModel, AccountTierModelView>();

        TinyMapper.Bind<CharacterModel, CharacterModelView>();
        TinyMapper.Bind<CharacterDetailModel, CharacterDetailModelView>();

        TinyMapper.Bind<CurrencyModel, CurrencyModelView>();
        TinyMapper.Bind<ItemModel, ItemModelView>();

        TinyMapper.Bind<MailModel, MailModelView>();
        TinyMapper.Bind<MailAttachItemModel, MailAttachItemModelView>();

        TinyMapper.Bind<QuestModel, QuestModelView>();
        TinyMapper.Bind<QuestTaskModel, QuestTaskModelView>();

        TinyMapper.Bind<LogCurrencyDataModel, LogCurrencyDataModelView>();

        TinyMapper.Bind<MessageModel, MessageModelView>();
        TinyMapper.Bind<List<MessageModel>, List<MessageModelView>>();

        TinyMapper.Bind<MessageModelView, MessageModel>();
        TinyMapper.Bind<List<MessageModelView>, List<MessageModel>>();

        TinyMapper.Bind<SystemMessageModel, SystemMessageModelView>();
        TinyMapper.Bind<List<SystemMessageModel>, List<SystemMessageModelView>>();

        TinyMapper.Bind<SystemMessageModelView, SystemMessageModel>();
        TinyMapper.Bind<List<SystemMessageModelView>, List<SystemMessageModel>>();

        TinyMapper.Bind<AirdropEventModel, AirdropEventModelView>();
        TinyMapper.Bind<List<AirdropEventModel>, List<AirdropEventModelView>>();

        TinyMapper.Bind<AirdropEventModelView, AirdropEventModel>();
        TinyMapper.Bind<List<AirdropEventModelView>, List<AirdropEventModel>>();

        TinyMapper.Bind<LogTierDataModel, LogTierDataModelView>();

        TinyMapper.Bind<LogMailDataModel, LogMailDataModelView>();

        TinyMapper.Bind<AdminMailModel, AdminMailModelView>();
        TinyMapper.Bind<List<AdminMailModel>, List<AdminMailModelView>>();

        TinyMapper.Bind<AdminMailAttachItemModel, AdminMailAttachItemModelView>();
        TinyMapper.Bind<List<AdminMailAttachItemModel>, List<AdminMailAttachItemModelView>>();

        TinyMapper.Bind<LogCharDataModel, LogCharDataModelView>();

        TinyMapper.Bind<LogShopDataModel, LogShopDataModelView>();
    }
}
