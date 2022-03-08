using HarmonyLib;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace pvp
{
    // Token: 0x02000009 RID: 9
    [HarmonyPatch(typeof(StageController))]
    public static class StageController_Path
    {
        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000028 RID: 40 RVA: 0x000021C9 File Offset: 0x000003C9
        // (set) Token: 0x06000029 RID: 41 RVA: 0x000021D5 File Offset: 0x000003D5
        public static StageController.StagePhase phase
        {
            get
            {
                return Singleton<StageController>.Instance.Phase;
            }
            set
            {
                Singleton<StageController>.Instance._phase = value;
            }
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00002B08 File Offset: 0x00000D08
        [HarmonyPatch("OnFixedUpdateLate")]
        [HarmonyPrefix]
        public static bool OnFixedUpdateLate(MethodBase __originalMethod, StageController __instance, float deltaTime)
        {
            if (StageController_Path.LastRoundTurn != Singleton<StageController>.Instance.RoundTurn && StageController_Path.phase == StageController.StagePhase.ApplyEnemyCardPhase)
            {
                List<BattleUnitModel> list = BattleObjectManager.instance.GetAliveList(Faction.Player).FindAll((BattleUnitModel x) => x.passiveDetail.IsActionable() && x.bufListDetail.IsActionable());
                if (list != null && list.Count > 0)
                {
                    foreach (BattleUnitModel battleUnitModel in list)
                    {
                        if (battleUnitModel.bufListDetail.GetActivatedBufList().Exists((BattleUnitBuf x) => !x.IsControllable))
                        {
                            for (int i = 0; i < battleUnitModel.speedDiceResult.Count; i++)
                            {
                                if (!battleUnitModel.speedDiceResult[i].breaked && battleUnitModel.cardSlotDetail.cardAry[i] == null)
                                {
                                    HP.CanDoActionCount = 1;
                                    HP.NowDoActionFaction = Faction.Enemy;
                                    battleUnitModel.cardOrder = i;
                                    battleUnitModel.allyCardDetail.PlayTurnAutoForPlayer(i);
                                }
                            }
                        }
                    }
                    SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.UpdateTargetList();
                    SingletonBehavior<BattleManagerUI>.Instance.ui_unitInformationPlayer.ReleaseSelectedCard();
                    SingletonBehavior<BattleManagerUI>.Instance.ui_unitInformationPlayer.CloseUnitInformation(true, -1);
                    SingletonBehavior<BattleManagerUI>.Instance.ui_unitCardsInHand.OnPointerOverInSpeedDice = null;
                    SingletonBehavior<BattleManagerUI>.Instance.ui_unitCardsInHand.SetToDefault();
                    if (SingletonBehavior<BattleManagerUI>.Instance.ui_emotionInfoBar.autoCardButton != null)
                    {
                        SingletonBehavior<BattleManagerUI>.Instance.ui_emotionInfoBar.autoCardButton.SetActivate(false);
                    }
                    if (SingletonBehavior<BattleManagerUI>.Instance.ui_emotionInfoBar.unequipcardallButton != null)
                    {
                        SingletonBehavior<BattleManagerUI>.Instance.ui_emotionInfoBar.unequipcardallButton.SetActivate(false);
                    }
                    SingletonBehavior<BattleManagerUI>.Instance.ui_emotionInfoBar.targetingToggle.SetToggle(0, true);
                    HP.LastEnemyCount = 0;
                    HP.LastEnemySpeedDiceResultCount = 0;
                    HP.CanDoActionCount = 1;
                    HP.NowDoActionFaction = Faction.Enemy;
                    HP.ApplyEnemyCardPhase();
                }
                StageController_Path.LastRoundTurn = Singleton<StageController>.Instance.RoundTurn;
            }
            return StageController_Path.phase != StageController.StagePhase.ApplyLibrarianCardPhase && StageController_Path.phase != StageController.StagePhase.ApplyEnemyCardPhase;
        }

        // Token: 0x0600002B RID: 43 RVA: 0x00002D40 File Offset: 0x00000F40
        [HarmonyPatch("OnUpdate")]
        [HarmonyPrefix]
        public static bool OnUpdate(MethodBase __originalMethod, StageController __instance, float deltatTime)
        {
            bool flag = false;
            if (Singleton<StageController>.Instance.IsTwistedArgaliaBattleEnd())
            {
                flag = true;
            }
            if (flag)
            {
                Singleton<StageController>.Instance.CheckInput(true);
                return false;
            }
            if (Input.GetKeyUp(KeyCode.Backspace) && StageController_Path.phase == StageController.StagePhase.ApplyLibrarianCardPhase)
            {
                HP.DoAction(Faction.Player, 1);
                if (BattleObjectManager.instance.GetAliveList(Faction.Enemy).Count <= HP.LastEnemyCount)
                {
                    StageController_Path.phase = StageController.StagePhase.ApplyLibrarianCardPhase;
                    HP.NowDoActionFaction = Faction.Player;
                    BattleUIInputController.Instance.ResetCursor();
                    Singleton<StageController>.Instance.CompleteApplyingLibrarianCardPhase(true);
                }
            }
            bool forcelyInput = false;
            if (StageController_Path.phase == StageController.StagePhase.ApplyLibrarianCardPhase && !Singleton<StageController>.Instance.EnemyStageManager.IsStageFinishable() && BattleObjectManager.instance.GetAliveList(Faction.Enemy).Count <= 0)
            {
                forcelyInput = true;
            }
            Singleton<StageController>.Instance.CheckInput(forcelyInput);
            return false;
        }

        // Token: 0x0600002C RID: 44 RVA: 0x00002DF8 File Offset: 0x00000FF8
        [HarmonyPatch(typeof(BattleUnitCardsInHandUI), "IsCardSelected")]
        [HarmonyPrefix]
        public static bool IsCardSelected(MethodBase __originalMethod, BattleUnitCardsInHandUI __instance, ref bool __result)
        {
            BattleDiceCardUI selectedCard = __instance.GetSelectedCard();
            __result = (selectedCard != null);
            if (__result)
            {
                CardRange ranged = selectedCard.CardModel.XmlData.Spec.Ranged;
                if (__instance.SelectedModel.faction != HP.NowDoActionFaction || (HP.InAoeTime && (ranged == CardRange.Far || ranged == CardRange.FarArea || ranged == CardRange.FarAreaEach)))
                {
                    __result = false;
                    return false;
                }
                if (ranged == CardRange.Instance)
                {
                    HP.CanDoActionCount++;
                }
            }
            return false;
        }

        // Token: 0x0400000E RID: 14
        public static int LastRoundTurn;

    }
}
