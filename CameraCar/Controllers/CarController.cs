using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Toolbelt.GPIO;

namespace CameraCar.Controllers
{
    public class CarController : Controller
    {
        private static object _sync = new object();

        private static TinyGPIO[] _GPIO;

        private static class MotorID
        {
            public const int LeftF = 0;
            public const int LeftB = 1;
            public const int RightF = 2;
            public const int RightB = 3;
            public const int LiftU = 4;
            public const int LiftD = 5;
        }
        public CarController()
        {
            lock (_sync)
            {
                if (_GPIO == null)
                {
                    _GPIO = new[] { 23, 24, 22, 27, 20, 21 }
                        .Select(TinyGPIO.Export)
                        .ToArray();
                    _GPIO.ToList()
                        .ForEach(gpio => gpio.Direction = GPIODirection.Out);
                    StopAll();
                }
            }
        }

        [HttpPost]
        public ActionResult StartMoveForward()
        {
            _GPIO[MotorID.LeftF].Value = 1;
            _GPIO[MotorID.LeftB].Value = 0;
            _GPIO[MotorID.RightF].Value = 1;
            _GPIO[MotorID.RightB].Value = 0;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult StartMoveBackward()
        {
            _GPIO[MotorID.LeftF].Value = 0;
            _GPIO[MotorID.LeftB].Value = 1;
            _GPIO[MotorID.RightF].Value = 0;
            _GPIO[MotorID.RightB].Value = 1;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult StartTurnLeft()
        {
            _GPIO[MotorID.LeftF].Value = 0;
            _GPIO[MotorID.LeftB].Value = 1;
            _GPIO[MotorID.RightF].Value = 1;
            _GPIO[MotorID.RightB].Value = 0;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult StartTurnRight()
        {
            _GPIO[MotorID.LeftF].Value = 1;
            _GPIO[MotorID.LeftB].Value = 0;
            _GPIO[MotorID.RightF].Value = 0;
            _GPIO[MotorID.RightB].Value = 1;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult StartLiftUp()
        {
            _GPIO[MotorID.LiftU].Value = 1;
            _GPIO[MotorID.LiftD].Value = 0;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult StartLiftDown()
        {
            _GPIO[MotorID.LiftU].Value = 0;
            _GPIO[MotorID.LiftD].Value = 1;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult StopAll()
        {
            _GPIO[MotorID.LeftF].Value = 1;
            _GPIO[MotorID.LeftB].Value = 1;
            _GPIO[MotorID.RightF].Value = 1;
            _GPIO[MotorID.RightB].Value = 1;
            _GPIO[MotorID.LiftU].Value = 1;
            _GPIO[MotorID.LiftD].Value = 1;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}