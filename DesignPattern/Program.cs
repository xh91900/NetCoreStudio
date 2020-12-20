using DesignPattern.Decorator;
using System;
using System.Net.Http;
using IdentityModel.Client;
using System.Threading.Tasks;
using IdentityModel;
using System.Threading;

namespace DesignPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.ThreadPool.GetMinThreads(out int q, out int a);
            var e = q;
            var r = a;


            while (true)
            {
                Task.Run(Producer);
                Thread.Sleep(200);
            }

            IdentityModel();

            // 装饰器
            StudentBase student = new Student()
            {
                Id = 1,
                Name = "altman"
            };

            // 给student包了一层，装饰了一下。
            student = new DecoratorBase(student);
            student = new StudentDecoratorVideo(student);//再装饰一层
            student = new StudentDecoratorHomeWork(student);
            student.Study();
        }

       async static void Producer()
        {
            var result = await Process();
        }

        static async Task<bool> Process()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(1000);
            });

            Console.WriteLine("Ended - " + DateTime.Now.ToLongTimeString());
            return true;
        }


        /// <summary>
        /// 安装IdentityModel包
        /// 可以使用discovery endpoint（ids4各种端点的地址）
        /// </summary>
        private static void IdentityModel()
        {
            var client = new HttpClient();

            //1.拿DiscoveryDocument
            var disco1 = Task.Run(async () =>
             {
                 var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000/");
                 return disco;
             }).GetAwaiter().GetResult();
            if (disco1.IsError)
            {
                Console.WriteLine(disco1.Error);
                return;
            }

            //2.拿token
            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = disco1.TokenEndpoint,//token端点从discovery endpoint里来
                ClientId = "console client",
                ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A",//客户端模式的凭证（只有这一个）
                Scope = "api1"//Scope不能为空,多个用空格分开openid
            }).Result;
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            //3.请求资源
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = client.GetAsync("http://localhost:5002/WeatherForecast").Result;//disco1.UserInfoEndpoint身份信息的地址
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }

            Console.ReadLine();
        }
    }
}
