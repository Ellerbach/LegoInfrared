# LegoInfrared

Implementation of the Lego Infrared protocol using SPI. Implementation for both .NET IoT and .NET nanoFramework.

This project allow to control any Lego Power Function and support all Lego modes including exclusive modes which can't be done thru any of the Lego remote control.

## Documentation

For reference only, all copyrights to [Lego(c)](https://www.lego.com/), the infrared protocol specifications are included in this repository here for the [initial v1.0 version](./Assets/LEGO_Power_Functions_RC.pdf) and a [further v1.10 version](./Assets/LEGO_Power_Functions_RC_v110.pdf).

So far, this implementation is compatible with the v1.0. It is intended to add v1.10 support in a short future.

## Required electronic

The infrared module require a bit of electronic. You'll need a to drive enough power in the infrared led to be able to control trains or other Lego Power Functions in large rooms or with a lot of lights.

For this, you'll need to build the equivalent electronic:

![Electronic for Infrared](./Assets/infrared.jpg)

Note that the chipselect with Enable is not mandatory. The schema can also work with 3.3V, the distance covered will be less than with 5V.

* The MOSI pin is 19 and Enabled is pin number 24 on a Raspberry PI. Those are the 2 pins you need for the infrared part.
* On an ESP32, this is configurable, install the nanoFramework.Hardware.Esp32 nuget and use the `Configuration.SetPinFunction` function.

## Usage

You need first to create an instance of the `LegoInfrared` class:

```csharp
LegoInfrared _infrared;
// On an ESP32, setup first the pins for the SPI
Configuration.SetPinFunction(14, DeviceFunction.SPI1_CLOCK);
Configuration.SetPinFunction(12, DeviceFunction.SPI1_MISO);
Configuration.SetPinFunction(13, DeviceFunction.SPI1_MOSI);

// We will use chip select 17
_infrared = new LegoInfrared(1, 17);

_infrared.ComboMode(Speed.BlueForward, Speed.RedForward, Channel.Channel1);

_infrared.SingleOutputContinuousAll(
    new Function[] { Function.Set, Function.Clear, Function.Toggle, Function.NoChange },
    new Output[] { Output.BluePinC2, Output.RedPinC2, Output.RedPinC1, Output.BluePinC1 });
```

And as shown in the example, you can then call any of the public function. You still need to understand what each function is doing.

Note that if you don't want to use an enable pin, you can set the chip select pin to -1. This will not use any chip select.

## WebServer API usage

You have 2 available applications, one which will require an ESP32 with PSRAM, the [nanoFramework.WebServerAndSerial](./Samples/nanoFramework/nanoFramework.WebServerAndSerial/) and one [nanoFramework.WebServerAndSerial.NoBluetooth](./Samples/nanoFramework/nanoFramework.WebServerAndSerial.NoBluetooth/) which can run without PSRAM but won't support Bluetooth wifi setup rather it will use Access Point wireless support.

### Bluetooth application setup

You have to use [Improv Wifi](https://www.improv-wifi.com) to setup the application. Just connect the application and enter your SSID and Password, the device will automatically connect. The new IP address is broadcasted in the Improv Wifi settings.

### Non Bluetooth, only Wifi setup

An SSID `LegoInfrared` will appear, connect to it and go to [<http://192.168.4.1>](http://192.168.4.1), enter your wifi credentials and save. The new IP address should be displayed. Connect back the device you've been using to setup the board to your network and you'll be good to go!

## Infrared configuration

Go to the configuration page (`/config`) and enter valid GPIO pin numbers. Note that for ChipSelect, this value can stay as -1 but all the others must be valid. Save the configuration and you'll be good to go!

## Security

By default basic authentication is used for security. Even if the default login and password are empty, you must specify basic authentication in all the calls you are doing. If you do not, you will get an unauthorized error.

## Details API

The detailed API protocol can be [found here](./Lego_api_doc.md). It is strongly recommended to read the documentation to understand how to best use all this.
