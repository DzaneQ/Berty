using Berty.Enums;
using Berty.Utility;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

namespace Berty.Network.Managers
{
    public class PlayerReadManager : SharedManagerSingleton<PlayerReadManager>
    {
        private AlignmentEnum _myAlignment = AlignmentEnum.None;

        public AlignmentEnum MyAlignment => _myAlignment;

        [ClientRpc]
        public void SetAlignmentClientRpc(AlignmentEnum alignment, ClientRpcParams rpcParams)
        {
            _myAlignment = alignment;
        }
    }
}