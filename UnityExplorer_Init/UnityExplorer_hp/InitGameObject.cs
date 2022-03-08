using System;
using UnityEngine;

namespace UnityExplorer_hp
{
    // Token: 0x02000005 RID: 5
    public class InitGameObject : MonoBehaviour
    {
        // Token: 0x06000012 RID: 18 RVA: 0x000020E6 File Offset: 0x000002E6
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F8))
            {
                if (Harmony_Patch.InitUE != "ison")
                {
                    Harmony_Patch.InitUE = "ison";
                    Harmony_Patch.InitUnityExplorer();
                    return;
                }
                Harmony_Patch.InitUE = "noton";
            }
        }

        // Token: 0x06000013 RID: 19 RVA: 0x0000211F File Offset: 0x0000031F
        public InitGameObject()
        {
        }
    }
}
