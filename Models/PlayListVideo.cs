using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyPlayList.Models
{
    public class PlayListVideo
    {
        [Key]
        public int Id { get; set; }

        public int PlayListId { get; set; }

        public int VideoId { get; set; }

        [ForeignKey("PlayListId")]
        public PlayList PlayList { get; set; }

        [ForeignKey("VideoId")]
        public Video Video { get; set; }
    }
}
