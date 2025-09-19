using Berty.BoardCards.Behaviours;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using System;
using UnityEngine;

namespace Berty.BoardCards.Listeners
{
    public class StatusListener : MonoBehaviour
    {
        private BoardCardCore core;
        private Game game;

        private void Awake()
        {
            core = GetComponent<BoardCardCore>();
            game = CoreManager.Instance.Game;
        }

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
                case StatusEnum.DisableEnemySpecialSkill:
                    if (status.GetAlign() != core.BoardCard.Align)
                        StatusManager.Instance.RemoveStatusFromProvider(core.BoardCard);
                    break;
            }
        }

        private void HandleStatusRemoved(object sender, StatusEventArgs args)
        {
            switch (args.StatusName)
            {
                case StatusEnum.DisableEnemySpecialSkill:
                    TryRetrievingUniqueStatus();
                    break;
                case StatusEnum.ForceSpecialRole:
                    if (game.HasStatusByName(StatusEnum.DisableEnemySpecialSkill))
                        TryRetrievingUniqueStatus();
                    break;
            }
        }

        private void TryRetrievingUniqueStatus()
        {
            switch (core.BoardCard.GetSkill())
            {
                case SkillEnum.RycerzBerti:
                    StatusManager.Instance.AddUniqueStatusWithProvider(StatusEnum.TelekineticArea, core.BoardCard);
                    break;
                case SkillEnum.SedziaBertt:
                    StatusManager.Instance.AddUniqueStatusWithProvider(StatusEnum.ForceSpecialRole, core.BoardCard);
                    break;
            }
        }
    }
}