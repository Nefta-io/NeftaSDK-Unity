using System;

namespace Nefta.ToolboxSdk.Nft
{
    [Serializable]
    public class NftCharacteristics
    {
        //public bool currency;
        //public string currency_id;
        //public float principal_amount;
        //public float interest;

        public int days_to_self_destruct;
        public int hours_to_self_destruct;
        public int minute_to_self_destruct;

        //public int rental_period;  // parameter with "rentable" 
        public string rental_period_type; // parameter with "rentable" can be "day" or "month"

        // The following are booleans indicating the present characteristics of the asset 
        public bool nft_staking;
        public bool burnable;
        public bool rentable;
        public bool timed_assets;
        public bool evolvable;
    }
}