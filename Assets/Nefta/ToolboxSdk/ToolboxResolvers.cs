#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace Nefta.ToolboxSdk.Resolvers
{
    using System;
    using Utf8Json;

    public class ToolboxResolvers : global::Utf8Json.IJsonFormatterResolver
    {
        public static readonly global::Utf8Json.IJsonFormatterResolver Instance = new ToolboxResolvers();

        ToolboxResolvers()
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
                var f = ToolboxResolversGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::Utf8Json.IJsonFormatter<T>)f;
                }
            }
        }
    }

    internal static class ToolboxResolversGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static ToolboxResolversGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(29)
            {
                {typeof(global::System.Collections.Generic.Dictionary<string, string>), 0 },
                {typeof(global::System.Collections.Generic.List<global::Nefta.ToolboxSdk.Marketplace.Auction>), 1 },
                {typeof(global::System.Collections.Generic.List<global::Nefta.ToolboxSdk.GamerManagement.OwnedAsset>), 2 },
                {typeof(global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailResponse[]), 3 },
                {typeof(global::Nefta.ToolboxSdk.Nft.NftCharacteristics), 4 },
                {typeof(global::Nefta.ToolboxSdk.Nft.NftAsset), 5 },
                {typeof(global::Nefta.ToolboxSdk.Marketplace.Auction), 6 },
                {typeof(global::Nefta.ToolboxSdk.Marketplace.AuctionsResponse), 7 },
                {typeof(global::Nefta.ToolboxSdk.Marketplace.CreateAuctionBidRequest), 8 },
                {typeof(global::Nefta.ToolboxSdk.Marketplace.CreateAuctionRequest), 9 },
                {typeof(global::Nefta.ToolboxSdk.GamerManagement.CurrencyBalance), 10 },
                {typeof(global::Nefta.ToolboxSdk.GamerManagement.GamerProfile), 11 },
                {typeof(global::Nefta.ToolboxSdk.GamerManagement.NativeCurrencyBalance), 12 },
                {typeof(global::Nefta.ToolboxSdk.GamerManagement.OwnedAsset), 13 },
                {typeof(global::Nefta.ToolboxSdk.GamerManagement.OwnedAssetsResponse), 14 },
                {typeof(global::Nefta.ToolboxSdk.GamerAssets.RewardUserCurrencyRequest), 15 },
                {typeof(global::Nefta.ToolboxSdk.Authorization.ConfirmGamerAccountWithEmailCode), 16 },
                {typeof(global::Nefta.ToolboxSdk.Authorization.ConvertGuestToFullUserRequest), 17 },
                {typeof(global::Nefta.ToolboxSdk.Authorization.LoginWithConfirmationCodeRequest), 18 },
                {typeof(global::Nefta.ToolboxSdk.Authorization.LoginWithWalletRequest), 19 },
                {typeof(global::Nefta.ToolboxSdk.Authorization.RequestLoginCodeRequest), 20 },
                {typeof(global::Nefta.ToolboxSdk.Authorization.SignUpGamerRequest), 21 },
                {typeof(global::Nefta.ToolboxSdk.Authorization.SignupGuestUserRequest), 22 },
                {typeof(global::Nefta.ToolboxSdk.Authorization.AuthProviders.AuthToken), 23 },
                {typeof(global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailResponse), 24 },
                {typeof(global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailsResponse), 25 },
                {typeof(global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessageData), 26 },
                {typeof(global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessage), 27 },
                {typeof(global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskResponseMessage), 28 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::Utf8Json.Formatters.DictionaryFormatter<string, string>();
                case 1: return new global::Utf8Json.Formatters.ListFormatter<global::Nefta.ToolboxSdk.Marketplace.Auction>();
                case 2: return new global::Utf8Json.Formatters.ListFormatter<global::Nefta.ToolboxSdk.GamerManagement.OwnedAsset>();
                case 3: return new global::Utf8Json.Formatters.ArrayFormatter<global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailResponse>();
                case 4: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Nft.NftCharacteristicsFormatter();
                case 5: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Nft.NftAssetFormatter();
                case 6: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Marketplace.AuctionFormatter();
                case 7: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Marketplace.AuctionsResponseFormatter();
                case 8: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Marketplace.CreateAuctionBidRequestFormatter();
                case 9: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Marketplace.CreateAuctionRequestFormatter();
                case 10: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.GamerManagement.CurrencyBalanceFormatter();
                case 11: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.GamerManagement.GamerProfileFormatter();
                case 12: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.GamerManagement.NativeCurrencyBalanceFormatter();
                case 13: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.GamerManagement.OwnedAssetFormatter();
                case 14: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.GamerManagement.OwnedAssetsResponseFormatter();
                case 15: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.GamerAssets.RewardUserCurrencyRequestFormatter();
                case 16: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization.ConfirmGamerAccountWithEmailCodeFormatter();
                case 17: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization.ConvertGuestToFullUserRequestFormatter();
                case 18: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization.LoginWithConfirmationCodeRequestFormatter();
                case 19: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization.LoginWithWalletRequestFormatter();
                case 20: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization.RequestLoginCodeRequestFormatter();
                case 21: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization.SignUpGamerRequestFormatter();
                case 22: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization.SignupGuestUserRequestFormatter();
                case 23: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization.AuthProviders.AuthTokenFormatter();
                case 24: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization.AuthProviders.EmailResponseFormatter();
                case 25: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization.AuthProviders.EmailsResponseFormatter();
                case 26: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessageDataFormatter();
                case 27: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessageFormatter();
                case 28: return new Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskResponseMessageFormatter();
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

namespace Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Nft
{
    using System;
    using Utf8Json;


    public sealed class NftCharacteristicsFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Nft.NftCharacteristics>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public NftCharacteristicsFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("days_to_self_destruct"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("hours_to_self_destruct"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("minute_to_self_destruct"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("rental_period_type"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("nft_staking"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("burnable"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("rentable"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("timed_assets"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("evolvable"), 8},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("days_to_self_destruct"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("hours_to_self_destruct"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("minute_to_self_destruct"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("rental_period_type"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("nft_staking"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("burnable"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("rentable"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("timed_assets"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("evolvable"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Nft.NftCharacteristics value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.days_to_self_destruct);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.hours_to_self_destruct);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.minute_to_self_destruct);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.rental_period_type);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteBoolean(value.nft_staking);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteBoolean(value.burnable);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteBoolean(value.rentable);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteBoolean(value.timed_assets);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteBoolean(value.evolvable);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Nft.NftCharacteristics Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __days_to_self_destruct__ = default(int);
            var __days_to_self_destruct__b__ = false;
            var __hours_to_self_destruct__ = default(int);
            var __hours_to_self_destruct__b__ = false;
            var __minute_to_self_destruct__ = default(int);
            var __minute_to_self_destruct__b__ = false;
            var __rental_period_type__ = default(string);
            var __rental_period_type__b__ = false;
            var __nft_staking__ = default(bool);
            var __nft_staking__b__ = false;
            var __burnable__ = default(bool);
            var __burnable__b__ = false;
            var __rentable__ = default(bool);
            var __rentable__b__ = false;
            var __timed_assets__ = default(bool);
            var __timed_assets__b__ = false;
            var __evolvable__ = default(bool);
            var __evolvable__b__ = false;

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
                        __days_to_self_destruct__ = reader.ReadInt32();
                        __days_to_self_destruct__b__ = true;
                        break;
                    case 1:
                        __hours_to_self_destruct__ = reader.ReadInt32();
                        __hours_to_self_destruct__b__ = true;
                        break;
                    case 2:
                        __minute_to_self_destruct__ = reader.ReadInt32();
                        __minute_to_self_destruct__b__ = true;
                        break;
                    case 3:
                        __rental_period_type__ = reader.ReadString();
                        __rental_period_type__b__ = true;
                        break;
                    case 4:
                        __nft_staking__ = reader.ReadBoolean();
                        __nft_staking__b__ = true;
                        break;
                    case 5:
                        __burnable__ = reader.ReadBoolean();
                        __burnable__b__ = true;
                        break;
                    case 6:
                        __rentable__ = reader.ReadBoolean();
                        __rentable__b__ = true;
                        break;
                    case 7:
                        __timed_assets__ = reader.ReadBoolean();
                        __timed_assets__b__ = true;
                        break;
                    case 8:
                        __evolvable__ = reader.ReadBoolean();
                        __evolvable__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Nft.NftCharacteristics();
            if(__days_to_self_destruct__b__) ____result.days_to_self_destruct = __days_to_self_destruct__;
            if(__hours_to_self_destruct__b__) ____result.hours_to_self_destruct = __hours_to_self_destruct__;
            if(__minute_to_self_destruct__b__) ____result.minute_to_self_destruct = __minute_to_self_destruct__;
            if(__rental_period_type__b__) ____result.rental_period_type = __rental_period_type__;
            if(__nft_staking__b__) ____result.nft_staking = __nft_staking__;
            if(__burnable__b__) ____result.burnable = __burnable__;
            if(__rentable__b__) ____result.rentable = __rentable__;
            if(__timed_assets__b__) ____result.timed_assets = __timed_assets__;
            if(__evolvable__b__) ____result.evolvable = __evolvable__;

            return ____result;
        }
    }


    public sealed class NftAssetFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Nft.NftAsset>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public NftAssetFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("IsAssetLoaded"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("asset_id"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("asset_name"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("image"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("asset_type"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("available_instances"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("instances"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("traits"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("asset_characteristics"), 8},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("IsAssetLoaded"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("asset_id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("asset_name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("image"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("asset_type"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("available_instances"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("instances"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("traits"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("asset_characteristics"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Nft.NftAsset value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteBoolean(value.IsAssetLoaded);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._id);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value._name);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value._imageUrl);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value._assetType);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteInt32(value._availableInstances);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteInt32(value._instances);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Serialize(ref writer, value._traits, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[8]);
            formatterResolver.GetFormatterWithVerify<global::Nefta.ToolboxSdk.Nft.NftCharacteristics>().Serialize(ref writer, value._characteristics, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Nft.NftAsset Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __IsAssetLoaded__ = default(bool);
            var __IsAssetLoaded__b__ = false;
            var ___id__ = default(string);
            var ___id__b__ = false;
            var ___name__ = default(string);
            var ___name__b__ = false;
            var ___imageUrl__ = default(string);
            var ___imageUrl__b__ = false;
            var ___assetType__ = default(string);
            var ___assetType__b__ = false;
            var ___availableInstances__ = default(int);
            var ___availableInstances__b__ = false;
            var ___instances__ = default(int);
            var ___instances__b__ = false;
            var ___traits__ = default(global::System.Collections.Generic.Dictionary<string, string>);
            var ___traits__b__ = false;
            var ___characteristics__ = default(global::Nefta.ToolboxSdk.Nft.NftCharacteristics);
            var ___characteristics__b__ = false;

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
                        __IsAssetLoaded__ = reader.ReadBoolean();
                        __IsAssetLoaded__b__ = true;
                        break;
                    case 1:
                        ___id__ = reader.ReadString();
                        ___id__b__ = true;
                        break;
                    case 2:
                        ___name__ = reader.ReadString();
                        ___name__b__ = true;
                        break;
                    case 3:
                        ___imageUrl__ = reader.ReadString();
                        ___imageUrl__b__ = true;
                        break;
                    case 4:
                        ___assetType__ = reader.ReadString();
                        ___assetType__b__ = true;
                        break;
                    case 5:
                        ___availableInstances__ = reader.ReadInt32();
                        ___availableInstances__b__ = true;
                        break;
                    case 6:
                        ___instances__ = reader.ReadInt32();
                        ___instances__b__ = true;
                        break;
                    case 7:
                        ___traits__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Deserialize(ref reader, formatterResolver);
                        ___traits__b__ = true;
                        break;
                    case 8:
                        ___characteristics__ = formatterResolver.GetFormatterWithVerify<global::Nefta.ToolboxSdk.Nft.NftCharacteristics>().Deserialize(ref reader, formatterResolver);
                        ___characteristics__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Nft.NftAsset();
            if(___id__b__) ____result._id = ___id__;
            if(___name__b__) ____result._name = ___name__;
            if(___imageUrl__b__) ____result._imageUrl = ___imageUrl__;
            if(___assetType__b__) ____result._assetType = ___assetType__;
            if(___availableInstances__b__) ____result._availableInstances = ___availableInstances__;
            if(___instances__b__) ____result._instances = ___instances__;
            if(___traits__b__) ____result._traits = ___traits__;
            if(___characteristics__b__) ____result._characteristics = ___characteristics__;

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

namespace Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Marketplace
{
    using System;
    using Utf8Json;


    public sealed class AuctionFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Marketplace.Auction>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AuctionFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("serial"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("auction_type"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("purchase_price"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sell_price"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sell_date"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("start_price"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("end_date"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("auction_id"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("start_date"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("user_auctions"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("number_of_bids"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("last_bid_price"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("asset"), 13},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("serial"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("auction_type"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("purchase_price"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sell_price"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sell_date"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("start_price"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("end_date"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("auction_id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("start_date"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("user_auctions"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("number_of_bids"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("last_bid_price"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("asset"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Marketplace.Auction value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value._serial);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._auctionType);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<float?>().Serialize(ref writer, value._purchasePrice, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<float?>().Serialize(ref writer, value._sellPrice, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime?>().Serialize(ref writer, value._sellDate, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<float?>().Serialize(ref writer, value._startPrice, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value._endDate, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value._auctionId);
            writer.WriteRaw(this.____stringByteKeys[8]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value._startDate, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteBoolean(value._userAuctions);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value._status);
            writer.WriteRaw(this.____stringByteKeys[11]);
            formatterResolver.GetFormatterWithVerify<int?>().Serialize(ref writer, value._numberBids, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[12]);
            formatterResolver.GetFormatterWithVerify<float?>().Serialize(ref writer, value._lastBidPrice, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[13]);
            formatterResolver.GetFormatterWithVerify<global::Nefta.ToolboxSdk.Nft.NftAsset>().Serialize(ref writer, value._asset, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Marketplace.Auction Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___serial__ = default(int);
            var ___serial__b__ = false;
            var ___auctionType__ = default(string);
            var ___auctionType__b__ = false;
            var ___purchasePrice__ = default(float?);
            var ___purchasePrice__b__ = false;
            var ___sellPrice__ = default(float?);
            var ___sellPrice__b__ = false;
            var ___sellDate__ = default(global::System.DateTime?);
            var ___sellDate__b__ = false;
            var ___startPrice__ = default(float?);
            var ___startPrice__b__ = false;
            var ___endDate__ = default(global::System.DateTime);
            var ___endDate__b__ = false;
            var ___auctionId__ = default(string);
            var ___auctionId__b__ = false;
            var ___startDate__ = default(global::System.DateTime);
            var ___startDate__b__ = false;
            var ___userAuctions__ = default(bool);
            var ___userAuctions__b__ = false;
            var ___status__ = default(string);
            var ___status__b__ = false;
            var ___numberBids__ = default(int?);
            var ___numberBids__b__ = false;
            var ___lastBidPrice__ = default(float?);
            var ___lastBidPrice__b__ = false;
            var ___asset__ = default(global::Nefta.ToolboxSdk.Nft.NftAsset);
            var ___asset__b__ = false;

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
                        ___serial__ = reader.ReadInt32();
                        ___serial__b__ = true;
                        break;
                    case 1:
                        ___auctionType__ = reader.ReadString();
                        ___auctionType__b__ = true;
                        break;
                    case 2:
                        ___purchasePrice__ = formatterResolver.GetFormatterWithVerify<float?>().Deserialize(ref reader, formatterResolver);
                        ___purchasePrice__b__ = true;
                        break;
                    case 3:
                        ___sellPrice__ = formatterResolver.GetFormatterWithVerify<float?>().Deserialize(ref reader, formatterResolver);
                        ___sellPrice__b__ = true;
                        break;
                    case 4:
                        ___sellDate__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime?>().Deserialize(ref reader, formatterResolver);
                        ___sellDate__b__ = true;
                        break;
                    case 5:
                        ___startPrice__ = formatterResolver.GetFormatterWithVerify<float?>().Deserialize(ref reader, formatterResolver);
                        ___startPrice__b__ = true;
                        break;
                    case 6:
                        ___endDate__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        ___endDate__b__ = true;
                        break;
                    case 7:
                        ___auctionId__ = reader.ReadString();
                        ___auctionId__b__ = true;
                        break;
                    case 8:
                        ___startDate__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        ___startDate__b__ = true;
                        break;
                    case 9:
                        ___userAuctions__ = reader.ReadBoolean();
                        ___userAuctions__b__ = true;
                        break;
                    case 10:
                        ___status__ = reader.ReadString();
                        ___status__b__ = true;
                        break;
                    case 11:
                        ___numberBids__ = formatterResolver.GetFormatterWithVerify<int?>().Deserialize(ref reader, formatterResolver);
                        ___numberBids__b__ = true;
                        break;
                    case 12:
                        ___lastBidPrice__ = formatterResolver.GetFormatterWithVerify<float?>().Deserialize(ref reader, formatterResolver);
                        ___lastBidPrice__b__ = true;
                        break;
                    case 13:
                        ___asset__ = formatterResolver.GetFormatterWithVerify<global::Nefta.ToolboxSdk.Nft.NftAsset>().Deserialize(ref reader, formatterResolver);
                        ___asset__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Marketplace.Auction();
            if(___serial__b__) ____result._serial = ___serial__;
            if(___auctionType__b__) ____result._auctionType = ___auctionType__;
            if(___purchasePrice__b__) ____result._purchasePrice = ___purchasePrice__;
            if(___sellPrice__b__) ____result._sellPrice = ___sellPrice__;
            if(___sellDate__b__) ____result._sellDate = ___sellDate__;
            if(___startPrice__b__) ____result._startPrice = ___startPrice__;
            if(___endDate__b__) ____result._endDate = ___endDate__;
            if(___auctionId__b__) ____result._auctionId = ___auctionId__;
            if(___startDate__b__) ____result._startDate = ___startDate__;
            if(___userAuctions__b__) ____result._userAuctions = ___userAuctions__;
            if(___status__b__) ____result._status = ___status__;
            if(___numberBids__b__) ____result._numberBids = ___numberBids__;
            if(___lastBidPrice__b__) ____result._lastBidPrice = ___lastBidPrice__;
            if(___asset__b__) ____result._asset = ___asset__;

            return ____result;
        }
    }


    public sealed class AuctionsResponseFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Marketplace.AuctionsResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AuctionsResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("results"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("page"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("perPage"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("count"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("results"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("page"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("perPage"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("count"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Marketplace.AuctionsResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Nefta.ToolboxSdk.Marketplace.Auction>>().Serialize(ref writer, value._results, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value._page);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value._perPage);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value._count);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Marketplace.AuctionsResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___results__ = default(global::System.Collections.Generic.List<global::Nefta.ToolboxSdk.Marketplace.Auction>);
            var ___results__b__ = false;
            var ___page__ = default(int);
            var ___page__b__ = false;
            var ___perPage__ = default(int);
            var ___perPage__b__ = false;
            var ___count__ = default(int);
            var ___count__b__ = false;

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
                        ___results__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Nefta.ToolboxSdk.Marketplace.Auction>>().Deserialize(ref reader, formatterResolver);
                        ___results__b__ = true;
                        break;
                    case 1:
                        ___page__ = reader.ReadInt32();
                        ___page__b__ = true;
                        break;
                    case 2:
                        ___perPage__ = reader.ReadInt32();
                        ___perPage__b__ = true;
                        break;
                    case 3:
                        ___count__ = reader.ReadInt32();
                        ___count__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Marketplace.AuctionsResponse();
            if(___results__b__) ____result._results = ___results__;
            if(___page__b__) ____result._page = ___page__;
            if(___perPage__b__) ____result._perPage = ___perPage__;
            if(___count__b__) ____result._count = ___count__;

            return ____result;
        }
    }


    public sealed class CreateAuctionBidRequestFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Marketplace.CreateAuctionBidRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CreateAuctionBidRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("bid_price"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("bid_price"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Marketplace.CreateAuctionBidRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteSingle(value._bidPrice);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Marketplace.CreateAuctionBidRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___bidPrice__ = default(float);
            var ___bidPrice__b__ = false;

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
                        ___bidPrice__ = reader.ReadSingle();
                        ___bidPrice__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Marketplace.CreateAuctionBidRequest();
            if(___bidPrice__b__) ____result._bidPrice = ___bidPrice__;

            return ____result;
        }
    }


    public sealed class CreateAuctionRequestFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Marketplace.CreateAuctionRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CreateAuctionRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ownership_id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("auction_type"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("start_price"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("purchase_price"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("ownership_id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("auction_type"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("start_price"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("purchase_price"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Marketplace.CreateAuctionRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._ownershipId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._auctionType);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteSingle(value._startPrice);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteSingle(value._purchasePrice);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Marketplace.CreateAuctionRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___ownershipId__ = default(string);
            var ___ownershipId__b__ = false;
            var ___auctionType__ = default(string);
            var ___auctionType__b__ = false;
            var ___startPrice__ = default(float);
            var ___startPrice__b__ = false;
            var ___purchasePrice__ = default(float);
            var ___purchasePrice__b__ = false;

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
                        ___ownershipId__ = reader.ReadString();
                        ___ownershipId__b__ = true;
                        break;
                    case 1:
                        ___auctionType__ = reader.ReadString();
                        ___auctionType__b__ = true;
                        break;
                    case 2:
                        ___startPrice__ = reader.ReadSingle();
                        ___startPrice__b__ = true;
                        break;
                    case 3:
                        ___purchasePrice__ = reader.ReadSingle();
                        ___purchasePrice__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Marketplace.CreateAuctionRequest();
            if(___ownershipId__b__) ____result._ownershipId = ___ownershipId__;
            if(___auctionType__b__) ____result._auctionType = ___auctionType__;
            if(___startPrice__b__) ____result._startPrice = ___startPrice__;
            if(___purchasePrice__b__) ____result._purchasePrice = ___purchasePrice__;

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

namespace Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.GamerManagement
{
    using System;
    using Utf8Json;


    public sealed class CurrencyBalanceFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.GamerManagement.CurrencyBalance>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CurrencyBalanceFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("balance"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("balance"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.GamerManagement.CurrencyBalance value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value._balance);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.GamerManagement.CurrencyBalance Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___balance__ = default(int);
            var ___balance__b__ = false;

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
                        ___balance__ = reader.ReadInt32();
                        ___balance__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.GamerManagement.CurrencyBalance();
            if(___balance__b__) ____result._balance = ___balance__;

            return ____result;
        }
    }


    public sealed class GamerProfileFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.GamerManagement.GamerProfile>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public GamerProfileFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("user_id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("username"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("address"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("user_id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("username"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("address"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.GamerManagement.GamerProfile value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._userId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._username);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value._address);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.GamerManagement.GamerProfile Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___userId__ = default(string);
            var ___userId__b__ = false;
            var ___username__ = default(string);
            var ___username__b__ = false;
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
                        ___userId__ = reader.ReadString();
                        ___userId__b__ = true;
                        break;
                    case 1:
                        ___username__ = reader.ReadString();
                        ___username__b__ = true;
                        break;
                    case 2:
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

            var ____result = new global::Nefta.ToolboxSdk.GamerManagement.GamerProfile();
            if(___userId__b__) ____result._userId = ___userId__;
            if(___username__b__) ____result._username = ___username__;
            if(___address__b__) ____result._address = ___address__;

            return ____result;
        }
    }


    public sealed class NativeCurrencyBalanceFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.GamerManagement.NativeCurrencyBalance>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public NativeCurrencyBalanceFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("coin_balance"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("coin_balance_name"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("coin_balance"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("coin_balance_name"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.GamerManagement.NativeCurrencyBalance value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteSingle(value._coinBalance);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._coinBalanceName);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.GamerManagement.NativeCurrencyBalance Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___coinBalance__ = default(float);
            var ___coinBalance__b__ = false;
            var ___coinBalanceName__ = default(string);
            var ___coinBalanceName__b__ = false;

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
                        ___coinBalance__ = reader.ReadSingle();
                        ___coinBalance__b__ = true;
                        break;
                    case 1:
                        ___coinBalanceName__ = reader.ReadString();
                        ___coinBalanceName__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.GamerManagement.NativeCurrencyBalance();
            if(___coinBalance__b__) ____result._coinBalance = ___coinBalance__;
            if(___coinBalanceName__b__) ____result._coinBalanceName = ___coinBalanceName__;

            return ____result;
        }
    }


    public sealed class OwnedAssetFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.GamerManagement.OwnedAsset>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public OwnedAssetFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("IsOwnerInstanceAuctioned"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ownership_id"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("uri"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("created_at"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ownership_amount"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("purchase_price"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("serial"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("asset"), 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("IsOwnerInstanceAuctioned"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ownership_id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("uri"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("created_at"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ownership_amount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("purchase_price"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("serial"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("asset"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.GamerManagement.OwnedAsset value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteBoolean(value.IsOwnerInstanceAuctioned);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._ownershipId);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value._uri);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value._createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteInt32(value._ownershipAmount);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteSingle(value._purchasePrice);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteInt32(value._serial);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<global::Nefta.ToolboxSdk.Nft.NftAsset>().Serialize(ref writer, value._nft, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.GamerManagement.OwnedAsset Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __IsOwnerInstanceAuctioned__ = default(bool);
            var __IsOwnerInstanceAuctioned__b__ = false;
            var ___ownershipId__ = default(string);
            var ___ownershipId__b__ = false;
            var ___uri__ = default(string);
            var ___uri__b__ = false;
            var ___createdAt__ = default(global::System.DateTime);
            var ___createdAt__b__ = false;
            var ___ownershipAmount__ = default(int);
            var ___ownershipAmount__b__ = false;
            var ___purchasePrice__ = default(float);
            var ___purchasePrice__b__ = false;
            var ___serial__ = default(int);
            var ___serial__b__ = false;
            var ___nft__ = default(global::Nefta.ToolboxSdk.Nft.NftAsset);
            var ___nft__b__ = false;

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
                        __IsOwnerInstanceAuctioned__ = reader.ReadBoolean();
                        __IsOwnerInstanceAuctioned__b__ = true;
                        break;
                    case 1:
                        ___ownershipId__ = reader.ReadString();
                        ___ownershipId__b__ = true;
                        break;
                    case 2:
                        ___uri__ = reader.ReadString();
                        ___uri__b__ = true;
                        break;
                    case 3:
                        ___createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        ___createdAt__b__ = true;
                        break;
                    case 4:
                        ___ownershipAmount__ = reader.ReadInt32();
                        ___ownershipAmount__b__ = true;
                        break;
                    case 5:
                        ___purchasePrice__ = reader.ReadSingle();
                        ___purchasePrice__b__ = true;
                        break;
                    case 6:
                        ___serial__ = reader.ReadInt32();
                        ___serial__b__ = true;
                        break;
                    case 7:
                        ___nft__ = formatterResolver.GetFormatterWithVerify<global::Nefta.ToolboxSdk.Nft.NftAsset>().Deserialize(ref reader, formatterResolver);
                        ___nft__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.GamerManagement.OwnedAsset();
            if(___ownershipId__b__) ____result._ownershipId = ___ownershipId__;
            if(___uri__b__) ____result._uri = ___uri__;
            if(___createdAt__b__) ____result._createdAt = ___createdAt__;
            if(___ownershipAmount__b__) ____result._ownershipAmount = ___ownershipAmount__;
            if(___purchasePrice__b__) ____result._purchasePrice = ___purchasePrice__;
            if(___serial__b__) ____result._serial = ___serial__;
            if(___nft__b__) ____result._nft = ___nft__;

            return ____result;
        }
    }


    public sealed class OwnedAssetsResponseFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.GamerManagement.OwnedAssetsResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public OwnedAssetsResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("results"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("page"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("perPage"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("count"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("results"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("page"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("perPage"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("count"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.GamerManagement.OwnedAssetsResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Nefta.ToolboxSdk.GamerManagement.OwnedAsset>>().Serialize(ref writer, value._results, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value._page);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value._perPage);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value._count);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.GamerManagement.OwnedAssetsResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___results__ = default(global::System.Collections.Generic.List<global::Nefta.ToolboxSdk.GamerManagement.OwnedAsset>);
            var ___results__b__ = false;
            var ___page__ = default(int);
            var ___page__b__ = false;
            var ___perPage__ = default(int);
            var ___perPage__b__ = false;
            var ___count__ = default(int);
            var ___count__b__ = false;

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
                        ___results__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Nefta.ToolboxSdk.GamerManagement.OwnedAsset>>().Deserialize(ref reader, formatterResolver);
                        ___results__b__ = true;
                        break;
                    case 1:
                        ___page__ = reader.ReadInt32();
                        ___page__b__ = true;
                        break;
                    case 2:
                        ___perPage__ = reader.ReadInt32();
                        ___perPage__b__ = true;
                        break;
                    case 3:
                        ___count__ = reader.ReadInt32();
                        ___count__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.GamerManagement.OwnedAssetsResponse();
            if(___results__b__) ____result._results = ___results__;
            if(___page__b__) ____result._page = ___page__;
            if(___perPage__b__) ____result._perPage = ___perPage__;
            if(___count__b__) ____result._count = ___count__;

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

namespace Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.GamerAssets
{
    using System;
    using Utf8Json;


    public sealed class RewardUserCurrencyRequestFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.GamerAssets.RewardUserCurrencyRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RewardUserCurrencyRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("amount"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("amount"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.GamerAssets.RewardUserCurrencyRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteSingle(value._amount);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.GamerAssets.RewardUserCurrencyRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___amount__ = default(float);
            var ___amount__b__ = false;

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
                        ___amount__ = reader.ReadSingle();
                        ___amount__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.GamerAssets.RewardUserCurrencyRequest();
            if(___amount__b__) ____result._amount = ___amount__;

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

namespace Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization
{
    using System;
    using Utf8Json;


    public sealed class ConfirmGamerAccountWithEmailCodeFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Authorization.ConfirmGamerAccountWithEmailCode>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ConfirmGamerAccountWithEmailCodeFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("email"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("code"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("email"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("code"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Authorization.ConfirmGamerAccountWithEmailCode value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._email);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._code);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Authorization.ConfirmGamerAccountWithEmailCode Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___email__ = default(string);
            var ___email__b__ = false;
            var ___code__ = default(string);
            var ___code__b__ = false;

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
                        ___email__ = reader.ReadString();
                        ___email__b__ = true;
                        break;
                    case 1:
                        ___code__ = reader.ReadString();
                        ___code__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Authorization.ConfirmGamerAccountWithEmailCode();
            if(___email__b__) ____result._email = ___email__;
            if(___code__b__) ____result._code = ___code__;

            return ____result;
        }
    }


    public sealed class ConvertGuestToFullUserRequestFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Authorization.ConvertGuestToFullUserRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ConvertGuestToFullUserRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("email"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("email"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Authorization.ConvertGuestToFullUserRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._email);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Authorization.ConvertGuestToFullUserRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___email__ = default(string);
            var ___email__b__ = false;

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
                        ___email__ = reader.ReadString();
                        ___email__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Authorization.ConvertGuestToFullUserRequest();
            if(___email__b__) ____result._email = ___email__;

            return ____result;
        }
    }


    public sealed class LoginWithConfirmationCodeRequestFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Authorization.LoginWithConfirmationCodeRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LoginWithConfirmationCodeRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("email"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("code"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("email"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("code"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Authorization.LoginWithConfirmationCodeRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._email);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._code);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Authorization.LoginWithConfirmationCodeRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___email__ = default(string);
            var ___email__b__ = false;
            var ___code__ = default(string);
            var ___code__b__ = false;

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
                        ___email__ = reader.ReadString();
                        ___email__b__ = true;
                        break;
                    case 1:
                        ___code__ = reader.ReadString();
                        ___code__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Authorization.LoginWithConfirmationCodeRequest();
            if(___email__b__) ____result._email = ___email__;
            if(___code__b__) ____result._code = ___code__;

            return ____result;
        }
    }


    public sealed class LoginWithWalletRequestFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Authorization.LoginWithWalletRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LoginWithWalletRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("wallet"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("mid"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("wallet"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("mid"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Authorization.LoginWithWalletRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._wallet);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._mid);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Authorization.LoginWithWalletRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___wallet__ = default(string);
            var ___wallet__b__ = false;
            var ___mid__ = default(string);
            var ___mid__b__ = false;

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
                        ___wallet__ = reader.ReadString();
                        ___wallet__b__ = true;
                        break;
                    case 1:
                        ___mid__ = reader.ReadString();
                        ___mid__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Authorization.LoginWithWalletRequest();
            if(___wallet__b__) ____result._wallet = ___wallet__;
            if(___mid__b__) ____result._mid = ___mid__;

            return ____result;
        }
    }


    public sealed class RequestLoginCodeRequestFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Authorization.RequestLoginCodeRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RequestLoginCodeRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("email"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("email"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Authorization.RequestLoginCodeRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._email);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Authorization.RequestLoginCodeRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___email__ = default(string);
            var ___email__b__ = false;

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
                        ___email__ = reader.ReadString();
                        ___email__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Authorization.RequestLoginCodeRequest();
            if(___email__b__) ____result._email = ___email__;

            return ____result;
        }
    }


    public sealed class SignUpGamerRequestFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Authorization.SignUpGamerRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SignUpGamerRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("public_project_id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("email"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("username"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("wallet"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("public_project_id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("email"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("username"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("wallet"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Authorization.SignUpGamerRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._publicProjectId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._email);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value._username);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value._wallet);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Authorization.SignUpGamerRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___publicProjectId__ = default(string);
            var ___publicProjectId__b__ = false;
            var ___email__ = default(string);
            var ___email__b__ = false;
            var ___username__ = default(string);
            var ___username__b__ = false;
            var ___wallet__ = default(string);
            var ___wallet__b__ = false;

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
                        ___publicProjectId__ = reader.ReadString();
                        ___publicProjectId__b__ = true;
                        break;
                    case 1:
                        ___email__ = reader.ReadString();
                        ___email__b__ = true;
                        break;
                    case 2:
                        ___username__ = reader.ReadString();
                        ___username__b__ = true;
                        break;
                    case 3:
                        ___wallet__ = reader.ReadString();
                        ___wallet__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Authorization.SignUpGamerRequest();
            if(___publicProjectId__b__) ____result._publicProjectId = ___publicProjectId__;
            if(___email__b__) ____result._email = ___email__;
            if(___username__b__) ____result._username = ___username__;
            if(___wallet__b__) ____result._wallet = ___wallet__;

            return ____result;
        }
    }


    public sealed class SignupGuestUserRequestFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Authorization.SignupGuestUserRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SignupGuestUserRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("public_project_id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("username"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("public_project_id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("username"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Authorization.SignupGuestUserRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._publicProjectId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._username);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Authorization.SignupGuestUserRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___publicProjectId__ = default(string);
            var ___publicProjectId__b__ = false;
            var ___username__ = default(string);
            var ___username__b__ = false;

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
                        ___publicProjectId__ = reader.ReadString();
                        ___publicProjectId__b__ = true;
                        break;
                    case 1:
                        ___username__ = reader.ReadString();
                        ___username__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Authorization.SignupGuestUserRequest();
            if(___publicProjectId__b__) ____result._publicProjectId = ___publicProjectId__;
            if(___username__b__) ____result._username = ___username__;

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

namespace Nefta.ToolboxSdk.Formatters.Nefta.ToolboxSdk.Authorization.AuthProviders
{
    using System;
    using Utf8Json;


    public sealed class AuthTokenFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Authorization.AuthProviders.AuthToken>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AuthTokenFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("access_token"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("expires_in"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("refresh_token"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("token_type"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("access_token"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("expires_in"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("refresh_token"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("token_type"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Authorization.AuthProviders.AuthToken value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.access_token);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.expires_in);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.refresh_token);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.token_type);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Authorization.AuthProviders.AuthToken Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __access_token__ = default(string);
            var __access_token__b__ = false;
            var __expires_in__ = default(int);
            var __expires_in__b__ = false;
            var __refresh_token__ = default(string);
            var __refresh_token__b__ = false;
            var __token_type__ = default(string);
            var __token_type__b__ = false;

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
                        __access_token__ = reader.ReadString();
                        __access_token__b__ = true;
                        break;
                    case 1:
                        __expires_in__ = reader.ReadInt32();
                        __expires_in__b__ = true;
                        break;
                    case 2:
                        __refresh_token__ = reader.ReadString();
                        __refresh_token__b__ = true;
                        break;
                    case 3:
                        __token_type__ = reader.ReadString();
                        __token_type__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Authorization.AuthProviders.AuthToken();
            if(__access_token__b__) ____result.access_token = __access_token__;
            if(__expires_in__b__) ____result.expires_in = __expires_in__;
            if(__refresh_token__b__) ____result.refresh_token = __refresh_token__;
            if(__token_type__b__) ____result.token_type = __token_type__;

            return ____result;
        }
    }


    public sealed class EmailResponseFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public EmailResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("email"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("email"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.email);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __email__ = default(string);
            var __email__b__ = false;

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
                        __email__ = reader.ReadString();
                        __email__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailResponse();
            if(__email__b__) ____result.email = __email__;

            return ____result;
        }
    }


    public sealed class EmailsResponseFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailsResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public EmailsResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("data"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("data"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailsResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailResponse[]>().Serialize(ref writer, value.data, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailsResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailResponse[]);
            var __data__b__ = false;

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
                        __data__ = formatterResolver.GetFormatterWithVerify<global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailResponse[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Authorization.AuthProviders.EmailsResponse();
            if(__data__b__) ____result.data = __data__;

            return ____result;
        }
    }


    public sealed class MetaMaskDecryptedMessageDataFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessageData>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MetaMaskDecryptedMessageDataFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("jsonrpc"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("result"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("jsonrpc"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("result"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessageData value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._version);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value._result);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessageData Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___id__ = default(string);
            var ___id__b__ = false;
            var ___version__ = default(string);
            var ___version__b__ = false;
            var ___result__ = default(string);
            var ___result__b__ = false;

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
                        ___version__ = reader.ReadString();
                        ___version__b__ = true;
                        break;
                    case 2:
                        ___result__ = reader.ReadString();
                        ___result__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessageData();
            if(___id__b__) ____result._id = ___id__;
            if(___version__b__) ____result._version = ___version__;
            if(___result__b__) ____result._result = ___result__;

            return ____result;
        }
    }


    public sealed class MetaMaskDecryptedMessageFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessage>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MetaMaskDecryptedMessageFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("data"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("data"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessage value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._name);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessageData>().Serialize(ref writer, value._data, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessage Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___name__ = default(string);
            var ___name__b__ = false;
            var ___data__ = default(global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessageData);
            var ___data__b__ = false;

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
                        ___name__ = reader.ReadString();
                        ___name__b__ = true;
                        break;
                    case 1:
                        ___data__ = formatterResolver.GetFormatterWithVerify<global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessageData>().Deserialize(ref reader, formatterResolver);
                        ___data__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskDecryptedMessage();
            if(___name__b__) ____result._name = ___name__;
            if(___data__b__) ____result._data = ___data__;

            return ____result;
        }
    }


    public sealed class MetaMaskResponseMessageFormatter : global::Utf8Json.IJsonFormatter<global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskResponseMessage>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MetaMaskResponseMessageFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("message"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("message"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskResponseMessage value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value._id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value._message);
            
            writer.WriteEndObject();
        }

        public global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskResponseMessage Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var ___id__ = default(string);
            var ___id__b__ = false;
            var ___message__ = default(string);
            var ___message__b__ = false;

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
                        ___message__ = reader.ReadString();
                        ___message__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Nefta.ToolboxSdk.Authorization.AuthProviders.MetaMaskResponseMessage();
            if(___id__b__) ____result._id = ___id__;
            if(___message__b__) ____result._message = ___message__;

            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
