using HarmonyLib;
using LOR_BattleUnit_UI;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace pvp
{
    // Token: 0x02000007 RID: 7
    public class HP
    {

        public HP()
        {
            try
            {
                Harmony harmony = new Harmony(base.GetType().Namespace);
                MethodInfo method = typeof(HP).GetMethod("AddCard");
                harmony.Patch(typeof(BattlePlayingCardSlotDetail).GetMethod("AddCard", AccessTools.all), null, new HarmonyMethod(method), null, null, null);
                harmony.PatchAll(typeof(StageController_Path));
            }
            catch (Exception message)
            {
                Debug.LogError(message);
            }
        }

        // Token: 0x0600001D RID: 29 RVA: 0x00002151 File Offset: 0x00000351
        public static void DoAction(Faction faction, int canDoActionCount)
        {
            HP.CanDoActionCount--;
            if (HP.CanDoActionCount < 1)
            {
                HP.CheckAndDoPairing();
                HP.CanDoActionCount = canDoActionCount;
                HP.ChangeActionFaction();
            }
        }

        // Token: 0x0600001E RID: 30 RVA: 0x00002177 File Offset: 0x00000377
        public static bool CheckCanDoAction(BattleUnitModel model)
        {
            return false;
        }

        // Token: 0x0600001F RID: 31 RVA: 0x00002177 File Offset: 0x00000377
        public static bool CheckCanUseCard(BattleUnitModel model)
        {
            return false;
        }


        public static void CheckAndDoPairing()
        {
            SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.OnStartUnitMoving();
            Singleton<StageController>.Instance.GetAllCards().Clear();
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(false))
            {
                List<BattlePlayingCardDataInUnitModel> cardAry = battleUnitModel.cardSlotDetail.cardAry;
                for (int i = 0; i < cardAry.Count; i++)
                {
                    BattlePlayingCardDataInUnitModel battlePlayingCardDataInUnitModel = cardAry[i];
                    if (battlePlayingCardDataInUnitModel != null && battleUnitModel.view.speedDiceSetterUI.GetSpeedDiceByIndex(i) != null)
                    {
                        SpeedDiceUI speedDiceByIndex = battlePlayingCardDataInUnitModel.target.view.speedDiceSetterUI.GetSpeedDiceByIndex(battlePlayingCardDataInUnitModel.targetSlotOrder);
                        if (speedDiceByIndex != null && speedDiceByIndex.CardInDice != null && speedDiceByIndex.CardInDice.target == battleUnitModel && speedDiceByIndex.CardInDice.targetSlotOrder == i)
                        {
                            Singleton<StageController>.Instance.GetAllCards().Add(battlePlayingCardDataInUnitModel);
                        }
                    }
                }
            }
            Singleton<StageController>.Instance.GetAllCards().Sort(delegate (BattlePlayingCardDataInUnitModel c1, BattlePlayingCardDataInUnitModel c2)
            {
                if (c1.card.GetSpec().Ranged == CardRange.Far)
                {
                    if (c2.card.GetSpec().Ranged != CardRange.Far)
                    {
                        return -1;
                    }
                    if (c1.speedDiceResultValue == c2.speedDiceResultValue)
                    {
                        if (c1.owner != c2.owner)
                        {
                            return 0;
                        }
                        if (c1.slotOrder < c2.slotOrder)
                        {
                            return -1;
                        }
                        return 1;
                    }
                    else
                    {
                        if (c1.speedDiceResultValue > c2.speedDiceResultValue)
                        {
                            return -1;
                        }
                        return 1;
                    }
                }
                else
                {
                    if (c2.card.GetSpec().Ranged == CardRange.Far)
                    {
                        return 1;
                    }
                    if (c1.speedDiceResultValue == c2.speedDiceResultValue)
                    {
                        if (c1.owner != c2.owner)
                        {
                            return 0;
                        }
                        if (c1.slotOrder < c2.slotOrder)
                        {
                            return -1;
                        }
                        return 1;
                    }
                    else
                    {
                        if (c1.speedDiceResultValue > c2.speedDiceResultValue)
                        {
                            return -1;
                        }
                        return 1;
                    }
                }
            });
            if (Singleton<StageController>.Instance.GetAllCards().Count > 0)
            {
                Traverse.Create(Singleton<StageController>.Instance).Field("CheckChangeMap").GetValue();
            }
        }

        // Token: 0x06000021 RID: 33 RVA: 0x0000217A File Offset: 0x0000037A
        public static void ChangeActionFaction()
        {
            if (HP.NowDoActionFaction == Faction.Player)
            {
                StageController_Path.phase = StageController.StagePhase.ApplyEnemyCardPhase;
                HP.NowDoActionFaction = Faction.Enemy;
                while (HP.NowDoActionFaction == Faction.Enemy && HP.CanDoActionCount > 0)
                {
                    HP.ApplyEnemyCardPhase();
                }
                return;
            }
            StageController_Path.phase = StageController.StagePhase.ApplyLibrarianCardPhase;
            HP.NowDoActionFaction = Faction.Player;
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000022 RID: 34 RVA: 0x000021B3 File Offset: 0x000003B3
        public static bool InAoeTime
        {
            get
            {
                return HP.CanDoActionCount > 1;
            }
        }

        public static void ApplyEnemyCardPhase()
        {
            try
            {
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(Faction.Enemy);
                if (aliveList.Count > HP.LastEnemyCount)
                {
                    if (aliveList[HP.LastEnemyCount].turnState != BattleUnitTurnState.BREAK)
                    {
                        aliveList[HP.LastEnemyCount].turnState = BattleUnitTurnState.WAIT_CARD;
                    }
                    if (aliveList[HP.LastEnemyCount].speedDiceResult.Count <= HP.LastEnemySpeedDiceResultCount)
                    {
                        HP.LastEnemyCount++;
                        HP.LastEnemySpeedDiceResultCount = 0;
                        HP.ApplyEnemyCardPhase();
                    }
                    else
                    {
                        if (!aliveList[HP.LastEnemyCount].speedDiceResult[HP.LastEnemySpeedDiceResultCount].breaked)
                        {
                            aliveList[HP.LastEnemyCount].allyCardDetail.PlayTurnAutoForEnemy(HP.LastEnemySpeedDiceResultCount, aliveList[HP.LastEnemyCount].speedDiceResult[HP.LastEnemySpeedDiceResultCount].value);
                        }
                        HP.LastEnemySpeedDiceResultCount++;
                    }
                    SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.ClearCloneArrows();
                    SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.ActiveTargetParent(true);
                    BattleUIInputController.Instance.ResetCharacterCursor(false, true);
                    SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.UpdateTargetList();
                }
                else
                {
                    HP.DoAction(Faction.Enemy, 1);
                }
            }
            catch
            {
                HP.LastEnemyCount = 0;
                HP.LastEnemySpeedDiceResultCount = 0;
                HP.ApplyEnemyCardPhase();
            }
        }

        // Token: 0x06000024 RID: 36 RVA: 0x000029F4 File Offset: 0x00000BF4
        public static void AddCard(BattlePlayingCardSlotDetail __instance, BattleDiceCardModel card, BattleUnitModel target, int targetSlot, bool isEnemyAuto = false)
        {
            BattlePlayingCardDataInUnitModel battlePlayingCardDataInUnitModel = __instance.cardAry[card.owner.cardOrder];
            HP.DoAction(battlePlayingCardDataInUnitModel.owner.faction, battlePlayingCardDataInUnitModel.subTargets.Count + 1);
        }

        // Token: 0x04000008 RID: 8
        public static Faction NowDoActionFaction;

        // Token: 0x04000009 RID: 9
        public static int CanDoActionCount;

        // Token: 0x0400000A RID: 10
        public static int LastEnemyCount;

        // Token: 0x0400000B RID: 11
        public static int LastEnemySpeedDiceResultCount;


    }
}
