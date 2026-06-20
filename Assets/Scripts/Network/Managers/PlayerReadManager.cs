using Berty.Enums;
using Berty.Utility;
using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace Berty.Network.Managers
{
    public class PlayerReadManager : SharedManagerSingleton<PlayerReadManager>
    {
        private Dictionary<AlignmentEnum, ulong> alignments = new(); // Read from server only

        public AlignmentEnum MyAlignment {get; private set; } = AlignmentEnum.None;

        public void InitializeAlignmentsForClients(IReadOnlyList<ulong> clients)
        {
            if (!IsServer) return;
            if (clients.Count != 2) throw new InvalidOperationException($"Expected 2 connected clients, got {clients.Count}");
            for (int i = 0; i < 2; i++)
            {
                AlignmentEnum assignedAlign = i == 0 ? AlignmentEnum.Player : AlignmentEnum.Opponent;
                alignments.Add(assignedAlign, clients[i]);
                ClientRpcParams sendRpcParam = new()
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { clients[i] }
                    }
                };
                SetAlignmentClientRpc(assignedAlign, sendRpcParam);
            }
        }

        public ulong GetClientIdFromAlignment(AlignmentEnum alignment)
        {
            if (!IsServer) throw new InvalidOperationException("Only server can get client ID for alignment");
            if (!alignments.ContainsKey(alignment)) throw new KeyNotFoundException($"No client found for alignment {alignment}");
            return alignments[alignment];
        }

        [ClientRpc]
        public void SetAlignmentClientRpc(AlignmentEnum alignment, ClientRpcParams rpcParams)
        {
            MyAlignment = alignment;
        }
    }
}