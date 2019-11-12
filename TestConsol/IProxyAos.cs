using System;

namespace TestConsol
{
    public interface IProxy
    {
        int DoSomeWork(int a);

    }

    public class RealObject : IProxy
    {
        public int DoSomeWork(int a)
        {
            Console.WriteLine("Real method");
            return a;
        }
    }

    public class ProxyObject : IProxy
    {
        private RealObject real;

        public ProxyObject()
        {
            real = new RealObject();
        }

        public int DoSomeWork(int a)
        {
            // 메소드 수행 전에 하고 싶은 일?
            Console.WriteLine("Before Invoke");
            // 그리고 실제 메소드를 수행.
            int result = real.DoSomeWork(a);
            // 메소드 수행 후에 하고 싶은 일?
            Console.WriteLine("After Invoke");

            return result; // 리턴값을 조작하고 싶다면 여기서...
        }
    }
    class IProxyAos
    {
        /*
        static void Main(string[] args)
        {
            IProxy prox = new ProxyObject();
            prox.DoSomeWork(10);
            //IProxy real = new RealObject();
            //real.DoSomeWork(10);
        }
         */
    }
}
