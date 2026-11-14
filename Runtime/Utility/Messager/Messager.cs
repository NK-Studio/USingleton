using System;
using System.Collections.Generic;
using UnityEngine;

namespace USingleton.Utility
{
    /// <summary>
    /// Messager 클래스는 메시지를 등록하고 보내는 기능을 제공합니다.
    /// </summary>
    public static class Messager
    {
        /// <summary>
        /// 매개 변수와 반환 값이 없는 메서드를 정의하는 대리자를 나타냅니다.
        /// </summary>
        public delegate void DoObject();
        
        private static readonly Dictionary<string, DoObject> RegisteredMessages;
        
        static Messager()
        {
            // Init
            RegisteredMessages = new Dictionary<string, DoObject>();
        }

        /// <summary>
        /// 플레이 모드에 들어갈 때마다 등록된 메시지를 비웁니다.
        /// 도메인 리로드를 끈 경우(Enter Play Mode Options) 정적 필드가 이전 플레이의 등록을 그대로 들고 있어,
        /// 같은 이름을 다시 등록하면 경고가 뜨고 이미 파괴된 오브젝트의 메서드가 호출되는 것을 막습니다.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnPlay()
        {
            RegisteredMessages.Clear();
        }

        /// <summary>
        /// 주어진 메시지 이름으로 메시지를 등록합니다.
        /// </summary>
        /// <param name="messageName">메시지의 이름입니다.</param>
        /// <param name="doObject">등록하려는 메시지 동작입니다.</param>
        public static void RegisterMessage(string messageName, DoObject doObject)
        {
            if (!RegisteredMessages.TryAdd(messageName, doObject))
                Debug.LogWarning($"Messager: The item {messageName} already contains a reference to the message.");
        }

        /// <summary>
        /// RegisteredMessages에서 이름을 기준으로 메시지를 제거합니다.
        /// </summary>
        /// <param name="messageName">제거할 메시지의 이름입니다.</param>
        public static void RemoveMessage(string messageName)
        {
            if (RegisteredMessages.ContainsKey(messageName))
                RegisteredMessages.Remove(messageName);
        }

        /// <summary>
        /// 등록된 모든 메시지를 제거합니다.
        /// </summary>
        public static void RemoveAllMessages()
        {
            RegisteredMessages.Clear();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// 지정된 이벤트 이름으로 메시지를 보냅니다.
        /// </summary>
        /// <param name="eventName">이벤트의 이름입니다.</param>
        public static void Send(string eventName)
        {
            if (RegisteredMessages.TryGetValue(eventName, out DoObject message))
            {
                try
                {
                    message?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Messager: An exception of type {e.GetType().Name} was caught while sending the {eventName} message.");
                    Debug.LogException(e);
                }
            }
            else
            {
                Debug.LogWarning($"Messager: The {eventName} event is not registered.");
            }
        }
    }
}