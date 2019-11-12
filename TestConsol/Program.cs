using System;
using System.Security.Permissions;

namespace TestConsol
{
    class Program 
    {
        
        [STAThread]
        static void Main(string[] args)
        {
            /*
            AutoCompleteAttribute_Example ace = new AutoCompleteAttribute_Example();

            ace.AutoCompleteAttribute_Ctor();
            ace.AutoCompleteAttribute_Ctor_Bool();
            ace.AutoCompleteAttribute_Value();
            */

            

            /* 어트리뷰트를 사용하지 않고 proxy를 사용하는 방법 
            Business obj = new Business("test");
            obj.foo();
            obj.bar();
            */

            //Console.WriteLine(obj.add(0, 0).ToString());
            //Console.WriteLine(obj.div(0, 0).ToString());
            //obj.foo();
            //obj.bar();

            string hotelType = "18";
            Console.WriteLine(hotelType.Substring(1));
        
        }
    }
    

   
}
