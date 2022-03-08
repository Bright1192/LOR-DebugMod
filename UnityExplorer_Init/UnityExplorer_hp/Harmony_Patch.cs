﻿using BaseMod;
using HarmonyLib;
using HarmonyLib.Internal.Patching;
using HarmonyLib.Public.Patching;
using MonoMod.Cil;
using MonoMod.Utils;
using Opening;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityExplorer;

namespace UnityExplorer_hp
{
    // Token: 0x02000002 RID: 2
    public class Harmony_Patch
    {

        public Harmony_Patch()
        {
            try
            {
                Harmony_Patch.path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                Harmony harmony = new Harmony(base.GetType().Namespace);
                MethodInfo method = typeof(Harmony_Patch).GetMethod("StopOpening");
                harmony.Patch(typeof(GameOpeningController).GetMethod("PlayOpening", AccessTools.all), new HarmonyMethod(method), null, null, null, null);
                method = typeof(Harmony_Patch).GetMethod("BattleUnitCardsInHandUI_Update");
                harmony.Patch(typeof(BattleUnitCardsInHandUI).GetMethod("Update", AccessTools.all), new HarmonyMethod(method), null, null, null, null);
                
            }
            catch (Exception message)
            {
                Debug.LogError(message);
            }
        }
        // Token: 0x06000005 RID: 5 RVA: 0x0000233C File Offset: 0x0000053C
        public static bool StopOpening()
        {
            GameOpeningController.Instance.StopOpening();
            ExplorerStandalone.CreateInstance();
            return false;
        }

        // Token: 0x06000008 RID: 8 RVA: 0x000023B0 File Offset: 0x000005B0
        public static void BattleUnitCardsInHandUI_Update(BattleUnitCardsInHandUI __instance)
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                Toggle toggle = __instance.toggle_ShowEgo;
                toggle.isOn = !toggle.isOn;
                SingletonBehavior<BattleUnitCardsInHandUI>.Instance.OnClickEgoButton();
            }
        }


        public static Type[] GetAllChildOverrideMethodClass(Type baseType, string method)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(delegate (Assembly a)
            {
                IEnumerable<Type> types = a.GetTypes();
                Func<Type, bool> predicate = (Type t) => t.BaseType == baseType;
                IEnumerable<Type> source = types.Where(predicate);
                Func<Type, bool> predicate2 = (Type t2) => t2.GetMethod(method).DeclaringType != baseType;
                return source.Where(predicate2);
            }).ToArray<Type>();
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002444 File Offset: 0x00000644
        public static void PatchAllChildOverrideMethod(Type baseType, string method, Harmony harmony, HarmonyMethod harmonyMethod, bool ispre)
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Func<Assembly, IEnumerable<Type>> selector = delegate (Assembly a)
            {
                IEnumerable<Type> types = a.GetTypes();
                Func<Type, bool> predicate = (Type t) => t.BaseType == baseType;
                return types.Where(predicate).ToArray<Type>();
            };
            foreach (Type type in assemblies.SelectMany(selector))
            {
                MethodInfo method2 = type.GetMethod(method);
                if (method2 != method2.GetBaseDefinition())
                {
                    harmony.Patch(method2, ispre ? harmonyMethod : null, ispre ? null : harmonyMethod, null, null, null);
                }
            }
        }

        // Token: 0x04000001 RID: 1
        public static string path = string.Empty;

    }
}