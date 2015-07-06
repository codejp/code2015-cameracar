using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CameraCar.Controllers
{
    public class PhotoController : Controller
    {
        private static int _AvailableBuffNo = 0;

        private static byte[] _PhotoPreviewBuff1;

        private static byte[] _PhotoPreviewBuff2;

        private static object _syncBuff = new object();

        private static DateTime _LastAccessUTC;

        private static Thread _TakingPhotoPreviewWorkerThread;

        private static object _syncThread = new object();

#if DEBUG
        private static int _Counter = 0;
#endif
        public static void TakingPhotoPreviewWorker(object param)
        {
            var server = param as HttpServerUtilityBase;
            var imagePath = "/home/pi/ramdisk/photopreview.jpg";

            for (;;)
            {
                var periodUTC = DateTime.UtcNow;

#if DEBUG
                var photoType = _Counter % 2;
                _Counter++;
                imagePath = server.MapPath(photoType == 0 ?
                    "~/App_Data/penguin.jpg" :
                    "~/App_Data/umiiguana.jpg");
#else
                // take photo using fswebcam - low resolution/low quality/high compress rate
                Process.Start("fswebcam", "-q -r 320 --jpeg 30 --no-banner " + imagePath)
                    .WaitForExit();
#endif
                var imageBuff = System.IO.File.ReadAllBytes(imagePath);
                if (_AvailableBuffNo == 1)
                    _PhotoPreviewBuff2 = imageBuff;
                else
                    _PhotoPreviewBuff1 = imageBuff;
                lock (_syncBuff)
                {
                    _AvailableBuffNo = _AvailableBuffNo == 1 ? 2 : 1;
                }

                // auto terminate this thread if no access.
                if ((DateTime.UtcNow - _LastAccessUTC).TotalSeconds > 10)
                {
                    lock (_syncThread)
                    {
                        _TakingPhotoPreviewWorkerThread = null;
                        return;
                    }
                }

                Thread.Sleep((int)Math.Max((periodUTC.AddSeconds(1)- DateTime.UtcNow).TotalMilliseconds, 0));
            }
        }

        public static void StartTakingPhotoPreviewWorkerIfNotStarted(HttpServerUtilityBase server)
        {
            lock (_syncThread)
            {
                if (_TakingPhotoPreviewWorkerThread == null)
                {
                    _TakingPhotoPreviewWorkerThread = new Thread(TakingPhotoPreviewWorker);
                    _TakingPhotoPreviewWorkerThread.Start(server);
                }
            }
        }

        public static byte[] GetPhotoPreviewBuff(HttpServerUtilityBase server)
        {
            _LastAccessUTC = DateTime.UtcNow;
            StartTakingPhotoPreviewWorkerIfNotStarted(server);

            var buff = default(byte[]);
            lock (_syncBuff)
            {
                buff = _AvailableBuffNo == 1 ? _PhotoPreviewBuff1 : _PhotoPreviewBuff2;
            }
            if (buff == null)
            {
                return System.IO.File.ReadAllBytes(server.MapPath("~/App_Data/_blank.png"));
            }
            return buff;
        }

        public ActionResult Preview()
        {
            var bin = GetPhotoPreviewBuff(this.Server);
            return File(bin, "image/jpeg");
        }

        private static object _syncTakePhoto = new object();

        [HttpPost]
        public ActionResult Take()
        {
            var imagePath = Server.MapPath("~/App_Data/" + Guid.NewGuid().ToString("N") + ".jpg");
#if DEBUG
            System.IO.File.Copy(Server.MapPath("~/App_Data/penguin.jpg"), imagePath);
#else
            Process.Start("fswebcam", "-q -r 1024 --jpeg 95 --no-banner " + imagePath)
               .WaitForExit();
#endif
            lock (_syncTakePhoto)
            {
                var saveDir = Server.MapPath("~/App_Data");
                var files = System.IO.Directory.GetFiles(saveDir, "*.jpg");
                var savePath = Enumerable.Range(0, int.MaxValue)
                    .Select(n => Path.Combine(saveDir, string.Format("photo{0:D3}.jpg", n)))
                    .First(path => files.Contains(path) == false);
                System.IO.File.Move(imagePath, savePath);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}