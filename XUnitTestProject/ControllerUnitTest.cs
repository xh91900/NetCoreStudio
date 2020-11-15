using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NetCoreWebBase;
using FluentAssertions;
using Xunit.Abstractions;
using System.Collections.Generic;
using Xunit.Sdk;
using System.Reflection;

namespace XUnitTestProject
{
    /// <summary>
    /// 用命令行执行测试 C:\Users\xh919\source\repos\NetCoreBase>dotnet test
    /// https://www.cnblogs.com/cgzl/p/9294431.html
    /// </summary>
    public class ControllerUnitTest
    {
        //自定义输出
        private readonly ITestOutputHelper _output;
        public ControllerUnitTest(ITestOutputHelper output)
        {
            _output = output;
        }


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
            _output.WriteLine("第一个测试");
            var context = GetContext();

            /*
             Moq
            针对隔离测试, 并不应该使用生产时用的依赖项, 所以我们使用模拟版本的依赖项
            提高测试运行速度, 例如可以模拟DB, Web Service等比较慢的服务, 以及算法等
            支持并行开发, 例如实际的依赖项还没有完成开发, 或者等待其他团队开发依赖项
             */
            var moq1 = new Moq.Mock<User>();
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

        /// <summary>
        /// 指定测试数据
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="expected"></param>
        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(2, 3, 5)]
        [InlineData(2, 3, 6)]
        public void TestHomeController(int x, int y, int expected)
        {
            Moq.Mock<MokInterface> mock = new Moq.Mock<MokInterface>();
            var sut = new User(mock.Object);
            var res = sut.add(x, y);
            Assert.Equal(expected, actual: res);
        }

        /// <summary>
        /// 可以共享测试数据
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="expected"></param>
        [Theory]
        [MemberData(nameof(TestData.TestDataList), MemberType = typeof(TestData))]
        public void TestHomeController1(int x, int y, int expected)
        {
            Moq.Mock<MokInterface> mock = new Moq.Mock<MokInterface>();

            //It代表需要被匹配的参数,It.IsAny<T>(), 它表示传递给方法的参数的类型只要是T就可以, 值是任意的. 只要满足了这个条件, 那么方法的返回值就是后边Returns()方法里设定的值.
            mock.Setup(p => p.MokMethod(Moq.It.IsAny<string>())).Returns("hello");
            var sut = new User(mock.Object);
            var res = sut.add(x, y);
            Assert.Equal(expected, actual: res);
        }

        /// <summary>
        /// 自定义数据源
        /// 也可以共享数据
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="expected"></param>
        [Theory]
        [TestData]
        public void TestHomeController2(int x, int y, int expected)
        {
            Moq.Mock<MokInterface> mock = new Moq.Mock<MokInterface>();
            var sut = new User(mock.Object);
            var res = sut.add(x, y);
            Assert.Equal(expected, actual: res);
        }


        /// <summary>
        /// 判断事件是否执行
        /// </summary>
        [Fact]
        [Trait("category", "new")]//测试用例分组
        public void RaiseEvent()
        {
            //Assert.Raises<EventArgs>();
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

    public class User
    {
        public MokInterface _MokInterface;

        public User(MokInterface mokInterface)
        {
            this._MokInterface = mokInterface;
        }
        public string name { get; set; }

        public int age { get; set; }

        public int add(int x, int y)
        {
            _MokInterface.MokMethod("");
            return x + y;
        }
    }

    public class TestData
    {
        private static readonly List<object[]> Data = new List<object[]>
        {
            new object[]{ 1,2,3},
            new object[]{ 1,3,4}
        };

        //这的数据源也可以是文件、数据库等，只要返回的是IEnumerable<object[]>就行。
        public static IEnumerable<object[]> TestDataList => Data;
    }

    /// <summary>
    /// 自定义数据源
    /// </summary>
    public class TestDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { 1, 2, 3 };
            yield return new object[] { 3, 2, 5 };
            yield return new object[] { 2, 2, 4 };
            yield return new object[] { 5, 2, 7 };
        }
    }

    public interface MokInterface
    {
        public string MokMethod(string par);
    }
}
