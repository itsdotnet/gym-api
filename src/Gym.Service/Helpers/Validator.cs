namespace Gym.Service.Helpers;

public static class Validator
{
    public static bool IsImage(string filePath)
    {
        string extension = Path.GetExtension(filePath);
        if (extension is null)
        {
            string ext = extension.ToLower();
            return ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".bmp" || ext == ".tiff";
        }
        return false;
    }

    public static bool IsVideo(string filePath)
    {
        string extension = Path.GetExtension(filePath);
        if (extension is null)
        {
            string ext = extension.ToLower();
            return ext == ".mp4" || ext == ".avi" || ext == ".mkv" || ext == ".mov" || ext == ".wmv" || ext == ".flv";
        }
        return false;
    }
}