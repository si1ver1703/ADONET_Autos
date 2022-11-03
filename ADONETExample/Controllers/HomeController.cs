using ADONETExample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;

namespace ADONETExample.Controllers
{
    public class HomeController : Controller
    { 

        MyMusicDBEntities db = new MyMusicDBEntities();


        public IActionResult Index(int Id)
        {
            List<Auto> auto = (from m in db.Autoes select m).ToList();

            return View(auto);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Auto>>> Get()
        {
            return await db.Autoes.Select(x => AutoOut(x)).ToListAsync();
        }


        [HttpGet("Autos")]
        public async Task<ActionResult<Auto>> GetFindAuto(int Id)
        {
            var FindTemp = await db.Autoes.FindAsync(Id);
            if (FindTemp == null)
            {
                return NotFound();
            }

            return AutoOut(FindTemp);

        }

        [HttpPost(Name = "PostAuto")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(Auto auto)
        {
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { mark = auto.mark_auto, color = auto.color_auto, date = auto.price, price = auto.price, type = auto.type_auto, engine = auto.engine, isKaz = auto.isKazakstan, desc = auto.decs }, auto);
        }

        [HttpDelete("Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int Id)
        {
            var findDeleteObject = await db.Autoes.FindAsync(Id);
            if (findDeleteObject == null)
            {
                return NotFound();
            }
            db.Autoes.Remove(findDeleteObject);
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("Id")]
        /*[ProducesResponseType(StatusCodes.Status204NoContent)]*/
        public async Task<ActionResult> Put(int Id, Auto auto)
        {
            if (Id == auto.Id)
            {

                var autoObject = db.Autoes.FindAsync(Id);

                try
                {
                    if (await autoObject != null)
                    {
                        db.Entry(autoObject).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        db.Autoes.Add(auto);
                    }

                    await db.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (Id != auto.Id)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                }

            }
            else
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPatch("Id")]
        public async Task<ActionResult> Patch(int Id, Auto auto)
        {
            var autoID = await db.Autoes.SingleAsync(x => x.Id == Id);

            autoID.mark_auto = auto.mark_auto;
            autoID.color_auto = auto.color_auto;
            autoID.date = auto.date;
            autoID.price = auto.price;
            autoID.type_auto = auto.type_auto;
            autoID.engine = auto.engine;
            autoID.isKazakstan = auto.isKazakstan;
            autoID.decs = auto.decs;

            await db.SaveChangesAsync();

            return NoContent();
        }




        private static Auto AutoOut(Auto auto) => new Auto
        {
            Id = auto.Id,
            mark_auto = auto.mark_auto,
            color_auto = auto.color_auto,
            date = auto.date,
            price = auto.price,
            type_auto = auto.type_auto,
            engine = auto.engine,
            isKazakstan = auto.isKazakstan,
            decs = auto.decs

        };
    }
}