using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using TweetSharp;
using System.Timers;

namespace DoesNotExistBot
{
    class Program
    {
        public static System.Timers.Timer uploadTime;
        private static int milisecondsToUpload = 7200000;

        static Dictionary<string, Stream> imageDict = new Dictionary<string, Stream>();
        static BotInfo botInfo;
        static TwitterService twitterService = ConnectToTwitter();


        static void Main(string[] args)
        {
            botInfo = new BotInfo("botInfo.txt");

            uploadTime = new System.Timers.Timer(milisecondsToUpload);
            
            uploadTime.Elapsed += new ElapsedEventHandler(CallBackPublicPicture);
            uploadTime.AutoReset = true;
            uploadTime.Enabled = true;

            Console.WriteLine("Write QUIT to quit the application");
            string quit;
            do
            {
                quit = Console.ReadLine();
            } while (quit != "QUIT");
        }

        static void CallBackPublicPicture(object source, ElapsedEventArgs e)
        {
            PublicPicture("https://thispersondoesnotexist.com/image", "image.jpg");
        }

        static Stream DownloadPicture(string url, string imagePath)
        {
            WebClient client = new WebClient();
            try
            {
                MemoryStream image = new MemoryStream(client.DownloadData(new Uri(url)));
                Console.WriteLine("Downloaded Image");

                return image;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: Couldn't download image");
                return null;
            }
        }

        static TwitterService ConnectToTwitter()
        {
            try
            {
                //Connect using credentials
                var service = new TwitterService("Service1", "Service2");
                service.AuthenticateWith("Authentication1", "Authentication2");
                Console.WriteLine("Connected to Twitter");

                return service;
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't connect to Twitter");
                return null;
            }
        }

        static void PublicPicture(string fromUrl, string imagePath)
        {
            Stream imageStream = DownloadPicture(fromUrl, imagePath);

            imageDict.Add(imagePath, imageStream);

            try
            {
                twitterService.SendTweetWithMedia(new SendTweetWithMediaOptions { Status = "Unknow Nº " + botInfo.ActualPublication, Images = imageDict });
                Console.WriteLine("Image " + botInfo.ActualPublication + " published");

                botInfo.UpdateInfo();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Can't upload image " + botInfo.ActualPublication);
            }

            imageDict[imagePath].Close();
            imageDict[imagePath].Dispose();
            imageDict.Clear();
        }

    }
}
