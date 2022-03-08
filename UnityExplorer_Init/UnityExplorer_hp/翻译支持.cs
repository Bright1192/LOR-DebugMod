using CustomInvitation;
using MyJsonTool;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

namespace UnityExplorer_hp
{
    // Token: 0x0200000B RID: 11
    public static class 翻译支持
    {
        // Token: 0x06000031 RID: 49 RVA: 0x00002E6C File Offset: 0x0000106C
        public static void GetAllText()
        {
            翻译JsonList 翻译JsonList = File.ReadAllText(Harmony_Patch.path + "/mod编辑器翻译支持.json").ToObject<翻译JsonList>();
            if (翻译JsonList == null)
            {
                翻译JsonList = new 翻译JsonList();
            }
            foreach (Text text in MainManager.instance.gameObject.GetComponentsInChildren<Text>(true))
            {
                翻译Json 翻译Json;
                if (翻译JsonList.翻译头.TryGetValue(text.text, out 翻译Json))
                {
                    text.text = 翻译Json.CN;
                }
            }
            Dropdown[] componentsInChildren2 = MainManager.instance.gameObject.GetComponentsInChildren<Dropdown>(true);
            for (int k = 0; k < componentsInChildren2.Length; k++)
            {
                foreach (Dropdown.OptionData optionData in componentsInChildren2[k].options)
                {
                    翻译Json 翻译Json2;
                    if (翻译JsonList.翻译头.TryGetValue(optionData.text, out 翻译Json2))
                    {
                        optionData.text = 翻译Json2.CN;
                    }
                }
            }
        }
    }
}
