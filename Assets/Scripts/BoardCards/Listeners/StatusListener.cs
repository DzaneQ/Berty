using Berty.BoardCards.Behaviours;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Managers;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Berty.BoardCards.Listeners
{
    public class StatusListener : BoardCardBehaviour
    {
        private void OnEnable()
        {
            EventManager.Instance.OnStatusUpdated += HandleStatusUpdated;
            EventManager.Instance.OnStatusRemoved += HandleStatusRemoved;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnStatusUpdated -= HandleStatusUpdated;
            EventManager.Instance.OnStatusRemoved -= HandleStatusRemoved;
        }

        private void HandleStatusUpdated(object sender, EventArgs args)
        {
            Status status = (Status)sender;
            switch (status.Name)
            {
                case StatusEnum.ClickToApplyEffect:
                    if (status.GetTargetAlign() == Core.BoardCard.Align && !ApplySkillEffectManager.Instance.DoesPreventEffect(Core.BoardCard, status.Provider))
                        StateMachine.SetEffectable();
                    else StateMachine.SetIdle();
                    break;
                case StatusEnum.DisableEnemySpecialSkill:
                    if (status.GetAlign() != Core.BoardCard.Align)
                        StatusManager.Instance.RemoveStatusFromProvider(Core.BoardCard);
                    break;
                case StatusEnum.RevivalSelect:
                    OverlayObjectManager.Instance.DisplayDeadCardsScreen();
                    break;
                case StatusEnum.Ventura:
                    if (status.Provider == Core.BoardCard)
                        Core.Bars.UpdateBar(StatEnum.Strength);
                    break;
                }
        }

        private void HandleStatusRemoved(object sender, StatusEventArgs args)
        {
            switch (args.StatusName)
            {
                case StatusEnum.ClickToApplyEffect:
                    StateMachine.SetMainState();
                    break;
                case StatusEnum.DisableEnemySpecialSkill:
                    TryRetrievingUniqueStatus();
                    break;
                case StatusEnum.ForceSpecialRole:
                    if (game.HasStatusByName(StatusEnum.DisableEnemySpecialSkill))
                        TryRetrievingUniqueStatus();
                    break;
                case StatusEnum.RevivalSelect:
                    OverlayObjectManager.Instance.HideDeadCardsScreen();
                    break;
                case StatusEnum.Ventura:
                    if (args.Alignment == Core.BoardCard.Align)
                        Core.Bars.UpdateBar(StatEnum.Strength);
                    break;
            }
        }

        private void TryRetrievingUniqueStatus()
        {
            switch (Core.BoardCard.GetSkill())
            {
                case SkillEnum.BertVentura:
                    StatusManager.Instance.SetChargedStatusWithProvider(StatusEnum.Ventura, Core.BoardCard, game.Grid.GetEnemyNeighborCount(Core.BoardCard));
                    break;
                case SkillEnum.RycerzBerti:
                    StatusManager.Instance.AddUniqueStatusWithProvider(StatusEnum.TelekineticArea, Core.BoardCard);
                    break;
                case SkillEnum.SedziaBertt:
                    StatusManager.Instance.AddUniqueStatusWithProvider(StatusEnum.ForceSpecialRole, Core.BoardCard);
                    break;
            }
        }
    }
}