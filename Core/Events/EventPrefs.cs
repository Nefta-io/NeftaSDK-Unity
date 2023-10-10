using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nefta.Core.Events
{
    [Serializable]
    public class EventPrefs
    {
        [DataMember(Name = "s")] public int _sequenceNumber;
        [DataMember(Name = "n")] public int _sessionNumber;
        [DataMember(Name = "p")] public long _pauseTime;
        [DataMember(Name = "d")] public long _sessionDuration;
        [DataMember(Name = "b")] public List<int> _batches;
    }
}