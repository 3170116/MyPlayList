using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyPlayList.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string YoutubeVideoId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public int MinutesToLoop { get; set; }

        public int SecondsToLoop { get; set; }

        public List<PlayListVideo> PlayListVideos { get; set; }

        [NotMapped]
        public bool Selected { get; set; }


        public int GetTotalSeconds()
        {
            return 60 * MinutesToLoop + SecondsToLoop;
        }


        public static IList<SelectListItem> GetMinutesList(int minutes)
        {
            IList<SelectListItem> result = new List<SelectListItem>();

            for (int i = 0; i <= 5; i++)
            {
                result.Add(new SelectListItem()
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = i == minutes
                });
            }

            return result;
        }

        public static IList<SelectListItem> GetSecondsList(int seconds)
        {
            IList<SelectListItem> result = new List<SelectListItem>();

            for (int i = 0; i <= 60; i++)
            {
                result.Add(new SelectListItem()
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = i == seconds
                });
            }

            return result;
        }
    }
}
