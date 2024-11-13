namespace Nefta.Events
{
    public enum ResourceCategory
    {
        Other = 0,
        SoftCurrency = 1,
        PremiumCurrency = 2,
        Resource = 3,
        Consumable = 4,
        CosmeticItem = 5,
        CoreItem = 6,
        Chest = 7,
        Experience = 8
    }

    public abstract class ResourceEvent : GameEvent
    {
        /// <summary>
        /// The category of the resource
        /// </summary>
        public ResourceCategory _resourceCategory;
        
        public long _quantity { get { return _value; } set { _value = value; } }

        internal override int _category => (int) _resourceCategory;
        
        protected ResourceEvent(ResourceCategory category)
        {
            _resourceCategory = category;
        }
    }
}