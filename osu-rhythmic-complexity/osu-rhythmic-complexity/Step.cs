namespace ppcalc
{
    enum NoteType 
    {
        circle = 1, slider = 2, spinner = 8
    }

    internal class Note
    {
        public double duration { get; set; }
        public int quantity { get; set; }
        public NoteType type { get; set; }

        public Note(double _duration, int _quantity, NoteType _type) {
            duration = _duration;
            quantity = _quantity;
            type = _type;
        }

        public override bool Equals(object obj)
        {
            Note rhs = (Note)obj;
            return this.duration == rhs.duration && this.type == rhs.type;
        }

        /*public static bool operator == (Note lhs, Note rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }

        public static bool operator != (Note lhs, Note rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }
            return !lhs.Equals(rhs);
        }*/
    }
}