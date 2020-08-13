namespace SimpBot
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class TenorRequest
    {
        [JsonProperty("weburl")]
        public Uri Weburl { get; set; }

        [JsonProperty("results")]
        public Result[] Results { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("tags")]
        public object[] Tags { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("media")]
        public Media[] Media { get; set; }

        [JsonProperty("created")]
        public double Created { get; set; }

        [JsonProperty("shares")]
        public long Shares { get; set; }

        [JsonProperty("itemurl")]
        public Uri Itemurl { get; set; }

        [JsonProperty("composite")]
        public object Composite { get; set; }

        [JsonProperty("hasaudio")]
        public bool Hasaudio { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("nanomp4")]
        public Mp4 Nanomp4 { get; set; }

        [JsonProperty("nanowebm")]
        public Webm Nanowebm { get; set; }

        [JsonProperty("tinygif")]
        public MediumgifClass Tinygif { get; set; }

        [JsonProperty("tinymp4")]
        public Mp4 Tinymp4 { get; set; }

        [JsonProperty("tinywebm")]
        public Webm Tinywebm { get; set; }

        [JsonProperty("webm")]
        public Webm Webm { get; set; }

        [JsonProperty("gif")]
        public GifClass Gif { get; set; }

        [JsonProperty("mp4")]
        public Mp4 Mp4 { get; set; }

        [JsonProperty("loopedmp4")]
        public Loopedmp4 Loopedmp4 { get; set; }

        [JsonProperty("mediumgif")]
        public MediumgifClass Mediumgif { get; set; }

        [JsonProperty("nanogif")]
        public MediumgifClass Nanogif { get; set; }
    }

    public partial class GifClass
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("dims")]
        public long[] Dims { get; set; }

        [JsonProperty("preview")]
        public Uri Preview { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }
    }

    public partial class Loopedmp4
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("dims")]
        public long[] Dims { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("preview")]
        public Uri Preview { get; set; }

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public long? Size { get; set; }
    }

    public partial class MediumgifClass
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("dims")]
        public long[] Dims { get; set; }

        [JsonProperty("preview")]
        public Uri Preview { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }
    }

    public partial class Mp4
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("dims")]
        public long[] Dims { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("preview")]
        public Uri Preview { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }
    }

    public partial class Webm
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("dims")]
        public long[] Dims { get; set; }

        [JsonProperty("preview")]
        public Uri Preview { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }
    }
}
