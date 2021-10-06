using System.Collections.Generic;


namespace RainstormTech.Storm.ImageProxy.Options
{
    public class ImageResizerOptions
    {
        public string? SmallSize { get; set; }

        public string? MediumSize { get; set; }

        public string? HeroSize { get; set; }

        public IDictionary<string, ImageSize> PredefinedImageSizes
        {
            get
            {
                var dic = new Dictionary<string, ImageSize>();
                if (!string.IsNullOrWhiteSpace(this.SmallSize))
                {
                    dic.Add("small", ImageSize.Parse(SmallSize, "small"));
                }
                if (!string.IsNullOrWhiteSpace(this.MediumSize))
                {
                    dic.Add("medium", ImageSize.Parse(MediumSize, "medium"));
                }
                if (!string.IsNullOrWhiteSpace(this.HeroSize))
                {
                    dic.Add("hero", ImageSize.Parse(HeroSize, "hero"));
                }

                return dic;
            }
        }
    }
}
