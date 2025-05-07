using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LauncherApp.Services;

public class IconExtractor
{
    private static readonly ConcurrentDictionary<string, ImageSource> _iconCache = new();

    public static ImageSource GetIcon(string processPath)
    {
        if (string.IsNullOrEmpty(processPath)) return null;

        return Application.Current.Dispatcher.Invoked(() =>
        {
            try
            {
                using var icon = Icon.ExtractAssociatedIcon(processPath);
                using var bitmap = icon.ToBitmap();
                
                var stream = new MemoryStream();
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Важно!
                return bitmapImage;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return DefaultIcon();
            }
        });
    }

    public static ImageSource DefaultIcon()
    {
        try
        {
            return new BitmapImage(new Uri("/assets/menu2.png"));
        }
        catch
        {
            return new BitmapImage();
        }
    }
}
public static class DispatcherExtensions
{
    public static T Invoked<T>(this Dispatcher dispatcher, Func<T> func)
    {
        if (dispatcher.CheckAccess())
            return func();
        
        return dispatcher.Invoke(func);
    }
}