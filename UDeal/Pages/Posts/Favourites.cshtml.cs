﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UDeal.Data;
using UDeal.Models;

namespace UDeal.Pages.Posts
{
    public class FavouritesModel : PageModel
    {
        private readonly UDeal.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;


        public FavouritesModel(UserManager<User> userManager, UDeal.Data.ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Post> Post { get;set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var favIds = _context.UserFavourites.Where(f => f.UserId == user.Id).Select(f => f.PostId);
            Post = await _context.Posts
                .Where(p => favIds.Contains(p.Id))
                .Include(p => p.Course)
                .Include(p => p.Campus)
                .ToListAsync();
        }


        public async Task<IActionResult> OnPostRemove(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            _context.UserFavourites.Remove(new Favourite
            {
                PostId = id,
                UserId = user.Id
            });
            await _context.SaveChangesAsync();
            return Redirect("/Posts/Favourites");
        }
    }
}
