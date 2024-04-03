namespace GameCore.Source.Domain.Data
{
    public struct EnvironmentData
    {

        public EnvironmentData(string i18NLang, bool isMobilePlatform)
        {
            IsMobilePlatform = isMobilePlatform;
            Lang = i18NLang;
        }

        public string Lang { get; }
        public bool IsMobilePlatform { get; }
    }
}