using System;

namespace DiffMatchPatch
{
    internal struct HalfMatchResult : IEquatable<HalfMatchResult>
    {
        public HalfMatchResult(string prefix1, string suffix1, string prefix2, string suffix2, string commonMiddle)
        {
            Prefix1 = prefix1 ?? throw new ArgumentNullException(nameof(prefix1));
            Suffix1 = suffix1 ?? throw new ArgumentNullException(nameof(suffix1));
            Prefix2 = prefix2 ?? throw new ArgumentNullException(nameof(prefix2));
            Suffix2 = suffix2 ?? throw new ArgumentNullException(nameof(suffix2));
            CommonMiddle = commonMiddle ?? throw new ArgumentNullException(nameof(commonMiddle));
        }

        public HalfMatchResult Reverse()
        {
            return new HalfMatchResult(Prefix2, Suffix2, Prefix1, Suffix1, CommonMiddle);
        }

        public string Prefix1 { get; }
        public string Suffix1 { get; }
        public string CommonMiddle { get; }
        public string Prefix2 { get; }
        public string Suffix2 { get; }
        public bool IsEmpty => string.IsNullOrEmpty(CommonMiddle);

        public static readonly HalfMatchResult Empty = new HalfMatchResult();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != GetType()) return false;
            return Equals((HalfMatchResult)obj);
        }

        public bool Equals(HalfMatchResult other)
        {
            return string.Equals(Prefix1, other.Prefix1) && string.Equals(Suffix1, other.Suffix1) && string.Equals(CommonMiddle, other.CommonMiddle) && string.Equals(Prefix2, other.Prefix2) && string.Equals(Suffix2, other.Suffix2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Prefix1 != null ? Prefix1.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Suffix1 != null ? Suffix1.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CommonMiddle != null ? CommonMiddle.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Prefix2 != null ? Prefix2.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Suffix2 != null ? Suffix2.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(HalfMatchResult left, HalfMatchResult right) => Equals(left, right);

        public static bool operator !=(HalfMatchResult left, HalfMatchResult right) => !Equals(left, right);

        public static bool operator >(HalfMatchResult left, HalfMatchResult right) => left.CommonMiddle.Length > right.CommonMiddle.Length;

        public static bool operator <(HalfMatchResult left, HalfMatchResult right) => left.CommonMiddle.Length < right.CommonMiddle.Length;

        public override string ToString() => $"[{Prefix1}/{Prefix2}] - {CommonMiddle} - [{Suffix1}/{Suffix2}]";
    }
}
