namespace CodeBase.Domain
{
    public struct InputData
    {
        public readonly float Horizontal;
        public readonly float Vertical;

        public InputData(float horizontal, float vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }
}