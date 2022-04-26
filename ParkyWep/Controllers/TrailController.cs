using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWep.Models;
using ParkyWep.Models.ViewModel;
using ParkyWep.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWep.Controllers
{
    public class TrailController : Controller
    {
        private readonly ITrailRepository _trailRepo;
        private readonly INationalParkRepository _npRepo;
        public TrailController(ITrailRepository trailRepo, INationalParkRepository npRepo)
        {
            _trailRepo = trailRepo;
            _npRepo = npRepo;
        }
        public IActionResult Index()
        {
            return View(new Trail { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {

            IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);

            TrailsVM objvm = new TrailsVM()
            {
                NationalParkList = npList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Trail = new Trail()

            };

            if (id == null)
            {
                //this will be true for Insert/Create
                return View(objvm);
            }

            objvm.Trail = await _trailRepo.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault());
            if (objvm.Trail == null)
            {
                return NotFound();
            }
            return View(objvm);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM obj)
        {
            if (ModelState.IsValid)
            {

                if (obj.Trail.Id == 0)
                {
                    await _trailRepo.CreateAsync(SD.TrailAPIPath, obj.Trail);
                }
                else
                {
                    await _trailRepo.UpdateAsync(SD.TrailAPIPath + obj.Trail.Id, obj.Trail);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {

                IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);

                TrailsVM objvm = new TrailsVM()
                {
                    NationalParkList = npList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = obj.Trail

                };

                return View(objvm);
            }
        }

        public async Task<IActionResult> GetAllTrail()
        {

            return Json(new { data = await _trailRepo.GetAllAsync(SD.TrailAPIPath) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(SD.TrailAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });

            }
            return Json(new { success = false, message = "Delete faild" });

        }



    }
}
