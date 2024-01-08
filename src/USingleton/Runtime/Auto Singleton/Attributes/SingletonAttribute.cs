using System;

namespace USingleton.AutoSingleton
{
    /// <summary>
    /// SingletonAttribute는 클래스의 단일 인스턴스만 애플리케이션에 존재해야 할 경우 사용됩니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SingletonAttribute : Attribute
    {
        /// <summary>
        /// 프리팹의 이름.
        /// </summary>
        public string PrefabName { get; }

        /// <summary>
        /// 속성 UseAddressable이 참인지 거짓인지를 나타냅니다.
        /// </summary>
        /// <value>
        /// 속성 UseAddressable이 참이면 <c>true</c>, 그렇지 않으면 <c>false</c>.
        /// </value>
        public bool UseAddressable { get; }

        /// <summary>
        /// 매니저로 전환합니다.
        /// </summary>
        /// <param name="prefabName">프리팹으로 변환할 때 사용할 이름</param>
        /// <param name="useAddressable">Addressable Asset System 스타일을 사용합니까? (요구사항: Addressable Asset System) </param>
        public SingletonAttribute(string prefabName, bool useAddressable = false)
        {
            PrefabName = prefabName;
#if !USE_ADDRESSABLES && USINGLETON_USE_ADDRESSABLE
            UseAddressable = false;
#else
            UseAddressable = useAddressable;
#endif
        }
    }
}