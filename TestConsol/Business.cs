using System;
using System.Reflection;
using System.EnterpriseServices;

namespace TestConsol
{

    [MyProxy]
    class Business : ContextBoundObject //MarshalByRefObject
    {
        /* 
        -- 비지니스에 프록시 객채를 생성하는 함수를 만들어서 실제 호출단에는 프록시 객체를 생성하는 과정을 보이지 않게한다.
        private Business() { }
        public static Business CreateInstance()
        {
            MyProxy proxy = new MyProxy(new Business());
            return (Business)proxy.GetTransparentProxy();
        }
        */

        public Business(string s)
        {
            Console.WriteLine("생성자 호출 !!! ");
        }
        
        [Log]
        public void foo()
        {
            Console.WriteLine("foo() 호출 !!!");
        }

        [Log]
        public void bar()
        {
            Console.WriteLine("bar() 호출 !!!");
        }
        
        public int add(int a, int b)
        {
            Console.WriteLine("add() 호출 !!!");
            return a + b;
        }

        [Log]
        [AutoComplete]
        public int div(int a, int b)
        {
            Console.WriteLine("div() 호출 !!!");
            return a / b;
        }
    }
}
