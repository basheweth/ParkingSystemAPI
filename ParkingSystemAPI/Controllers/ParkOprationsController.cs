using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data.Entity;
using System.Web.Http;
using ParkingSystemAPI.ViewModel;
using ParkingSystemAPI.Models;
using System.Data.Entity.Infrastructure;
using System.Web.Http.Cors;

namespace ParkingSystemAPI.Controllers
{
    [EnableCorsAttribute("*","*","*")]
    public class ParkOprationsController : ApiController
    {

        private ParkingDb db;
        public ParkOprationsController()
        {
            db = new ParkingDb();
        }
        [HttpGet]
        [Route("Parks/Get")]
        public IHttpActionResult GetParks()
        {
            List<ParkVM> parkvm = new List<ParkVM>();
            var parks = db.Parks.ToList();
            if(parks == null)
            {
                return Content(HttpStatusCode.NotFound,"No parks");
            }
            
            
            foreach (var item in parks)
            {
                var ParkViewModel = new ParkVM
                {
                    ParkId = item.ParkId,
                    ParkName = item.ParkName,
                    ParkStatus = item.ParkStatus
                };
                parkvm.Add(ParkViewModel);
            }

            return Ok(parkvm);
        }
        [HttpGet]
        [Route("Parking/GetReserved/{id}")]
        public IHttpActionResult GetReserved(int id)
        {
            
            var _reservedpark = db.ReservedParks.Find(id);
            if (_reservedpark == null)
            {
                return Content(HttpStatusCode.NotFound, "No parks");
            }

            ReservedParkVM _GetRequset = new ReservedParkVM
            {
                ResId = _reservedpark.ResId,
                CarName = _reservedpark.CarName,
                ParkName = _reservedpark.ParkName,
                PlateNum = _reservedpark.PlateNum,
                TimeIn = _reservedpark.TimeIn
            };


            return Ok(_GetRequset);
        } 
        [HttpPost]
        [Route("Parks/AddReseverd")]
        public IHttpActionResult AddReverstions([FromBody] ReservedParkVM value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int MaxId = db.ReservedParks.Count();
            int Id =  MaxId >0 ? MaxId+1 : 1;
            var AddRequst = new ReservedPark
            {
                ResId = Id,
                CarName = value.CarName,
                ParkName = value.ParkName,
                PlateNum = value.PlateNum,
                TimeIn = value.TimeIn
            };
            db.ReservedParks.Add(AddRequst);
            var park = db.Parks.First(pa => pa.ParkName == AddRequst.ParkName);
            park.ParkStatus = true;
            db.Entry(park).State = EntityState.Modified;

            try
            {
                
                db.SaveChanges();
                
            }
            catch (DbUpdateException)
            {
                if (db.ReservedParks.Any(I => I.ResId == AddRequst.ResId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.Created);

        }

        // PUT: api/ParkOprations/5
        [HttpPut]
        [Route("Parking/Update/{id}")]
        public IHttpActionResult EidteResrved(int id, [FromBody]ReservedParkVM value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != value.ResId)
            {
                return BadRequest();
            }
            var lastResved = db.ReservedParks.Find(id);
            var lastpark = db.Parks.First(pa => pa.ParkName == lastResved.ParkName);
            var EditRequset = db.ReservedParks.First(I => I.ResId == id);



            EditRequset.CarName = value.CarName;
            EditRequset.ParkName = value.ParkName;
            EditRequset.PlateNum = value.PlateNum;
            EditRequset.TimeIn = value.TimeIn;
            
            
            if(lastpark.ParkName != EditRequset.ParkName)
            {
                lastpark.ParkStatus = false;
                db.Entry(lastpark).State = EntityState.Modified;
                var newpark = db.Parks.First(pa => pa.ParkName == EditRequset.ParkName);
                newpark.ParkStatus = true;
                db.Entry(newpark).State = EntityState.Modified;

            }
            
            
            db.Entry(EditRequset).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/ParkOprations/5
        [HttpDelete]
        [Route("Parking/Delete/{id}")]
        public IHttpActionResult EndReverstion(int id)
        {
            ReservedPark _DelRequst = db.ReservedParks.Find(id);
            if (_DelRequst == null)
            {
                return NotFound();
            }
            DateTime D2 = DateTime.Now;
            var D3 = (_DelRequst.TimeIn < D2) ? (D2 - _DelRequst.TimeIn) : (_DelRequst.TimeIn - D2);
            var TotalTime = (int)D3.TotalHours;
            double TotalCost = 50 * TotalTime;
            int HitId = db.HistoryResevings.Count();
            var HisParkId = db.Parks.First(I => I.ParkName == _DelRequst.ParkName);
            var _HisRequset = new HistoryReseving
            {
                HistId = HitId > 0 ? HitId + 1 : 1,
                HisParkId = HisParkId.ParkId,
                HisCarName = _DelRequst.CarName,
                HisPlateNum = _DelRequst.ParkName,
                HisTimeIn = _DelRequst.TimeIn,
                HisTimeOut = DateTime.Now,
                Cost = TotalCost

            };
            var lastpark = db.Parks.First(I => I.ParkName == _DelRequst.ParkName);
            lastpark.ParkStatus = false;
            db.HistoryResevings.Add(_HisRequset);
            db.ReservedParks.Remove(_DelRequst);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            Dictionary<string, string> InfoBill = new Dictionary<string, string>()
            {
                {"Car name",_HisRequset.HisCarName },
                {"Park name",_DelRequst.ParkName },
                {"Coming time",_HisRequset.HisTimeIn.ToString() },
                {"Exit time",_HisRequset.HisTimeOut.ToString()},
                {"Total time",TotalTime.ToString() },
                {"Total Cost",_HisRequset.Cost.ToString() }
            };
            return Ok(InfoBill);



        }
        [HttpGet]
        [Route("Parking/Report")]
        public IHttpActionResult ResevingHistory()
        {

            List<HistoryResevingVM> ReportHistory = new List<HistoryResevingVM>();
            var _readHistory = db.HistoryResevings.ToList();
            if (_readHistory == null)
            {
                return Content(HttpStatusCode.NotFound, "No History yet");
            }

            var _histo = new HistoryReseving { Park = new Park() };

            foreach (var item in _readHistory)
            {
                var ViewModel = new HistoryResevingVM
                {
                    HisCarName = item.HisCarName,
                    HisPlateNum = item.HisPlateNum,
                    HisTimeIn = item.HisTimeIn,
                    HisTimeOut = item.HisTimeOut,
                    Cost = item.Cost,
                   
                    Park = new ParkVM
                    {
                        ParkName = _histo.Park.ParkName
                    }

                };
                ReportHistory.Add(ViewModel);
            }

            return Ok(ReportHistory);

        }
        [HttpGet]
        
        [HttpPost]
        [Route("Parks/New")]
        public IHttpActionResult AddParks([FromBody]ParkVM value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int count = db.Parks.Count();

            int Id = count > 0 ? db.Parks.Max(I => I.ParkId)+1:12023;
            
            var AddRequst = new Park
            {
                ParkId = Id,
                ParkName = value.ParkName,
                ParkStatus = false
            };

            db.Parks.Add(AddRequst);

            try
            {

                db.SaveChanges();

            }
            catch (DbUpdateException)
            {   
                   throw; 
            }

            return StatusCode(HttpStatusCode.Created);

        }
        
    }


}
