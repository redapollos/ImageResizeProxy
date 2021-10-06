
using System.IO;
using System.Threading.Tasks;

namespace RainstormTech.Storm.ImageProxy
{
    public interface IImageResizerService
    {
        Task<Stream> ResizeAsync(string url, string size, string output, string mode);
    }
}
