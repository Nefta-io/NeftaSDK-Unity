using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.GamerManagement
{
    [Serializable]
    public class NativeCurrencyBalance
    {
        [DataMember(Name = "coin_balance")] public float _coinBalance;
        [DataMember(Name = "coin_balance_name")] public string _coinBalanceName;
    }
}