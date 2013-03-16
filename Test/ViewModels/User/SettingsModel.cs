using System;
using InjectCC.Model;
using System.Collections.Generic;

namespace InjectCC.Web.ViewModels.User
{
    public class SettingsModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public byte[] Timestamp { get; set; }

        public SettingsModel(Model.User user)
        {
            // TODO: Automapper?
            UserId = user.UserId;
            Email = user.Email;
            Timestamp = user.Timestamp;
        }
    }
}