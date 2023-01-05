namespace CodeBase.Extensions
{
    public static class Extensions
    {
        public static float AsPercentFactor(this float value) => value / 100;

        public static bool ContainsInInterval(this float value, float min, float max) => value >= min && value <= max;
        public static bool ContainsInInterval(this int value, int min, int max) => value >= min && value <= max;
    }
}