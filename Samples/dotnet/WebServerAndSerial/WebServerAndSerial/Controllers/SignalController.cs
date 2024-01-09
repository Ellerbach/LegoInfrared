// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using WebServerAndSerial.Models;

namespace WebServerAndSerial.Controllers
{
    public class SignalController : Controller
    {
        private readonly AppConfiguration _configuration;

        public SignalController(AppConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: SignalController
        public ActionResult Index()
        {
            return View(_configuration.Signals);
        }

        // GET: SignalController/Details/5
        public ActionResult Details(int id)
        {
            var sig = _configuration.Signals.Where(m => m.Id == id).FirstOrDefault();
            if (sig == null)
            {
                return NotFound();
            }

            return View(sig);
        }

        // GET: SignalController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SignalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Signal collection)
        {
            try
            {
                // Find an available index
                int id = -1;
                for (int i = 0; i < SwitchManagement.MaximumNumberSwotches; i++)
                {
                    var sig = _configuration.Signals.Find(m => m.Id == i);
                    if (sig == null)
                    {
                        id = i;
                        break;
                    }
                }

                if (id != -1)
                {
                    collection.Id = id;
                    _configuration.Signals.Add(collection);
                    _configuration.Save();
                    return RedirectToAction(nameof(Index));
                }

                return NotFound();
            }
            catch
            {
            }

            return View();
        }

        // GET: SignalController/Edit/5
        public ActionResult Edit(int id)
        {
            var sig = _configuration.Signals.Where(m => m.Id == id).FirstOrDefault();
            if (sig == null)
            {
                return NotFound();
            }

            return View(sig);
        }

        // POST: SignalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Signal collection)
        {
            try
            {
                var sig = _configuration.Signals.Find(m => m.Id == id);
                if (sig != null)
                {
                    _configuration.Signals.Remove(sig);
                    _configuration.Signals.Add(collection);
                    _configuration.Save();
                    return RedirectToAction(nameof(Index));
                }

                return NotFound();
            }
            catch
            {
            }

            return View();
        }

        // GET: SignalController/Delete/5
        public ActionResult Delete(int id)
        {
            var sig = _configuration.Signals.Where(m => m.Id == id).FirstOrDefault();
            if (sig == null)
            {
                return NotFound();
            }

            return View(sig);
        }

        // POST: SignalController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var sig = _configuration.Signals.Find(m => m.Id == id);
                if (sig != null)
                {
                    _configuration.Signals.Remove(sig);
                    _configuration.Save();
                    return RedirectToAction(nameof(Index));
                }

                return NotFound();
            }
            catch
            {
            }

            return View();
        }
    }
}
