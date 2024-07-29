using System;
using System.IO;

namespace ProDisposable
{
    class Program
    {
        static void Main(string[] args)
        {
            //使用using语句块创建了一个FileReader对象，并读取了文件内容。
            //当离开using语句块时，系统会自动调用Dispose方法，释放由FileReader对象所使用的所有资源。
            using (var reader = new FileReader(@"C:\Users\Prozkb\Desktop\文档\腾讯Mall\GetItem.txt"))
            {
                byte[] buffer = reader.Read(1024);

                //使用buffer
            }

            var readerS = new FileReader(@"C:\Users\Prozkb\Desktop\文档\腾讯Mall\GetItem.txt");
            byte[] bufferS = readerS.Read(1024);
            readerS.Dispose();
        }
    }

    /// <summary>
    /// 在上面的示例中，FileReader实现了IDisposable接口，实现了Dispose()方法用于释放资源。
    /// 在Dispose方法中，首先检查对象是否已经被释放，然后释放_fileStream对象。
    /// 在类的析构函数中，调用了Dispose方法，用于释放资源。这样，即使用户没有调用Dispose方法，也能确保资源在对象被垃圾回收之前被释放。
    /// </summary>
    public class FileReader : IDisposable
    {
        private FileStream _fileStream;
        private bool _disposed = false;

        public FileReader(string filePath)
        {
            _fileStream = new FileStream(filePath, FileMode.Open);
        }

        public byte[] Read(int length)
        {
            byte[] buffer = new byte[length];
            _fileStream.Read(buffer, 0, length);
            return buffer;
        }

        //当程序离开using的【}】的时候，才会进到这里
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); //this-->ProDisposable.FileReader  就是通知垃圾回收器不需要再调用系统的垃圾回收了，我代码里已经回收了

            //在C#中，GC.SuppressFinalize(this)用于通知垃圾回收器，在不需要进行对象终结时不要调用该对象的Finalize方法。
            //也就是说，当我们已经在Dispose方法中主动释放了资源时，就可以使用SuppressFinalize方法告诉垃圾回收器不要调用该对象的Finalize方法。

            //这个方法的作用是，避免垃圾回收器调用对象的Finalize方法，从而节省系统资源，提高程序的性能。

            //需要注意的是，GC.SuppressFinalize(this)方法只能在实现了IDisposable接口的类中使用，并且最好在Dispose方法中调用，以确保对象能够及时释放资源。
            //此外，在实现Dispose方法时，也应该根据需要在Finalize方法中调用Dispose方法，以确保在对象未被垃圾回收之前资源得到释放。

            //可以简单理解为：告诉垃圾回收器，我已经在Dispose方法中手动释放了资源，对象的析构函数（Finalize方法）可以不用调用了，这样可以提高程序性能，避免资源浪费。
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_fileStream != null)
                    {
                        _fileStream.Dispose();
                        _fileStream = null;
                    }
                }

                _disposed = true;
            }
        }

        ~FileReader()
        {
            Dispose(false);
        }
    }

    public class FileReaders : IDisposable
    {
        public void Dispose() { }
    }
}
