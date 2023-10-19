using System;
using System.Runtime.Serialization;

namespace Nefta.ToolboxSdk.GamerManagement
{
    [Serializable]
    public class CurrencyBalance
    {
        [DataMember(Name = "balance")] public int _balance;
    }
}