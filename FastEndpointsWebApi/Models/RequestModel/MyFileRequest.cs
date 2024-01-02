namespace EndpointsFastWebApi.Models.RequestModel
{
    public class MyFileRequest
    {
        //public int Width { get; set; }
        //public int Height { get; set; }
        //public IFormFile File1 { get; set; }
        //public IFormFile File2 { get; set; }
        //public IFormFile File3 { get; set; }

        public IEnumerable<IFormFile> Cars { get; set; }
        public List<IFormFile> Boats { get; set; }
        public IFormFileCollection Jets { get; set; }
    }
}
