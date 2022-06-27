using App;
using Microsoft.EntityFrameworkCore;
using Tool.Compet.Core;

var builder = WebApplication.CreateBuilder(args);

// [Services]
builder.Services.AlsoDk(services => {
	var config = builder.Configuration;

	// Binds settings in appsettings.json file
	// We can obtain instances inside this method or other class
	// Ref: https://stackoverflow.com/questions/40470556/actually-read-appsettings-in-configureservices-phase-in-asp-net-core
	var serviceCollection = services.Configure<AppSetting>(config.GetSection(R.setting.app_setting_entry_name));

	// Configure DI (dependecy injection)
	// - Singleton: IoC container will create and share a single instance of a service throughout the application's lifetime.
	// - Transient: The IoC container will create a new instance of the specified service type every time you ask for it.
	// - Scoped: IoC container will create an instance of the specified service type once per request and will be shared in a single request.
	// Ref: https://www.tutorialsteacher.com/core/aspnet-core-introduction
	services
		.AddScoped<AuthService>()
		.AddScoped<AuthDao>()
		.AddScoped<UserService>()
		.AddScoped<UserDao>()
		.AddScoped<PlayerService>()
		.AddScoped<PlayerDao>()
		.AddScoped<AppService>()
		.AddScoped<CardanoNodeRepo>()
		.AddControllers();

		var appSetting = config.GetSection("App").Get<AppSetting>();

	// Config database connection
	services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(appSetting.connectionStrings.itself));

	// Config authentication
	// As above mention, we can obtain instance of setting here
	services.ConfigureJwtAuthenticationDk(appSetting);
});

// [App]
builder.Build().AlsoDk(app => {
	// At develop mode, we use own exception page for easy debugging
	if (app.Environment.IsDevelopment()) {
		app.UseDeveloperExceptionPage();
	}

	// app.ApplicationServices.GetService<JwtSetting>
	// var jwtSetting = serviceProvider.GetService<JwtSetting>();

	// We use https redirection at all except development env
	if (!app.Environment.IsDevelopment()) {
		app.UseHttpsRedirection();
	}

	// Use routing
	app.UseRouting();

	// Add authentication middleware (authenticate with JWT)
	app.UseAuthentication();

	// Requires authenticated access via [Authenticate] annotation
	app.UseAuthorization();

	// Add StaticFileMiddleware to pipeline, allow access static-file inside `wwwroot`.
	// For eg,. file at `public/html/helloworld.html` can be accessed via https://localhost:8080/html/helloworld.html
	app.UseStaticFiles();

	// Declare it to auto mapping route in controller classes
	app.UseEndpoints(endpoints => {
		endpoints.MapControllers();
	});

	app.Run();
});
