using System;
using System.Runtime.Serialization;

namespace Nefta.Core.Events
{
    [Serializable]
    public class RecordedEvent
    {
        [DataMember(Name = "nefta_user_id")] public string _userId;
        [DataMember(Name = "sequence_number")] public long _sequenceNumber;
        [DataMember(Name = "event_time")] public long _time;
        [DataMember(Name = "event_version")] public string _eventVersion;
        [DataMember(Name = "event_type")] public string _type;
        [DataMember(Name = "event_category")] public string _category;
        [DataMember(Name = "event_sub_category")] public string _subCategory;
        [DataMember(Name = "item_name")] public string _itemName;
        [DataMember(Name = "value")] public long _value;
        [DataMember(Name = "custom_publisher_payload")] public string _customPayload;
        [DataMember(Name = "test")] public bool _isTest;
        
        [DataMember(Name = "sdk_version")] public string _sdkVersion;
        [DataMember(Name = "device_language")] public string _language;
        [DataMember(Name = "platform")] public string _platform;
        [DataMember(Name = "device_type")] public string _deviceType;
        [DataMember(Name = "device_make")] public string _deviceMake;
        [DataMember(Name = "hardware_version")] public string _hardwareVersion;
        [DataMember(Name = "device_os")] public string _deviceOs;
        [DataMember(Name = "device_os_version")] public string _deviceOsVersion;
        [DataMember(Name = "screen_size_width")] public int _screenWidth;
        [DataMember(Name = "screen_size_height")] public int _screenHeight;
        [DataMember(Name = "device_dpi")] public int _dpi;
        [DataMember(Name = "device_pixel_ratio")] public float _ratio;
        [DataMember(Name = "app_id")] public string _appId;
        [DataMember(Name = "app_version")] public string _appVersion;
    }
}