﻿// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using nanoFramework.WebServer;
using nanoFramework.WebServerAndSerial.Models;
using System;
using System.Net;
using System.Text;

namespace nanoFramework.WebServerAndSerial.Controllers
{
    [Authentication("Basic")]
    internal class ApiController
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

        [Route("api/test")]
        public void Test(WebServerEventArgs e)
        {
            e.Context.Response.ContentType = "text/html";
            // Note that we won't send the size, it's going to be guessed by the serveur when the connection will end
            e.Context.Response.KeepAlive = false;
            e.Context.Response.StatusCode = (int)HttpStatusCode.OK;

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
            WebServer.WebServer.OutPutStream(e.Context.Response, strResp);
        }

        [Route("api/" + PageCombo)]
        public void Combo(WebServerEventArgs e)
        {
            // http://192.168.1.85/combo?rd=0&bl=0&ch=0
            var ret = LegoInfraredExecute.Combo(e.Context.Request.RawUrl);

            WebServer.WebServer.OutputHttpCode(e.Context.Response, ret ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }

        [Route("api/" + PageSinglePwm)]
        public void SinglePwm(WebServerEventArgs e)
        {
            // http://192.168.1.85/singlepwm?pw=0&op=0&ch=0
            var ret = LegoInfraredExecute.SinglePwm(e.Context.Request.RawUrl);

            WebServer.WebServer.OutputHttpCode(e.Context.Response, ret ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }

        [Route("api/" + PageContinuous)]
        public void Continuous(WebServerEventArgs e)
        {
            // http://192.168.1.85/continuous?fc=0&op=0
            var ret = LegoInfraredExecute.Continuous(e.Context.Request.RawUrl);

            WebServer.WebServer.OutputHttpCode(e.Context.Response, ret ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }

        [Route("api/" + PageSingleCst)]
        public void SingleCst(WebServerEventArgs e)
        {
            // http://192.168.1.85/singlecst?pw=0&op=0&ch=0
            var ret = LegoInfraredExecute.SingleCst(e.Context.Request.RawUrl);

            WebServer.WebServer.OutputHttpCode(e.Context.Response, ret ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }

        [Route("api/" + PageTimeout)]
        public void Timeout(WebServerEventArgs e)
        {
            // http://192.168.1.85/timeout?fc=0&op=0&ch=0
            var ret = LegoInfraredExecute.SingleTimeout(e.Context.Request.RawUrl);

            WebServer.WebServer.OutputHttpCode(e.Context.Response, ret ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }

        [Route("api/" + PageComboAll)]
        public void ComboAll(WebServerEventArgs e)
        {
            // http://192.168.1.85/comboall?rd0=0&bl0=0&rd1=0&bl1=0&rd2=0&bl2=0&rd3=0&bl3=0
            var ret = LegoInfraredExecute.ComboAll(e.Context.Request.RawUrl);

            WebServer.WebServer.OutputHttpCode(e.Context.Response, ret ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }

        [Route("api/" + PageContinuousAll)]
        public void ContinuousAll(WebServerEventArgs e)
        {
            // http://192.168.1.85/continuousall?fc0=0&op0=0&fc1=0&op1=0&fc2=0&op2=0&fc3=0&op3=0
            var ret = LegoInfraredExecute.ContinuousAll(e.Context.Request.RawUrl);

            WebServer.WebServer.OutputHttpCode(e.Context.Response, ret ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }

        [Route("api/" + PageSinglePwmAll)]
        public void SinglePwmAll(WebServerEventArgs e)
        {
            // http://192.168.1.85/singlepwmall?pw0=0&op0=0&pw1=0&op1=0&pw2=0&op2=0&pw3=0&op3=0
            var ret = LegoInfraredExecute.SinglePwmAll(e.Context.Request.RawUrl);

            WebServer.WebServer.OutputHttpCode(e.Context.Response, ret ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }

        private static void OutPutStream(HttpListenerResponse response, string strResp)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(strResp);
            response.OutputStream.Write(bytes, 0, bytes.Length);

            // We need to clean things to get some memory
            Runtime.Native.GC.Run(true);
        }
    }
}
