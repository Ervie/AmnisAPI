namespace API.Model
{
    public class RadioStation
    {
        public int Id { get; set; }

        public string ChannelName { get; set; }

        public string ChannelUrl { get; set; }

        public bool HasMetadata { get; set; }

        public string LogoUrl { get; set; }
    }
}
