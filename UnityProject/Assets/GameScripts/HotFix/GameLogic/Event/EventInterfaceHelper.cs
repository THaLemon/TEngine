﻿using System;
using TEngine;

namespace GameLogic {
    /// <summary>
    /// 事件接口帮助类。
    /// </summary>
    internal class EventInterfaceHelper {
        /// <summary>
        /// 初始化。
        /// </summary>
        public static void Init() {
            RegisterEventInterface_Logic.Register(GameEvent.EventMgr);
            RegisterEventInterface_UI.Register(GameEvent.EventMgr);
            //GameEvent.EventMgr.PrintEventEntryMap();//打印
            //var temp = GameEvent.Get<ILoginUI>();
            //Log.Warning(temp);
        }
    }
}