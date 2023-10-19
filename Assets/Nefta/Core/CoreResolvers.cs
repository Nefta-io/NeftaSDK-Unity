#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace Nefta.Core.Resolvers
{
    using System;
    using Utf8Json;

    public class CoreResolvers : global::Utf8Json.IJsonFormatterResolver
    {
        public static readonly global::Utf8Json.IJsonFormatterResolver Instance = new CoreResolvers();

        CoreResolvers()
        {

        }

        public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

            static FormatterCache()
            {
                var f = CoreResolversGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::Utf8Json.IJsonFormatter<T>)f;
                }
            }
        }
    }

    internal static class CoreResolversGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static CoreResolversGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(7)
            {
                {typeof(global::System.Collections.Generic.List<global::Nefta.Core.Data.AdUnit>), 0 },
                {typeof(global::System.Collections.Generic.List<int>), 1 },
                {typeof(global::Nefta.Core.Data.AdUnit), 2 },
                {typeof(global::Nefta.Core.Data.InitResponse), 3 },
                {typeof(global::Nefta.Core.Data.NeftaUser), 4 },
                {typeof(global::Nefta.Core.Events.EventPrefs), 5 },
                {typeof(global::Nefta.Core.Events.RecordedEvent), 6 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::Utf8Json.Formatters.ListFormatter<global::Nefta.Core.Data.AdUnit>();
                case 1: return new global::Utf8Json.Formatters.ListFormatter<int>();
                case 2: return new Nefta.Core.Formatters.Nefta.Core.Data.AdUnitFormatter();
                case 3: return new Nefta.Core.Formatters.Nefta.Core.Data.InitResponseFormatter();
                case 4: return new Nefta.Core.Formatters.Nefta.Core.Data.NeftaUserFormatter();
                case 5: return new Nefta.Core.Formatters.Nefta.Core.Events.EventPrefsFormatter();
                case 6: return new Nefta.Core.Formatters.Nefta.Core.Events.RecordedEventFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

namespace Nefta.Core.Formatters.Nefta.Core.Data
{
    using System;
    using Utf8Json;


    public sealed class AdUnitFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.Core.Data.AdUnit>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AdUnitFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("app_id"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("type"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("width"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("height"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("app_id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("width"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("height"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.Core.Data.AdUnit value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._appId);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value._type);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<int?>().Serialize(ref writer, value._width, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<int?>().Serialize(ref writer, value._height, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Nefta.Core.Data.AdUnit Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___id__ = default(string);
            var ___id__b__ = false;
            var ___appId__ = default(string);
            var ___appId__b__ = false;
            var ___type__ = default(string);
            var ___type__b__ = false;
            var ___width__ = default(int?);
            var ___width__b__ = false;
            var ___height__ = default(int?);
            var ___height__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        ___id__ = reader.ReadString();
                        ___id__b__ = true;
                        break;
                    case 1:
                        ___appId__ = reader.ReadString();
                        ___appId__b__ = true;
                        break;
                    case 2:
                        ___type__ = reader.ReadString();
                        ___type__b__ = true;
                        break;
                    case 3:
                        ___width__ = formatterResolver.GetFormatterWithVerify<int?>().Deserialize(ref reader, formatterResolver);
                        ___width__b__ = true;
                        break;
                    case 4:
                        ___height__ = formatterResolver.GetFormatterWithVerify<int?>().Deserialize(ref reader, formatterResolver);
                        ___height__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.Core.Data.AdUnit();
            if(___id__b__) ____result._id = ___id__;
            if(___appId__b__) ____result._appId = ___appId__;
            if(___type__b__) ____result._type = ___type__;
            if(___width__b__) ____result._width = ___width__;
            if(___height__b__) ____result._height = ___height__;

            return ____result;
        }
    }


    public sealed class InitResponseFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.Core.Data.InitResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public InitResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("nuid"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ad_units"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("nuid"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ad_units"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.Core.Data.InitResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._nuid);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Nefta.Core.Data.AdUnit>>().Serialize(ref writer, value._adUnits, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Nefta.Core.Data.InitResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___nuid__ = default(string);
            var ___nuid__b__ = false;
            var ___adUnits__ = default(global::System.Collections.Generic.List<global::Nefta.Core.Data.AdUnit>);
            var ___adUnits__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        ___nuid__ = reader.ReadString();
                        ___nuid__b__ = true;
                        break;
                    case 1:
                        ___adUnits__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Nefta.Core.Data.AdUnit>>().Deserialize(ref reader, formatterResolver);
                        ___adUnits__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.Core.Data.InitResponse();
            if(___nuid__b__) ____result._nuid = ___nuid__;
            if(___adUnits__b__) ____result._adUnits = ___adUnits__;

            return ____result;
        }
    }


    public sealed class NeftaUserFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.Core.Data.NeftaUser>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public NeftaUserFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("user_token"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("user_id"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("username"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("email"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("address"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("user_token"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("user_id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("username"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("email"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("address"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.Core.Data.NeftaUser value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._token);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._userId);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value._username);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value._email);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value._address);
            
            writer.WriteEndObject();
        }

        public global::Nefta.Core.Data.NeftaUser Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___token__ = default(string);
            var ___token__b__ = false;
            var ___userId__ = default(string);
            var ___userId__b__ = false;
            var ___username__ = default(string);
            var ___username__b__ = false;
            var ___email__ = default(string);
            var ___email__b__ = false;
            var ___address__ = default(string);
            var ___address__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        ___token__ = reader.ReadString();
                        ___token__b__ = true;
                        break;
                    case 1:
                        ___userId__ = reader.ReadString();
                        ___userId__b__ = true;
                        break;
                    case 2:
                        ___username__ = reader.ReadString();
                        ___username__b__ = true;
                        break;
                    case 3:
                        ___email__ = reader.ReadString();
                        ___email__b__ = true;
                        break;
                    case 4:
                        ___address__ = reader.ReadString();
                        ___address__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.Core.Data.NeftaUser();
            if(___token__b__) ____result._token = ___token__;
            if(___userId__b__) ____result._userId = ___userId__;
            if(___username__b__) ____result._username = ___username__;
            if(___email__b__) ____result._email = ___email__;
            if(___address__b__) ____result._address = ___address__;

            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

namespace Nefta.Core.Formatters.Nefta.Core.Events
{
    using System;
    using Utf8Json;


    public sealed class EventPrefsFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.Core.Events.EventPrefs>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public EventPrefsFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("s"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("n"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("p"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("d"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("b"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("s"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("n"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("p"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("d"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("b"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.Core.Events.EventPrefs value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value._sequenceNumber);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value._sessionNumber);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt64(value._pauseTime);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt64(value._sessionDuration);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<int>>().Serialize(ref writer, value._batches, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Nefta.Core.Events.EventPrefs Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___sequenceNumber__ = default(int);
            var ___sequenceNumber__b__ = false;
            var ___sessionNumber__ = default(int);
            var ___sessionNumber__b__ = false;
            var ___pauseTime__ = default(long);
            var ___pauseTime__b__ = false;
            var ___sessionDuration__ = default(long);
            var ___sessionDuration__b__ = false;
            var ___batches__ = default(global::System.Collections.Generic.List<int>);
            var ___batches__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        ___sequenceNumber__ = reader.ReadInt32();
                        ___sequenceNumber__b__ = true;
                        break;
                    case 1:
                        ___sessionNumber__ = reader.ReadInt32();
                        ___sessionNumber__b__ = true;
                        break;
                    case 2:
                        ___pauseTime__ = reader.ReadInt64();
                        ___pauseTime__b__ = true;
                        break;
                    case 3:
                        ___sessionDuration__ = reader.ReadInt64();
                        ___sessionDuration__b__ = true;
                        break;
                    case 4:
                        ___batches__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<int>>().Deserialize(ref reader, formatterResolver);
                        ___batches__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.Core.Events.EventPrefs();
            if(___sequenceNumber__b__) ____result._sequenceNumber = ___sequenceNumber__;
            if(___sessionNumber__b__) ____result._sessionNumber = ___sessionNumber__;
            if(___pauseTime__b__) ____result._pauseTime = ___pauseTime__;
            if(___sessionDuration__b__) ____result._sessionDuration = ___sessionDuration__;
            if(___batches__b__) ____result._batches = ___batches__;

            return ____result;
        }
    }


    public sealed class RecordedEventFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.Core.Events.RecordedEvent>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RecordedEventFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("event_type"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("event_category"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("event_sub_category"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("item_name"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("value"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("custom_publisher_payload"), 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("event_type"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("event_category"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("event_sub_category"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("item_name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("value"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("custom_publisher_payload"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.Core.Events.RecordedEvent value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._type);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._category);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value._subCategory);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value._itemName);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteInt64(value._value);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value._customPayload);
            
            writer.WriteEndObject();
        }

        public global::Nefta.Core.Events.RecordedEvent Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___type__ = default(string);
            var ___type__b__ = false;
            var ___category__ = default(string);
            var ___category__b__ = false;
            var ___subCategory__ = default(string);
            var ___subCategory__b__ = false;
            var ___itemName__ = default(string);
            var ___itemName__b__ = false;
            var ___value__ = default(long);
            var ___value__b__ = false;
            var ___customPayload__ = default(string);
            var ___customPayload__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        ___type__ = reader.ReadString();
                        ___type__b__ = true;
                        break;
                    case 1:
                        ___category__ = reader.ReadString();
                        ___category__b__ = true;
                        break;
                    case 2:
                        ___subCategory__ = reader.ReadString();
                        ___subCategory__b__ = true;
                        break;
                    case 3:
                        ___itemName__ = reader.ReadString();
                        ___itemName__b__ = true;
                        break;
                    case 4:
                        ___value__ = reader.ReadInt64();
                        ___value__b__ = true;
                        break;
                    case 5:
                        ___customPayload__ = reader.ReadString();
                        ___customPayload__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.Core.Events.RecordedEvent();
            if(___type__b__) ____result._type = ___type__;
            if(___category__b__) ____result._category = ___category__;
            if(___subCategory__b__) ____result._subCategory = ___subCategory__;
            if(___itemName__b__) ____result._itemName = ___itemName__;
            if(___value__b__) ____result._value = ___value__;
            if(___customPayload__b__) ____result._customPayload = ___customPayload__;

            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
