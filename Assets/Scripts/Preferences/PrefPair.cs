namespace Preferences
{
    public class PrefPair<T>
    {
        public string Key { get; set; }
        public T DefaultValue { get; set; }
        
        public PrefPair(string key, T defaultValue)
        {
            Key = key;
            DefaultValue = defaultValue;
        }
    }
}