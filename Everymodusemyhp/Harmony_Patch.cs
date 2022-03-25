using GameSave;
using HarmonyLib;
using HarmonyLib.Public.Patching;
using Mod;
using Opening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace Everymodusemyhp
{
    public class DebugMono : MonoBehaviour
    {
        // Token: 0x06000013 RID: 19 RVA: 0x00003874 File Offset: 0x00001A74
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                Everymodusemyhpstatic.outputDll();
                Application.OpenURL(Everymodusemyhp.path + "/output");
            }
        }
    }
    public static class Everymodusemyhpstatic
    {       

        public static void CheckPriorty(string uniqueId, int PriortyCount)
        {

            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            SaveData saveData = Singleton<SaveManager>.Instance.LoadData(Singleton<ModContentManager>.Instance.savePath);
            if (saveData == null)
            {
                return;
            }
            SaveData data = saveData.GetData("orders");
            SaveData data2 = saveData.GetData("lastActivated");
            if (data != null)
            {

                foreach (SaveData saveData2 in data)
                {
                    string stringSelf = saveData2.GetStringSelf();
                    if (!string.IsNullOrEmpty(stringSelf))
                    {
                        list.Add(stringSelf);
                    }
                }
            }
            if (data2 != null)
            {
                foreach (SaveData saveData3 in data2)
                {
                    string stringSelf2 = saveData3.GetStringSelf();
                    if (!string.IsNullOrEmpty(stringSelf2))
                    {
                        list2.Add(stringSelf2);
                    }
                }
            }
            if (PriortyCount < 0)
            {
                PriortyCount = list.Count + PriortyCount;
            }
            if (PriortyCount < 0)
            {
                PriortyCount = 0;
            }
            if (PriortyCount >= list.Count)
            {
                PriortyCount = list.Count - 1;
            }
            if (list[PriortyCount] == uniqueId)
            {
                return;
            }
            if (!list.Remove(uniqueId))
            {
                return;
            }
            list.Insert(PriortyCount, uniqueId);
            SaveData saveData1 = new SaveData();
            SaveData saveData12 = new SaveData(SaveDataType.List);
            SaveData saveData13 = new SaveData(SaveDataType.List);
            foreach (string value in list)
            {
                saveData12.AddToList(new SaveData(value));
            }
            foreach (string value2 in list2)
            {
                saveData13.AddToList(new SaveData(value2));
            }
            saveData1.AddData("orders", saveData12);
            saveData1.AddData("lastActivated", saveData13);
            Singleton<SaveManager>.Instance.SaveData(Singleton<ModContentManager>.Instance.savePath, saveData1);

        }
        public static void DoHP()
        {
            try
            {

                Harmony harmony = new Harmony("Everymodusemyhp");
                MethodInfo method = typeof(Everymodusemyhpstatic).GetMethod("StopOpening");
                harmony.Patch(typeof(GameOpeningController).GetMethod("StopOpening", AccessTools.all), new HarmonyMethod(method), null, null, new HarmonyMethod(typeof(Everymodusemyhpstatic).GetMethod("Finalizer")),  null);
            }
            catch (Exception message)
            {
                Debug.LogError(message);
            }
        }
        public static Exception Finalizer(Exception __exception)
        {
            if (__exception != null)
            {
                Debug.LogError(__exception);

                Application.OpenURL(Application.persistentDataPath + "/Player.log");
                Application.OpenURL(Everymodusemyhp.path + "/output");
                Application.Quit();
            }
            return null;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x000039B4 File Offset: 0x00001BB4
        public static void LookWhatFuckHarmony_PatchDoing(Type type, string Method)
        {
            ManagedMethodPatcher methodPatcher = (ManagedMethodPatcher)type.GetMethod(Method, AccessTools.all).GetMethodPatcher();
            var Assembly = methodPatcher.hookBody.Method.Module.Assembly;
            var Replace = new Regex("[\\+\\<\\>:]+").Replace(methodPatcher.hookBody.Method.Name, ".");

            var dllname = Everymodusemyhp.path + "/output/" + Replace + ".dll";
            Debug.LogWarning(dllname);
            Assembly.Write(dllname);
        }
        public static void LookWhatFuckHarmony_PatchDoing(MethodInfo Method)
        {
            ManagedMethodPatcher methodPatcher = (ManagedMethodPatcher)Method.GetMethodPatcher();
            var Assembly = methodPatcher.hookBody.Method.Module.Assembly;
            var Replace = new Regex("[\\+\\<\\>:]+").Replace(methodPatcher.hookBody.Method.Name, ".");

            var dllname = Everymodusemyhp.path + "/output/" + Replace + ".dll";
            Debug.LogWarning(dllname);
            Assembly.Write(dllname);
        }

        // Token: 0x06000019 RID: 25 RVA: 0x00003A13 File Offset: 0x00001C13
        public static bool StopOpening(GameOpeningController __instance)
        {
            GameOpeningController.OnPlayEnd onPlayEnd = __instance._onPlayEnd;
            if (onPlayEnd != null)
            {
                onPlayEnd();
            }
            CheckPriorty("UnityExplorer", 0);

            return false;
        }

        public static void outputDll()
        {
            foreach (var x in PatchManager.MethodPatchers.Values)
            {
                var methodPatcher = (x as ManagedMethodPatcher);
                if (methodPatcher != null)
                {
                    var hookBody = methodPatcher.hookBody;
                    var Assembly = hookBody.Method.Module.Assembly;
                    var Replace = new Regex("[\\+\\<\\>:]+").Replace(hookBody.Method.Name, ".");
                    var dllname = Everymodusemyhp.path + "/output/" + Replace + ".dll";
                    Debug.LogWarning(dllname);
                    Assembly.Write(dllname);
                }
            }
        }
        public static IEnumerator InternetAccessCheck(Action<bool> callback, int timeOut = 4)
        {
            string url = "https://weibo.com/";
            UnityWebRequest request = new UnityWebRequest(url);
            request.timeout = timeOut;
            yield return request.SendWebRequest();
            if (!request.isDone)
            {
                callback(obj: false);
            }
            else
            {
                callback(obj: true);
            }
        }

    }
    // Token: 0x02000004 RID: 4
    public class Everymodusemyhp : ModInitializer
    {
        // Token: 0x06000016 RID: 22 RVA: 0x000038E0 File Offset: 0x00001AE0
        static Everymodusemyhp()
        {
            try
            {
                Everymodusemyhp.path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var CanvasRoot = new GameObject("[Debug]Everymodusemyhp");
                CanvasRoot.AddComponent<DebugMono>();
                UnityEngine.Object.DontDestroyOnLoad(CanvasRoot);
                CanvasRoot.hideFlags |= HideFlags.HideAndDontSave;
                CanvasRoot.layer = 5;
                DirectoryInfo directoryInfo2 = new DirectoryInfo(Everymodusemyhp.path + "/dlls");
                foreach (FileInfo fileInfo in directoryInfo2.GetFiles())
                {
                    if (fileInfo.Name.EndsWith(".dll"))
                    {

                        Assembly assembly = Assembly.LoadFile(fileInfo.FullPath);
                        if (fileInfo.Name == "UnityExplorer_Init.dll")
                        {
                            Activator.CreateInstance(assembly.GetType("UnityExplorer_hp.Harmony_Patch"));


                        }
                    }
                }
                if (!Directory.Exists(Everymodusemyhp.path + "/output"))
                {
                    Directory.CreateDirectory(Everymodusemyhp.path + "/output"); ;
                }               
                Everymodusemyhpstatic.DoHP();
            }

            catch (Exception message)
            {
                Debug.LogError(message);
            }
        }

        public override void OnInitializeMod()
        {
            base.OnInitializeMod();
        }

        // Token: 0x04000006 RID: 6
        public static string path = string.Empty;


    }
}
