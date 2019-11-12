using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Services;
using System.Threading;
using System.Reflection;
using System.Text;

namespace TestConsol
{
    class MyProxy : RealProxy
    {
        //String myURIString;
        private MarshalByRefObject target;

        public MyProxy(MarshalByRefObject target, Type serverType) : base(serverType)
        {
            this.target = target;
        }
        public MyProxy(MarshalByRefObject target) : base(target.GetType())
        {
            this.target = target;
        }

        
          private bool MethodEnterLog(IMethodCallMessage callMsg)
          {
              // reflection을 통해 Log 특성이 메쏘드에 존재하는지 확인한다.
              MethodBase method = callMsg.MethodBase;
              object[] attributes = method.GetCustomAttributes(typeof(LogAttribute), false);
              if (attributes == null || attributes.Length == 0)
                  return false; // Log 특성이 없다면 아무런 작업도 하지 않는다.
              StringBuilder sb = new StringBuilder(128);
              if (callMsg is IConstructionCallMessage)
                  sb.Append(".ctor(");
              else
                  sb.AppendFormat("{0} {1}(",
                  ((MethodInfo)method).ReturnType.Name, method.Name);
              for (int i = 0; i < callMsg.ArgCount; i++)
              {
                  sb.AppendFormat("{0}={1}",
                  callMsg.GetArgName(i), callMsg.GetArg(i));
                  if (i < callMsg.ArgCount - 1)
                      sb.Append(',');
              }
              sb.Append(')');
              Console.WriteLine(sb.ToString(), "SM");
              return true;
          }
        

        
          private void MethodLeaveLog(IMethodReturnMessage retMsg)
          {
              if (retMsg is IConstructionReturnMessage)
              {
                  Console.WriteLine("end of construction", "EM");
              }
              else
              {
                  Console.WriteLine("end of method : ret = " +
                  retMsg.ReturnValue.ToString(), "EM");
              }
          }
          
        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage callMsg = msg as IMethodCallMessage;
            IMethodReturnMessage retMsg = null;
            bool fLog = false;

            //메서드 시작 로그
            fLog = MethodEnterLog(callMsg);

            // 생성자 메서드 일경우
            if (msg is IConstructionCallMessage)
            {
                // 객체 생성 메시지 처리
                IConstructionCallMessage ctorMsg = (IConstructionCallMessage)msg;
                RealProxy proxy = RemotingServices.GetRealProxy(target);
                // 실제 객체를 생성한다.
                proxy.InitializeServerObject(ctorMsg);
                // 객체 생성 결과를 ‘만들어’ 반환한다.
                retMsg = EnterpriseServicesHelper.CreateConstructionReturnMessage(ctorMsg, (MarshalByRefObject)this.GetTransparentProxy());
            }
            //일반 메서드 
            else if (msg is IMethodCallMessage)
            {
                /*
                // 파라메터 있는 메서드와 아닌 메서드 나눌때 
                if (callMsg.ArgCount > 0)
                {
                    // 파라메터 리스트 작성
                    Console.WriteLine("Parameter list :");

                    for (int i = 0; i < callMsg.ArgCount; i++)
                    {
                        Console.WriteLine("{0} | {1}: {2}", i, callMsg.GetArgName(i), callMsg.GetArg(i));
                    }
                    retMsg = RemotingServices.ExecuteMessage(target, callMsg);
                }
                else
                {
                    //메소드 실행 코드 
                    retMsg = RemotingServices.ExecuteMessage(target, callMsg);
                }
                */
                retMsg = RemotingServices.ExecuteMessage(target, callMsg);
            }

            if (retMsg.Exception != null)
            {
                Console.WriteLine(retMsg.MethodName + " 에서 " + retMsg.Exception.Message);
                //Environment.Exit(0);

            }

            // 메소드 종료 로그
            if (fLog)
                MethodLeaveLog(retMsg);

            return retMsg;
            
        }
    }
    
    
    [AttributeUsage(AttributeTargets.Class)]
    class MyProxyAttribute : ProxyAttribute
    {
        public override MarshalByRefObject CreateInstance(Type serverType)
        {
            MarshalByRefObject target = base.CreateInstance(serverType);
            MyProxy proxy = new MyProxy(target, serverType);
            MarshalByRefObject obj = (MarshalByRefObject)proxy.GetTransparentProxy();
            return obj;
        }
        
    }
    
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property)]
    public class LogAttribute : Attribute
    {
        // 마크(mark)용 애트리뷰트이므로 내부 구현이 없다.
    }


    class AttributeAop
    {   
       
    }
}
