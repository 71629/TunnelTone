namespace TunnelTone.GameSystem
{
    [System.Serializable]
    public class Song
    {
        public string title { get; set; }
        public string artist { get; set; }
        public float bpm { get; set; }
        public int[] difficulty { get; set; }
        public float previewStart { get; set; }
        public float previewDuration { get; set; }
    }
}