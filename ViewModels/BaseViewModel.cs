using Microsoft.AspNetCore.Mvc.Rendering;
using MyPlayList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPlayList.ViewModels
{
    public class BaseViewModel
    {
        public Video MyVideo { get; set; }

        public PlayList MyPlayList { get; set; }

        public IList<Video> MyVideos { get; set; }

        public IList<SelectListItem> MyPlayListItems { get; set; }
    }
}
