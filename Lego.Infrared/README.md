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

## WebServer API usage for .NET nanoFramework

You have 2 available applications, one which will require an ESP32 with PSRAM, the [nanoFramework.WebServerAndSerial](./Samples/nanoFramework/nanoFramework.WebServerAndSerial/) and one [nanoFramework.WebServerAndSerial.NoBluetooth](./Samples/nanoFramework/nanoFramework.WebServerAndSerial.NoBluetooth/) which can run without PSRAM but won't support Bluetooth wifi setup rather it will use Access Point wireless support.

### Bluetooth application setup

You have to use [Improv Wifi](https://www.improv-wifi.com) to setup the application. Just connect the application and enter your SSID and Password, the device will automatically connect. The new IP address is broadcasted in the Improv Wifi settings.

### Non Bluetooth, only Wifi setup

An SSID `LegoInfrared` will appear, connect to it and go to [<http://192.168.4.1>](http://192.168.4.1), enter your wifi credentials and save. The new IP address should be displayed. Connect back the device you've been using to setup the board to your network and you'll be good to go!

### Infrared configuration

Go to the configuration page (`/config`) and enter valid GPIO pin numbers. Note that for ChipSelect, this value can stay as -1 but all the others must be valid. Save the configuration and you'll be good to go!

### Security

By default basic authentication is used for security. Even if the default login and password are empty, you must specify basic authentication in all the calls you are doing. If you do not, you will get an unauthorized error.

## .NET IoT WebServer API usage

You have one full example implementing quite a lot of features on the [WebServerAndSerial](./Samples/dotnet/WebServerAndSerial/WebServerAndSerial/). You have the ability to also control signals (red/green) and switches through servo motors.

### Infrared configuration on Raspberry PI on System on Chip

Like for the .NET nanoFramework implementation, the Infrared uses SPI to output the commands. So you have to activate SPI and use one of the channel. Please see the [documentation here](https://github.com/dotnet/iot/blob/main/Documentation/raspi-spi.md) to see how to best do it on a Raspberry Pi. You can activate it differently depending on the platform you are using.

In all cases, you can go to the configuration page <http://ipaddress/Configuration> of the board to adjust the settings. You can select a specific SPI channel (default on a Raspberry is 0) and a chip select. The chip select is optional. If you don't need chip select, just set it to -1.

If your configuration, is valid, you will get a confirmation that the Infrared Lego class has been able to be created. This doesn't mean that all will perfectly work. Go to the test page and try one of the command. Replace the infrared led by a normal led, you'll see it blinking!

### Switches with servo motors

Piloting switches require as well specific electronic. In this case multiplexing as we have only 1 PWM output to pilot up to 16 switches. We are using a 74HC4515:

[schema of the 74HC4515](/http://www.nxp.com/documents/data_sheet/74HC_HCT4515_CNV.pdf)

Here is a proposed configuration on a Raspberry Pi:

* A0 -> GPIO16 (pin 36)
* A1 -> GPIO20 (pin 38)
* A2 -> GPIO21 (pin 40)
* A3 -> GPIO19 (pin 35)
* E -> Ground
* LE -> GPIO12 (pin 33), this is the default PWM0 pin

All output to be plugged to the driving pin of every servo motor. Servo motor to be at +5V voltage. The level of high for the 4515 is lower than the 3.3V delivered by the RPI, so no need of level converter to 5V.

For the hardware part, you'll need simple servo motor, here is a basic view on how to integrate them with the Lego switch:

![Servo motor integration](/Assets/switches.jpg)

You must have the same servo everywhere as the piloting is done with the same configuration. Go to the <http://ipaddress/Configuration> and adjust the Switch part.

In the configuration parameters, you'll need to select all the pins used for the multiplexing. If you only have 2 switches, you only need 1 GPIO setup, if you are using the 16 ones, you'll need the 4 of them!

You also need to give the characteristics of the servo motors you are using. IT does require the minimum pulse and the maximum one. By default, the command will close the switch at the minimum pulse and open it at the maximum one. It is recommended to test the setup to find the optimal values.

You also need to make sure PWM is properly activated on your device. For this, please follow the [documentation here](https://github.com/dotnet/iot/blob/main/Documentation/raspi-pwm.md).

You can only pilot 1 switch at a time, this is a limitation.

### Signals with led

Piloting signals is quite straight forward using the a 74HC595. You can virtually chain as many as you want to have as many signals.
Please note that so far, it is limited in the code to 16.

![Electronic for signals](/Assets/signal.jpg)

Every 74HC595 can handle 4 signals, chain them as for any 74HC595. Here is an example of configuration for a Raspberry Pi:

* Pin 8 is ground
* Pin 16 is VCC (+5V recommended)
* Pin 14 to MOSI GPIO10 (physical pin 19) for the first 74HC595 of the chain. For the next one it has to be linked to the Pin 9 of the previous 74HC595
* Pin 13 to ground (used to activate or not the 8 output)
* Pin 12 to GPIO7 (physical pin 26), used to select the SPI
* Pin 11 to SCLK GPIO11 (physical pin 23)
* Pin 10 to +VSS (this one is used to reset if needed)
* Pin 15, 1, 2, 3, 4, 5, 6, 7 are the output pin
* Pin 9 has to be linked to the next 74HC595 of the chain if there are more than 1.

The configuration can be adjusted in the <http://ipaddress/Configuration> page. You will have to set the SPI bus number and you need to use a chip select if you are sharing the SPI with the Infrared part. See how to activate SPI and chip select in the [documentation here](https://github.com/dotnet/iot/blob/main/Documentation/raspi-spi.md).

### Running in docker

You can run this solution on docker by building the images or pulling them.
TBD

* params for the ports `-p 80:80`
* volume mount for the config `-v ~/config:/app/config`
* privileged mode `--privileged`

## Using the API

The detailed API protocol can be [found here](./Lego_api_doc.md). It is strongly recommended to read the documentation to understand how to best use all this.
