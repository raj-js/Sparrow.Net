using Microsoft.AspNetCore.Mvc;
using Sparrow.AspNetCore.Tests.Data.Models;
using Sparrow.Core.Domain.Repositories;
using Sparrow.Core.Domain.Uow;
using System;
using System.Collections.Generic;

namespace Sparrow.AspNetCore.Tests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IUowManager _uowManager;
        private readonly IRepository<Blog, string> _blogResp;
        private readonly IRepository<Post> _postResp;
        private readonly ICurrentUowProvider _currentUowProvider;

        public ValuesController(IRepository<Blog, string> blogResp, IUowManager uowManager, IRepository<Post> postResp, ICurrentUowProvider currentUowProvider)
        {
            _blogResp = blogResp;
            _uowManager = uowManager;
            _postResp = postResp;
            _currentUowProvider = currentUowProvider;
        }

        [HttpGet]
        [Uow(IsDisabled = true)]
        public ActionResult<IEnumerable<int>> Get()
        {
            var blog = new Blog()
            {
                Id = Guid.NewGuid().ToString("N"),
                Author = "Raj",
                Signature = "做到极致，便是大师",
                CreationTime = DateTime.Now
            };
            _blogResp.Insert(blog);

            _postResp.Insert(new Post()
            {
                BlogId = blog.Id,
                Title = "test post title",
                Content = "test post content",
                PostTime = DateTime.Now
            });

            return new[] { _blogResp.Count(), _postResp.Count() };
        }
    }
}
