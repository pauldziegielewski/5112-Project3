using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project3.Controllers
{
    public class DataController : ApiController
    {

        //ListTags

        //FindTag(tagid)

        //ListTagsForArticle(int articleid)
        [HttpGet]
        [Route("api/tagData/ListTagsForArticle")]
        public string Test()
        {
            return "hey hey hey";
        }

        //AssociateTagWithArticle(int tagid, int articleid

        //UnassociateTagWithArticle(int tagid, int articleid)

        //AddTag(tag)

        //DeleteTag(id)

        //UpdateTag(id, tag)

    }
}
