﻿// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace WebServerAndSerial.Models
{
    public class Switch
    {
        [Display(Name = "Switch's ID")]
        public int Id { get; set; }

        [Display(Name = "Switch's name")]
        public string Name { get; set; } = string.Empty;


        [Display(Name = "X position on the circuit")]
        public int X { get; set; }

        [Display(Name = "Y position on the circuit")]
        public int Y { get; set; }

        [Display(Name = "Check for left switch")]
        public bool IsLeft { get; set; }
    }
}
