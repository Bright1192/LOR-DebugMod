using HarmonyLib;
using MonoMod.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UI;
using UnityEngine;
using Workshop;

namespace UnityExplorer_hp
{
    /// <summary>
    /// 值得注意的是在执行任何Patch后原本方法实际上已经不存在了
    /// </summary>
    public static class Patchs
    {
        /// <summary>
        /// Prefix在Patch的方法前运行
        /// </summary>
        /// <param name="__originalMethod">意义不明，ilcpp修补静态方法似乎必须使用</param>
        /// <param name="__instance">修补非静态方法的实例</param>
        /// <param name="__runOriginal">检查是否有其他Prefix返回false,意义不明</param>
        /// <param name="__state">用于传递给修补同一方法且与自身在同一方法的Postfix,意义不明</param>
        /// <param name="__result">修改返回值</param>
        /// <returns>返回false阻止原本方法运行</returns>		
        public static bool Prefix(MethodBase __originalMethod, object __instance, ref object __result, out object __state, ref bool __runOriginal)
        {
            __state = null;
            return false;
        }

        /// <summary>
        /// Postfix在Patch的方法后运行
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerator Postfix(IEnumerator values)
        {
            return null;
        }

        /// <summary>
        /// 修改il代码，此方法只会执行一次
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {

            return null;
        }

        /// <summary>
        /// 终结器是一种使Harmony将原始方法和所有其他补丁包装在try/catch块中的方法,它可以接收抛出的异常，禁止显示异常或返回不同的异常
        /// </summary>
        /// <param name="__exception"></param>
        /// <returns></returns>
        public static Exception Finalizer(Exception __exception)
        {

            return null;
        }

        /// <summary>
        /// ILManipulator是Transpier的替代品
        /// </summary>
        /// <param name="il"></param>
        /// <param name="original"></param>
        /// <param name="retLabel"></param>
        public static void ILManipulator(ILContext il, MethodBase original, ILLabel retLabel)
        {

        }
    }
}
