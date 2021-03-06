﻿using System;
using InjectCC.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace InjectCC.Web.ViewModels.User
{
    public class SettingsModel : SettingsBaseModel
    {
        [Required]
        public int UserId { get; set; }

        [Display(Name = "Email")]
        [Required, RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage="A valid email address is required.")]
        public string Email { get; set; }

        [Required]
        public byte[] Timestamp { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}