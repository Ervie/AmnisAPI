namespace API.Model
{
    public class SongMetadata
    {
        public string Title { get; set; }

        public string Artist { get; set; }

        public SongMetadata()
        {
        }

        public SongMetadata(string title, string artist)
        {
            this.Title = title;
            this.Artist = artist;
        }

        public SongMetadata(string metadata)
        {
            var splittedMetadata = metadata.Split(" - ");

            if (splittedMetadata.Length >= 2)
            {
                Title = splittedMetadata[1];
                Artist = splittedMetadata[0];
            }
            else
            {
                Title = metadata;
            }
        }
    }
}
