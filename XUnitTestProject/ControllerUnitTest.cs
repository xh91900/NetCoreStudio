using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NetCoreWebBase;
using FluentAssertions;

namespace XUnitTestProject
{
    public class ControllerUnitTest
    {
        private UserContext GetContext()
        {
            /*
             Microsoft.EntityFrameworkCore
            Microsoft.EntityFrameworkCore.InMemory
             */
            var options = new DbContextOptionsBuilder<UserContext>().UseInMemoryDatabase(databaseName: "noname").Options;

            var context = new UserContext(options);

            return context;
        }

        [Fact]
        public void TestHomeControllerGet()
        {
            var context = GetContext();

            /*
             Moq
             */
            var moq = new Moq.Mock<ICacheEntry>();//模拟对象

            var res = new HomeController().Details(1);
            Assert.IsType<Action>(res);

            /*
             FluentAssertions
            链式的判断类型、值
            object是一个具体的对象，s可以拿到对象里面的值
             */
            var result = res.Should().BeOfType<Action>().Subject;
            var s = result.Should().BeAssignableTo<object>().Subject;
        
        }
    }

    public class UserContext : DbContext
    {
        public UserContext()
        { }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        { }

        public DbSet<object> table { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }
    }
}
