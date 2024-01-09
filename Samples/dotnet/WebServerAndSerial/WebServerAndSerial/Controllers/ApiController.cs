// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using Lego.Infrared;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebServerAndSerial.Models;

namespace WebServerAndSerial.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        public const string PageCombo = "combo";
        public const string PageSinglePwm = "singlepwm";
        public const string PageContinuous = "continuous";
        public const string PageSingleCst = "singlecst";
        public const string PageTimeout = "timeout";
        public const string PageComboAll = "comboall";
        public const string PageContinuousAll = "continuousall";
        public const string PageSinglePwmAll = "singlepwmall";
        public const string PageComboPwm = "combopwm";
        public const string PageComboPwmAll = "combopwmall";

        private readonly ILogger<ApiController> _logger;
        private readonly AppConfiguration _config;

        public ApiController(ILogger<ApiController> logger, AppConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }

        [HttpGet("Test")]
        public IActionResult Get()
        {
            string strResp = string.Empty;
            // Start HTML document
            strResp = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
            strResp += "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Test Lego Infrared modes</title><link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\"></head><body>";
            // first form is for Combo mode
            strResp += "<fieldset><form method=\"get\" action=\"" + PageCombo + "\" target=\"_blank\"><legend>Combo Mode</legend><br />Speed Red<select id=\"RedSpeed\" name=\"rd\">";
            strResp += "<option label='RED_FLT'>0</option>" +
                "<option label='RED_FWD'>1</option>" +
                "<option label='RED_REV'>2</option>" +
                "<option label='RED_BRK'>3</option>";
            strResp += "</select> Speed Blue<select id=\"BlueSpeed\" name=\"bl\">" +
                "<option label='BLUE_FLT'>0</option>" +
                "<option label='BLUE_FWD'>4</option>" +
                "<option label='BLUE_REV'>8</option>" +
                "<option label='BLUE_BRK'>12</option>";
            strResp += "</select> Channel<select id=\"Channel\" name=\"ch\">" +
                "<option label='CH1'>0</option>" +
                "<option label='CH2'>1</option>" +
                "<option label='CH3'>2</option>" +
                "<option label='CH4'>3</option>";
            strResp += "</select>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id=\"Submit1\" type=\"submit\" value=\"Send\" />";
            strResp += "</form></fieldset>";
            //second form is singlepwm
            strResp += "<fieldset><form method=\"get\" action=\"" + PageSinglePwm + "\" target=\"_blank\"><legend>SinglePWM Mode</legend><br />PWM<select id=\"PWM\" name=\"pw\">";
            strResp += "<option label='FLT'>0</option>";
            strResp += "<option label='FWD1'>1</option>";
            strResp += "<option label='FWD2'>2</option>";
            strResp += "<option label='FWD3'>3</option>";
            strResp += "<option label='FWD4'>4</option>";
            strResp += "<option label='FWD5'>5</option>";
            strResp += "<option label='FWD6'>6</option>";
            strResp += "<option label='FWD7'>7</option>";
            strResp += "<option label='BRK'>8</option>";
            strResp += "<option label='REV7'>9</option>";
            strResp += "<option label='REV6'>10</option>";
            strResp += "<option label='REV5'>11</option>";
            strResp += "<option label='REV4'>12</option>";
            strResp += "<option label='REV3'>13</option>";
            strResp += "<option label='REV2'>14</option>";
            strResp += "<option label='REV1'>15</option>";
            strResp += "</select> Output<select id=\"Output\" name=\"op\">" +
                "<option label='RED'>0</option>" +
                "<option label='BLUE'>1</option>";
            strResp += "</select> Channel<select id=\"Channel\" name=\"ch\">" +
                "<option label='CH1'>0</option>" +
                "<option label='CH2'>1</option>" +
                "<option label='CH3'>2</option>" +
                "<option label='CH4'>3</option>";
            strResp += "</select>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id=\"Submit2\" type=\"submit\" value=\"Send\" />";
            strResp += "</form></fieldset>";
            //3rd is continuous
            strResp += "<fieldset><form method=\"get\" action=\"" + PageContinuous + "\" target=\"_blank\"><legend>SingleCountinuous Mode</legend>";
            strResp += "<br />Function<select id=\"Function\" name=\"fc\">";
            strResp += "<option label='NO_CHANGE'>0</option>" +
                "<option label='CLEAR'>1</option>" +
                "<option label='SET'>2</option>" +
                "<option label='TOGGLE'>3</option>";
            strResp += "</select> Output<select id=\"Output\" name=\"op\">" +
                "<option label='RED_PINC1'>0</option>" +
                "<option label='RED_PINC2'>1</option>";
            strResp += "<option label='BLUE_PINC1'>2</option>" +
                "<option label='BLUE_PINC2'>3</option>";
            strResp += "</select> Channel<select id=\"Channel\" name=\"ch\">" +
                "<option label='CH1'>0</option>" +
                "<option label='CH2'>1</option>" +
                "<option label='CH3'>2</option>" +
                "<option label='CH4'>3</option>";
            strResp += "</select>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id=\"Submit3\" type=\"submit\" value=\"Send\" />";
            strResp += "</form></fieldset>";
            //4 ComboPWM
            strResp += "<form method=\"get\" action=\"combopwm\" target=\"_blank\"><legend>ComboPWM Mode</legend><br />PWM Red<select id=\"PWM1\" name=\"p1\">";
            strResp += "<option label='FLT'>0</option>";
            strResp += "<option label='FWD1'>1</option>";
            strResp += "<option label='FWD2'>2</option>";
            strResp += "<option label='FWD3'>3</option>";
            strResp += "<option label='FWD4'>4</option>";
            strResp += "<option label='FWD5'>5</option>";
            strResp += "<option label='FWD6'>6</option>";
            strResp += "<option label='FWD7'>7</option>";
            strResp += "<option label='BRK'>8</option>";
            strResp += "<option label='REV7'>9</option>";
            strResp += "<option label='REV6'>10</option>";
            strResp += "<option label='REV5'>11</option>";
            strResp += "<option label='REV4'>12</option>";
            strResp += "<option label='REV3'>13</option>";
            strResp += "<option label='REV2'>14</option>";
            strResp += "<option label='REV1'>15</option></select>";
            strResp += "PWM Blue<select id=\"PWM2\" name=\"p2\">";
            strResp += "<option label='FLT'>0</option>";
            strResp += "<option label='FWD1'>1</option>";
            strResp += "<option label='FWD2'>2</option>";
            strResp += "<option label='FWD3'>3</option>";
            strResp += "<option label='FWD4'>4</option>";
            strResp += "<option label='FWD5'>5</option>";
            strResp += "<option label='FWD6'>6</option>";
            strResp += "<option label='FWD7'>7</option>";
            strResp += "<option label='BRK'>8</option>";
            strResp += "<option label='REV7'>9</option>";
            strResp += "<option label='REV6'>10</option>";
            strResp += "<option label='REV5'>11</option>";
            strResp += "<option label='REV4'>12</option>";
            strResp += "<option label='REV3'>13</option>";
            strResp += "<option label='REV2'>14</option>";
            strResp += "<option label='REV1'>15</option>";
            strResp += "</select> Channel<select id=\"Channel\" name=\"ch\"><option label='CH1'>0</option><option label='CH2'>1</option><option label='CH3'>2</option><option label='CH4'>3</option>";
            strResp += "</select>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id=\"Submit4\" type=\"submit\" value=\"Send\" />";
            strResp += "</form></fieldset>";
            //4 ComboPWMAll
            strResp += "<form method=\"get\" action=\"combopwmall\" target=\"_blank\"><legend>ComboPWMAll Mode</legend><br />";
            for (int i = 0; i < 4; i++)
            {
                strResp += "PWM Red<select id=\"PWM1\" name=\"pwr" + i + "\">";
                strResp += "<option label='FLT'>0</option>";
                strResp += "<option label='FWD1'>1</option>";
                strResp += "<option label='FWD2'>2</option>";
                strResp += "<option label='FWD3'>3</option>";
                strResp += "<option label='FWD4'>4</option>";
                strResp += "<option label='FWD5'>5</option>";
                strResp += "<option label='FWD6'>6</option>";
                strResp += "<option label='FWD7'>7</option>";
                strResp += "<option label='BRK'>8</option>";
                strResp += "<option label='REV7'>9</option>";
                strResp += "<option label='REV6'>10</option>";
                strResp += "<option label='REV5'>11</option>";
                strResp += "<option label='REV4'>12</option>";
                strResp += "<option label='REV3'>13</option>";
                strResp += "<option label='REV2'>14</option>";
                strResp += "<option label='REV1'>15</option></select>";
                strResp += "PWM Blue<select id=\"PWM2\" name=\"pwb" + i + "\">";
                strResp += "<option label='FLT'>0</option>";
                strResp += "<option label='FWD1'>1</option>";
                strResp += "<option label='FWD2'>2</option>";
                strResp += "<option label='FWD3'>3</option>";
                strResp += "<option label='FWD4'>4</option>";
                strResp += "<option label='FWD5'>5</option>";
                strResp += "<option label='FWD6'>6</option>";
                strResp += "<option label='FWD7'>7</option>";
                strResp += "<option label='BRK'>8</option>";
                strResp += "<option label='REV7'>9</option>";
                strResp += "<option label='REV6'>10</option>";
                strResp += "<option label='REV5'>11</option>";
                strResp += "<option label='REV4'>12</option>";
                strResp += "<option label='REV3'>13</option>";
                strResp += "<option label='REV2'>14</option>";
                strResp += "<option label='REV1'>15</option>";
                //strResp += "</select> Channel<select id=\"Channel\" name=\"ch\"><option label='CH1'>0</option><option label='CH2'>1</option><option label='CH3'>2</option><option label='CH4'>3</option>";
                strResp += "</select>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            }
            strResp += "<input id=\"Submit4\" type=\"submit\" value=\"Send\" />";
            strResp += "</form></fieldset>";
            //5 Single CST
            strResp += "<fieldset><form method=\"get\" action=\"" + PageSingleCst + "\" target=\"_blank\"><legend>SingleCST Mode</legend><br />CST<select id=\"CST\" name=\"pw\">";
            strResp += "<option label='CLEARC1_CLEARC2'>0</option>";
            strResp += "<option label='SETC1_SETC2'>1</option>";
            strResp += "<option label='CLEARC1_SETC2'>2</option>";
            strResp += "<option label='SETC1_CLEARC2'>3</option>";
            strResp += "<option label='INC_PWM'>4</option>";
            strResp += "<option label='DEC_PWM'>5</option>";
            strResp += "<option label='FULLFWD'>6</option>";
            strResp += "<option label='FULLBKD'>7</option>";
            strResp += "<option label='TOGGLE_FWD_BKD'>8</option>";
            strResp += "</select> Output<select id=\"Output\" name=\"op\"><option label='RED'>0</option><option label='BLUE'>1</option>";
            strResp += "</select> Channel<select id=\"Channel\" name=\"ch\">" +
                "<option label='CH1'>0</option>" +
                "<option label='CH2'>1</option>" +
                "<option label='CH3'>2</option>" +
                "<option label='CH4'>3</option>";
            strResp += "</select>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id=\"Submit5\" type=\"submit\" value=\"Send\" />";
            strResp += "</form></fieldset>";
            //6 Single Timeout
            strResp += "<fieldset><form method=\"get\" action=\"" + PageTimeout + "\" target=\"_blank\"><legend>SingleTimeout Mode</legend>";
            strResp += "<br />Function<select id=\"Function\" name=\"fc\">";
            strResp += "<option label='NO_CHANGE'>0</option>" +
                "<option label='CLEAR'>1</option>" +
                "<option label='SET'>2</option>" +
                "<option label='TOGGLE'>3</option>";
            strResp += "</select> Output<select id=\"Output\" name=\"op\">" +
                "<option label='RED_PINC1'>0</option>" +
                "<option label='RED_PINC2'>1</option>";
            strResp += "<option label='BLUE_PINC1'>2</option>" +
                "<option label='BLUE_PINC2'>3</option>";
            strResp += "</select> Channel<select id=\"Channel\" name=\"ch\">" +
                "<option label='CH1'>0</option>" +
                "<option label='CH2'>1</option>" +
                "<option label='CH3'>2</option>" +
                "<option label='CH4'>3</option>";
            strResp += "</select>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id=\"Submit6\" type=\"submit\" value=\"Send\" />";
            strResp += "</form></fieldset>";
            // 7 form is for Combo mode all
            strResp += "<fieldset><form method=\"get\" action=\"" + PageComboAll + "\" target=\"_blank\"><legend>Combo Mode All</legend>";
            for (int i = 0; i < 4; i++)
            {
                strResp += "<br />Speed Red channel " + (i + 1) + "<select id=\"RedSpeed\" name=\"rd" + i + "\">";
                strResp += "<option label='RED_FLT'>0</option>" +
                    "<option label='RED_FWD'>1</option>" +
                    "<option label='RED_REV'>2</option>" +
                    "<option label='RED_BRK'>3</option>";
                strResp += "</select> Speed Blue channel " + (i + 1) + "<select id=\"BlueSpeed\" name=\"bl" + i + "\">" +
                    "<option label='BLUE_FLT'>0</option>" +
                    "<option label='BLUE_FWD'>4</option>" +
                    "<option label='BLUE_REV'>8</option>" +
                    "<option label='BLUE_BRK'>12</option>";
                strResp += "</select>";
            }
            strResp += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id=\"Submit7\" type=\"submit\" value=\"Send\" /></p>";
            strResp += "</form></fieldset>";
            //8 is continuous
            strResp += "<fieldset><form method=\"get\" action=\"" + PageContinuousAll + "\" target=\"_blank\"><legend>SingleCountinuous Mode All</legend>";
            for (int i = 0; i < 4; i++)
            {
                strResp += "<br />Function channel " + (i + 1) + "<select id=\"Function\" name=\"fc" + i + "\">";
                strResp += "<option label='NO_CHANGE'>0</option>" +
                    "<option label='CLEAR'>1</option>" +
                    "<option label='SET'>2</option>" +
                    "<option label='TOGGLE'>3</option>";
                strResp += "</select> Output channel " + (i + 1) + "<select id=\"Output\" name=\"op" + i + "\">" +
                    "<option label='RED_PINC1'>0</option>" +
                    "<option label='RED_PINC2'>1</option>";
                strResp += "<option label='BLUE_PINC1'>2</option>" +
                    "<option label='BLUE_PINC2'>3</option></select>";
            }
            strResp += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id=\"Submit8\" type=\"submit\" value=\"Send\" />";
            strResp += "</form></fieldset>";
            // 9 form is singlepwm
            strResp += "<fieldset><form method=\"get\" action=\"" + PageSinglePwmAll + "\" target=\"_blank\"><legend>SinglePWM Mode All<legend>";
            for (int a = 0; a < 4; a++)
            {
                strResp += "<br />PWM channel " + (a + 1) + "<select id=\"PWM\" name=\"pw" + a + "\">";
                strResp += "<option label='FLT'>0</option>";
                strResp += "<option label='FWD1'>1</option>";
                strResp += "<option label='FWD2'>2</option>";
                strResp += "<option label='FWD3'>3</option>";
                strResp += "<option label='FWD4'>4</option>";
                strResp += "<option label='FWD5'>5</option>";
                strResp += "<option label='FWD6'>6</option>";
                strResp += "<option label='FWD7'>7</option>";
                strResp += "<option label='BRK'>8</option>";
                strResp += "<option label='REV7'>9</option>";
                strResp += "<option label='REV6'>10</option>";
                strResp += "<option label='REV5'>11</option>";
                strResp += "<option label='REV4'>12</option>";
                strResp += "<option label='REV3'>13</option>";
                strResp += "<option label='REV2'>14</option>";
                strResp += "<option label='REV1'>15</option>";
                strResp += "</select> Output channel " + (a + 1) + "<select id=\"Output\" name=\"op" + a + "\">" +
                    "<option label='RED'>0</option>" +
                    "<option label='BLUE'>1</option></select>";
            }
            strResp += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id=\"Submit9\" type=\"submit\" value=\"Send\" />";
            strResp += "</form></fieldset>";

            strResp += "</body></html>";


            return Content(strResp, "text/html", Encoding.UTF8);
        }

        [HttpGet(PageCombo)]
        public IActionResult Combo(Lego.Infrared.Channel ch, Speed bl, Speed rd)
        {
            if (_config.LegoInfrared == null)
            {
                return BadRequest();
            }

            var res = _config.LegoInfrared.ComboMode(bl, rd, ch);
            return res ? Ok() : BadRequest();
        }

        [HttpGet(PageComboAll)]
        public IActionResult ComboAll(Speed rd0, Speed bl0, Speed rd1, Speed bl1, Speed rd2, Speed bl2, Speed rd3, Speed bl3)
        {
            if (_config.LegoInfrared == null)
            {
                return BadRequest();
            }

            Speed[] mComboBlue = new Speed[4] { bl0, bl1, bl2, bl3 };
            Speed[] mComboRed = new Speed[4] { rd0, rd1, rd2, rd3 };
            var res = _config.LegoInfrared.ComboModeAll(mComboBlue, mComboRed);
            return res ? Ok() : BadRequest();
        }

        [HttpGet(PageTimeout)]
        public IActionResult SingleTimeout(Lego.Infrared.Channel ch, Function fc, Output op)
        {
            if (_config.LegoInfrared == null)
            {
                return BadRequest();
            }

            var res = _config.LegoInfrared.SingleOutputTimeout(fc, op, ch);
            return res ? Ok() : BadRequest();
        }

        [HttpGet(PageContinuousAll)]
        public IActionResult ContinuousAll(Function fc0, Output op0, Function fc1, Output op1, Function fc2, Output op2, Function fc3, Output op3)
        {
            if (_config.LegoInfrared == null)
            {
                return BadRequest();
            }

            Function[] mFunction = new Function[4] { fc0, fc1, fc2, fc3 };
            Output[] mOutPut = new Output[4] { op0, op1, op2, op3 };
            var res = _config.LegoInfrared.SingleOutputContinuousAll(mFunction, mOutPut);
            return res ? Ok() : BadRequest();
        }

        [HttpGet(PageContinuous)]
        public IActionResult Continuous(Lego.Infrared.Channel ch, Function fc, Output op)
        {
            if (_config.LegoInfrared == null)
            {
                return BadRequest();
            }

            var res = _config.LegoInfrared.SingleOutputContinuous(fc, op, ch);
            return res ? Ok() : BadRequest();
        }

        [HttpGet(PageComboPwmAll)]
        public IActionResult ComboPwmAll(PwmSpeed pwr0, PwmSpeed pwb0, PwmSpeed pwr1, PwmSpeed pwb1, PwmSpeed pwr2, PwmSpeed pwb2, PwmSpeed pwr3, PwmSpeed pwb3)
        {
            if (_config.LegoInfrared == null)
            {
                return BadRequest();
            }

            PwmSpeed[] mPWMR = new PwmSpeed[4] { pwr0, pwr1, pwr2, pwr3 };
            PwmSpeed[] mPWMB = new PwmSpeed[4] { pwb0, pwb1, pwb2, pwb3 };
            var res = _config.LegoInfrared.ComboPwmAll(mPWMR, mPWMB);
            return res ? Ok() : BadRequest();
        }

        [HttpGet(PageComboPwm)]
        public IActionResult ComboPwm(Lego.Infrared.Channel ch, PwmSpeed p1, PwmSpeed p2)
        {
            if (_config.LegoInfrared == null)
            {
                return BadRequest();
            }

            var res = _config.LegoInfrared.ComboPwm(p1, p2, ch);
            return res ? Ok() : BadRequest();
        }

        [HttpGet(PageSingleCst)]
        public IActionResult SingleCst(Lego.Infrared.Channel ch, ClearSetToggle pw, PwmOutput op)
        {
            if (_config.LegoInfrared == null)
            {
                return BadRequest();
            }

            var res = _config.LegoInfrared.SingleOutputClearSetToggle(pw, op, ch);
            return res ? Ok() : BadRequest();
        }


        [HttpGet(PageSinglePwmAll)]
        public IActionResult SinglePwmAll(PwmSpeed pw0, PwmOutput op0, PwmSpeed pw1, PwmOutput op1, PwmSpeed pw2, PwmOutput op2, PwmSpeed pw3, PwmOutput op3)
        {
            if (_config.LegoInfrared == null)
            {
                return BadRequest();
            }

            PwmSpeed[] mPWM = new PwmSpeed[4] { pw0, pw1, pw2, pw3 };
            PwmOutput[] mOutPut = new PwmOutput[4] { op0, op1, op2, op3 };
            var res = _config.LegoInfrared.SingleOutputPwmAll(mPWM, mOutPut);
            return res ? Ok() : BadRequest();
        }

        [HttpGet(PageSinglePwm)]
        public IActionResult SinglePwm(Lego.Infrared.Channel ch, PwmSpeed pw, PwmOutput op)
        {
            if (_config.LegoInfrared == null)
            {
                return BadRequest();
            }

            var res = _config.LegoInfrared.SingleOutputPwm(pw, op, ch);
            return res ? Ok() : BadRequest();
        }

        [HttpGet(nameof(Signal))]
        public IActionResult Signal(byte si, bool md)
        {
            if (_config.SignalManagement == null)
            {
                return BadRequest();
            }

            try
            {
                _config.SignalManagement.ChangeSignal(si, md);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet(nameof(Switch))]
        public IActionResult Switch(byte si, bool md)
        {
            if (_config.SwitchManagement == null)
            {
                return BadRequest();
            }

            try
            {
                _config.SwitchManagement.ChangeSwitch(si, md);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}