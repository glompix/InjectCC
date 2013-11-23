using AutoMapper;
using InjectCC.Model.Domain;
using InjectCC.Web.ViewModels.Medication;
using InjectCC.Web.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace InjectCC.Web
{
    public static class AutomapperConfig
    {
        public static void Register()
        {
            Mapper.CreateMap<NewModel, Medication>()
                .ForMember(m => m.UserId, x => x.ResolveUsing(m => WebSecurity.CurrentUserId));
            Mapper.CreateMap<SettingsModel, User>();
            Mapper.CreateMap<User, SettingsModel>();
        }
    }
}