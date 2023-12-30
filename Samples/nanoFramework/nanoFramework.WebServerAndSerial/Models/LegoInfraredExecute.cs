using Lego.Infrared;
using nanoFramework.WebServer;
using System;

namespace nanoFramework.WebServerAndSerial.Models
{
    internal class LegoInfraredExecute
    {
        private const string ParamComboBlue = "bl";
        private const string ParamComboRed = "rd";
        private const string ParamChannel = "ch";
        private const string ParamSinglePWM = "pw";
        private const string ParamSingleOutput = "op";
        private const string ParamComboPWM1 = "p1";
        private const string ParamComboPWM2 = "p2";
        private const string ParamComboPWMR = "pwr";
        private const string ParamComboPWMB = "pwb";
        private const string ParamContinuousFct = "fc";

        static public bool SingleTimeout(string strDecrypt)
        {
            // decode params
            var Params = WebServer.WebServer.DecodeParam(strDecrypt);
            //check if Params contains anything and is valid
            if ((Params == null) || (Params.Length == 0))
            {
                return false;
            }

            int mChannel = 0;
            int mFunction = 0;
            int mOutPut = 0;
            foreach (UrlParameter Param in Params)
            {
                if (ParamChannel == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mChannel, (int)Channel.Channel1, (int)Channel.Channel4))
                    {
                        return false;
                    }

                    continue;
                }

                if (ParamContinuousFct == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mFunction, (int)Function.NoChange, (int)Function.Toggle))
                    {
                        return false;
                    }

                    continue;
                }

                if (ParamSingleOutput == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mOutPut, (int)Output.RedPinC1, (int)Output.BluePinC2))
                    {
                        return false;
                    }

                    continue;
                }
            }

            return Application.LegoInfrared.SingleOutputTimeout((Function)mFunction, (Output)mOutPut, (Channel)mChannel);
        }

        static public bool ContinuousAll(string strDecrypt)
        {
            // decode params
            var Params = WebServer.WebServer.DecodeParam(strDecrypt);
            //check if Params contains anything and is valid
            if ((Params == null) || (Params.Length == 0))
            {
                return false;
            }

            Function[] mFunction = new Function[4];
            Output[] mOutPut = new Output[4];
            int temp = 0;
            for (int a = 0; a < 4; a++)
            {
                foreach (UrlParameter Param in Params)
                {
                    if (ParamContinuousFct + a.ToString() == Param.Name)
                    {
                        if (!TryConvertInt32(Param.Value, out temp, (int)Function.NoChange, (int)Function.Toggle))
                        {
                            return false;
                        }

                        mFunction[a] = (Function)temp;
                        continue;
                    }

                    if (ParamSingleOutput + a.ToString() == Param.Name)
                    {
                        if (!TryConvertInt32(Param.Value, out temp, (int)Output.RedPinC1, (int)Output.BluePinC2))
                        {
                            return false;
                        }

                        mOutPut[a] = (Output)temp;
                        continue;
                    }
                }
            }

            return Application.LegoInfrared.SingleOutputContinuousAll(mFunction, mOutPut);
        }

        static public bool Continuous(string strDecrypt)
        {
            // decode params
            var Params = WebServer.WebServer.DecodeParam(strDecrypt);
            //check if Params contains anything and is valid
            if ((Params == null) || (Params.Length == 0))
            {
                return false;
            }

            int mChannel = 0;
            int mFunction = 0;
            int mOutPut = 0;
            foreach (UrlParameter Param in Params)
            {
                if (ParamChannel == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mChannel, (int)Channel.Channel1, (int)Channel.Channel4))
                    {
                        return false;
                    }

                    continue;
                }

                if (ParamContinuousFct == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mFunction, (int)Function.NoChange, (int)Function.Toggle))
                    {
                        return false;
                    }

                    continue;
                }

                if (ParamSingleOutput == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mOutPut, (int)Output.RedPinC1, (int)Output.BluePinC2))
                    {
                        return false;
                    }

                    continue;
                }
            }

            return Application.LegoInfrared.SingleOutputContinuous((Function)mFunction, (Output)mOutPut, (Channel)mChannel);
        }

        static public bool ComboPwmAll(string strDecrypt)
        {
            // decode params
            var Params = WebServer.WebServer.DecodeParam(strDecrypt);
            //check if Params contains anything and is valid
            if ((Params == null) || (Params.Length == 0))
            {
                return false;
            }

            PwmSpeed[] mPWMR = new PwmSpeed[4];
            PwmSpeed[] mPWMB = new PwmSpeed[4];
            int temp = 0;
            for (int i = 0; i < 4; i++)
            {
                foreach (UrlParameter Param in Params)
                {
                    if (ParamComboPWMR + i.ToString() == Param.Name)
                    {
                        if (!TryConvertInt32(Param.Value, out temp, (int)PwmSpeed.Float, (int)PwmSpeed.Reverse1))
                        {
                            return false;
                        }

                        mPWMR[i] = (PwmSpeed)temp;
                        continue;
                    }

                    if (ParamComboPWMB + i.ToString() == Param.Name)
                    {
                        if (!TryConvertInt32(Param.Value, out temp, (int)PwmSpeed.Float, (int)PwmSpeed.Reverse1))
                        {
                            return false;
                        }

                        mPWMB[i] = (PwmSpeed)temp;
                        continue;
                    }
                }
            }

            return Application.LegoInfrared.ComboPwmAll(mPWMR, mPWMB);
        }

        static public bool ComboPwm(string strDecrypt)
        {
            // decode params
            var Params = WebServer.WebServer.DecodeParam(strDecrypt);
            //check if Params contains anything and is valid
            if ((Params == null) || (Params.Length == 0))
            {
                return false;
            }

            int mChannel = 0;
            int mPWM1 = 0;
            int mPWM2 = 0;
            foreach (UrlParameter Param in Params)
            {
                if (ParamChannel == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mChannel, (int)Channel.Channel1, (int)Channel.Channel4))
                    {
                        return false;
                    }

                    continue;
                }

                if (ParamComboPWM1 == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mPWM1, (int)PwmSpeed.Float, (int)PwmSpeed.Reverse1))
                    {
                        return false;
                    }

                    continue;
                }

                if (ParamComboPWM2 == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mPWM2, (int)PwmSpeed.Float, (int)PwmSpeed.Reverse1))
                    {
                        return false;
                    }

                    continue;
                }
            }

            return Application.LegoInfrared.ComboPwm((PwmSpeed)mPWM1, (PwmSpeed)mPWM2, (Channel)mChannel);
        }


        static public bool SingleCst(string strDecrypt)
        {
            // decode params
            var Params = WebServer.WebServer.DecodeParam(strDecrypt);
            //check if Params contains anything and is valid
            if ((Params == null) || (Params.Length == 0))
            {
                return false;
            }

            int mChannel = 0;
            int mPWM = 0;
            int mOutPut = 0;
            foreach (UrlParameter Param in Params)
            {
                if (ParamChannel == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mChannel, (int)Channel.Channel1, (int)Channel.Channel4))
                    {
                        return false;
                    }

                    continue;
                }

                if (ParamSinglePWM == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mPWM, (int)ClearSetToggle.ClearC1ClearC2, (int)ClearSetToggle.ToggleFullForwardBackward))
                    {
                        return false;
                    }

                    continue;
                }

                if (ParamSingleOutput == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mOutPut, (int)PwmOutput.Red, (int)PwmOutput.Blue))
                    {
                        return false;
                    }

                    continue;
                }
            }

            return Application.LegoInfrared.SingleOutputClearSetToggle((ClearSetToggle)mPWM, (PwmOutput)mOutPut, (Channel)mChannel);
        }

        static public bool SinglePwmAll(string strDecrypt)
        {
            // decode params
            var Params = WebServer.WebServer.DecodeParam(strDecrypt);
            //check if Params contains anything and is valid
            if ((Params == null) || (Params.Length == 0))
            {
                return false;
            }

            PwmSpeed[] mPWM = new PwmSpeed[4];
            PwmOutput[] mOutPut = new PwmOutput[4];
            int temp = 0;
            for (int a = 0; a < 4; a++)
            {
                foreach (UrlParameter Param in Params)
                {
                    if (ParamSinglePWM + a.ToString() == Param.Name)
                    {
                        if (!TryConvertInt32(Param.Value, out temp, (int)PwmSpeed.Float, (int)PwmSpeed.Reverse1))
                        {
                            return false;
                        }

                        mPWM[a] = (PwmSpeed)temp;
                        continue;
                    }

                    if (ParamSingleOutput + a.ToString() == Param.Name)
                    {
                        if (!TryConvertInt32(Param.Value, out temp, (int)PwmOutput.Red, (int)PwmOutput.Blue))
                        {
                            return false;
                        }

                        mOutPut[a] = (PwmOutput)temp;
                        continue;
                    }
                }
            }

            return Application.LegoInfrared.SingleOutputPwmAll(mPWM, mOutPut);
        }

        static public bool SinglePwm(string strDecrypt)
        {
            // decode params
            var Params = WebServer.WebServer.DecodeParam(strDecrypt);
            //check if Params contains anything and is valid
            if ((Params == null) || (Params.Length == 0))
            {
                return false;
            }

            int mChannel = 0;
            int mPWM = 0;
            int mOutPut = 0;
            foreach (UrlParameter Param in Params)
            {
                if (ParamChannel == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mChannel, (int)Channel.Channel1, (int)Channel.Channel4))
                    {
                        return false;
                    }

                    continue;
                }

                if (ParamSinglePWM == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mPWM, (int)PwmSpeed.Float, (int)PwmSpeed.Reverse1))
                    {
                        return false;
                    }

                    continue;
                }

                if (ParamSingleOutput == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mOutPut, (int)PwmOutput.Red, (int)PwmOutput.Blue))
                    {
                        return false;
                    }

                    continue;
                }

            }

            return Application.LegoInfrared.SingleOutputPwm((PwmSpeed)mPWM, (PwmOutput)mOutPut, (Channel)mChannel);
        }

        static public bool ComboAll(string strDecrypt)
        {
            // decode params
            var Params = WebServer.WebServer.DecodeParam(strDecrypt);
            //check if Params contains anything and is valid
            if ((Params == null) || (Params.Length == 0))
            {
                return false;
            }

            Speed[] mComboBlue = new Speed[4];
            Speed[] mComboRed = new Speed[4];
            int temp = 0;
            for (int a = 0; a < 4; a++)
            {
                foreach (UrlParameter Param in Params)
                {
                    if (ParamComboRed + a.ToString() == Param.Name)
                    {
                        if (!TryConvertInt32(Param.Value, out temp, (int)Speed.RedFloat, (int)Speed.RedBreak))
                        {
                            return false;
                        }

                        mComboRed[a] = (Speed)temp;
                        continue;
                    }

                    if (ParamComboBlue + a.ToString() == Param.Name)
                    {
                        if (!TryConvertInt32(Param.Value, out temp, (int)Speed.RedFloat, (int)Speed.BlueBreak))
                        {
                            return false;
                        }

                        if ((temp != (int)Speed.BlueForward) &&
                            (temp != (int)Speed.BlueBreak) &&
                            (temp != (int)Speed.BlueFloat) &&
                            (temp != (int)Speed.BlueReverse))
                        {
                            return false;
                        }

                        mComboBlue[a] = (Speed)temp;
                        continue;
                    }
                }
            }

            return Application.LegoInfrared.ComboModeAll(mComboBlue, mComboRed);
        }

        static public bool Combo(string strDecrypt)
        {
            // decode params
            var Params = WebServer.WebServer.DecodeParam(strDecrypt);
            //check if Params contains anything and is valid
            if ((Params == null) || (Params.Length == 0))
            {
                return false;
            }

            int mChannel = 0;
            int mComboBlue = 0;
            int mComboRed = 0;
            foreach (UrlParameter Param in Params)
            {
                if (ParamChannel == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mChannel, (int)Channel.Channel1, (int)Channel.Channel4))
                    {
                        return false;
                    }

                    continue;
                }

                if (ParamComboBlue == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mComboBlue, (int)Speed.RedFloat, (int)Speed.BlueBreak))
                    {
                        return false;
                    }

                    if ((mComboBlue != (int)Speed.BlueForward) &&
                        (mComboBlue != (int)Speed.BlueBreak) &&
                        (mComboBlue != (int)Speed.BlueFloat) &&
                        (mComboBlue != (int)Speed.BlueReverse))
                    {
                        return false;
                    }

                    continue;
                }

                if (ParamComboRed == Param.Name)
                {
                    if (!TryConvertInt32(Param.Value, out mComboRed, (int)Speed.RedFloat, (int)Speed.RedBreak))
                    {
                        return false;
                    }

                    continue;
                }
            }

            return Application.LegoInfrared.ComboMode((Speed)mComboBlue, (Speed)mComboRed, (Channel)mChannel);
        }

        private static bool TryConvertInt32(string val, out int result, int min, int max)
        {
            if (!int.TryParse(val, out result))
            {
                return false;
            }

            if (!((result >= min) && (result <= max)))
            {
                return false;
            }

            return true;
        }
    }
}
