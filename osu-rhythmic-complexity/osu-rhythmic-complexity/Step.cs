using System;
using System.Collections.Generic;

namespace ppcalc
{
    enum NoteType 
    {
        circle = 1, slider = 2, spinner = 8
    }

    class Note : IComparable<Note>, IEquatable<Note>
    {
        public double duration { get; set; }
        public int quantity { get; set; }
        public NoteType type { get; set; }

        public Note(double _duration, int _quantity, NoteType _type) {
            duration = _duration;
            quantity = _quantity;
            type = _type;
        }

        public override string ToString()
        {
            return type + ":" + quantity + ":" + duration;
        }

        public bool Equals(Note other)
        {
            Note rhs = (Note)other;
            return this.duration == rhs.duration && this.type == rhs.type && this.quantity == rhs.quantity;
        }

        public int CompareTo(Note other)
        {
            if (other == null)
            {
                return 1;
            }
            return duration.CompareTo(other.duration);
        }

        public static bool operator >(Note lhs, Note rhs)
        {
            return lhs.CompareTo(rhs) == 1;
        }

        public static bool operator <(Note lhs, Note rhs)
        {
            return lhs.CompareTo(rhs) == -1;
        }

        public static bool operator >=(Note lhs, Note rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        public static bool operator <=(Note lhs, Note rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }

        public static bool operator ==(Note lhs, Note rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Note lhs, Note rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }
            return !lhs.Equals(rhs);
        }

        public override int GetHashCode() {
            return (int)duration;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj);
        }
    }
}