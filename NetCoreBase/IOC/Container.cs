using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NetCoreBase.IOC
{
    public interface IContainer
    {
        void Register<F, T>(string instanceName = null, LifeTime lifeTime = LifeTime.Transient) where T : F;

        F Resolve<F>(string instanceName = null);

        IContainer CreateChildContainer();
    }


    public class Container : IContainer
    {

        /// <summary>
        /// 创建子容器用于实现作用域生命周期的注入
        /// 这里的this.ContainerDictionary是把源对象的注册关系传递给子对象
        /// 每次创建子容器，它里面的单例对象都是空的，全新的
        /// </summary>
        /// <returns></returns>
        public IContainer CreateChildContainer()
        {
            return new Container(this.ContainerDictionary,new Dictionary<string, object>());
        }

        /// <summary>
        /// 调用构造函数的时候会创建一个新的对象，这里的this.ContainerDictionary是新对象的ContainerDictionary
        /// </summary>
        /// <param name="container"></param>
        private Container(Dictionary<string, ContainerRegistModel> container,Dictionary<string, object> scopeContainerDictionary)
        {
            this.ContainerDictionary = container;
            this.ScopeContainerDictionary = scopeContainerDictionary;
        }

        public Container() { }

        private Dictionary<string, ContainerRegistModel> ContainerDictionary = new Dictionary<string, ContainerRegistModel>();
       
        //用于保存作用域单例的对象，因为作用域生命周期其实就是一个作用域创建一个子容器，然后容器里是一个单例
        private Dictionary<string, object> ScopeContainerDictionary = new Dictionary<string, object>();


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="F"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="instanceName">用于实现单接口多实现的注册</param>
        /// <param name="lifeTime">生命周期</param>
        public void Register<F, T>(string instanceName = null, LifeTime lifeTime = LifeTime.Transient) where T : F
        {
            this.ContainerDictionary.Add(GetKey(typeof(F).FullName, instanceName), new ContainerRegistModel() { TargetType = typeof(T), LifeTime = lifeTime });
        }

        private string GetKey(string fullName, string instanceName) => $"{fullName}-{instanceName}";

        public F Resolve<F>(string instanceName = null)
        {
            return (F)this.ResolveObject(typeof(F));
        }

        /// <summary>
        /// 递归实现获取注入的实例
        /// </summary>
        /// <param name="TargetType"></param>
        /// <param name="instanceName"></param>
        /// <returns></returns>
        private object ResolveObject(Type TargetType, string instanceName = null)
        {
            string key = this.GetKey(TargetType.FullName, instanceName);
            var model = this.ContainerDictionary[key];

            switch (model.LifeTime)
            {
                case LifeTime.Transient:; break;
                case LifeTime.Singleton:
                    if (model.SingletonInstance != null)
                    {
                        return model.SingletonInstance;
                    }
                    ; break;
                case LifeTime.Scope:
                    //作用域其实就是作用域子容器里的单例
                    if (this.ScopeContainerDictionary.ContainsKey(key))
                    {
                        return this.ScopeContainerDictionary[key];
                    }
                    ; break;
                default: break;
            }


            Type type = model.TargetType;


            //构造函数注入
            ConstructorInfo constructorInfo = null;
            //获取标记有特性的构造函数
            //如果都没有标记则获取参数最多的构造函数
            constructorInfo = type.GetConstructors().FirstOrDefault(p => p.IsDefined(typeof(ConstructorInjectionAttribute), true));
            if (constructorInfo == null)
            {
                constructorInfo = type.GetConstructors().OrderByDescending(p => p.GetParameters().Length).First();
            }
            //递归获取构造函数的参数
            List<object> paraList = new List<object>();
            foreach (var para in constructorInfo.GetParameters())
            {
                Type paraType = para.ParameterType;

                string parmInstanceName = GetInstanceName(para);

                object paraInstance = this.ResolveObject(paraType, parmInstanceName);
                paraList.Add(paraInstance);
            }
            object instance = Activator.CreateInstance(type, paraList.ToArray());


            //属性注入
            foreach (var prop in type.GetProperties().Where(p => p.IsDefined(typeof(PropertyInjectionAttribute), true)))
            {
                Type propType = prop.PropertyType;
                string parmInstanceName = GetInstanceName(prop);
                object propInstance = this.ResolveObject(propType, parmInstanceName);
                prop.SetValue(instance, propInstance);
            }


            //方法注入
            foreach (var method in type.GetMethods().Where(p => p.IsDefined(typeof(MethodInjectionAttribute), true)))
            {
                List<object> methodPara = new List<object>();
                foreach (var para in method.GetParameters())
                {
                    Type methodParaType = para.ParameterType;
                    string parmInstanceName = GetInstanceName(para);
                    object paraInstance = this.ResolveObject(methodParaType, parmInstanceName);
                    methodPara.Add(paraInstance);
                }

                //调用方法完成注入
                method.Invoke(instance, methodPara.ToArray());
            }

            switch (model.LifeTime)
            {
                case LifeTime.Transient:; break;
                case LifeTime.Singleton:
                    if (model.SingletonInstance == null)
                    {
                        model.SingletonInstance = instance;
                    }
                    ; break;
                case LifeTime.Scope:
                    ScopeContainerDictionary[key] = instance;
                    ; break;
                default: break;
            }

            return instance;
        }

        /// <summary>
        /// 获取单接口多实例 需要Resolve哪个实例的名字
        /// </summary>
        /// <param name="provider">为什么要用ICustomAttributeProvider,为了同时支持参数和属性，
        /// ParameterInfo和PropertyInfo都继承自ICustomAttributeProvider</param>
        /// <returns></returns>
        private string GetInstanceName(ICustomAttributeProvider provider)
        {
            if (provider.IsDefined(typeof(ParameterNameInjectionAttribute), true))
            {
                var attribute = (ParameterNameInjectionAttribute)(provider.GetCustomAttributes(typeof(ParameterNameInjectionAttribute), true)[0]);
                return attribute.InstanceName;
            }
            return null;
        }
    }
}
