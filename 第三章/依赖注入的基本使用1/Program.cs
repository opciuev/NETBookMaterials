using Microsoft.Extensions.DependencyInjection;

//普通注册方式

//ServiceCollection services = new ServiceCollection();
//services.AddTransient<TestServiceImpl>();
//using (ServiceProvider sp = services.BuildServiceProvider())
//{
//    //TestServiceImpl testService = sp.GetRequiredService<TestServiceImpl>();
//    TestServiceImpl testService=sp.GetService<TestServiceImpl>();
//    testService.Name = "tom";
//    testService.SayHi();
//}
ServiceCollection services = new ServiceCollection();
//services.AddTransient<TestServiceImpl>();
//services.AddSingleton<TestServiceImpl>();
services.AddScoped<TestServiceImpl>();
using (ServiceProvider sp = services.BuildServiceProvider())
{
    //var ts1 = sp.GetRequiredService<TestServiceImpl>();
    //ts1.Name = "lily";
    //ts1.SayHi();
    //var ts2 = sp.GetRequiredService<TestServiceImpl>();
    //ts2.Name = "tom";
    //ts2.SayHi();
    //ts1.SayHi();

    //Console.WriteLine(object.ReferenceEquals(ts1, ts2));
    using (IServiceScope serviceScope = sp.CreateScope())
    {
        //在scope中获取Scope相关的对象，serviceScope.ServiceProvider获取的是scope中的ServiceProvider
        TestServiceImpl? ts1 = serviceScope.ServiceProvider.GetService<TestServiceImpl>();

        //var ts1 = sp.GetRequiredService<TestServiceImpl>();
        ts1.Name = "lily";
        ts1.SayHi();
        TestServiceImpl? ts2 = serviceScope.ServiceProvider.GetService<TestServiceImpl>();

        ts2.Name = "tom";
        ts2.SayHi();
        ts1.SayHi();
        Console.WriteLine(object.ReferenceEquals(ts1, ts2));
    }
}