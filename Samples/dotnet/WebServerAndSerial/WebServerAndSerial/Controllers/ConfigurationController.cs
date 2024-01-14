// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using WebServerAndSerial.Models;

namespace WebServerAndSerial.Controllers
{
    [Route("[controller]")]
    public class ConfigurationController : Controller
    {
        private readonly AppConfiguration _configuration;

        public ConfigurationController(AppConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: ConfigurationController
        [HttpGet()]
        public ActionResult Index()
        {
            return View(_configuration);
        }

        // GET: ConfigurationController/Edit/5
        [HttpGet(nameof(Edit))]
        public ActionResult Edit()
        {
            return View(_configuration);
        }

        // POST: ConfigurationController/Edit/5
        [HttpPost(nameof(Edit))]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AppConfiguration collection)
        {
            try
            {
                _configuration.InfraredSpiBusNumber = collection.InfraredSpiBusNumber;
                _configuration.InfraredSpiChipSelect = collection.InfraredSpiChipSelect;
                _configuration.SignalSpiBusNumber = collection.SignalSpiBusNumber;                                
                _configuration.SignalSpiChipSelect = collection.SignalSpiChipSelect;
                _configuration.SwitchMaximumDuration = collection.SwitchMaximumDuration;
                _configuration.SwitchMinimumDuration = collection.SwitchMinimumDuration;
                _configuration.SwitchMultiplexPins = collection.SwitchMultiplexPins;
                _configuration.SwitchPwmChannel = collection.SwitchPwmChannel;
                _configuration.SwitchPwmChip = collection.SwitchPwmChip;
                _configuration.Save();
                _configuration.UpdateConfiguration();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(_configuration);
            }
        }
    }
}
