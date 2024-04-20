using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class testFramework : MonoBehaviour
    {

        private ActorEventDispatcher _event;
        /// <summary>
        /// 局部事件管理器。
        /// <remark>只分发和监听这个Event内部的事件</remark>
        /// </summary>
        public ActorEventDispatcher Event => _event ??= MemoryPool.Acquire<ActorEventDispatcher>();

        async void Start()
        {
            //OnTestListRemove();

            // owner局部发送事件。
            // 测试actorevent的删除事件：
            var owner = this;
            //object owner1 = new(), owner2 = new();  
            owner.Event.AddEventListener<string>(RuntimeId.ToRuntimeId("testEventId"), EventCallBack1, owner);
            owner.Event.AddEventListener<string>(RuntimeId.ToRuntimeId("testEventId"), EventCallBack2, owner);
            owner.Event.AddEventListener<string>(RuntimeId.ToRuntimeId("testEventId"), EventCallBack3, owner);
            owner.Event.AddEventListener<string>(RuntimeId.ToRuntimeId("testEventId"), EventCallBack4, owner);

            TestAScynDeleteEvent();
            while (true)
            {
                owner.Event.SendEvent<string>(RuntimeId.ToRuntimeId("testEventId"), "testEvent");
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }
        }
        private async void EventCallBack1(string eventID)
        {
            //await EventCallBackImp();
            Log.Warning("EventCallBack1, eventID:{0}", eventID);
        }
        private async UniTask EventCallBackImp()
        {
            Log.Warning("EventCallBack1, start!");
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            Log.Warning("EventCallBack1, end!");
        }

        private void EventCallBack2(string eventID) { Log.Warning("EventCallBack2, eventID:{0}", eventID); }
        private void EventCallBack3(string eventID) { Log.Warning("EventCallBack3, eventID:{0}", eventID); }
        private void EventCallBack4(string eventID) { Log.Warning("EventCallBack4, eventID:{0}", eventID); }

        private async UniTask TestAScynDeleteEvent()
        {
            //Event.RemoveAllListenerByOwner(this);
            Log.Warning("TestAScynDeleteEvent start, time: {0}", Time.realtimeSinceStartupAsDouble);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Event.RemoveEventListener<string>(RuntimeId.ToRuntimeId("testEventId"), EventCallBack1);
            Log.Warning("TestAScynDeleteEvent end, time: {0}", Time.realtimeSinceStartupAsDouble);
        }

        private void OnDestroy()
        {
            if (_event != null)
            {
                MemoryPool.Release(_event);
            }
        }
    }
    /// <summary>
    /// 游戏框架模块抽象类。ModuleImp为具体框架模块实现。
    /// </summary>
    internal abstract class ModuleImp
    {
        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        internal virtual int Priority => 0;

        /// <summary>
        /// 游戏框架模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal abstract void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 关闭并清理游戏框架模块。
        /// </summary>
        internal abstract void Shutdown();
    }

    //=====================================================================//

    /// <summary>
    /// 游戏框架模块抽象类。Module 为Mono调用层。
    /// </summary>
    public abstract class Module : MonoBehaviour
    {
        /// <summary>
        /// 游戏框架模块初始化。
        /// </summary>
        protected virtual void Awake()
        {
            //ModuleSystem.RegisterModule(this);
        }
    }
}