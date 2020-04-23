using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MyStream : Stream
    {
        FileStream inStream;
        long totalBytesRead = 0;
        long size = 0;
        internal MyStream(string filePath)
        {
            inStream = File.OpenRead(filePath);
        }
        public override long Position
        {
            get { return totalBytesRead; }
            set { totalBytesRead = value; }

        }

        public override bool CanRead => throw new NotImplementedException();

        public override bool CanSeek => throw new NotImplementedException();

        public override bool CanWrite => throw new NotImplementedException();

        public override long Length
        {
            get { return size; }
        }

        public override void Flush()
        {

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int countRead = inStream.Read(buffer, offset, count);
            return countRead;
        }

        public override void Close()
        {
            inStream.Close();
            base.Close();
        }
        protected override void Dispose(bool disposing)
        {
            inStream.Dispose();
            base.Dispose(disposing);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            size = value;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
