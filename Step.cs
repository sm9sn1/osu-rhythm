namespace ppcalc
{
    class Note
    {
        double duration;
        int quantity;

        public Note(doublt _duration, int _quantity) {
            duration = _duration;
            quantity = _quantity;
        }

        public override bool Equals(object obj)
        {
            Step rhs = (Step)obj;
            return this.duration == rhs.duration && this.quantity == rhs.quantity;
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