namespace ppcalc
{
    enum NoteType 
    {
        circle, slider, bouncer, spinner
    }

    class Note
    {
        NoteType type;
        double duration;
        int quantity;

        public Note(doublt _duration, int _quantity, NoteType _type) {
            duration = _duration;
            quantity = _quantity;
            type = _type;
        }

        public override bool Equals(object obj)
        {
            Step rhs = (Step)obj;
            return this.duration == rhs.duration && this.quantity == rhs.quantity 
                                    && this.type = rhs.type;
        }

        /*public static bool operator == (Step lhs, Step rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }

        public static bool operator != (Step lhs, Step rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }
            return !lhs.Equals(rhs);
        }*/
    }
}