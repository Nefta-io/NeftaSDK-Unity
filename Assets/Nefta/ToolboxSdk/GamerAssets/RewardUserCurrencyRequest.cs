using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.GamerAssets
{
    [Serializable]
    public class RewardUserCurrencyRequest
    {
        [DataMember(Name = "amount")] public float _amount;
    }
}