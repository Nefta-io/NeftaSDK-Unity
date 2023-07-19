namespace Nefta.Core.Events
{
    public class SessionEvent : InterestEvent
    {
        public long _sessionCount;
        public long _previousSessionLengthInSeconds;
        
        public override RecordedEvent GetRecordedEvent()
        {
            return new RecordedEvent()
            {
                _type = "session",
                _category = _sessionCount.ToString(),
                _subCategory = null,
                _value = _previousSessionLengthInSeconds,
            };
        }
    }
}