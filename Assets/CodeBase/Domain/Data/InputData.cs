namespace CodeBase.Domain
{
    public struct InputData
    {
        public float Horizontal;
        public float Vertical;

        public InputData(float horizontal, float vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }
}