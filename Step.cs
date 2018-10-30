namespace ppcalc
{
    class Step
    {
        int distance;
        int angle;
        //slider shit

        public override bool Equals(object obj)
        {
            Step rhs = (Step)obj;
            return this.distance == rhs.distance && this.angle == rhs.angle;
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