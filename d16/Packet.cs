namespace Day16;

using static Helpers;
using static Utils;

public record Packet(bool[] bits)
{
    private const int SumTypeId = 0;
    private const int ProductTypeId = 1;
    private const int MinimumProductTypeId = 2;
    private const int MaximumProductTypeId = 3;
    private const int LiteralTypeId = 4;
    private const int GreaterThanTypeId = 5;
    private const int LessThanTypeId = 6;
    private const int EqualToTypeId = 7;

    public static Packet FromHex(string hex) => new Packet(ToBits(PadHex(hex)).ToArray());

    //public string BitString => ToBitString(this.bits);
    public int Version => ToInt(this.bits[0..(0 + 3)]);
    public int TypeId => ToInt(this.bits[3..(3 + 3)]);

    public bool IsLiteral => this.TypeId == LiteralTypeId;
    public bool IsOperator => !this.IsLiteral;

    public bool OperatorLengthTypeId => this.bits[6];

    public int OperatorTotalLength => this.IsOperator && !this.OperatorLengthTypeId ? ToInt(this.bits[7..(7 + 15)]) : -1;
    //public string OperatorTotalLengthBitString => this.IsOperator && !this.OperatorLengthTypeId ? ToBitString(this.bits[7..(7 + 15)]) : string.Empty;

    public int OperatorSubPacketCount => this.IsOperator && this.OperatorLengthTypeId ? ToInt(this.bits[7..(7 + 11)]) : -1;
    //public string OperatorSubPacketCountBitString => this.IsOperator && this.OperatorLengthTypeId ? ToBitString(this.bits[7..(7 + 11)]) : string.Empty;

    public long LiteralValue => this.IsLiteral ? this.GetLiteralValue(out _) : -1;
    //public string LiteralValueBitString => this.IsLiteral ? ToBitString(this.bits[6..this.PacketLength]) : string.Empty;
    private long GetLiteralValue(out int packetLength)
    {
        if (!this.IsLiteral)
        {
            packetLength = -1;
            return -1;
        }
        packetLength = 6;
        var payload = this.bits[6..];
        var hasMore = true;
        var literalBits = new List<bool>();

        while (hasMore)
        {
            packetLength += 5;
            hasMore = payload[0];
            literalBits.AddRange(payload[1..5]);
            payload = payload[5..];
        }

        return ToLong(literalBits);
    }


    public long OperatorValue => this.GetOperatorValue();
    private long GetOperatorValue()
    {
        if (!this.IsOperator)
        {
            return -1L;
        }
        var children = this.GetOperatorSubPackets().ToList();
        return this.TypeId switch
        {
            SumTypeId => children.Sum(item => item.Value),
            ProductTypeId => children.Count == 0 ? 0L : children.Aggregate(1L, (acc, item) => acc * item.Value),
            MinimumProductTypeId => children.Min(item => item.Value),
            MaximumProductTypeId => children.Max(item => item.Value),
            GreaterThanTypeId => children[0].Value > children[1].Value ? 1L : 0L,
            LessThanTypeId => children[0].Value < children[1].Value ? 1L : 0L,
            EqualToTypeId => children[0].Value == children[1].Value ? 1L : 0L,
            _ => throw new NotImplementedException(),
        };
    }

    public long Value => this.IsLiteral ? this.LiteralValue : this.OperatorValue;

    public IEnumerable<Packet> GetOperatorSubPackets()
    {
        if (!this.IsOperator)
        {
            return Enumerable.Empty<Packet>();
        }

        if (!this.OperatorLengthTypeId)
        {
            var result = new List<Packet>();
            int remainingBits = this.OperatorTotalLength;
            int start = 3 + 3 + 1 + 15;
            int end = start + remainingBits;
            int offset = start;

            while (remainingBits > 0 && offset < end)
            {
                var packet = new Packet(this.bits[offset..end]);
                result.Add(packet);
                offset += packet.PacketLength;
            }

            return result;
        }
        else
        {
            var result = new List<Packet>();
            int remainingPackets = this.OperatorSubPacketCount;
            int start = 3 + 3 + 1 + 11;
            int end = this.bits.Length;
            int offset = start;

            while (remainingPackets > 0 && offset < end)
            {
                var packet = new Packet(this.bits[offset..end]);
                result.Add(packet);
                offset += packet.PacketLength;
                remainingPackets--;
            }

            return result;
        }
    }

    public IEnumerable<Packet> GetAllPackets() => new[] { this }.Concat(this.GetOperatorSubPackets().SelectMany(packet => packet.GetAllPackets())).ToList();

    public int PacketLength
      => this.IsLiteral
        ? (this.GetLiteralValue(out var length) > -1 ? length : -1)
        : (
            3 + 3 + 1 +
            (this.OperatorLengthTypeId ? 11 : 15) +
            (this.OperatorLengthTypeId ? 0 : this.OperatorTotalLength) +
            (this.OperatorLengthTypeId ? this.GetOperatorSubPackets().Sum(item => item.PacketLength) : 0)
        );
}
