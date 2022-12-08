using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LibSmi.Net.Node
{
    public unsafe class SmiNode: LazyStructProxy, IComparable<SmiNode>, IEquatable<SmiNode>
    {
        private readonly SmiNodeStruct* _nodePtr;

        private readonly Lazy<string?> _name;

        private readonly Lazy<string?> _format;

        private readonly Lazy<string?> _units;

        private readonly Lazy<string?> _description;

        private readonly Lazy<string?> _reference;

        private int? _hashCode;

        internal SmiNode(SmiNodeStruct* nodePtr): base(nodePtr)
        {
            _nodePtr = nodePtr;

            _name = LazyAnsiStringMarshal(_nodePtr->Name);

            _format = LazyAnsiStringMarshal(_nodePtr->Format);

            _units = LazyAnsiStringMarshal(_nodePtr->Units);

            _description = LazyAnsiStringMarshal(_nodePtr->Description);

            _reference = LazyAnsiStringMarshal(_nodePtr->Reference);
        }

        internal static unsafe SmiNode? FromPtr(SmiNodeStruct* nodePtr)
        {
            if (nodePtr == null)
            {
                return null;
            }

            return new(nodePtr);
        }

        internal SmiNodeStruct* UnderlyingPtr => _nodePtr;

        public string? Name => _name.Value;

        public string? Format => _format.Value;

        public string? Units => _units.Value;

        public string? Description => _description.Value;

        public string? Reference => _reference.Value;
        
        public SmiDecl Decl => _nodePtr->Decl;

        public SmiAccess Access => _nodePtr->Access;

        public SmiStatus Status => _nodePtr->Status;

        public SmiNodeKind NodeKind => _nodePtr->NodeKind;
        
        public SmiValue Value => _nodePtr->Value;

        public SmiIndexKind IndexKind => _nodePtr->IndexKind;

        public int Implied => _nodePtr->Implied;

        public int Create => _nodePtr->Create;

        public uint OidLen => _nodePtr->OidLen;

        public unsafe ReadOnlySpan<uint> GetOid()
        {
            return new Span<uint>(
                (void*)_nodePtr->OidPtr,
                (int)_nodePtr->OidLen);
        }

        public unsafe string ToOidString()
        {
            var oid = GetOid();
            var sb = new StringBuilder();
            for (var i = 0; i < _nodePtr->OidLen; i++)
            {
                sb.Append($".{oid[i]}");
            }
            return sb.ToString();
        }

        /*
        public static bool operator ==(SmiNode s, SmiNode t)
        {
            return s.CompareTo(t) == 0;
        }

        public static bool operator !=(SmiNode s, SmiNode t)
        {
            return s.CompareTo(t) != 0;
        }

        public static bool operator <(SmiNode s, SmiNode t)
        {
            return s.CompareTo(t) < 0;
        }

        public static bool operator >(SmiNode s, SmiNode t)
        {
            return s.CompareTo(t) > 0;
        }

        public static bool operator <=(SmiNode s, SmiNode t)
        {
            return s.CompareTo(t) <= 0;
        }

        public static bool operator >=(SmiNode s, SmiNode t)
        {
            return s.CompareTo(t) >= 0;
        }
        */

        public bool Equals(SmiNode? other)
        {
            return CompareTo(other) == 0;
        }
        

        public override bool Equals(object? obj)
        {
            return obj is SmiNode node && Equals(node);
        }

        public override int GetHashCode()
        {
            // _hashCode ??= GetOid().ToArray().GetHashCode();
            // return _hashCode.Value;
            return ((nint)_nodePtr).GetHashCode();
        }

        /// <summary>
        /// S=(s(1), s(2), ...s(p)) 
        /// (it has "p" elements)
        /// T=(t(1), t(2), ... t(q)) 
        /// (it has "q" elements)
        ///
        /// Sequence S is lexicographically equal to sequence T if the lengths are equal (that is, p=q) and:
        /// for all i<=p, s(i)=t(i)
        ///
        /// Sequence S is lexicographically less than sequence T if either of the following are true:
        /// (1) p < q, and for all i <= p, s(i)=t(i)
        /// there exists an i such that (i<=p) & (i<=q), s(i) < t(i) and for all j < i, s(j)=t(j)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(SmiNode? other)
        {
            const int ThisEqualsOther = 0;
            const int ThisFollowsOther = 1;
            const int ThisPrecedesOther = -1;

            if (other?.OidLen == null)
            {
                return ThisFollowsOther;
            }

            var s = this.GetOid();
            var t = other.GetOid();

            var p = s.Length;
            var q = t.Length;


            if (p > q)
            {
                return ThisFollowsOther;
            }

            for (var i = 0; i < p; i++)
            {
                var si = s[i];
                var ti = t[i];

                if (si == ti)
                {
                    // s(i) == t(i)
                    // keep going
                    continue;
                }
                
                if (si < ti)
                {
                    // s(i) < t(i)
                    // s(j) == t(j) for all j < i
                    // then: s < t
                    return ThisPrecedesOther;
                }

                // s(i) > t(i)
                // s(j) == t(j) for all j < i
                // then: s > t
                return ThisFollowsOther;
            }

            // at this point
            // (1) : p <= q
            // (2) : s(i) == t(i) for all i < p
            // check the lenght
            return p == q
                ? ThisEqualsOther     // p == q
                : ThisPrecedesOther;  // p < q
        }
    }
}