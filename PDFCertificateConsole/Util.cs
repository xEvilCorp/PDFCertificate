namespace PDFCertificateConsole
{
    public class Util
    {
        public static bool FileToByteArray(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }

        public static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine($"Path {Path.GetDirectoryName(path)} created.");
            }
        }

        public static void Wait()
        {
            new AutoResetEvent(false).WaitOne();
        }


    }
}
