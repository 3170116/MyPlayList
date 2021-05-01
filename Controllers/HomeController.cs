using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using MyPlayList.Models;
using MyPlayList.ViewModels;

namespace MyPlayList.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDB db;

        public HomeController(AppDB _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var myLists = db.PlayLists.Include(x => x.PlayListVideos).ToList();

            return View(myLists);
        }

        public IActionResult EditVideo(int videoId = 0)
        {
            var viewModel = new BaseViewModel()
            {
                MyVideo = videoId == 0 ? new Video() : db.Videos.Include(x => x.PlayListVideos).Single(x => x.Id == videoId),
                MyPlayListItems = new List<SelectListItem>()
            };

            var myLists = db.PlayLists.ToList();

            foreach (var list in myLists)
            {
                viewModel.MyPlayListItems.Add(new SelectListItem()
                {
                    Text = list.Title,
                    Value = list.Id.ToString(),
                    Selected = viewModel.MyVideo.PlayListVideos != null && viewModel.MyVideo.PlayListVideos.Any(x => x.PlayListId == list.Id)
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditVideo(BaseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var myVideo = db.Videos.Include(x => x.PlayListVideos).SingleOrDefault(x => x.Id == viewModel.MyVideo.Id);

            if (myVideo == null)
            {
                myVideo = new Video()
                {
                    PlayListVideos = new List<PlayListVideo>()
                };
            }

            myVideo.YoutubeVideoId = viewModel.MyVideo.YoutubeVideoId;
            myVideo.Title = viewModel.MyVideo.Title ?? "";
            myVideo.MinutesToLoop = viewModel.MyVideo.MinutesToLoop;
            myVideo.SecondsToLoop = viewModel.MyVideo.SecondsToLoop;

            if (myVideo.Id == 0)
            {
                db.Videos.Add(myVideo);
                db.SaveChanges();
            }

            if (viewModel.MyPlayListItems != null)
            {
                foreach (var item in viewModel.MyPlayListItems)
                {
                    if (item.Selected && !myVideo.PlayListVideos.Any(x => x.PlayListId == Int32.Parse(item.Value)))
                    {
                        db.PlayListVideos.Add(new PlayListVideo()
                        {
                            PlayListId = Int32.Parse(item.Value),
                            VideoId = myVideo.Id
                        });
                    } else if (!item.Selected && myVideo.PlayListVideos.Any(x => x.PlayListId == Int32.Parse(item.Value)))
                    {
                        db.PlayListVideos.Remove(myVideo.PlayListVideos.Single(x => x.PlayListId == Int32.Parse(item.Value)));
                    }
                }

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveVideo(int videoId)
        {
            var myVideo = db.Videos.Single(x => x.Id == videoId);

            db.Videos.Remove(myVideo);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult EditList(int listId)
        {
            var list = listId == 0 ? new PlayList() : db.PlayLists.Single(x => x.Id == listId);

            return View(list);
        }

        [HttpPost]
        public IActionResult EditList(PlayList list)
        {
            if (!ModelState.IsValid)
            {
                return View(list);
            }

            var myList = db.PlayLists.SingleOrDefault(x => x.Id == list.Id);

            if (myList == null)
            {
                myList = new PlayList();
            }

            myList.Title = list.Title ?? "";

            if (myList.Id == 0)
            {
                db.PlayLists.Add(myList);
            }

            db.SaveChanges();

            return RedirectToAction("ViewList", new { listId = myList.Id });
        }

        public IActionResult RemoveList(int listId)
        {
            var myList = db.PlayLists.Single(x => x.Id == listId);

            db.PlayLists.Remove(myList);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult ViewList(int listId, int videoId = 0)
        {
            var viewModel = new BaseViewModel()
            {
                MyVideos = new List<Video>(),
                MyPlayList = db.PlayLists.Include(x => x.PlayListVideos).ThenInclude(x => x.Video).Single(x => x.Id == listId)
            };

            foreach (var item in viewModel.MyPlayList.PlayListVideos.OrderBy(x => x.Video.Title))
            {
                viewModel.MyVideos.Add(item.Video);
            }

            if (viewModel.MyVideos.Any(x => x.Id == videoId))
            {
                viewModel.MyVideos.Single(x => x.Id == videoId).Selected = true;
            } else if (viewModel.MyVideos.Any())
            {
                viewModel.MyVideos.First().Selected = true;
            }

            return View(viewModel);
        }

    }
}
