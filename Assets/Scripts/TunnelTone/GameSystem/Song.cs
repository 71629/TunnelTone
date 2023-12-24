namespace TunnelTone.GameSystem
{
    [System.Serializable]
    public class Song
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public float Bpm { get; set; }
        public int[] Difficulty { get; set; }
        public float PreviewStart { get; set; }
        public float PreviewDuration { get; set; }
    }
}