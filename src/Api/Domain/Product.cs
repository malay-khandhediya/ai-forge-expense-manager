using System.Runtime.Serialization;

namespace Peddle.ParameterStore.Domain;

public enum Product
{
    [EnumMember(Value = "auction")]
    Auction,
    [EnumMember(Value = "buyer")]
    Buyer,
    [EnumMember(Value = "carrier")]
    Carrier,
    [EnumMember(Value = "charity")]
    Charity,
    [EnumMember(Value = "comms")]
    Comms,
    [EnumMember(Value = "finance")]
    Finance,
    [EnumMember(Value = "peddler")]
    Peddler,
    [EnumMember(Value = "publisher")]
    Publisher,
    [EnumMember(Value = "seller")]
    Seller,
    [EnumMember(Value = "universal")]
    Universal,
    [EnumMember(Value = "valuations")]
    Valuations,
    [EnumMember(Value = "identity")]
    Identity
}