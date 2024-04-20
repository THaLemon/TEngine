using System;
using System.Collections.Generic;
using TEngine;

namespace GameLogic {
    /// <summary>
    /// UI层事件接口。
    /// </summary>
    internal class RegisterEventInterface_UI {
        /// <summary>
        /// 注册UI层事件接口。
        /// </summary>
        /// <param name="mgr">事件管理器。</param>
        public static void Register(EventMgr mgr) {
            // rum:筛选出事件接口实现类
            HashSet<Type> types = CodeTypes.Instance.GetTypes(typeof(EventInterfaceImpAttribute));

            foreach (Type type in types) {
                object[] attrs = type.GetCustomAttributes(typeof(EventInterfaceImpAttribute), false);
                if (attrs.Length == 0) {
                    continue;
                }

                EventInterfaceImpAttribute httpHandlerAttribute = (EventInterfaceImpAttribute)attrs[0];
                // rum:筛选出UI层的事件监听类
                if (httpHandlerAttribute.EventGroup != EEventGroup.GroupUI) {
                    continue;
                }
                // rum:实例化该类并传递事件管理器进去
                object obj = Activator.CreateInstance(type, mgr.Dispatcher);
                // rum:将该类注册到事件管理器,接口与接口的实现类是一对一关系
                mgr.RegWrapInterface(obj.GetType().GetInterfaces()[0]?.FullName, obj);
            }
        }
    }
}