using DI魅力渐显_依赖注入;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;

ServiceCollection services = new ServiceCollection();
services.AddScoped<IDbConnection>(sp => {
    string connStr = "Data Source=192.168.2.104,1433;Initial Catalog=test;User Id=sa;Password=Sankou3H;";
    var conn = new SqlConnection(connStr);
    conn.Open();
    return conn;
});
services.AddScoped<IUserDAO, UserDAO>();
services.AddScoped<IUserBiz, UserBiz>();
using (ServiceProvider sp = services.BuildServiceProvider())
{
    var userBiz = sp.GetRequiredService<IUserBiz>();
    bool b = userBiz.CheckLogin("user1", "password1");
    Console.WriteLine(b);
}
